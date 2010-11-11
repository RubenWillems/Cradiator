using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Cradiator.Extensions;
using log4net;

namespace Cradiator.Config
{
    public class ViewSettingsReader
    {
        private const string ProjectRegex = "project-regex";
        private const string CategoryRegex = "category-regex";
        private const string Url = "url";
        private const string Skin = "skin";

        static readonly ILog _log = LogManager.GetLogger(typeof(ViewSettingsReader).Name);
        private readonly string _configFile;

        public ViewSettingsReader(IConfigLocation configLocation)
        {
            _configFile = configLocation.FileName;
        }

        public string Xml { private get; set; } // todo for testing only, reconsider

        public ICollection<ViewSettings> Read()
        {
            var xDoc = Xml.HasValue() ? XDocument.Parse(Xml) : XDocument.Load(_configFile);

            return new ReadOnlyCollection<ViewSettings>(
                (from view in xDoc.Elements("configuration")
                        .Elements("views")
                        .Elements("view")
                    select new ViewSettings
                    {
                        URL = view.Attribute(Url).Value,
                        ProjectNameRegEx = view.Attribute(ProjectRegex).Value,
                        CategoryRegEx = view.Attribute(CategoryRegex).Value,
                        SkinName = view.Attribute(Skin).Value,
                    }).ToList());
        }

        public string Write(ViewSettings settings)
        {
            var xDoc = Xml.HasValue() ? XDocument.Parse(Xml) : XDocument.Load(_configFile);

            var view = xDoc.Elements("configuration")
                .Elements("views")
                .Elements("view")
                .First();

            view.Attribute(Url).SetValue(settings.URL);
            view.Attribute(ProjectRegex).SetValue(settings.ProjectNameRegEx);
            view.Attribute(CategoryRegex).SetValue(settings.CategoryRegEx);
            view.Attribute(Skin).SetValue(settings.SkinName);

            if (Xml.HasValue()) return xDoc.ToString(); 
            xDoc.Save(_configFile);
            return "";      //todo need find a way here to make test code and real/saving code, both look good
        }
    }
}
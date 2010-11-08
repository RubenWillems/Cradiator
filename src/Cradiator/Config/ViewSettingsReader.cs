using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Cradiator.Extensions;

namespace Cradiator.Config
{
    public class ViewSettingsReader
    {
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
                        ID = view.Attribute("id").Value,
                        URL = view.Attribute("url").Value,
                        ProjectNameRegEx = view.Attribute("project-regex").Value,
                        CategoryRegEx = view.Attribute("category-regex").Value,
                        SkinName = view.Attribute("skin").Value,
                    }).ToList());
        }

        public string Write(string id, ViewSettings settings)
        {
            var xDoc = Xml.HasValue() ? XDocument.Parse(Xml) : XDocument.Load(_configFile);

            var view = xDoc.Elements("configuration")
                .Elements("views")
                .Elements("view")
                .Where(v => v.Attribute("id").Value == id).First();

            view.Attribute("url").SetValue(settings.URL);
            view.Attribute("project-regex").SetValue(settings.ProjectNameRegEx);
            view.Attribute("category-regex").SetValue(settings.CategoryRegEx);
            view.Attribute("skin").SetValue(settings.SkinName);

            if (Xml.HasValue()) return xDoc.ToString(); 
            xDoc.Save(_configFile);
            return "";      //todo need find a way here to make test code and real/saving code, both look good
        }
    }
}
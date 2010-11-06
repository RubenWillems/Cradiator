using System.Collections.Generic;
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

        public IEnumerable<ViewSettings> Read()
        {
            var xDoc = Xml.HasValue() ? XDocument.Parse(Xml) : XDocument.Load(_configFile);

            return (from view in xDoc.Elements("configuration")
                        .Elements("views")
                        .Elements("view")
                    select new ViewSettings
                               {
                                   URL = view.Attribute("url").Value,
                                   ProjectNameRegEx = view.Attribute("project-regex").Value,
                                   CategoryRegEx = view.Attribute("category-regex").Value,
                                   SkinName = view.Attribute("skin").Value,
                               });
        }
    }
}
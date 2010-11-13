using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Cradiator.Config
{
    public class ViewSettingsParser
    {
        const string ProjectRegex = "project-regex";
        const string CategoryRegex = "category-regex";
        const string Url = "url";
        const string Skin = "skin";

        readonly XDocument _xdoc;

        public ViewSettingsParser(TextReader xml)
        {
            _xdoc = XDocument.Parse(xml.ReadToEnd());
        }

        public static ICollection<ViewSettings> Read(string xmlFile)
        {
            using (var stream = new StreamReader(xmlFile))
            {
                var reader = new ViewSettingsParser(stream);
                return reader.ParseXml();
            }
        }

        public ICollection<ViewSettings> ParseXml()
        {
            return new ReadOnlyCollection<ViewSettings>(
                (from view in _xdoc.Elements("configuration")
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

        //-----
        // modify functionality (below) is only for the settings dialog save functionality
        // someone less enamoured with guis might be tempted to delete this & the functionality
        //-----

        public static void Modify(string xmlFile, ViewSettings viewSettings)
        {
            string xmlUpdated;
            using (var stream = new StreamReader(xmlFile))
            {
                var parser = new ViewSettingsParser(stream);
                xmlUpdated = parser.CreateUpdatedXml(viewSettings);
            }
            using (var stream = new StreamWriter(xmlFile))
            {
                stream.Write(xmlUpdated);
            }
        }

        public string CreateUpdatedXml(IViewSettings settings)
        {
            var view1 = _xdoc.Elements("configuration")
                             .Elements("views")
                             .Elements("view").First(); // the assumption that there is only 1 must be valid

            view1.Attribute(Url).Value = settings.URL;
            view1.Attribute(ProjectRegex).Value = settings.ProjectNameRegEx;
            view1.Attribute(CategoryRegex).Value = settings.CategoryRegEx;
            view1.Attribute(Skin).Value = settings.SkinName;

            var xml = new StringBuilder();
            using (var xmlWriter = XmlWriter.Create(xml, new XmlWriterSettings
                                    {
                                        OmitXmlDeclaration = true,
                                        NewLineHandling = NewLineHandling.None,
                                        Indent = true,
                                    }))
            {
                _xdoc.WriteTo(xmlWriter);
            }
            return xml.ToString();
        }
    }
}
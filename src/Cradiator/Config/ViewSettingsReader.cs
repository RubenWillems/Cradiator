using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Cradiator.Config
{
    public class ViewSettingsReader
    {
        const string ProjectRegex = "project-regex";
        const string CategoryRegex = "category-regex";
        const string Url = "url";
        const string Skin = "skin";

        readonly XDocument _xdoc;

        public ViewSettingsReader(TextReader xml)
        {
            _xdoc = XDocument.Parse(xml.ReadToEnd());
        }

        public ICollection<ViewSettings> Read()
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

        public string Write(ViewSettings settings)
        {
            var view1 = _xdoc.Elements("configuration")
                .Elements("views")
                .Elements("view")
                .First();           // we only call this, when this assumption is valid

            view1.Attribute(Url).Value = settings.URL;
            view1.Attribute(ProjectRegex).Value = settings.ProjectNameRegEx;
            view1.Attribute(CategoryRegex).Value = settings.CategoryRegEx;
            view1.Attribute(Skin).Value = settings.SkinName;

            var xml = new StringBuilder();
            using (var xmlTextWriter = XmlWriter.Create(xml, new XmlWriterSettings
                                                          {
                                                              OmitXmlDeclaration = true,
                                                              NewLineHandling = NewLineHandling.None,
                                                              Indent = true,
                                                          }))
            {
                _xdoc.WriteTo(xmlTextWriter);
            }
            return xml.ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Cradiator.MigrateConfig
{
	public class Migrate
	{
		private readonly XDocWrapper _xdoc;

		public Migrate(string xmlFile)
		{
			_xdoc = new XDocWrapper(xmlFile);
		}

		public string Update()
		{
			var addElements = _xdoc.Element("configuration")
									.Element("appSettings")
									.Elements("add");

			var oldElements = from element in addElements
							  let e = element.Attribute("key").Value
							  where e == "URL" ||
									e == "Skin" ||
									e == "ProjectNameRegEx" ||
									e == "CategoryRegEx"
							  select element;

			if (!oldElements.Any())
			{
				Console.WriteLine("no pre multi-view config detected");
				return "";
			}

			var config = new Dictionary<string, string>();
			Console.WriteLine("removing pre multi-view xml elements...");
			foreach (var oldElement in oldElements)
			{
				config[oldElement.Attribute("key").Value] = oldElement.Attribute("value").Value;
			}

			oldElements.Remove();

			Console.WriteLine("Adding multi-view xml elements...");

			_xdoc.Element("configuration").Element("configSections").AddAfterSelf(
				new XElement("views",
				new XElement("view",
					new XAttribute("url", config["URL"]),
					new XAttribute("skin", config["Skin"]),
					new XAttribute("project-regex", config["ProjectNameRegEx"]),
					new XAttribute("category-regex", config["CategoryRegEx"])
				)));

			_xdoc.Element("configuration")
				.Element("configSections").Add(
					new XElement("section",
						new XAttribute("name", "views"),
						new XAttribute("type", "System.Configuration.IgnoreSectionHandler")
			));

			return _xdoc.Save();
		}
	}
}
using System.Collections.Generic;
using System.Xml.Linq;

namespace Cradiator.MigrateConfig
{
	public class XDocWrapper
	{
		private XDocument Xdoc { get; set; }
		private string Xml { get; set; }
		private bool IsFile { get; set; }

		public XDocWrapper(string xml)
		{
			Xml = xml;
			IsFile = xml.EndsWith(".config");
			Xdoc = IsFile ? XDocument.Load(xml) : XDocument.Parse(xml);
		}
		
		public string Save()
		{
			if (IsFile) Xdoc.Save(Xml);
			else Xml = Xdoc.ToString();
			return IsFile ? "" : Xml;
		}

		public XElement Element(XName xname)
		{
			return Xdoc.Element(xname);
		}

		public IEnumerable<XElement> Elements(XName xname)
		{
			return Xdoc.Elements(xname);
		}
	}
}
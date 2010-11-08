using System.Xml.Linq;

namespace Cradiator.Extensions
{
	public static class XmlExtensions
	{
		public static string GetValue(this XAttribute attribute)
		{
			return attribute == null ? string.Empty : attribute.Value;
		}
	}
}
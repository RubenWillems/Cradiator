using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Cradiator.Extensions;

namespace Cradiator.Config
{
	public class UserNameMappingReader
	{
		readonly string _configFile;

		public UserNameMappingReader(IConfigLocation configLocation)
		{
			_configFile = configLocation.FileName;
		}

		public string Xml { private get; set; }

		public IDictionary<string,string> Read()
		{
			var xDoc = Xml.HasValue() ? XDocument.Parse(Xml) : XDocument.Load(_configFile);

			return (from username in xDoc.Elements("configuration")
						.Elements("usernames")
			        	.Elements("add")
			        select new 
			        {
						UserName = username.FirstAttribute.Value,
			            FullName= username.LastAttribute.Value
					}).ToDictionary(x => x.UserName, x => x.FullName);
		}
	}
}
using System;
using System.IO;

namespace Cradiator.MigrateConfig
{
	class Program
	{
		static int Main(string[] args)
		{
			var xmlFile = args.Length == 1 ? args[0] : "Cradiator.exe.config";

			if (!File.Exists(xmlFile))
			{
				Console.WriteLine("xml config file doesn't exist: '{0}'", xmlFile);
				return 1;
			}

			var migrate = new Migrate(xmlFile);
			var returnVal = migrate.Update();

			return string.IsNullOrEmpty(returnVal) ? 0 : 1;
		}
	}
}

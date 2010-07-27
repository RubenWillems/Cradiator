using Cradiator.Config;
using NUnit.Framework;

namespace Cradiator.Tests.Config
{
	[TestFixture]
	public class ConfigFileWatcher_Tests
	{
		[Test]
		public void CanCreate()
		{
			//TODO not a great test, just scratches the surface
			var settings = new ConfigSettings();

			const string file = @"c:\windows\win.ini";
			var configFileWatcher = new ConfigFileWatcher(settings, file);
			configFileWatcher.Start();
			Assert.That(configFileWatcher.FileSystemWatcher.Filter, Is.EqualTo("win.ini"));
		}
	}
}
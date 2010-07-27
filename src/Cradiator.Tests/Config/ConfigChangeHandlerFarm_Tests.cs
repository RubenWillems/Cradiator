using System.Collections.Generic;
using Cradiator.Config;
using Cradiator.Config.ChangeHandlers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Cradiator.Tests.Config
{
	[TestFixture]
	public class ConfigChangeHandlerFarm_Tests
	{
		IConfigChangeHandler _updater;
		ConfigSettings _configSettings;

		[SetUp]
		public void SetUp()
		{
			_updater = MockRepository.GenerateMock<IConfigChangeHandler>();
			_configSettings = new ConfigSettings();
		}

		[Test]
		public void CanUpdate_1_Updater()
		{
			var configUpdater = new ConfigChangeHandlerFarm(new List<IConfigChangeHandler> { _updater });
			configUpdater.UpdateAll(_configSettings);

			_updater.AssertWasCalled(u=>u.ConfigUpdated(_configSettings));
		}

		[Test]
		public void CanUpdate_2_Updaters()
		{
			var updater2 = MockRepository.GenerateMock<IConfigChangeHandler>();

			var configUpdater = new ConfigChangeHandlerFarm(new List<IConfigChangeHandler> { _updater, updater2 });
			configUpdater.UpdateAll(_configSettings);

			_updater.AssertWasCalled(u => u.ConfigUpdated(_configSettings));
			updater2.AssertWasCalled(u => u.ConfigUpdated(_configSettings));
		}
	}
}
using System.Collections.Generic;
using BigVisibleCruise2.Config;
using NUnit.Framework;
using Rhino.Mocks;

namespace BigVisibleCruise2.Tests.Config
{
	[TestFixture]
	public class ConfigUpdater_Tests
	{
		IConfigObserver _updater;
		ConfigSettings _configSettings;

		[SetUp]
		public void SetUp()
		{
			_updater = MockRepository.GenerateMock<IConfigObserver>();
			_configSettings = new ConfigSettings();
		}

		[Test]
		public void CanUpdate_1_Updater()
		{
			var configUpdater = new ConfigUpdateResponderZoo(new List<IConfigObserver> { _updater });
			configUpdater.UpdateAll(_configSettings);

			_updater.AssertWasCalled(u=>u.ConfigUpdated(_configSettings));
		}

		[Test]
		public void CanUpdate_2_Updaters()
		{
			var updater2 = MockRepository.GenerateMock<IConfigObserver>();

			var configUpdater = new ConfigUpdateResponderZoo(new List<IConfigObserver> { _updater, updater2 });
			configUpdater.UpdateAll(_configSettings);

			_updater.AssertWasCalled(u => u.ConfigUpdated(_configSettings));
			updater2.AssertWasCalled(u => u.ConfigUpdated(_configSettings));
		}
	}
}
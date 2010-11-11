using Cradiator.App;
using Cradiator.Config;
using Cradiator.Model;
using Cradiator.Views;
using Ninject;
using NUnit.Framework;
using Rhino.Mocks;
using Shouldly;

namespace Cradiator.Tests.Views
{
	[TestFixture]
	public class Presenter_Tests
	{
		ICradiatorView _view;
		IConfigSettings _configSettings;
		ISkinLoader _skinLoader;
		IKernel _kernel;
		IScreenUpdater _screenUpdater;
		IConfigFileWatcher _configFileWatcher;

		[SetUp]
		public void SetUp()
		{
			_view = Create.Mock<ICradiatorView>();
			_configSettings = Create.Mock<IConfigSettings>();
			_configSettings.Expect(c => c.ProjectNameRegEx).Return(".*").Repeat.Any();
			_configSettings.Expect(c => c.CategoryRegEx).Return(".*").Repeat.Any();

			_skinLoader = Create.Mock<ISkinLoader>();
			_screenUpdater = Create.Mock<IScreenUpdater>();
			_configFileWatcher = Create.Mock<IConfigFileWatcher>();

			var bootstrapper = new Bootstrapper(_configSettings, _view);
			_kernel = bootstrapper.CreateKernel();
			_kernel.Rebind<ISkinLoader>().ToConstant(_skinLoader);
			_kernel.Rebind<IScreenUpdater>().ToConstant(_screenUpdater);
			_kernel.Rebind<IConfigFileWatcher>().ToConstant(_configFileWatcher);
		}

		//TODO this test takes ages... can this be avoided?
		[Test]
		public void CanCreatePresenter()
		{
			var presenter = _kernel.Get<CradiatorPresenter>();
			presenter.Init();

			_configSettings.ShouldHaveBeenCalled(c => c.AddObserver(presenter));
            _skinLoader.ShouldHaveBeenCalled(s => s.Load(Arg<Skin>.Is.Anything));
            _screenUpdater.ShouldHaveBeenCalled(s => s.Update());
            _configFileWatcher.ShouldHaveBeenCalled(c => c.Start());
		}
	}
}
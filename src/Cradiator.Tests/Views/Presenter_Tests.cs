using Cradiator.App;
using Cradiator.Config;
using Cradiator.Model;
using Cradiator.Views;
using Ninject;
using NUnit.Framework;
using Rhino.Mocks;

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
			_view = MockRepository.GenerateMock<ICradiatorView>();
			_configSettings = MockRepository.GenerateMock<IConfigSettings>();
			_configSettings.Expect(c => c.ProjectNameRegEx).Return(".*").Repeat.Any();
			_configSettings.Expect(c => c.CategoryRegEx).Return(".*").Repeat.Any();

			_skinLoader = MockRepository.GenerateMock<ISkinLoader>();
			_screenUpdater = MockRepository.GenerateMock<IScreenUpdater>();
			_configFileWatcher = MockRepository.GenerateMock<IConfigFileWatcher>();

			var bootstrapper = new Bootstrapper(_configSettings, _view);
			_kernel = bootstrapper.CreateKernel();
			_kernel.Rebind<ISkinLoader>().ToConstant(_skinLoader);
			_kernel.Rebind<IScreenUpdater>().ToConstant(_screenUpdater);
			_kernel.Rebind<IConfigFileWatcher>().ToConstant(_configFileWatcher);
		}

		//TODO this test takes 3.5 seconds... can this be avoided?
		[Test]
		public void CanCreatePresenter()
		{
			var presenter = _kernel.Get<CradiatorPresenter>();
			presenter.Init();

			_configSettings.AssertWasCalled(c => c.AddObserver(presenter));
			_skinLoader.AssertWasCalled(s=>s.Load(Arg<Skin>.Is.Anything));
			_screenUpdater.AssertWasCalled(s=>s.Update());
			_configFileWatcher.AssertWasCalled(c => c.Start());
		}
	}
}
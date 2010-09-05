using System.Linq;
using Cradiator.Audio;
using Cradiator.Config;
using Cradiator.Config.ChangeHandlers;
using Cradiator.Model;
using Cradiator.Views;
using Cradiator.Extensions;
using Ninject;
using Ninject.Modules;

namespace Cradiator.Services
{
	public class CradiatorNinjaModule : NinjectModule
	{
		readonly ICradiatorView _view;
		readonly IConfigSettings _configSettings;

		public CradiatorNinjaModule(ICradiatorView view, IConfigSettings settings)
		{
			_view = view;
			_configSettings = settings;
		}

		public override void Load()
		{
			Bind<ICradiatorView>().ToConstant(_view);
			Bind<IConfigSettings>().ToConstant(_configSettings);

			Bind<IWebClientFactory>().To<WebClientFactory>().InSingletonScope();
			Bind<IAudioPlayer>().To<AudioPlayer>().InSingletonScope();
			Bind<ICountdownTimer>().To<CountdownTimer>().InSingletonScope();
			Bind<IPollTimer>().To<PollTimer>().InSingletonScope();
			Bind<ISpeechSynthesizer>().To<CradiatorSpeechSynthesizer>().InSingletonScope();
			Bind<ISkinLoader>().To<SkinLoader>().InSingletonScope();
			Bind<IScreenUpdater>().To<ScreenUpdater>().InSingletonScope();
			Bind<ISettingsWindow>().To<SettingsWindow>().InSingletonScope();
			Bind<ISpeechTextParser>().To<SpeechTextParser>().InSingletonScope();
			Bind<IAppLocation>().To<AppLocation>().InSingletonScope();

			IConfigLocation configLocation = new ConfigLocation();
			Bind<IConfigLocation>().ToConstant(configLocation).InSingletonScope();
			Bind<IConfigFileWatcher>().ToConstant(new ConfigFileWatcher(_configSettings, configLocation.FileName));

			Bind<IBuildBuster>().To<BuildBuster>()
				.WhenTargetHas<InjectBuildBusterAttribute>().InSingletonScope();

			Bind<IBuildBuster>().To<BuildBusterImageDecorator>()
				.WhenTargetHas<InjectBuildBusterImageDecoratorAttribute>().InSingletonScope();
			
			Bind<IBuildBuster>().To<BuildBusterFullNameDecorator>()
				.WhenTargetHas<InjectBuildBusterFullNameDecoratorAttribute>().InSingletonScope();

			Bind<CradiatorPresenter>().ToSelf().InSingletonScope();

			BindConfigChangeHandlers();
		}

	    private void BindConfigChangeHandlers()
	    {
	        var changeHandlers = from type in typeof (ConfigChangeHandlerFarm).Assembly.GetExportedTypes()
	                             where !type.IsInterface
	                             where typeof (IConfigChangeHandler).IsAssignableFrom(type)
	                             select type;

	        changeHandlers.ForEach(handler => Bind<IConfigChangeHandler>().To(handler).InSingletonScope());
	    }
	}
}
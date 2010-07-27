using System;
using System.Windows;
using Cradiator.Config;
using Cradiator.Views;
using log4net;
using Ninject;

namespace Cradiator.App
{
	/// <summary>
	/// CradiatorApp
	/// </summary>
	public class CradiatorApp : Application
	{
        static readonly ILog _log = LogManager.GetLogger(typeof(CradiatorApp).Name);

		/// <summary>
		/// Application Entry Point
		/// </summary>
		[STAThread]
		public static void Main()
		{
		    try
		    {
		        var app = new CradiatorApp();
		        var configSettings = new ConfigSettings();
				configSettings.Load();
				var mainWindow = new CradiatorWindow(configSettings);
		    	var bootstrapper = new Bootstrapper(configSettings, mainWindow);
		    	var kernel = bootstrapper.CreateKernel();
		    	var presenter = kernel.Get<CradiatorPresenter>();

		        mainWindow.Show();
				presenter.Init();

		        app.Run();
		    }
		    catch (Exception exception)
		    {
		        _log.Error(exception.Message, exception);

		        var messageWindow = new MessageWindow(null);
				messageWindow.ShowMessage(5, "Application Exception - see log for details\nShutting down...");
		    }
		}
	}
}
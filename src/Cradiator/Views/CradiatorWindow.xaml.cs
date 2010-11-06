using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Cradiator.Config;
using log4net;

namespace Cradiator.Views
{
	public partial class CradiatorWindow : ICradiatorView, IConfigObserver
	{
		static readonly ILog _log = LogManager.GetLogger(typeof(CradiatorWindow).Name);

		int _pollFrequency;
		bool _isShowProgressConfigured;	// named to avoid confusion with property on this view with what was a similar name
		private IConfigSettings _configSettings;
		private int currentMultiServerIndex;

		public CradiatorWindow(IConfigSettings configSettings)
		{
			try
			{
				InitializeComponent();
			}
			catch (Exception exception)
			{
				_log.Error(exception);	// usually xaml issues 
				throw;
			}

			_pollFrequency = configSettings.PollFrequency;
			_isShowProgressConfigured = configSettings.ShowProgress;
			_configSettings = configSettings;
			configSettings.AddObserver(this);
		}

		public CradiatorPresenter Presenter { get; set; }

		bool ICradiatorView.ShowProgress
		{
			set
			{
				Invoke(() => progressBar.Visibility =
								value && _isShowProgressConfigured ? 
									Visibility.Visible : Visibility.Hidden);
			}
		}

		object ICradiatorView.DataContext
		{
			set { Invoke(() => DataContext = value); }
		}

		void ICradiatorView.AddWindowBinding(InputBinding binding)
		{
			InputBindings.Add(binding);
		}

		void ICradiatorView.AddSettingsLinkBinding(InputBinding binding)
		{
			settingsLink.InputBindings.Add(binding);
		}

		public event EventHandler ScreenUpdating;

		void ICradiatorView.UpdateScreen()
		{
			FireScreenUpdating(null);
			Presenter.UpdateScreen();
		}

		void FireScreenUpdating(EventArgs e)
		{
			if (ScreenUpdating != null)
				ScreenUpdating(this, e);
		}

		Window ICradiatorView.Window
		{
			get { return this; }
		}

		void ICradiatorView.ShowMessage(string message)
		{
			var messageWindow = new MessageWindow(this);
			messageWindow.ShowMessage(_pollFrequency, message);
		}

		void ICradiatorView.UpdateCountdownTimer(TimeSpan timeRemaining)
		{
			countdownText.Text = string.Format("{0:00}:{1:00}",
											  timeRemaining.Minutes,
											  timeRemaining.Seconds);

			if (timeRemaining.Seconds == 0)
			{
				if (_configSettings.ViewCount > 0)
				{
					_configSettings.UpdateViewSettings(currentMultiServerIndex);
					currentMultiServerIndex++;

                    if (currentMultiServerIndex == _configSettings.ViewCount)
					{
						currentMultiServerIndex = 0;
					}

					_configSettings.NotifyObservers();
				}
			}
		}

		public void Invoke(Action action)
		{
			Dispatcher.Invoke(DispatcherPriority.Normal, action);
		}

		void ICradiatorView.ShowCountdown(bool show)
		{
			countdownText.Visibility = show ? Visibility.Visible : Visibility.Hidden;
		}

		public void ConfigUpdated(ConfigSettings newSettings)
		{
			_isShowProgressConfigured = newSettings.ShowProgress;
			_pollFrequency = newSettings.PollFrequency;
		}
	}
}
using Cradiator.Model;
using Cradiator.Views;

namespace Cradiator.Config.ChangeHandlers
{
	public class ShowCountdownChangeHandler : IConfigChangeHandler
	{
		private readonly ICountdownTimer _countdownTimer;
		private readonly ICradiatorView _view;

		public ShowCountdownChangeHandler(ICountdownTimer countdownTimer, ICradiatorView view)
		{
			_countdownTimer = countdownTimer;
			_view = view;
		}

		void IConfigChangeHandler.ConfigUpdated(ConfigSettings newSettings)
		{
			lock (_countdownTimer)
			{
				if (newSettings.ShowCountdown != _countdownTimer.IsSwitchedOn)
				{
					if (newSettings.ShowCountdown)
					{
						_countdownTimer.SwitchOn();
						_countdownTimer.Start();
					}
					else
					{
						_countdownTimer.Stop();
						_countdownTimer.SwitchOff();
					}

					_view.Invoke(() =>
					{
						_countdownTimer.Execute();	// ensure the countdown is set/executed before making visible
						_view.ShowCountdown(newSettings.ShowCountdown);
					});
				}
			}
		}
	}
}
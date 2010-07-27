using Cradiator.Model;

namespace Cradiator.Config.ChangeHandlers
{
	public class PollIntervalChangeHandler : IConfigChangeHandler
	{
		readonly IPollTimer _pollTimer;
		readonly ICountdownTimer _countdownTimer;

		public PollIntervalChangeHandler(IPollTimer pollTimer, ICountdownTimer countTimer)
		{
			_pollTimer = pollTimer;
			_countdownTimer = countTimer;
		}

		void IConfigChangeHandler.ConfigUpdated(ConfigSettings newSettings)
		{
			lock (_pollTimer)
			{
				if (_pollTimer.Interval != newSettings.PollFrequencyTimeSpan)
				{
					_pollTimer.Stop();

					_pollTimer.Interval = newSettings.PollFrequencyTimeSpan;

					lock (_countdownTimer)
					{
						_countdownTimer.PollFrequency = newSettings.PollFrequencyTimeSpan;
						_countdownTimer.Reset();
					}

					_pollTimer.Start();
				}
			}
		}
	}
}
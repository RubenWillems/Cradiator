using System;
using System.Windows.Threading;
using Cradiator.Config;
using Cradiator.Views;

namespace Cradiator.Model
{
	public interface ICountdownTimer
	{
		void Start();
		void Stop();
		void SwitchOn();
		void SwitchOff();
		void Execute();
		DateTime Reset();
		bool IsSwitchedOn { get; }
		TimeSpan PollFrequency { set; }
	}

	public class CountdownTimer : ICountdownTimer
	{
		static readonly TimeSpan OneSecond = new TimeSpan(0, 0, 1);

		bool _isSwitchedOn;
		DateTime _nextRefresh;
		readonly ICradiatorView _view;
		readonly DispatcherTimer _countdownTimer;
		IDateTimeNow _date = new DateTimeNow();

		public TimeSpan PollFrequency { private get; set; }

		public IDateTimeNow Date
		{
			set
			{
				_date = value;
				Reset();
			}
		}

		public bool IsSwitchedOn
		{
			get { return _isSwitchedOn; }
		}

		public CountdownTimer(IConfigSettings configSettings, ICradiatorView view)
		{
			_view = view;
			_isSwitchedOn = configSettings.ShowCountdown;
			PollFrequency = configSettings.PollFrequencyTimeSpan;

			_countdownTimer = new DispatcherTimer { Interval = OneSecond };
			_countdownTimer.Tick += ((sender, e) => Execute());
			Reset();
		}

		public void Start()
		{
			if (IsSwitchedOn) _countdownTimer.Start();
		}

		public void Stop()
		{
			if (IsSwitchedOn) _countdownTimer.Stop();
		}

		public DateTime Reset()
		{
			return _nextRefresh = _date.Now + PollFrequency;
		}

		public DateTime CalculateNext()
		{
			return _nextRefresh = (_nextRefresh < _date.Now) ? Reset() : _nextRefresh;
		}

		public void Execute()
		{
			var remaining = CalculateTimeToGo();
			_view.UpdateCountdownTimer(remaining);
		}

		public TimeSpan CalculateTimeToGo()
		{
			return CalculateNext() - _date.Now;
		}

		public void SwitchOff()
		{
			_isSwitchedOn = false;
		}

		public void SwitchOn()
		{
			_isSwitchedOn = true;
		}
	}
}
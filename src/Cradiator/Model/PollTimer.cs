using System;
using System.Windows.Threading;
using Cradiator.Config;

namespace Cradiator.Model
{
	public interface IPollTimer
	{
		TimeSpan Interval { get; set; }
		void Stop();
		void Start();
		EventHandler Tick { set;}
	}

	/// <summary>
	/// A simple adapter (which wraps the DispatcherTimer class)
	/// </summary>
	public class PollTimer : IPollTimer
	{
		readonly DispatcherTimer _pollTimer;

		public PollTimer(IConfigSettings configSettings)
		{
			_pollTimer = new DispatcherTimer
			{
				Interval = configSettings.PollFrequencyTimeSpan
			};
		}

		public TimeSpan Interval
		{
			get { return _pollTimer.Interval; }
			set { _pollTimer.Interval = value; }
		}

		public void Stop()
		{
			_pollTimer.Stop();
		}

		public void Start()
		{
			_pollTimer.Start();
		}

		public EventHandler Tick
		{
			set { _pollTimer.Tick += value; }
		}
	}
}
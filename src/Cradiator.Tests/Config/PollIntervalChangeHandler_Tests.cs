using System;
using Cradiator.Config;
using Cradiator.Config.ChangeHandlers;
using Cradiator.Model;
using NUnit.Framework;
using Rhino.Mocks;
using Shouldly;

namespace Cradiator.Tests.Config
{
	[TestFixture]
	public class PollIntervalChangeHandler_Tests
	{
		IPollTimer _pollTimer;
		ICountdownTimer _countdownChangeHandler;
		IConfigChangeHandler _pollingChangeHandler;

		[SetUp]
		public void SetUp()
		{
			_pollTimer = Create.Mock<IPollTimer>();
            _countdownChangeHandler = Create.Mock<ICountdownTimer>();
			_pollingChangeHandler = new PollIntervalChangeHandler(_pollTimer, _countdownChangeHandler);
		}

		[Test]
		public void DoesNotUpdate_If_IntervalAlreadyEqual()
		{
			_pollTimer.Expect(p => p.Interval).Return(TimeSpan.FromSeconds(5));

			_pollingChangeHandler.ConfigUpdated(new ConfigSettings { PollFrequency = 5 });

			_pollTimer.AssertWasNotCalled(p=>p.Stop());
			_countdownChangeHandler.AssertWasNotCalled(c => c.Reset());
			_pollTimer.AssertWasNotCalled(p=>p.Start());
		}

		[Test]
		public void CanUpdate_If_IntervalNotEqual()
		{
			_pollTimer.Expect(p => p.Interval).Return(TimeSpan.FromSeconds(1));
			_pollTimer.Interval = TimeSpan.FromSeconds(2);
			_countdownChangeHandler.PollFrequency = TimeSpan.FromSeconds(2);

			_pollingChangeHandler.ConfigUpdated(new ConfigSettings { PollFrequency = 2 });

			_pollTimer.ShouldHaveBeenCalled(p => p.Stop());
            _countdownChangeHandler.ShouldHaveBeenCalled(c => c.Reset());
            _pollTimer.ShouldHaveBeenCalled(p => p.Start());

			_pollTimer.VerifyAllExpectations();
		}
	}
}
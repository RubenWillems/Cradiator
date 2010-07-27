using System;
using BigVisibleCruise2.Config;
using BigVisibleCruise2.Services;
using NUnit.Framework;
using Rhino.Mocks;

namespace BigVisibleCruise2.Tests.Config
{
	[TestFixture]
	public class PollingTimerUpdater_Tests
	{
		IPollingTimer _pollingTimer;
		ICountdownResetTimer _countdownResetTimer;
		IConfigObserver _pollingUpdaterResponder;

		[SetUp]
		public void SetUp()
		{
			_pollingTimer = MockRepository.GenerateMock<IPollingTimer>();
			_countdownResetTimer = MockRepository.GenerateMock<ICountdownResetTimer>();
			_pollingUpdaterResponder = new PollingUpdateResponder(_pollingTimer, _countdownResetTimer);
		}

		[Test]
		public void DoesNotUpdate_If_IntervalAlreadyEqual()
		{
			_pollingTimer.Expect(p => p.Interval).Return(TimeSpan.FromSeconds(5));

			_pollingUpdaterResponder.ConfigUpdated(new ConfigSettings { PollFrequency = 5 });

			_pollingTimer.AssertWasNotCalled(p=>p.Stop());
			_countdownResetTimer.AssertWasNotCalled(c => c.Reset());
			_pollingTimer.AssertWasNotCalled(p=>p.Start());
		}

		[Test]
		public void CanUpdate_If_IntervalNotEqual()
		{
			_pollingTimer.Expect(p => p.Interval).Return(TimeSpan.FromSeconds(1));
			_pollingTimer.Interval = TimeSpan.FromSeconds(2);
			_countdownResetTimer.PollFrequency = TimeSpan.FromSeconds(2);

			_pollingUpdaterResponder.ConfigUpdated(new ConfigSettings { PollFrequency = 2 });

			_pollingTimer.AssertWasCalled(p => p.Stop());
			_countdownResetTimer.AssertWasCalled(c => c.Reset());
			_pollingTimer.AssertWasCalled(p => p.Start());

			_pollingTimer.VerifyAllExpectations();
		}
	}
}
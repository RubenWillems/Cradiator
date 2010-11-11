using System;
using BigVisibleCruise2.Config;
using BigVisibleCruise2.Services;
using BigVisibleCruise2.Views;
using NUnit.Framework;
using Rhino.Mocks;

namespace BigVisibleCruise2.Tests
{
	[TestFixture]
	public class CountdownTimerUpdater_Tests
	{
		IBigVisibleCruiseView _view;
		ICountdownTimer _timer;
		IConfigObserver _updateResponder;

		[SetUp]
		public void SetUp()
		{
			_view = MockRepository.GenerateMock<IBigVisibleCruiseView>();
			_timer = MockRepository.GenerateMock<ICountdownTimer>();
			_updateResponder = new ShowCountdownUpdateResponder(_timer, _view);
		}

		[Test]
		public void DoesNothing_If_ShowCountdown_IsFalse_And_IsSwitchedOff()
		{
			_updateResponder.ConfigUpdated(new ConfigSettings { ShowCountdown = false });

			_timer.AssertWasNotCalled(t => t.SwitchOff());
			_timer.AssertWasNotCalled(t => t.SwitchOn());
			_view.AssertWasNotCalled(v => v.Invoke(Arg<Action>.Is.Anything));
		}

		[Test]
		public void IsSwitchedOn_If_ShowCountdownTrue_And_WasSwitchedOff()
		{
			_timer.Expect(t => t.IsSwitchedOn).Return(false);

			_updateResponder.ConfigUpdated(new ConfigSettings { ShowCountdown = true });

			_timer.AssertWasCalled(t => t.SwitchOn());
			_view.AssertWasCalled(v => v.Invoke(Arg<Action>.Is.Anything));
		}

		[Test]
		public void IsSwitchedOff_If_ShowCountdownFalse_And_WasSwitchedOn()
		{
			_timer.Expect(t => t.IsSwitchedOn).Return(true);

			_updateResponder.ConfigUpdated(new ConfigSettings { ShowCountdown = false });

			_timer.AssertWasCalled(t => t.SwitchOff());
			_view.AssertWasCalled(v => v.Invoke(Arg<Action>.Is.Anything));
		}
	}
}
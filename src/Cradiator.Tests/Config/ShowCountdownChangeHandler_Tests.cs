using System;
using Cradiator.Config;
using Cradiator.Config.ChangeHandlers;
using Cradiator.Model;
using Cradiator.Views;
using NUnit.Framework;
using Rhino.Mocks;
using Shouldly;

namespace Cradiator.Tests.Config
{
	[TestFixture]
	public class ShowCountdownChangeHandler_Tests
	{
		ICradiatorView _view;
		ICountdownTimer _timer;
		IConfigChangeHandler _changeHandler;

		[SetUp]
		public void SetUp()
		{
            _view = Create.Mock<ICradiatorView>();
            _timer = Create.Mock<ICountdownTimer>();
			_changeHandler = new ShowCountdownChangeHandler(_timer, _view);
		}

		[Test]
		public void DoesNothing_If_ShowCountdown_IsFalse_And_IsSwitchedOff()
		{
			_changeHandler.ConfigUpdated(new ConfigSettings { ShowCountdown = false });

			_timer.AssertWasNotCalled(t => t.SwitchOff());
			_timer.AssertWasNotCalled(t => t.SwitchOn());
			_view.AssertWasNotCalled(v => v.Invoke(Arg<Action>.Is.Anything));
		}

		[Test]
		public void IsSwitchedOn_If_ShowCountdownTrue_And_WasSwitchedOff()
		{
			_timer.Expect(t => t.IsSwitchedOn).Return(false);

			_changeHandler.ConfigUpdated(new ConfigSettings { ShowCountdown = true });

			_timer.ShouldHaveBeenCalled(t => t.SwitchOn());
            _view.ShouldHaveBeenCalled(v => v.Invoke(Arg<Action>.Is.Anything));
		}

		[Test]
		public void IsSwitchedOff_If_ShowCountdownFalse_And_WasSwitchedOn()
		{
			_timer.Expect(t => t.IsSwitchedOn).Return(true);

			_changeHandler.ConfigUpdated(new ConfigSettings { ShowCountdown = false });

            _timer.ShouldHaveBeenCalled(t => t.SwitchOff());
            _view.ShouldHaveBeenCalled(v => v.Invoke(Arg<Action>.Is.Anything));
		}
	}
}
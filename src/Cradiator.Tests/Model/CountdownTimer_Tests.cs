using System;
using Cradiator.Config;
using Cradiator.Model;
using Cradiator.Views;
using NUnit.Framework;
using Rhino.Mocks;

namespace Cradiator.Tests.Model
{
	[TestFixture]
	public class CountdownTimer_Tests
	{
		ICradiatorView _view;
		IDateTimeNow _date;
		TimeSpan _pollFrequency;
		CountdownTimer _timer;
		static readonly TimeSpan TenSeconds = new TimeSpan(0,0,10);

		[SetUp]
		public void SetUp()
		{
			_view = MockRepository.GenerateMock<ICradiatorView>();
			_date = MockRepository.GenerateStub<IDateTimeNow>();

			_pollFrequency = TenSeconds;

			_timer = new CountdownTimer(new ConfigSettings { PollFrequency = 10 }, _view)
			{
				Date = _date
			};
		}

		[Test]
		public void CanReset_NextRefresh()
		{
			_date.Now = new DateTime(2009, 2, 21, 1, 0, 0);
			Assert.That(_timer.Reset(), Is.EqualTo(_date.Now + _pollFrequency));
		}

		[Test]
		public void CanReset_CalculateNextRefresh()
		{
			_date.Now = new DateTime(2009, 2, 21, 1, 0, 0);
			Assert.That(_timer.CalculateNext(), Is.EqualTo(_date.Now + _pollFrequency));
		}

		[Test]
		public void CanReset_CalculateNextRefresh_WhenNow_HasPast()
		{
			_date.Now = new DateTime(1, 1, 1, 0, 0, 0);
			Assert.That(_timer.CalculateNext(), Is.EqualTo(_date.Now + _pollFrequency));
		}

		[Test]
		public void CanCalculate_TimeToGo()
		{
			_date.Now = new DateTime(2009, 2, 21, 1, 0, 0);
			Assert.That(_timer.CalculateTimeToGo(), Is.EqualTo(TenSeconds));
		}

		[Test]
		public void CanCalculate_TimeToGo_001()
		{
			_date.Now = new DateTime(2009, 2, 21, 1, 0, 0, 001);
			Assert.That(_timer.CalculateTimeToGo(), Is.EqualTo(TenSeconds));
		}

		[Test]
		public void CanCalculate_TimeToGo_999()
		{
			_date.Now = new DateTime(2009, 2, 21, 1, 0, 0, 999);
			Assert.That(_timer.CalculateTimeToGo(), Is.EqualTo(TenSeconds));
		}

		[Test]
		public void CanCalculate_TimeToGo_501()
		{
			_date.Now = new DateTime(2009, 2, 21, 1, 0, 0, 501);
			Assert.That(_timer.CalculateTimeToGo(), Is.EqualTo(TenSeconds));
		}

		[Test]
		public void CanCalculate_TimeToGo_500()
		{
			_date.Now = new DateTime(2009, 2, 21, 1, 0, 0, 500);
			Assert.That(_timer.CalculateTimeToGo(), Is.EqualTo(TenSeconds));
		}

		[Test]
		public void CanCalculate_AfterPollFrequency_Updated()
		{
			_date.Now = new DateTime(2009, 2, 21, 1, 0, 0);
			Assert.That(_timer.CalculateNext(), Is.EqualTo(_date.Now + _pollFrequency));

			var newPollFrequency = new TimeSpan(0, 0, 15);

			_timer.PollFrequency = newPollFrequency;
			_timer.Reset();

			Assert.That(_timer.CalculateNext(), Is.EqualTo(_date.Now + newPollFrequency));
		}
	}
}
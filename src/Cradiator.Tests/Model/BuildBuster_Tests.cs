using Cradiator.Config;
using Cradiator.Model;
using NUnit.Framework;
using Rhino.Mocks;

namespace Cradiator.Tests.Model
{
	[TestFixture]
	public class BuildBuster_Tests
	{
		BuildBuster _buildBuster;
		IConfigSettings _configSettings;
		GuiltFactory _guiltFactory;
		readonly FixerStrategy _fixterStrategy = new FixerStrategy();

		[SetUp]
		public void SetUp()
		{
			_configSettings = MockRepository.GenerateStub<IConfigSettings>();
			_guiltFactory = new GuiltFactory();
		}

		private void SetUpBreaker(GuiltStrategyType guiltStrategy)
		{
			_configSettings.Stub(s => s.BreakerGuiltStrategy).Return(guiltStrategy);
			_buildBuster = new BuildBuster(_configSettings, _fixterStrategy, _guiltFactory);
		}

		[Test]
		public void CanBust_LastBreaker_With_1()
		{
			SetUpBreaker(GuiltStrategyType.Last);

			Assert.That(_buildBuster.FindBreaker("Breakers: bob"), Is.EqualTo("bob"));
		}

		[Test]
		public void CanBust_LastBreaker_With_2()
		{
			SetUpBreaker(GuiltStrategyType.Last);

			Assert.That(_buildBuster.FindBreaker("Breakers: bob, mary"), Is.EqualTo("mary"));
		}

		[Test]
		public void CanBust_LastBreaker_With_3()
		{
			SetUpBreaker(GuiltStrategyType.Last);

			Assert.That(_buildBuster.FindBreaker("Breakers: bob, mary, joseph"), Is.EqualTo("joseph"));
		}

		[Test]
		public void CanBust_LastBreaker_EvenIf_ExpectedStartWord_Changes()
		{
			SetUpBreaker(GuiltStrategyType.Last);

			Assert.That(_buildBuster.FindBreaker("Bombers: bob, mary"), Is.EqualTo("mary"));
		}

		[Test]
		public void CanBust_Fixer_If_BeingFixed_Despite_LastStrategy()
		{
			SetUpBreaker(GuiltStrategyType.Last);

			Assert.That(_buildBuster.FindBreaker("john is fixing the build"), Is.EqualTo("john"));
		}

		[Test]
		public void CanBust_Fixer_If_BeingFixed_Despite_FirstStrategy()
		{
			SetUpBreaker(GuiltStrategyType.First);

			Assert.That(_buildBuster.FindBreaker("john is fixing the build"), Is.EqualTo("john"));
		}

		[Test]
		public void CanBust_Nobody_If_EmptyString()
		{
			Assert.That(_buildBuster.FindBreaker(""), Is.EqualTo(string.Empty));
		}

		[Test]
		public void CanBust_Nobody_If_Null()
		{
			Assert.That(_buildBuster.FindBreaker(null), Is.EqualTo(string.Empty));
		}

		[Test]
		public void CanBust_FirstBreaker_With_1()
		{
			SetUpBreaker(GuiltStrategyType.First);

			Assert.That(_buildBuster.FindBreaker("Breakers: bob"), Is.EqualTo("bob"));
		}

		[Test]
		public void CanBust_FirstBreaker_With_2()
		{
			SetUpBreaker(GuiltStrategyType.First);

			Assert.That(_buildBuster.FindBreaker("Breakers: bob, mary"), Is.EqualTo("bob"));
		}

		[Test]
		public void CanBust_FirstBreaker_With_3()
		{
			SetUpBreaker(GuiltStrategyType.First);

			Assert.That(_buildBuster.FindBreaker("Breakers: zombie, freak, smurf"), Is.EqualTo("zombie"));
		}

        [Test]
        public void CanBust_FirstBreaker_With_3_NewMessageFormat()
        {
            SetUpBreaker(GuiltStrategyType.First);

            Assert.That(_buildBuster.FindBreaker("zombie, freak, smurf"), Is.EqualTo("zombie"));
        }

        [Test]
        public void CanBust_LastBreaker_With_3_NewMessageFormat()
        {
            SetUpBreaker(GuiltStrategyType.Last);

            Assert.That(_buildBuster.FindBreaker("zombie, freak, smurf"), Is.EqualTo("smurf"));
        }
	}
}
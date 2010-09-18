using Cradiator.Config;
using Cradiator.Model;
using NUnit.Framework;
using Rhino.Mocks;
using Shouldly;

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
			_configSettings = Create.Stub<IConfigSettings>();
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

			_buildBuster.FindBreaker("Breakers: bob").ShouldBe("bob");
		}

		[Test]
		public void CanBust_LastBreaker_With_2()
		{
			SetUpBreaker(GuiltStrategyType.Last);

            _buildBuster.FindBreaker("Breakers: bob, mary").ShouldBe("mary");
		}

		[Test]
		public void CanBust_LastBreaker_With_3()
		{
			SetUpBreaker(GuiltStrategyType.Last);

			_buildBuster.FindBreaker("Breakers: bob, mary, joseph").ShouldBe("joseph");
		}

		[Test]
		public void CanBust_LastBreaker_EvenIf_ExpectedStartWord_Changes()
		{
			SetUpBreaker(GuiltStrategyType.Last);

			_buildBuster.FindBreaker("Bombers: bob, mary").ShouldBe("mary");
		}

		[Test]
		public void CanBust_Fixer_If_BeingFixed_Despite_LastStrategy()
		{
			SetUpBreaker(GuiltStrategyType.Last);

			_buildBuster.FindBreaker("john is fixing the build").ShouldBe("john");
		}

		[Test]
		public void CanBust_Fixer_If_BeingFixed_Despite_FirstStrategy()
		{
			SetUpBreaker(GuiltStrategyType.First);

			_buildBuster.FindBreaker("john is fixing the build").ShouldBe("john");
		}

		[Test]
		public void CanBust_Nobody_If_EmptyString()
		{
			_buildBuster.FindBreaker("").ShouldBe(string.Empty);
		}

		[Test]
		public void CanBust_Nobody_If_Null()
		{
			_buildBuster.FindBreaker(null).ShouldBe(string.Empty);
		}

		[Test]
		public void CanBust_FirstBreaker_With_1()
		{
			SetUpBreaker(GuiltStrategyType.First);

			_buildBuster.FindBreaker("Breakers: bob").ShouldBe("bob");
		}

		[Test]
		public void CanBust_FirstBreaker_With_2()
		{
			SetUpBreaker(GuiltStrategyType.First);

			_buildBuster.FindBreaker("Breakers: bob, mary").ShouldBe("bob");
		}

		[Test]
		public void CanBust_FirstBreaker_With_3()
		{
			SetUpBreaker(GuiltStrategyType.First);

			_buildBuster.FindBreaker("Breakers: zombie, freak, smurf").ShouldBe("zombie");
		}

        [Test]
        public void CanBust_FirstBreaker_With_3_NewMessageFormat()
        {
            SetUpBreaker(GuiltStrategyType.First);

            _buildBuster.FindBreaker("zombie, freak, smurf").ShouldBe("zombie");
        }

        [Test]
        public void CanBust_LastBreaker_With_3_NewMessageFormat()
        {
            SetUpBreaker(GuiltStrategyType.Last);

            _buildBuster.FindBreaker("zombie, freak, smurf").ShouldBe("smurf");
        }
	}
}
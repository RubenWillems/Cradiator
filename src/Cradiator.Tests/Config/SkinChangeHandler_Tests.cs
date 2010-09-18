using Cradiator.Config;
using Cradiator.Config.ChangeHandlers;
using Cradiator.Model;
using NUnit.Framework;
using Rhino.Mocks;
using Shouldly;

namespace Cradiator.Tests.Config
{
	[TestFixture]
	public class SkinChangeHandler_Tests
	{
		IConfigChangeHandler _skinChangeHandler;
		ISkinLoader _skinLoader;

		[SetUp]
		public void SetUp()
		{
            _skinLoader = Create.Mock<ISkinLoader>();
			_skinChangeHandler = new SkinChangeHandler(_skinLoader);
		}

		[Test]
		public void CanUpdateSkin_FirstTime_WhenCurrentSkinIsNull()
		{
			_skinChangeHandler.ConfigUpdated(new ConfigSettings { SkinName = "newSkin"});

			_skinLoader.ShouldHaveBeenCalled(l => l.Load(Arg<Skin>.Matches(s => s.Name == "newSkin")));
		}

		[Test]
		public void CanUpdateSkin_SecondTime_WhenCurrentSkinIsNotNull_2times()
		{
			_skinChangeHandler.ConfigUpdated(new ConfigSettings { SkinName = "newSkin" });
			_skinChangeHandler.ConfigUpdated(new ConfigSettings { SkinName = "newSkin" });

            _skinLoader.AssertWasCalled(sl => sl.Load(Arg<Skin>.Matches(s => s.Name == "newSkin")), x => x.Repeat.Once());
//            _skinLoader.ShouldHaveBeenCalled(sl => sl.Load(Arg<Skin>.Matches(s => s.Name == "newSkin")), x => x.Repeat.Once());
		}

		[Test]
		public void CanUpdateSkin_SecondTime_WhenCurrentSkinIsNotNull_3times()
		{
			_skinChangeHandler.ConfigUpdated(new ConfigSettings { SkinName = "newSkin" });
			_skinChangeHandler.ConfigUpdated(new ConfigSettings { SkinName = "newSkin" });
			_skinChangeHandler.ConfigUpdated(new ConfigSettings { SkinName = "newSkin" });

            _skinLoader.AssertWasCalled(sl => sl.Load(Arg<Skin>.Matches(s => s.Name == "newSkin")), x => x.Repeat.Once());
		}

		[Test]
		public void CanUpdateSkin_Whenever_NewSkin_IsDifferent()
		{
			_skinChangeHandler.ConfigUpdated(new ConfigSettings { SkinName = "newSkin"});

			_skinChangeHandler.ConfigUpdated(new ConfigSettings { SkinName = "newerSkin"});
            _skinLoader.ShouldHaveBeenCalled(sl => sl.Load(Arg<Skin>.Matches(s => s.Name == "newerSkin")));

			_skinChangeHandler.ConfigUpdated(new ConfigSettings { SkinName = "yetNewerSkin"});
            _skinLoader.ShouldHaveBeenCalled(sl => sl.Load(Arg<Skin>.Matches(s => s.Name == "yetNewerSkin")));
		}
	}
}
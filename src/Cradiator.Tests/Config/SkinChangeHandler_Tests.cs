using Cradiator.Config;
using Cradiator.Config.ChangeHandlers;
using Cradiator.Model;
using Cradiator.Views;
using NUnit.Framework;
using Rhino.Mocks;

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
			MockRepository.GenerateMock<ICradiatorView>();
			MockRepository.GenerateMock<IConfigSettings>();
			_skinLoader = MockRepository.GenerateMock<ISkinLoader>();
			_skinChangeHandler = new SkinChangeHandler(_skinLoader);
		}

		[Test]
		public void CanUpdateSkin_FirstTime_WhenCurrentSkinIsNull()
		{
			_skinChangeHandler.ConfigUpdated(new ConfigSettings { SkinName = "newSkin"});

			_skinLoader.AssertWasCalled(l => l.Load(Arg<Skin>.Matches(s => s.Name == "newSkin")));
		}

		[Test]
		public void CanUpdateSkin_SecondTime_WhenCurrentSkinIsNotNull_2times()
		{
			_skinChangeHandler.ConfigUpdated(new ConfigSettings { SkinName = "newSkin" });
			_skinChangeHandler.ConfigUpdated(new ConfigSettings { SkinName = "newSkin" });
			_skinLoader.AssertWasCalled(sl => sl.Load(Arg<Skin>.Matches(s => s.Name == "newSkin")), x => x.Repeat.Once());
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
			_skinLoader.AssertWasCalled(sl => sl.Load(Arg<Skin>.Matches(s => s.Name == "newerSkin")));

			_skinChangeHandler.ConfigUpdated(new ConfigSettings { SkinName = "yetNewerSkin"});
			_skinLoader.AssertWasCalled(sl => sl.Load(Arg<Skin>.Matches(s => s.Name == "yetNewerSkin")));
		}
	}
}
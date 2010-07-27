using System;
using BigVisibleCruise2.Config;
using BigVisibleCruise2.Views;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace BigVisibleCruise2.Tests.Config
{
	[TestFixture]
	public class SkinUpdater_Tests
	{
		IBigVisibleCruiseView _view;
		IConfigObserver _skinUpdateResponder;
		const string NewSkin = "newSkin";

		[SetUp]
		public void SetUp()
		{
			_view = MockRepository.GenerateMock<IBigVisibleCruiseView>();

			_skinUpdateResponder = new SkinUpdateResponder(_view);
			_skinUpdateResponder.ConfigUpdated(new ConfigSettings { SkinName = NewSkin });
		}

		[Test]
		public void CanUpdateSkin_FirstTime_WhenCurrentSkinIsNull()
		{
			_view.AssertWasCalled(v=>v.Invoke(Arg<Action>.Is.Anything));
		}

		[Test]
		public void CanUpdateSkin_SecondTime_WhenCurrentSkinIsNotNull()
		{
			const string MoreNewerSkin = "moreNewerSkin";

			// make 2nd call
			_skinUpdateResponder.ConfigUpdated(new ConfigSettings { SkinName = MoreNewerSkin });
		}
	}
}
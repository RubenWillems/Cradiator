using Cradiator.Model;
using NUnit.Framework;
using Rhino.Mocks;

namespace Cradiator.Tests.Model
{
	[TestFixture]
	public class ScreenUpdater_Tests
	{
		IPollTimer _pollTimer;

		[SetUp]
		public void SetUp()
		{
			_pollTimer = MockRepository.GenerateMock<IPollTimer>();
		}

//		[Test]
//		public void CanCallScreenUpdater()
//		{
//			var screenUpdater = new ScreenUpdater(null, null, null, _pollTimer, null, null, null, null);
//			//TODO
//		}
	}
}
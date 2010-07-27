using System;
using System.Windows.Data;
using Cradiator.Converters;
using Cradiator.Model;
using NUnit.Framework;
using Rhino.Mocks;

namespace Cradiator.Tests.Converters
{
	[TestFixture]
	public class OneBreakerConverter_Tests : ConverterTestBase
	{
		IBuildBuster _buildBuster;

		protected override IValueConverter CreateConverter()
		{
			_buildBuster = MockRepository.GenerateMock<IBuildBuster>();
			return new OneBreakerConverter(_buildBuster);
		}

		[Test]
		public void CanConvert_If_CurrentMessage_Has_1_Breaker()
		{
			// NB we only test what is the responsibility of the OneBreakerConverter 
			// which is just formatting the results of IBuildBuster (which is already tested, DRY)
			_buildBuster.Stub(b => b.FindBreaker(Arg<string>.Is.Anything)).Return("bob");
			Assert.That(DoConvert("irrelevant"), Is.EqualTo("(bob)"));
		}

		[Test]
		public void CanConvert_If_CurrentMessage_IsEmptyString()
		{
			_buildBuster.Stub(b => b.FindBreaker(Arg<string>.Is.Anything)).Return("");
			Assert.That(DoConvert("irrelevant"), Is.EqualTo(""));
		}

		[Test]
		public void CanConvert_If_CurrentMessage_IsNull()
		{
			_buildBuster.Stub(b => b.FindBreaker(Arg<string>.Is.Anything)).Return(null);
			Assert.That(DoConvert("irrelevant"), Is.EqualTo(""));
		}

		[Test]
		public void CanConvertBackIsNotImplemented()
		{
			var converter = new OneBreakerConverter(_buildBuster);
			
			Assert.Throws<NotImplementedException>(() => 
				converter.ConvertBack(null, null, null, null)
			);
		}
	}
}
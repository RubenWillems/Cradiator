using System;
using System.Windows.Data;
using Cradiator.Converters;
using Cradiator.Model;
using NUnit.Framework;
using Rhino.Mocks;

namespace Cradiator.Tests.Converters
{
	[TestFixture]
	public class ImagePathConverter_Tests : ConverterTestBase
	{
		IBuildBuster _buildBuster;

		protected override IValueConverter CreateConverter()
		{
			_buildBuster = MockRepository.GenerateMock<IBuildBuster>();
			return new ImagePathConverter(_buildBuster);
		}

		[Test]
		public void CanCall_BuildBuster_IfValue_IsNotNullOrEmpty()
		{
			const string currentMessage = "Breakers: Bob, Mary";

			DoConvert(currentMessage);

			// this is all we want to know - the fact that the BuildBuster "is called" (with the correct arguments)
			// (the BuildBuster is tested elsewhere - ie DRY)
			_buildBuster.AssertWasCalled(b => b.FindBreaker(currentMessage));
		}

		[Test]
		public void CanConvertBackIsNotImplemented()
		{
			Assert.Throws<NotImplementedException>(
				() => new ImagePathConverter(_buildBuster).ConvertBack(null, null, null, null)
			);
		}
	}
}
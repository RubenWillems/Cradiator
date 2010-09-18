using Cradiator.Converters;
using NUnit.Framework;
using Shouldly;

namespace Cradiator.Tests.Converters
{
	[TestFixture]
	public class InstalledVoiceConverter_Tests
	{
        const string dummy = "dummy";

		InstalledVoiceConverter _converter;

		[SetUp]
		public void SetUp()
		{
			_converter = new InstalledVoiceConverter();
		}

		[Test]
		public void CanConvertBack()
		{
			_converter.ConvertBack(dummy, null, null, null).ShouldBe(dummy);
		}
	}
}
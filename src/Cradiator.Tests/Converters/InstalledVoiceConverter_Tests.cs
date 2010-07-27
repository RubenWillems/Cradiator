using Cradiator.Converters;
using NUnit.Framework;

namespace Cradiator.Tests.Converters
{
	[TestFixture]
	public class InstalledVoiceConverter_Tests
	{
		InstalledVoiceConverter _converter;

		[SetUp]
		public void SetUp()
		{
			_converter = new InstalledVoiceConverter();
		}

		[Test]
		public void CanConvertBack()
		{
			const string dummy = "dummy";
			Assert.That(_converter.ConvertBack(dummy, null, null, null), Is.EqualTo(dummy));
		}
	}
}
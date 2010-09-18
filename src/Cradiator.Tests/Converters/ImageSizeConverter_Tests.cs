using System;
using Cradiator.Converters;
using NUnit.Framework;
using Shouldly;

namespace Cradiator.Tests.Converters
{
	[TestFixture]
	public class ImageSizeConverter_Tests
	{
		private ImageSizeConverter _converter;

		[SetUp]
		public void SetUp()
		{
			_converter = new ImageSizeConverter();
		}

		private object DoConvert(string text)
		{
			return _converter.Convert(text, typeof(int), null, null);
		}

		[Test]
		public void CanConvert_If_ValueIs_EmptyString()
		{
			DoConvert("").ShouldBe(0);
		}

		[Test]
		public void CanConvert_If_ValueIsNot_EmptyString()
		{
			Assert.That(DoConvert("Breakers: Bob"), Is.GreaterThanOrEqualTo(10));
		}

		[Test]
		public void ConvertBackIsntImplemented()
		{
			Assert.Throws<NotImplementedException>(() => 
				_converter.ConvertBack(null, typeof(int), null, null)
			);
		}
	}
}
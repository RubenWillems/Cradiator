using System;
using System.Windows.Data;
using Cradiator.Converters;
using NUnit.Framework;
using Shouldly;

namespace Cradiator.Tests.Converters
{
	[TestFixture]
	public class BuildNameAndMessageConverter_Test
	{
		private readonly IMultiValueConverter _converter = new BuildNameToMessageConverter();

		private object DoConvert(object[] obj)
		{
			return _converter.Convert(obj, typeof(string), null, null);
		}

		[Test]
		public void BuildBrokenOK_JohnDoeBrokeIt()
		{
			DoConvert(new object[] {"foo_bar_yo", "Breakers : Johndoe"})
				   .ShouldBe("foo bar yo\nBreakers : Johndoe");
		}

		[Test]
		public void BuildBrokenOK_JohnDoeIsFixingIt()
		{
			DoConvert(new object[] {"foo_bar_yo", "Johndoe is fixing the build"})
                .ShouldBe("foo bar yo\nJohndoe is fixing the build");
		}

		[Test]
		public void BuildOK_name_with_underscores_has_them_removed()
		{
			DoConvert(new object[] {"foo_bar_yo", ""})
                .ShouldBe("foo bar yo");
		}

		[Test]
		public void TestName()
		{
			Assert.Throws<NotImplementedException>(() => 
				_converter.ConvertBack("ThomasTheTankEngine", null, null, null)
			);
		}
	}
}
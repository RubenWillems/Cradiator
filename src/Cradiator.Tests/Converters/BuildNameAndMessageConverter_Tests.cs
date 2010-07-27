using System;
using System.Windows.Data;
using Cradiator.Converters;
using NUnit.Framework;

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
			Assert.That(DoConvert(
				new object[] {"foo_bar_yo", "Breakers : Johndoe"}), 
				   Is.EqualTo("foo bar yo\nBreakers : Johndoe"));
		}

		[Test]
		public void BuildBrokenOK_JohnDoeIsFixingIt()
		{
			Assert.That(DoConvert(new object[] {"foo_bar_yo", "Johndoe is fixing the build"}), 
									 Is.EqualTo("foo bar yo\nJohndoe is fixing the build"));
		}

		[Test]
		public void BuildOK_name_with_underscores_has_them_removed()
		{
			Assert.That(DoConvert(new object[] {"foo_bar_yo", ""}), 
									 Is.EqualTo("foo bar yo"));
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
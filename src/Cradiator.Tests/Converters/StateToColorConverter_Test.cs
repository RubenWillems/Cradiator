using System.Windows.Data;
using System.Windows.Media;
using Cradiator.Converters;
using Cradiator.Model;
using NUnit.Framework;

namespace Cradiator.Tests.Converters
{
	[TestFixture]
	public class StateToColorConverter_Test
	{
		private readonly IValueConverter _colorConverter;
		private readonly IValueConverter _gradientConverter;

		public StateToColorConverter_Test()
		{
			_colorConverter = new StateToColorConverter();
			_gradientConverter = new StateToGradientConverter();
		}

		[Test]
		public void build_status_of_building_is_yellow()
		{
			var convertedValue = _colorConverter.Convert(ProjectStatus.BUILDING, null, null, null);
			var gradientValue = _gradientConverter.Convert(ProjectStatus.BUILDING, null, null, null);

			Assert.That(convertedValue, Is.EqualTo(Colors.Yellow));
			Assert.That(gradientValue, Is.EqualTo(Color.FromArgb(255, 255, 255, 200)));
		}

		[Test]
		public void build_status_of_exception_is_red()
		{
			var convertedValue = _colorConverter.Convert(ProjectStatus.FAILURE, null, null, null);
			var gradientValue = _gradientConverter.Convert(ProjectStatus.FAILURE, null, null, null);

			Assert.That(convertedValue, Is.EqualTo(Colors.Red));
			Assert.That(gradientValue, Is.EqualTo(Color.FromArgb(255, 255, 150, 150)));
		}

		[Test]
		public void build_status_of_failure_is_red()
		{
			var convertedValue = _colorConverter.Convert(ProjectStatus.EXCEPTION, null, null, null);
			var gradientValue = _gradientConverter.Convert(ProjectStatus.EXCEPTION, null, null, null);

			Assert.That(convertedValue, Is.EqualTo(Colors.Red));
			Assert.That(gradientValue, Is.EqualTo(Color.FromArgb(255, 255, 150, 150)));
		}

		[Test]
		public void build_status_of_success_is_green()
		{
			var convertedValue = _colorConverter.Convert(ProjectStatus.SUCCESS, null, null, null);
			var gradientValue = _gradientConverter.Convert(ProjectStatus.SUCCESS, null, null, null);

			Assert.That(convertedValue, Is.EqualTo(Colors.LimeGreen));
			Assert.That(gradientValue, Is.EqualTo(Colors.LightGreen));
		}

		[Test]
		public void build_status_of_unknown_is_white()
		{
			var convertedValue = _colorConverter.Convert("Unknown", null, null, null);
			var gradientValue = _gradientConverter.Convert("Unknown", null, null, null);

			Assert.That(convertedValue, Is.EqualTo(Colors.White));
			Assert.That(gradientValue, Is.EqualTo(Colors.White));
		}

		[Test]
		public void convertback_returns_null()
		{
			Assert.That(_colorConverter.ConvertBack("ThomasTheTankEngine", null, null, null), Is.Null);
			Assert.That(_gradientConverter.ConvertBack("ThomasTheTankEngine", null, null, null), Is.Null);
		}

		[Test]
		public void any_other_color_that_we_havent_considered_will_be_white()
		{
			var convertedValue = _colorConverter.Convert("ThomasTheTankEngine", null, null, null);
			var gradientValue = _gradientConverter.Convert("Rumplestiltskin", null, null, null);

			Assert.That(convertedValue, Is.EqualTo(Colors.White));
			Assert.That(gradientValue, Is.EqualTo(Colors.White));
		}
	}
}
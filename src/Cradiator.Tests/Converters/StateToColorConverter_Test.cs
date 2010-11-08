using System.Windows.Data;
using System.Windows.Media;
using Cradiator.Converters;
using Cradiator.Model;
using NUnit.Framework;
using Shouldly;

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
			var color = _colorConverter.Convert(ProjectStatus.BUILDING, null, null, null);
			var gradient = _gradientConverter.Convert(ProjectStatus.BUILDING, null, null, null);

			color.ShouldBe(Colors.Yellow);
			gradient.ShouldBe(Color.FromArgb(255, 255, 255, 200));
		}

		[Test]
		public void build_status_of_exception_is_red()
		{
			var color = _colorConverter.Convert(ProjectStatus.FAILURE, null, null, null);
			var gradient = _gradientConverter.Convert(ProjectStatus.FAILURE, null, null, null);

			color.ShouldBe(Colors.Red);
			gradient.ShouldBe(Color.FromArgb(255, 255, 150, 150));
		}

		[Test]
		public void build_status_of_failure_is_red()
		{
			var color = _colorConverter.Convert(ProjectStatus.EXCEPTION, null, null, null);
			var gradient = _gradientConverter.Convert(ProjectStatus.EXCEPTION, null, null, null);

			color.ShouldBe(Colors.Red);
			gradient.ShouldBe(Color.FromArgb(255, 255, 150, 150));
		}

		[Test]
		public void build_status_of_success_is_green()
		{
			var color = _colorConverter.Convert(ProjectStatus.SUCCESS, null, null, null);
			var gradient = _gradientConverter.Convert(ProjectStatus.SUCCESS, null, null, null);

			color.ShouldBe(Colors.LimeGreen);
			gradient.ShouldBe(Colors.LightGreen);
		}

		[Test]
		public void build_status_of_unknown_is_white()
		{
			var color = _colorConverter.Convert("Unknown", null, null, null);
			var gradient = _gradientConverter.Convert("Unknown", null, null, null);

			color.ShouldBe(Colors.White);
			gradient.ShouldBe(Colors.White);
		}

		[Test]
		public void convertback_returns_null()
		{
			_colorConverter.ConvertBack("ThomasTheTankEngine", null, null, null).ShouldBe(null);
			_gradientConverter.ConvertBack("ThomasTheTankEngine", null, null, null).ShouldBe(null);
		}

		[Test]
		public void any_other_color_that_we_havent_considered_will_be_white()
		{
			var color = _colorConverter.Convert("ThomasTheTankEngine", null, null, null);
			var gradient = _gradientConverter.Convert("Rumplestiltskin", null, null, null);

			color.ShouldBe(Colors.White);
			gradient.ShouldBe(Colors.White);
		}
	}
}
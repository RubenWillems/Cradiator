using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Cradiator.Model;
using Cradiator.Services;

namespace Cradiator.Converters
{
	[MarkupExtensionReturnType(typeof(StateToColorConverter))]
	[ValueConversion(typeof(string), typeof(Color))]
	public class StateToColorConverter : MarkupExtension, IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch (value.ToString().ToLower())
			{
				case ProjectStatus.SUCCESS:
                case ProjectStatus.NORMAL:
					return Colors.LimeGreen;

				case ProjectStatus.BUILDING:
					return Colors.Yellow;

				case ProjectStatus.FAILURE:
				case ProjectStatus.EXCEPTION:
                case ProjectStatus.ERROR:
					return Colors.Red;

				default:
					return Colors.White;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return Ninjector.Get<StateToColorConverter>();
		}
	}
}

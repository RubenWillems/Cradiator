using System;
using System.Globalization;
using System.Windows.Data;

namespace BigVisibleCruise2.Converters
{
	public class BreakerMessageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var message = value as string;
			return string.IsNullOrEmpty(message) ? "" : "breaker";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
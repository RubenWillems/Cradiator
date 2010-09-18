using System;
using System.Globalization;
using System.Windows.Data;

namespace Cradiator.Converters
{
	[ValueConversion(typeof(double), typeof(string))]
	public class SecondsToTimeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var secondsValue = (double) value;
			var timeString = string.Empty;

			if (secondsValue >= 60 && secondsValue < 61)
			{
				timeString = "1 minute";
			}
			else if (secondsValue < 60)
			{
				timeString = ((int) secondsValue) + " seconds";
			}
			else if (secondsValue > 60)
			{
				var minutes = Math.Floor(secondsValue/60);
				var seconds = Math.Floor(secondsValue%60);

				if (minutes > 1 && seconds == 0)
					timeString = string.Format("{0} minutes", minutes);
				else
					timeString = string.Format("{0} minute{1} and {2} second{3}", 
											minutes, minutes > 1 ? "s" : "", 
											seconds, seconds > 1 ? "s" : "");
			}

			return "Every " + timeString;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Cradiator.Services;

namespace Cradiator.Converters
{
	[MarkupExtensionReturnType(typeof(BuildNameToMessageConverter))]
	[ValueConversion(typeof(string), typeof(string))]
	public class BuildNameToMessageConverter : MarkupExtension, IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var projectName = values[0].ToString().Replace("_", " ");
			var message = values[1].ToString();
			
			return message.Trim().Length == 0 ? 
				projectName : string.Format("{0}\n{1}", projectName, message);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return Ninjector.Get<BuildNameToMessageConverter>();
		}
	}
}

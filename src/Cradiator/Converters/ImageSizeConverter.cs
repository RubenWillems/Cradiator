using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Cradiator.Extensions;
using Cradiator.Services;

namespace Cradiator.Converters
{
	[MarkupExtensionReturnType(typeof(ImageSizeConverter))]
	[ValueConversion(typeof(string), typeof(int))]
	public class ImageSizeConverter : MarkupExtension, IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var currentMessage = value as string;
			return currentMessage.IsEmpty() ? 0 : 15;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return Ninjector.Get<ImageSizeConverter>();
		}
	}
}
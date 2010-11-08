using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Cradiator.Model;
using Cradiator.Services;

namespace Cradiator.Converters
{
	[MarkupExtensionReturnType(typeof(ImagePathConverter))]
	public class ImagePathConverter : MarkupExtension, IValueConverter
	{
		readonly IBuildBuster _buildBuster;

		/// <summary>
		/// xaml insists on this DO NOT USE
		/// </summary>
		public ImagePathConverter() {}

		public ImagePathConverter([InjectBuildBusterImageDecorator] IBuildBuster buildBuster)
		{
			_buildBuster = buildBuster;
		}

		/// <summary>
		/// convert from CurrentMessage to the FileName of an image to load (using breaker's name eg bsimpson.jpg)
		/// </summary>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return _buildBuster.FindBreaker(value as string);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return Ninjector.Get<ImagePathConverter>();
		}
	}
}
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Cradiator.Extensions;
using Cradiator.Model;
using Cradiator.Services;

namespace Cradiator.Converters
{
	[MarkupExtensionReturnType(typeof(OneBreakerConverter))]
	[ValueConversion(typeof(string), typeof(string))]
	public class OneBreakerConverter : MarkupExtension, IValueConverter
	{
		readonly IBuildBuster _buildBuster;

		/// <summary>
		/// xaml insists on this - DO NOT USE
		/// </summary>
		[Obsolete]
		public OneBreakerConverter() {}

		public OneBreakerConverter([InjectBuildBuster] IBuildBuster buildBuster)
		{
			_buildBuster = buildBuster;
		}

		/// <summary>
		/// Convert from CurrentMessage to the build breaker's name in brackets
		/// </summary>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var breaker = _buildBuster.FindBreaker(value as string);
			return breaker.IsEmpty() ? string.Empty : string.Format("({0})", breaker);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return Ninjector.Get<OneBreakerConverter>();
		}
	}
}
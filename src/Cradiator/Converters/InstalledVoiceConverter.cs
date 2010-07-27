using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Cradiator.Audio;
using Cradiator.Services;

namespace Cradiator.Converters
{
	[ValueConversion(typeof(string), typeof(string))]
	public class InstalledVoiceConverter : MarkupExtension, IValueConverter
	{
		readonly VoiceSelector _voiceSelector;

		/// <summary>
		/// xaml insists on this - DO NOT USE
		/// </summary>
		[Obsolete]
		public InstalledVoiceConverter() {}

		public InstalledVoiceConverter(VoiceSelector voiceSelector)
		{
			_voiceSelector = voiceSelector;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return _voiceSelector.GetClosestMatchingInstalledVoice(value as string);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return Ninjector.Get<InstalledVoiceConverter>();
		}
	}
}
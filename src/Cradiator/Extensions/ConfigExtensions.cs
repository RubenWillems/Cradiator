using System;
using System.Collections.Generic;
using System.Configuration;
using Cradiator.Config;

namespace Cradiator.Extensions
{
	public static class ConfigExtensions
	{
		public static void ForEach<T>(this IEnumerable<T> col, Action<T> action)
		{
			foreach (var item in col)
				action(item);
		}

		public static bool GetBoolProperty(this Configuration config, string settingsKey)
		{
			var boolProperty = GetProperty(config, settingsKey);

			bool boolValue;
			var boolString = boolProperty.Value;
			return bool.TryParse(boolString, out boolValue) ? boolValue : true;
		}

		public static int GetIntProperty(this Configuration config, string settingsKey, int defaultValue)
		{
			var intProperty = GetProperty(config, settingsKey);

			int intValue;
			var intString = intProperty.Value;
			return int.TryParse(intString, out intValue) ? intValue : defaultValue;
		}

		public static KeyValueConfigurationElement GetProperty(this Configuration config, string key)
		{
			var property = config.AppSettings.Settings[key];
			if (property == null) throw new ConfigSettingsException(key);
			return property;
		}

		public static string GetRegEx(this string regExString)
		{
			return string.IsNullOrEmpty(regExString) ? ".*" : regExString;
		}
	}
}
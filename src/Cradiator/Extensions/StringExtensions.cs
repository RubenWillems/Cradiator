using System;
using System.IO;

namespace Cradiator.Extensions
{
	public static class StringExtensions
	{
		public static bool IsEmpty(this string value)
		{
			return string.IsNullOrEmpty(value);
		}

		public static bool HasValue(this string value)
		{
			return !IsEmpty(value);
		}

		public static bool ContainsIgnoreCase(this string me, string other)
		{
			return me.ToLower().Contains(other.ToLower());
		}

        public static bool EqualsIgnoreCase(this string me, string them)
        {
            return me.Equals(them, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool ContainsInvalidChars(this string me)
        {
            return me.IndexOfAny(Path.GetInvalidFileNameChars()) != -1;
        }
	}
}
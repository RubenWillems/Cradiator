using System.Collections.Generic;
using System.Linq;

namespace Cradiator.Extensions
{
	public static class ListExtensions
	{
		public static T Second<T>(this IEnumerable<T> items)
		{
			return items.Skip(1).First();
		}

		public static T Third<T>(this IEnumerable<T> items)
		{
			return items.Skip(2).First();
		}
	}
}
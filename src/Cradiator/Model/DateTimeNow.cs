using System;

namespace Cradiator.Model
{
	public interface IDateTimeNow
	{
		DateTime Now { get; set; }
	}

	// a class just used for mocking out calls to DateTime
	public class DateTimeNow : IDateTimeNow
	{
		public DateTime Now
		{
			get { return DateTime.Now; }
			set {  }
		}
	}
}
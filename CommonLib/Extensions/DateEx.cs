using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib.Extensions
{
	public static class DateEx
	{
		public static string ToShortDateTimeString(this DateTime dt){
			return dt.ToShortDateString() + " " + dt.ToShortTimeString();
		}

		public static DateTime TrimMilliseconds(this DateTime self) {
			if (self.Millisecond != 0)
				return self.AddMilliseconds(-self.Millisecond);
			else
				return self;
		}

		public static DateTime TrimSecondsAndMilliseconds(this DateTime self) {
			if (self.Millisecond != 0 && self.Second != 0)
				return self.TrimMilliseconds().AddSeconds(-self.Second);
			else
				return self;
		}
	}
}

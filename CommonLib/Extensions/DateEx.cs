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
			return self.AddMilliseconds(-self.Millisecond);
		}

		public static DateTime TrimSecondsAndMilliseconds(this DateTime self) {
			return self.TrimMilliseconds().AddSeconds(-self.Second);
		}
	}
}

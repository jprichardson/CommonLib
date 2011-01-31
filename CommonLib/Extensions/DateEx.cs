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

		public static DateTime TrimTicks(this DateTime self) {
			return new DateTime(self.Year, self.Month, self.Day, self.Hour, self.Minute, self.Second, self.Millisecond, self.Kind);
		}

		public static DateTime TrimMilliseconds(this DateTime self) {
			var dt = new DateTime(self.Year, self.Month, self.Day, self.Hour, self.Minute, self.Second, self.Kind);
			return dt;
		}

		public static DateTime TrimSecondsAndMilliseconds(this DateTime self) {
			var dt = new DateTime(self.Year, self.Month, self.Day, self.Hour, self.Minute, 0, 0);
			dt = DateTime.SpecifyKind(dt, self.Kind);
			return dt;
		}
	}
}

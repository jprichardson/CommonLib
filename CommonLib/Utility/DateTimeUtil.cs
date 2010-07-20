using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CommonLib.Utility
{
	public static class DateTimeUtil
	{
		public static DateTime GetStartOfDay(DateTime day) {
			return new DateTime(day.Year, day.Month, day.Day, 0, 0, 0, 0, day.Kind);
		}

		public static DateTime GetEndOfDay(DateTime day) {
			return new DateTime(day.Year, day.Month, day.Day, 23, 59, 59, day.Kind);
		}

		public static DateTime HourFloor(DateTime dt) {
			return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0, dt.Kind);
		}

		public static DateTime HourCeil(DateTime dt) {
			return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour + 1, 0, 0, dt.Kind);
		}

		public static DateTime[] LoadList(string file) {
			StreamReader sr = new StreamReader(file);

			List<DateTime> dates = new List<DateTime>();
			while (!sr.EndOfStream) {
				DateTime dt = Convert.ToDateTime(sr.ReadLine());
				dt = dt.ToLocalTime();
				dates.Add(dt);
			}

			sr.Close();

			return dates.ToArray();
		}

		public static void SaveList(DateTime[] dates, string file) {
			StreamWriter sw = new StreamWriter(file);
			foreach (DateTime date in dates) {
				sw.WriteLine(date.ToUniversalTime());
			}
			sw.Close();
		}
	}
}

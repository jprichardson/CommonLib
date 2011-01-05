
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLib.Utility;

namespace CommonLib.Date
{
	[Serializable]
	public struct DayRange{
		public DateTime Start;
		public DateTime End;

		public DayRange(DateTime start, DateTime end) {
			this.Start = start;
			this.End = end;
		}

		public double TotalMinutes { get { return (this.End - this.Start).TotalMinutes; } }

		public static DayRange[] DaysToDayRanges(DateTime[] days) {
			DayRange[] ranges = new DayRange[days.Length];
			for (int x = 0; x < days.Length; x++) {
				DayRange dr = new DayRange();
				dr.Start = DateTimeUtil.GetStartOfDay(days[x]);
				dr.End = DateTimeUtil.GetEndOfDay(days[x]);
				ranges[x] = dr;
			}

			return ranges;
		}

		public override string ToString() {
			return "Start: " + this.Start + " End: " + this.End;
		}
	}
}

using CommonLib.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestCommonLib
{

	[TestClass()]
	public class DateExTest
	{
		private TestContext testContextInstance;

		public TestContext TestContext {
			get {
				return testContextInstance;
			}
			set {
				testContextInstance = value;
			}
		}

		[TestMethod()]
		public void ToShortDateTimeStringTest() {
			var dt = new DateTime(2010, 4, 5, 4, 32, 2, 134);
			Assert.AreEqual(dt.ToShortDateString() + " " + dt.ToShortTimeString(), dt.ToShortDateTimeString());
			Assert.AreNotEqual(dt.ToString(), dt.ToShortDateTimeString());
		}

		[TestMethod()]
		public void TrimMillisecondsTest() {
			var dt = new DateTime(2010, 4, 5, 4, 32, 2, 134);
			Assert.AreEqual(new DateTime(2010, 4, 5, 4, 32, 2), dt.TrimMilliseconds());
		}

		[TestMethod()]
		public void TrimSecondsAndMillisecondsTest() {
			var dt = new DateTime(2010, 4, 5, 4, 32, 2, 134);
			Assert.AreEqual(new DateTime(2010, 4, 5, 4, 32, 0), dt.TrimSecondsAndMilliseconds());

			dt = new DateTime(2010, 4, 5, 4, 32, 2, 0);
			Assert.AreEqual(new DateTime(2010, 4, 5, 4, 32, 0), dt.TrimSecondsAndMilliseconds());
		}

		/*[TestMethod()]
		public void RemoveDaylightSavingsTimeTest() {
			var s = "2010-06-12 1:06:00 PM";
			var dt = Convert.ToDateTime(s);
			var dt2 = dt.RemoveDaylightSavingsTime();
			Assert.AreEqual(dt.AddHours(-1), dt2);

			var dt3 = dt2.RemoveDaylightSavingsTime();
			Assert.AreEqual(dt2, dt3);

			s = "2010-11-12 1:06:00 PM";
			dt = Convert.ToDateTime(s);
			dt2 = dt.RemoveDaylightSavingsTime();
			Assert.AreEqual(dt, dt2);
		}*/
	}
}

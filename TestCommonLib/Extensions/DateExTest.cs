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

		/*[TestMethod()]
		public void TrimMillisecondsTest() {
			var dt = new DateTime(2010, 4, 5, 4, 32, 2, 134);
			Assert.AreEqual(new DateTime(2010, 4, 5, 4, 32, 2), dt.TrimMilliseconds());
		}*/

		[TestMethod()]
		public void TrimSecondsAndMillisecondsTest() {
			var dt = new DateTime(2010, 4, 5, 4, 32, 2, 134);
			Assert.AreEqual(new DateTime(2010, 4, 5, 4, 32, 0), dt.TrimSecondsAndMilliseconds());

			dt = new DateTime(2010, 4, 5, 4, 32, 2, 0);
			Assert.AreEqual(new DateTime(2010, 4, 5, 4, 32, 0), dt.TrimSecondsAndMilliseconds());

			var dt3 = new DateTime(634299220800007877); //the 7877 ticks screws it up!
			var dt4 = new DateTime(634299220800000000);
			var dt5 = DateTime.Parse("2011-01-06 2:48 PM");

			Assert.IsTrue(dt3 != dt4);
			Assert.IsTrue(dt3 != dt5);
			Assert.IsTrue(dt5 == dt4);
			Assert.IsTrue(dt3.ToShortDateTimeString() == dt4.ToShortDateTimeString()); 
			Assert.IsTrue(dt5.ToShortDateTimeString() == dt3.ToShortDateTimeString()); //year, day, month, hour, min are the same

			var dt6 = dt3.TrimSecondsAndMilliseconds();
			Assert.IsTrue(dt5 == dt6);
		}
	}
}

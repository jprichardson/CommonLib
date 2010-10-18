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
	}
}

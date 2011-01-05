using CommonLib.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TestCommonLib
{

	[TestClass()]
	public class DayCollectionTest
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
		public void GetItemsTest() {
			var dc = new DayCollection<int>();

			var dt1 = new DateTime(2000, 1, 1);
			var dt2 = new DateTime(2005, 2, 1);
			var dt3 = new DateTime(2010, 3, 1);

			dc.Add(dt2, 200);
			dc.Add(dt3, 300);
			dc.Add(dt1, 100);

			//Verify order
			int i = dc[0][0];
			Assert.AreEqual(100, i);
			Assert.AreEqual(200, dc[1][0]);
			Assert.AreEqual(300, dc[2][0]);
			
		}
	}
}

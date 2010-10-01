using CommonLib.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TestCommonLib
{
   
	[TestClass()]
	public class IEnumerableExTest
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
		public void StandardDeviationTest() {
			double[] X = { 4, 9, 11, 12, 17, 5, 8, 12, 14 };
			Assert.AreEqual(4.18, Math.Round(X.StandardDeviation(), 2));
		}
	}
}

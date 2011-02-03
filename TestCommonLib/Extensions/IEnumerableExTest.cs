using CommonLib.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
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
		public void DifferenceTest() {
			var a = new[] { 1, 2, 3, 4, 5, 6, 7 };
			var b = new[] { 4, 5, 6, 7, 8, 9, 10 };

			IEnumerable<int> expected = new[] { 1, 2, 3, 8, 9, 10 };
			var result = a.Difference(b);

			Assert.AreEqual(expected.Count(), result.Count());
			for (int x = 0; x < expected.Count(); ++x)
				Assert.AreEqual(expected.ElementAt(x), result.ElementAt(x));
		}

		[TestMethod()]
		public void StandardDeviationTest() {
			double[] X = { 4, 9, 11, 12, 17, 5, 8, 12, 14 };
			Assert.AreEqual(4.18, Math.Round(X.StandardDeviation(), 2));
		}
	}
}

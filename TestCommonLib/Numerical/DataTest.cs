using CommonLib.Numerical;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestCommonLib
{

	[TestClass()]
	public class DataTest
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
		public void PercentDifferenceTest() {
			var x1 = 20.0;
			var x2 = 10.0;

			var edif = Math.Abs((x1 - x2) / (x1 + x2) * 2.0);
			var dif = Data.PercentDifference(x1, x2);
			Assert.AreEqual(edif, dif);

			x1 = 98.0;
			x2 = 100.0;
			dif = Data.PercentDifference(x1, x2);
			Assert.AreEqual(0.020202, Math.Round(dif, 6));

			x1 = 150.0;
			x2 = 50.0;
			dif = Data.PercentDifference(x1, x2);
			Assert.AreEqual(1.0, dif);
		}

		[TestMethod()]
		public void PercentErrorTest() {
			var e = 98.0;
			var t = 100.0;
			var dif = Data.PercentError(e, t);
			Assert.AreEqual(-0.02, dif);

			e = 150.0;
			t = 50.0;
			dif = Data.PercentError(e, t);
			Assert.AreEqual(2.0, dif);
		}

		[TestMethod()]
		public void PercentErrorAbsTest() {
			var e = 98.0;
			var t = 100.0;
			var dif = Data.PercentErrorAbs(e, t);
			Assert.AreEqual(0.02, dif);
		}

		[TestMethod()]
		public void StandardDeviationTest() {
			double[] X = {4,9,11,12,17,5,8,12,14};
			Assert.AreEqual(4.18, Math.Round(Data.StandardDeviation(X), 2));

			double[] M = {5};
			Assert.AreEqual(0, Math.Round(Data.StandardDeviation(M), 2));
		}

		[TestMethod()]
		public void LeastSquaresCoefTest() {
			double[] X = {1.0, 2.3, 3.1, 4.8, 5.6, 6.3};
			double[] Y = {2.6, 2.8, 3.1, 4.7, 5.1, 5.3};

			double m;
			double b;
			double merr;
			double berr;
			Data.LeastSquaresCoef(Y, X, out m, out b, out merr, out berr);

			Assert.AreEqual(0.5842, Math.Round(m, 4));
			Assert.AreEqual(1.6842, Math.Round(b, 4));
		}
	}
}

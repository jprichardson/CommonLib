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

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion

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
	}
}

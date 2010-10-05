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

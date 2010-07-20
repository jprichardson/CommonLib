using CommonLib.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TestCommonLib
{
    
    
    /// <summary>
    ///This is a test class for DayCollectionTest and is intended
    ///to contain all DayCollectionTest Unit Tests
    ///</summary>
	[TestClass()]
	public class DayCollectionTest
	{


		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
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

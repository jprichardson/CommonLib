using CommonLib.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TestCommonLib
{
   
	[TestClass()]
	public class ListExTest
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
		public void AutoFillDefaultTest() {
			var list = new List<int>();
			list.AutoFillDefault(5);

			Assert.AreEqual(5, list.Count);
			foreach (var li in list)
				Assert.AreEqual(0, li);

			var listS = new List<string>();
			listS.AutoFillDefault(14);

			Assert.AreEqual(14, listS.Count);
			foreach (var li in listS)
				Assert.AreEqual(null, li);
		}
	}
}

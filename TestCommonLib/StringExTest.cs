using CommonLib.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestCommonLib
{
   
	[TestClass()]
	public class StringExTest
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
		public void CountLinesTest() {
			var s = "Hello JP!" + Environment.NewLine;
			s += "How are you?" + Environment.NewLine;
			s += "I'm well.";

			Assert.AreEqual(3, s.CountLines());
		}
	}
}

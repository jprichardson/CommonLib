using CommonLib.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;

namespace TestCommonLib
{

	[TestClass()]
	public class ProcessUtilTest
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
		public void IsRunningTest() {
			Assert.IsFalse(ProcessUtil.IsRunning(null));	
		}

		
	}
}

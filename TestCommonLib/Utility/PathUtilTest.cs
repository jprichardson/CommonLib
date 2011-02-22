using CommonLib.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestCommonLib
{

	[TestClass()]
	public class PathUtilTest
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
		public void GetPathAndFileNameWithoutExtensionTest() {
			var s = @"C:\somedir\somefile.ext";
			Assert.AreEqual(@"C:\somedir\somefile", PathUtil.GetPathAndFileNameWithoutExtension(s));
			s = @"somefile.ext";
			Assert.AreEqual("somefile", PathUtil.GetPathAndFileNameWithoutExtension(s));
		}
	}
}

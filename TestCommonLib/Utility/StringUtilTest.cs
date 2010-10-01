using CommonLib.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestCommonLib
{

	[TestClass()]
	public class StringUtilTest
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
		public void ToCsvTest() {
			var csv = "";
			csv = StringUtil.ToCsv("1", "2", "3");
			Assert.AreEqual("\"1\",\"2\",\"3\"", csv);

			csv = StringUtil.ToCsv(new string[] { "blue", "green", "red" });
			Assert.AreEqual("\"blue\",\"green\",\"red\"", csv);
		}
	}
}

using CommonLib.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestCommonLib
{
	[TestClass()]
	public class FlexDictionaryTest
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
		public void FlexDictonaryTest() {
			Func<FlexDictionary<string, FlexDictionary<string, int>>> cf = () =>
			{
				return new FlexDictionary<string, FlexDictionary<string, int>>(
					() => { return new FlexDictionary<string, int>(() => { return 0; }); }
					);
			};

			var fd = new FlexDictionary<string, FlexDictionary<string, FlexDictionary<string, int>>>(cf);

			Assert.AreEqual(0, fd["1"]["2"]["3"]);

			fd["1"]["2"]["3"] = 4;

			Assert.AreEqual(4, fd["1"]["2"]["3"]);

			fd["something"]["2"]["3"] = 10;
			Assert.AreEqual(10, fd["something"]["2"]["3"]);
		}
	}
}

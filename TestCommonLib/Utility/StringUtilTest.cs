using CommonLib.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CommonLib.Extensions;

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

		[TestMethod()]
		public void HexConversionsText() {
			var rnd1 = new Random(Environment.TickCount);
			var rnd2 = new Random(Environment.TickCount + 1200);

			var TRIALS = 1;

			for (int j = 0; j < TRIALS; ++j) {
				var SIZE = rnd1.Next(64);
				var ba = new byte[SIZE];
				for (int x = 0; x < SIZE; ++x) {
					ba[x] = (byte)rnd2.Next((int)byte.MaxValue);
				}

				string hex = StringUtil.ByteArrayToHexString(ba);
				var ba2 = StringUtil.HexStringToByteArray(hex);

				for (int x = 0; x < ba2.Length; ++x)
					Assert.AreEqual(ba[x], ba2[x]);
			}
		}
	}
}

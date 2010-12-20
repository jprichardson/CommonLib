using CommonLib.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;

namespace TestCommonLib
{

	[TestClass()]
	public class NumberUtilTest
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
		public void ConvertNibbleToBitarrayTest() {
			var ba = NumberUtil.ConvertNibbleToBitarray(0); //0000
			Assert.IsFalse(ba[0]); Assert.IsFalse(ba[1]); Assert.IsFalse(ba[2]); Assert.IsFalse(ba[3]);

			ba = NumberUtil.ConvertNibbleToBitarray(1); //0001
			Assert.IsFalse(ba[0]); Assert.IsFalse(ba[1]); Assert.IsFalse(ba[2]); Assert.IsTrue(ba[3]);

			ba = NumberUtil.ConvertNibbleToBitarray(2); //0010
			Assert.IsFalse(ba[0]); Assert.IsFalse(ba[1]); Assert.IsTrue(ba[2]); Assert.IsFalse(ba[3]);

			ba = NumberUtil.ConvertNibbleToBitarray(3); //0011
			Assert.IsFalse(ba[0]); Assert.IsFalse(ba[1]); Assert.IsTrue(ba[2]); Assert.IsTrue(ba[3]);

			ba = NumberUtil.ConvertNibbleToBitarray(4); //0100
			Assert.IsFalse(ba[0]); Assert.IsTrue(ba[1]); Assert.IsFalse(ba[2]); Assert.IsFalse(ba[3]);

			ba = NumberUtil.ConvertNibbleToBitarray(5); //0101
			Assert.IsFalse(ba[0]); Assert.IsTrue(ba[1]); Assert.IsFalse(ba[2]); Assert.IsTrue(ba[3]);

			ba = NumberUtil.ConvertNibbleToBitarray(6); //0110
			Assert.IsFalse(ba[0]); Assert.IsTrue(ba[1]); Assert.IsTrue(ba[2]); Assert.IsFalse(ba[3]);

			ba = NumberUtil.ConvertNibbleToBitarray(7); //0111
			Assert.IsFalse(ba[0]); Assert.IsTrue(ba[1]); Assert.IsTrue(ba[2]); Assert.IsTrue(ba[3]);

			ba = NumberUtil.ConvertNibbleToBitarray(8); //1000
			Assert.IsTrue(ba[0]); Assert.IsFalse(ba[1]); Assert.IsFalse(ba[2]); Assert.IsFalse(ba[3]);

			ba = NumberUtil.ConvertNibbleToBitarray(9); //1001
			Assert.IsTrue(ba[0]); Assert.IsFalse(ba[1]); Assert.IsFalse(ba[2]); Assert.IsTrue(ba[3]);

			ba = NumberUtil.ConvertNibbleToBitarray(10); //1010
			Assert.IsTrue(ba[0]); Assert.IsFalse(ba[1]); Assert.IsTrue(ba[2]); Assert.IsFalse(ba[3]);

			ba = NumberUtil.ConvertNibbleToBitarray(11); //1011
			Assert.IsTrue(ba[0]); Assert.IsFalse(ba[1]); Assert.IsTrue(ba[2]); Assert.IsTrue(ba[3]);

			ba = NumberUtil.ConvertNibbleToBitarray(12); //1100
			Assert.IsTrue(ba[0]); Assert.IsTrue(ba[1]); Assert.IsFalse(ba[2]); Assert.IsFalse(ba[3]);

			ba = NumberUtil.ConvertNibbleToBitarray(13); //1101
			Assert.IsTrue(ba[0]); Assert.IsTrue(ba[1]); Assert.IsFalse(ba[2]); Assert.IsTrue(ba[3]);

			ba = NumberUtil.ConvertNibbleToBitarray(14); //1110
			Assert.IsTrue(ba[0]); Assert.IsTrue(ba[1]); Assert.IsTrue(ba[2]); Assert.IsFalse(ba[3]);

			ba = NumberUtil.ConvertNibbleToBitarray(15); //1111
			Assert.IsTrue(ba[0]); Assert.IsTrue(ba[1]); Assert.IsTrue(ba[2]); Assert.IsTrue(ba[3]);

			ba = NumberUtil.ConvertNibbleToBitarray(16); //0001 0000 every input is ANDed with 0xF
			Assert.IsFalse(ba[0]); Assert.IsFalse(ba[1]); Assert.IsFalse(ba[2]); Assert.IsFalse(ba[3]);
		}

		[TestMethod()]
		public void TrimZerosTest(){
			var num = "0.000153989000";
			Assert.AreEqual("0.000153989", NumberUtil.TrimZeros(num));

			num = "-0.000153989000";
			Assert.AreEqual("-0.000153989", NumberUtil.TrimZeros(num));

			num = "1.000153989000";
			Assert.AreEqual("1.000153989", NumberUtil.TrimZeros(num));

			num = "-1.000153989000";
			Assert.AreEqual("-1.000153989", NumberUtil.TrimZeros(num));

			num = "100.000153989000";
			Assert.AreEqual("100.000153989", NumberUtil.TrimZeros(num));

			num = "-100.000153989000";
			Assert.AreEqual("-100.000153989", NumberUtil.TrimZeros(num));

			num = "1.0";
			Assert.AreEqual("1.0", NumberUtil.TrimZeros(num));

			num = "-1.0";
			Assert.AreEqual("-1.0", NumberUtil.TrimZeros(num));

			num = "1.000";
			Assert.AreEqual("1.0", NumberUtil.TrimZeros(num));

			num = "-1.000";
			Assert.AreEqual("-1.0", NumberUtil.TrimZeros(num));

			num = "100.0";
			Assert.AreEqual("100.0", NumberUtil.TrimZeros(num));

			num = "-100.0";
			Assert.AreEqual("-100.0", NumberUtil.TrimZeros(num));

			num = "1";
			Assert.AreEqual("1", NumberUtil.TrimZeros(num));

			num = "100";
			Assert.AreEqual("100", NumberUtil.TrimZeros(num));

		}
	}
}

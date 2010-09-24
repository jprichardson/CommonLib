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

		[TestMethod()]
		public void Chop() {
			Assert.AreEqual("string", "string\r\n".Chop());
			Assert.AreEqual("string\n", "string\n\r".Chop());
			Assert.AreEqual("string", "string\n".Chop());
			Assert.AreEqual("strin", "string".Chop());
			Assert.AreEqual("", "x".Chop().Chop());
		}

		[TestMethod()]
		public void LastCharAtTest() {
			var s = "hello";
			Assert.AreEqual('o', s.LastCharAt(-1));
			Assert.AreEqual('l', s.LastCharAt(-2));
			Assert.AreEqual('l', s.LastCharAt(-3));
			Assert.AreEqual('e', s.LastCharAt(-4));
			Assert.AreEqual('h', s.LastCharAt(-5));

			bool ex = false;
			try {
				Assert.AreEqual("SHOULD THROW EX", s.LastCharAt(-6));
			}
			catch { ex = true; }
			Assert.IsTrue(ex);

			ex = false;
			try {
				Assert.AreEqual("SHOULD THROW EX", s.LastCharAt(0));
			}
			catch { ex = true; }
			Assert.IsTrue(ex);
		}
	}
}

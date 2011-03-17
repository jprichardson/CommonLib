using CommonLib.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

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

		[TestMethod()]
		public void FirstLineTest() {
			string target = "Hi JP!!" + Environment.NewLine + "Bye JP!!";
			Assert.AreEqual("Hi JP!!", target.FirstLine());
		}

		[TestMethod()]
		public void IsAlphaNumericTest() {
			string s = "asdflajsf352q3ajlfasjf";
			Assert.IsTrue(s.IsAlphaNumeric());
			s = "45g";
			Assert.IsTrue(s.IsAlphaNumeric());
			s = "45-";
			Assert.IsFalse(s.IsAlphaNumeric());
			s = "3ABdc";
			Assert.IsTrue(s.IsAlphaNumeric());
		}

		[TestMethod()]
		public void RemoveLastCharTest() {
			var s = "";
			s = "hello";
			s = s.RemoveLastChar();
			Assert.AreEqual("hell", s.ToString());

			s = "";
			Assert.AreEqual("", s.ToString());
			s = s.RemoveLastChar();
			Assert.AreEqual("", s.ToString());

			s = "a";
			s = s.RemoveLastChar();
			Assert.AreEqual("", s.ToString());
		}

		[TestMethod()]
		public void SplitAtTest() {
			var s = "JonPaul";
			var res = s.SplitAt(0, 3).ToArray();
			Assert.AreEqual(2, res.Length);
			Assert.AreEqual("Jon", res[0]);
			Assert.AreEqual("Paul", res[1]);

			s = "JonPaul";
			res = s.SplitAt(3).ToArray();
			Assert.AreEqual(2, res.Length);
			Assert.AreEqual("Jon", res[0]);
			Assert.AreEqual("Paul", res[1]);

			s = "JP";
			res = s.SplitAt(0).ToArray();
			Assert.AreEqual(1, res.Length);
			Assert.AreEqual("JP", res[0]);

			s = "JP";
			res = s.SplitAt(null).ToArray();
			Assert.AreEqual(1, res.Length);
			Assert.AreEqual("JP", res[0]);

			s = "JP";
			res = s.SplitAt(10).ToArray();
			Assert.AreEqual(1, res.Length);
			Assert.AreEqual("JP", res[0]);

			s = "JP";
			res = s.SplitAt(-10).ToArray();
			Assert.AreEqual(1, res.Length);
			Assert.AreEqual("JP", res[0]);

			s = "JP";
			res = s.SplitAt(1).ToArray();
			Assert.AreEqual(2, res.Length);
			Assert.AreEqual("J", res[0]);
			Assert.AreEqual("P", res[1]);

			s = "JP";
			res = s.SplitAt(2).ToArray();
			Assert.AreEqual(1, res.Length);
			Assert.AreEqual("JP", res[0]);

			s = "JonPaulLikesSoftware";
			res = s.SplitAt(0, 3, 7, 12).ToArray();
			Assert.AreEqual(4, res.Length);
			Assert.AreEqual("Jon", res[0]);
			Assert.AreEqual("Paul", res[1]);
			Assert.AreEqual("Likes", res[2]);
			Assert.AreEqual("Software", res[3]);

			s = "JonPaulLikesSoftware";
			res = s.SplitAt(0, 3, 7, 12, 20).ToArray();
			Assert.AreEqual(4, res.Length);
			Assert.AreEqual("Jon", res[0]);
			Assert.AreEqual("Paul", res[1]);
			Assert.AreEqual("Likes", res[2]);
			Assert.AreEqual("Software", res[3]);

			s = "JonPaulLikesSoftware";
			res = s.SplitAt(-10, 25, 0, 3, 7, 12, 3, 20, 0, 20).ToArray();
			Assert.AreEqual(4, res.Length);
			Assert.AreEqual("Jon", res[0]);
			Assert.AreEqual("Paul", res[1]);
			Assert.AreEqual("Likes", res[2]);
			Assert.AreEqual("Software", res[3]);
		}
	}
}

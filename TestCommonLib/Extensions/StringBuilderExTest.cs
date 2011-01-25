using CommonLib.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;


namespace TestCommonLib
{

	[TestClass()]
	public class StringBuilderExTest
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
		public void RemoveLastCharTest() {
			var s = new StringBuilder();
			s.Append("hello");
			s.RemoveLastChar();
			Assert.AreEqual("hell", s.ToString());

			s.Clear();
			Assert.AreEqual("", s.ToString());
			s.RemoveLastChar();
			Assert.AreEqual("", s.ToString());

			s.Append('a');
			s.RemoveLastChar();
			Assert.AreEqual("", s.ToString());
		}
	}
}

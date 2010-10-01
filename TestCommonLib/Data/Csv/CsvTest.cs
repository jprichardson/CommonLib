using CommonLib.Data.Csv;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestCommonLib
{
	[TestClass()]
	public class CsvTest
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

		[TestInitialize()]
		public void MyTestInitialize()
		{
		
		}
		
		[TestCleanup()]
		public void MyTestCleanup()
		{
		
		}

		[TestMethod()]
		public void RecordSplitTest() {
			var data = Csv.RecordSplit("'Jon','Paul'", ',', '\'');
			Assert.AreEqual(2, data.Length);
			Assert.AreEqual("Jon", data[0]);
			Assert.AreEqual("Paul", data[1]);

			data = Csv.RecordSplit("'Jon','Paul','Richardson'", ',', '\'');
			Assert.AreEqual(3, data.Length);
			Assert.AreEqual("Jon", data[0]);
			Assert.AreEqual("Paul", data[1]);
			Assert.AreEqual("Richardson", data[2]);

			data = Csv.RecordSplit("'J'n','Paul','Richardson'", ',', '\'');
			Assert.AreEqual(3, data.Length);
			Assert.AreEqual("J'n", data[0]);
			Assert.AreEqual("Paul", data[1]);
			Assert.AreEqual("Richardson", data[2]);

			data = Csv.RecordSplit("'J'n','Pa,l','Richardson'", ',', '\'');
			Assert.AreEqual(3, data.Length);
			Assert.AreEqual("J'n", data[0]);
			Assert.AreEqual("Pa,l", data[1]);
			Assert.AreEqual("Richardson", data[2]);

			data = Csv.RecordSplit("''on','Pa,l','Richardson'", ',', '\'');
			Assert.AreEqual(3, data.Length);
			Assert.AreEqual("'on", data[0]);
			Assert.AreEqual("Pa,l", data[1]);
			Assert.AreEqual("Richardson", data[2]);

			data = Csv.RecordSplit("''on',',a,l','Richardson'", ',', '\'');
			Assert.AreEqual(3, data.Length);
			Assert.AreEqual("'on", data[0]);
			Assert.AreEqual(",a,l", data[1]);
			Assert.AreEqual("Richardson", data[2]);

			data = Csv.RecordSplit("'Jon','Paul','Richardson'", ',', null);
			Assert.AreEqual(3, data.Length);
			Assert.AreEqual("'Jon'", data[0]);
			Assert.AreEqual("'Paul'", data[1]);
			Assert.AreEqual("'Richardson'", data[2]);

			data = Csv.RecordSplit("Jon,Paul,Richardson", ',', null);
			Assert.AreEqual(3, data.Length);
			Assert.AreEqual("Jon", data[0]);
			Assert.AreEqual("Paul", data[1]);
			Assert.AreEqual("Richardson", data[2]);
		}
	}
}

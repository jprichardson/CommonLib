using CommonLib.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TestCommonLib
{

	[TestClass()]
	public class DictionaryExTest
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
		public void ToXmlStringTest() {
			var dict = new Dictionary<string, string>();
			dict.Add("key1", "hi");
			dict.Add("key2", "hello");

			string xmlHeader = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n";

			string xml = xmlHeader +
@"<Dictionary>
	<Item Key='key1'>hi</Item>
	<Item Key='key2'>hello</Item>
</Dictionary>";

			xml = xml.Replace('\'', '"');

			Assert.AreEqual(xml, dict.ToXmlString());

			var dict2 = new Dictionary<string, decimal>();
			dict2.Add("key1", 34.56m);
			dict2.Add("key2", -100.242m);

			xml = xmlHeader +
@"<Dictionary>
	<Item Key='key1'>34.56</Item>
	<Item Key='key2'>-100.242</Item>
</Dictionary>";

			xml = xml.Replace('\'', '"');

			Assert.AreEqual(xml, dict2.ToXmlString());

			var d1 = Convert.ToDateTime("2010-11-29T14:40:51.0282682-06:00");
			var d2 = d1.AddHours(1);

			var dict3 = new Dictionary<decimal, DateTime>();
			dict3.Add(-1.23m, d1);
			dict3.Add(0.234m, d2);

			xml = xmlHeader +
@"<Dictionary>
	<Item Key='-1.23'>2010-11-29T14:40:51.0282682-06:00</Item>
	<Item Key='0.234'>2010-11-29T15:40:51.0282682-06:00</Item>
</Dictionary>";

			xml = xml.Replace('\'', '"');

			Assert.AreEqual(xml, dict3.ToXmlString());
		}

		[TestMethod()]
		public void RemoveAllTest() {
			var sd = new SortedDictionary<string, int>();
			sd.Add("first", 1);
			sd.Add("second", 2);
			sd.Add("third", 3);

			var keysToRemove = new List<string>();
			keysToRemove.Add("first");
			keysToRemove.Add("third");

			Assert.AreEqual(3, sd.Count);
			sd.RemoveAll(keysToRemove);
			Assert.AreEqual(1, sd.Count);

			Assert.AreEqual(2, sd["second"]);
		}
	}
}

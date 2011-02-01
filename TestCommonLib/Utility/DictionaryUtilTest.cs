using CommonLib.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;
using CommonLib.Extensions;
using System.IO;

namespace TestCommonLib
{

	[TestClass()]
	public class DictionaryUtilTest
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
		public void LoadSaveDictionaryListTest() {
			var dictList = new Dictionary<DateTime, List<string>>();

			var dt1 = DateTime.Now.TrimMilliseconds();
			var dt2 = dt1.AddMinutes(1);
			var dt3 = dt1.AddMinutes(2);

			dictList.Add(dt1, "1-1");
			dictList.Add(dt2, "2-1");
			dictList.Add(dt3, "3-1");
			dictList.Add(dt1, "1-2");
			dictList.Add(dt2, "2-2");
			dictList.Add(dt2, "2-3");

			var file = Path.GetTempFileName();

			DictionaryUtil.SaveDictionaryList(dictList, file);

			var newDictList = DictionaryUtil.LoadDictionaryList<DateTime, string>(file);

			Assert.AreEqual(dictList.Count, newDictList.Count);

			foreach (var key in newDictList.Keys) {
				Assert.AreEqual(dictList[key].Count, newDictList[key].Count);
				for (int x = 0; x < newDictList[key].Count; ++x)
					Assert.AreEqual(dictList[key][x], newDictList[key][x]);
			}

			//test for file not existing
			var dictList2 = DictionaryUtil.LoadDictionaryList<string, string>(null);
			Assert.IsNotNull(dictList2);

			//test for empty file
			var dictList3 = DictionaryUtil.LoadDictionaryList<string, string>(Path.GetTempFileName());
			Assert.AreEqual(0, dictList3.Count);
		}

		[TestMethod()]
		public void LoadSaveDictionaryTest() {
			var dict = new Dictionary<string, DateTime>();

			var dt = DateTime.Now.TrimMilliseconds();
			dict.Add("1", dt);
			dict.Add("2", dt.AddMinutes(1));
			dict.Add("3", dt.AddMinutes(2));

			var file = Path.GetTempFileName();

			DictionaryUtil.SaveDictionary(dict, file);

			var newDict = DictionaryUtil.LoadDictionary<string, DateTime>(file);

			Assert.AreEqual(dict.Count, newDict.Count);
			foreach (var key in newDict.Keys)
				Assert.AreEqual(dict[key], newDict[key]);

			//test for file not existing
			var dict2 = DictionaryUtil.LoadDictionary<string, string>(null);
			Assert.IsNotNull(dict2);

			//test for empty file
			var dict3 = DictionaryUtil.LoadDictionary<string, string>(Path.GetTempFileName());
			Assert.AreEqual(0, dict3.Count);
		}
	}
}

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
		}
	}
}

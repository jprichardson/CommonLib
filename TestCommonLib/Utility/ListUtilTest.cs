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
	public class ListUtilTest
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
		public void LoadSaveTest() {
			var list = new List<string>();

			list.Add("1-1");
			list.Add("2-1");
			list.Add("3-1");
			list.Add("1-2");
			list.Add("2-2");
			list.Add("2-3");

			var file = Path.GetTempFileName();

			ListUtil.Save(list, file);

			var newList = ListUtil.Load<string>(file);

			Assert.AreEqual(list.Count, newList.Count);
			
			Assert.AreEqual(list.Count, newList.Count);
			for (int x = 0; x < newList.Count; ++x)
				Assert.AreEqual(list[x], newList[x]);

			var exThrown = false;
			list = null;
			try {
				list = ListUtil.Load<string>(null);
			}
			catch { exThrown = true; }
			Assert.IsFalse(exThrown);
			Assert.IsNotNull(list);

			//test for empty file
			var list3 = ListUtil.Load<string>(Path.GetTempFileName());
			Assert.AreEqual(0, list3.Count);
		}
	}
}

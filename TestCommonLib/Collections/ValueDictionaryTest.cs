using CommonLib.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using CommonLib.Utility;

namespace TestCommonLib
{
    
	[TestClass()]
	public class ValueDictionaryTest
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
		public void ConstructorTest() {
			var vd = new ValueDictionary<bool>(true);
			Assert.IsTrue(vd.DefaultReturn);
			vd = new ValueDictionary<bool>(false);
			Assert.IsFalse(vd.DefaultReturn);

			var vd2 = new ValueDictionary<DateTime>(new DateTime(1983, 4, 5));
			Assert.AreEqual(new DateTime(1983, 4, 5), vd2.DefaultReturn);
		}

		/*[TestMethod()]
		public void SaveLoadTest() {
			var vd = new ValueDictionary<bool>(true);
			vd[1] = true;
			vd[2] = false;
			vd[-100] = false;
			Assert.AreEqual(3, vd.Count);

			var dir = Environment.CurrentDirectory + Path.DirectorySeparatorChar;

			vd.Save(dir + "f.txt");

			var vd2 = ValueDictionary<bool>.Load(dir + "f.txt");
			Assert.IsTrue(vd2[1]);
			Assert.IsFalse(vd2[2]);
			Assert.IsFalse(vd2[-100]);
			Assert.AreEqual(3, vd2.Count);
			Assert.IsTrue(vd2.DefaultReturn);
		}*/

		[TestMethod()]
		public void NegativeIndicesTest() {
			var vd = new ValueDictionary<bool>(false);
			vd[-1] = false;
			vd[-2] = true;
			vd[-100] = false;
			vd[-200] = false;
			vd[100] = true;
			vd[10000] = true;

			Assert.AreEqual(4, vd.NegativeIndices.Count);

			vd.AllowNegativeIndices = false;
			bool error = false;
			try {
				vd[-1] = true;
			}
			catch { error = true; }
			Assert.IsTrue(error);
		}

		[TestMethod()]
		public void XmlSerializationTest() {
			var vd = new ValueDictionary<bool>(true);
			vd[1] = true;
			vd[2] = false;
			vd[-100] = false;
			Assert.AreEqual(3, vd.Count);

			var dir = Environment.CurrentDirectory + Path.DirectorySeparatorChar;
			var file = dir + "f.txt";

			FileUtil.WriteToXmlFile(vd, file);

			var txt = File.ReadAllText(file);

			var vd2 = FileUtil.ReadFromXmlFile<ValueDictionary<bool>>(file);
			Assert.IsTrue(vd2[1]);
			Assert.IsFalse(vd2[2]);
			Assert.IsFalse(vd2[-100]);
			Assert.AreEqual(3, vd2.Count);
			Assert.IsTrue(vd2.DefaultReturn);
		}

		[TestMethod()]
		public void NegativeIndicesSerializationRegressionTest() {
			var vd = new ValueDictionary<bool>(true);
			vd[0] = true;
			vd[1] = true;
			vd[-1] = true;
			Assert.AreEqual(1, vd.NegativeIndices.Count); 

			var txtFile = Path.GetTempFileName();
			//vd.Save(txtFile);
			FileUtil.WriteToXmlFile(vd, txtFile);

			//var vd2 = ValueDictionary<bool>.Load(txtFile);
			var vd2 = FileUtil.ReadFromXmlFile<ValueDictionary<bool>>(txtFile);
			Assert.AreEqual(1, vd2.NegativeIndices.Count);

			var xmlFile = Path.GetTempFileName();
			FileUtil.WriteToXmlFile(vd, xmlFile);

			var vd3 = FileUtil.ReadFromXmlFile<ValueDictionary<bool>>(xmlFile);
			Assert.AreEqual(1, vd3.NegativeIndices.Count);
		}

		[TestMethod()]
		public void AllowDefaultReturnsTest() {
			var vd = new ValueDictionary<bool>();
			Assert.IsFalse(vd[0]);

			vd.AllowDefaultReturns = false;
			bool error = false;
			try {
				Assert.IsFalse(vd[0]);
				Assert.IsFalse(vd[1]);
			}
			catch {
				error = true;
			}

			Assert.IsTrue(error);
		}
	}
}

using CommonLib.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.IO;

namespace TestCommonLib
{

	[TestClass()]
	public class FileUtilTest
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
		public void CreateDirectoryTest() {
			var rnd = new Random();
			var dir = Path.GetTempPath() + Path.DirectorySeparatorChar + rnd.Next() + Path.DirectorySeparatorChar;
			Assert.IsFalse(Directory.Exists(dir));

			FileUtil.CreateDirectory(dir);
			Assert.IsTrue(Directory.Exists(dir));

			bool exThrown = false;
			try {
				FileUtil.CreateDirectory(dir);
			}
			catch { exThrown = true; }
			Assert.IsFalse(exThrown); //shouldn't throw exception because it only creates if it doesn't exist
			Assert.IsTrue(Directory.Exists(dir));
		}

		[TestMethod()]
		public void DeleteDirectoryTest() {
			var rnd = new Random();
			var dir = Path.GetTempPath() + Path.DirectorySeparatorChar + rnd.Next() + Path.DirectorySeparatorChar;
			Assert.IsFalse(Directory.Exists(dir));

			FileUtil.CreateDirectory(dir);
			File.WriteAllText(dir + rnd.Next() + ".txt", "some text");

			Assert.IsTrue(Directory.Exists(dir));
			FileUtil.DeleteDirectory(dir);
			Assert.IsFalse(Directory.Exists(dir));

			bool exThrown = false;
			try {
				FileUtil.DeleteDirectory(dir);
			}
			catch { exThrown = true; }
			Assert.IsFalse(exThrown); //shouldn't throw exception because it only deletes if it exists

		}

		[TestMethod()]
		public void ChangeExtensionText() {
			var file = @"C:\WeirdDirName.doc\file.txt";
			Assert.AreEqual(@"C:\WeirdDirName.doc\file.doc", FileUtil.ChangeExtension(file, "doc"));
			Assert.AreEqual(@"C:\WeirdDirName.doc\file.doc", FileUtil.ChangeExtension(file, ".doc"));
			Assert.AreEqual(".doc", Path.GetExtension(FileUtil.ChangeExtension(file, ".doc")));

		}
	}
}

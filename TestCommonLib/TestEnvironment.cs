using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TestCommonLib
{
	class TestEnvironment
	{
		public static string DataPath = ""; 

		public static void Bootstrap() {
			int i = (new Random(Environment.TickCount)).Next(0, int.MaxValue);
			DataPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "CommonLib-Data-" + i + Path.DirectorySeparatorChar;
			if (!Directory.Exists(DataPath))
				Directory.CreateDirectory(DataPath);
		}

		public static void Destroy() {
			Directory.Delete(DataPath, true);
		}
	}
}

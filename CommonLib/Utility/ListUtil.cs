using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;

namespace CommonLib.Utility
{
	public static class ListUtil
	{
		public static List<V> Load<V>(string file) {
			return ListUtil.Load<V>(file, TypeDescriptor.GetConverter(typeof(V)));
		}

		public static List<V> Load<V>(string file, TypeConverter tc) {
			var list = new List<V>();
			if (!File.Exists(file))
				return list; 

			var sr = new StreamReader(file);

			while (!sr.EndOfStream)
				list.Add((V)tc.ConvertFromString(sr.ReadLine()));

			sr.Close();

			return list;
		}

		public static void Save<V>(List<V> self, string file) {
			var sw = new StreamWriter(file);

			foreach (var val in self)
				sw.WriteLine(val.ToString());

			sw.Close();
		}
	}
}

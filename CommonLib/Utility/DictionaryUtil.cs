using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using CommonLib.Utility;
using CommonLib.Data.Csv;
using CommonLib.Extensions;

namespace CommonLib.Utility
{
	public static class DictionaryUtil
	{
		public static IDictionary<TKey, List<TValue>> LoadDictionaryList<TKey, TValue>(string file) {
			var sr = new StreamReader(file);
			//var line = sr.ReadLine();
			//var types = line.Split(',');
			var keyConverter = TypeDescriptor.GetConverter(typeof(TKey)/*Type.GetType(types[0])*/);
			var valueConverter = TypeDescriptor.GetConverter(typeof(TValue));

			IDictionary<TKey, List<TValue>> dictList = new Dictionary<TKey, List<TValue>>();

			while (!sr.EndOfStream) {
				var fields = Csv.RecordSplit(sr.ReadLine(), ',', '"');
				TKey key = (TKey)keyConverter.ConvertFromString(fields[0]);
				for (int i = 1; i < fields.Length; ++i) {
					TValue value = (TValue)valueConverter.ConvertFromString(fields[i]);
					if (!dictList.ContainsKey(key))
						dictList.Add(key, new List<TValue>());

					dictList[key].Add(value);
				}
					
			}

			return dictList;
		}

		public static void SaveDictionaryList<TKey, TValue>(this IDictionary<TKey, List<TValue>> dictList, string file) {
			var sw = new StreamWriter(file);
			//sw.WriteLine(typeof(TKey) + "," + typeof(TValue));
			foreach (var key in dictList.Keys) {
				var sb = new StringBuilder(255);
				sb.Append('"');
				sb.Append(key);
				sb.Append("\",");
				foreach (var val in dictList[key]) {
					sb.Append('"');
					sb.Append(val);
					sb.Append("\",");
				}
				sb.RemoveLastChar();

				sw.WriteLine(sb.ToString());
			}
			sw.Close();
		}
	}
}

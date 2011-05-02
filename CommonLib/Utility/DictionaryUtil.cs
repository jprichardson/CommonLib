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
			var keyConverter = TypeDescriptor.GetConverter(typeof(TKey)/*Type.GetType(types[0])*/);
			var valueConverter = TypeDescriptor.GetConverter(typeof(TValue));

			return LoadDictionaryList<TKey, TValue>(file, keyConverter, valueConverter);
		}

		public static IDictionary<TKey, List<TValue>> LoadDictionaryList<TKey, TValue>(string file, TypeConverter keyTypeConverter, TypeConverter valueTypeConverter) {
			IDictionary<TKey, List<TValue>> dictList = new Dictionary<TKey, List<TValue>>();
			if (!File.Exists(file))
				return dictList;

			var sr = new StreamReader(file);
			while (!sr.EndOfStream) {
				var fields = Csv.RecordSplit(sr.ReadLine(), ',', '"');
				TKey key = (TKey)keyTypeConverter.ConvertFromString(fields[0]);
				for (int i = 1; i < fields.Length; ++i) {
					TValue value = (TValue)valueTypeConverter.ConvertFromString(fields[i]);
					if (!dictList.ContainsKey(key))
						dictList.Add(key, new List<TValue>());

					dictList[key].Add(value);
				}
			}

			sr.Close();

			return dictList;
		}

		public static IDictionary<TKey, TValue> LoadDictionary<TKey, TValue>(string file) {
			return LoadDictionary<TKey, TValue>(file, TypeDescriptor.GetConverter(typeof(TKey)), TypeDescriptor.GetConverter(typeof(TValue)));
		}

		public static IDictionary<TKey, TValue> LoadDictionary<TKey, TValue>(string file, TypeConverter keyTypeConverter, TypeConverter valueTypeConverter) {
			IDictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
			if (!File.Exists(file))
				return dict;

			var sr = new StreamReader(file);
			while (!sr.EndOfStream) {
				var fields = Csv.RecordSplit(sr.ReadLine(), ',', '"');
				TKey key = (TKey)keyTypeConverter.ConvertFromString(fields[0]);
				TValue value = (TValue)valueTypeConverter.ConvertFromString(fields[1]);

				dict.Add(key, value);
			}

			sr.Close();

			return dict;
		}

		public static void SaveDictionaryList<TKey, TValue>(this IDictionary<TKey, List<TValue>> dictList, string file) {
			var sw = new StreamWriter(file);
			var sb = new StringBuilder(255);
			foreach (var key in dictList.Keys) {
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
				sb.Clear();
			}
			sw.Close();
		}

		public static void SaveDictionary<TKey, TValue>(this IDictionary<TKey, TValue> self, string file) {
			var sw = new StreamWriter(file);
			var sb = new StringBuilder(255);
			foreach (var key in self.Keys) {
				sb.Append('"');
				sb.Append(key);
				sb.Append("\",\"");
				sb.Append(self[key]);
				sb.Append('"');

				sw.WriteLine(sb.ToString());
				sb.Clear();
			}
			sw.Close();
		}
	}
}

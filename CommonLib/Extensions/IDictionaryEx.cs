using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using CommonLib.Utility;

namespace CommonLib.Extensions
{
	public static class IDictionaryEx
	{
		public static void Add<TKey,TValue>(this Dictionary<TKey, List<TValue>> dictList, TKey key, TValue value) {
			if (!dictList.ContainsKey(key))
				dictList.Add(key, new List<TValue>());

			dictList[key].Add(value);
		}

		public static string ToXmlString<K,V>(this IDictionary<K, V> self)  {
			var xmlData = new XElement("Dictionary",
				from key in self.Keys
				select new XElement("Item",
					new XAttribute("Key", key), 
					self[key]
			));

			return StringUtil.XElementToString(xmlData);
		}

		public static void RemoveAll<K, V>(this IDictionary<K, V> self, IEnumerable<K> keys) {
			foreach (var key in keys)
				self.Remove(key);
		}

		public static void Save<TKey, TValue>(this IDictionary<TKey, List<TValue>> data, string file) {

		}
	}
}

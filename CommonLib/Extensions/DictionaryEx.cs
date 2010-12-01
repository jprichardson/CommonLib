using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using CommonLib.Utility;

namespace CommonLib.Extensions
{
	public static class DictionaryEx
	{
		public static string ToXmlString<K,V>(this Dictionary<K, V> self)  {
			var xmlData = new XElement("Dictionary",
				from key in self.Keys
				select new XElement("Item",
					new XAttribute("Key", key), 
					self[key]
			));

			return StringUtil.XElementToString(xmlData);
		}
	}
}

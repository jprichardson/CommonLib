using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib.Collections
{
	public class SortedListDictionary<TKey, TValue> : SortedDictionary<TKey, List<TValue>>
	{
		public void Add(TKey key, TValue value) {
			if (!base.ContainsKey(key))
				base.Add(key, new List<TValue>());

			base[key].Add(value);
		}
	}
}

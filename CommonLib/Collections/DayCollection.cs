using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLib.Utility;

namespace CommonLib.Collections
{
	public class DayCollection<T> : SortedDictionary<DateTime, List<T>>
	{
		public DayCollection() {}

		public int DayCount { get { return this.Keys.Count; } }

		public IEnumerable<DateTime> Days { get { return this.Keys; } }

		public List<T> this[int index] { get { return this.GetItems(index); } }

		public void Add(DateTime dt, T obj){
			var startOfDay = DateTimeUtil.GetStartOfDay(dt);
			if (!this.ContainsKey(startOfDay))
				this.Add(startOfDay, new List<T>());

			this[startOfDay].Add(obj);
		}

		public List<T> GetItems(int index) {
			var count = 0;
			List<T> items = null;
			foreach (var day in this.Keys) {
				if (count <= index) {
					items = this[day];
					++count;
				}
				else
					break;
			}

			return items;
		}
	}
}

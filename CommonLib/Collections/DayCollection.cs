using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using CommonLib.Utility;

namespace CommonLib.Collections
{
	[Serializable]
	public class DayCollection<T> : SortedDictionary<DateTime, List<T>>//, ISerializable
	{
		public DayCollection() {}

		//protected DayCollection(SerializationInfo info, StreamingContext context) /*: base(info, context)*/ { }

		public int DayCount { get { return base.Keys.Count; } }

		public IEnumerable<DateTime> Days { get { return base.Keys; } }

		public List<T> this[int index] { get { return this.GetItems(index); } }

		public void Add(DateTime dt, T obj){
			var startOfDay = DateTimeUtil.GetStartOfDay(dt);
			if (!this.ContainsKey(startOfDay))
				base.Add(startOfDay, new List<T>());

			base[startOfDay].Add(obj);
		}

		public List<T> GetItems(int index) {
			var count = 0;
			List<T> items = null;
			foreach (var day in base.Keys) {
				if (count <= index) {
					items = base[day];
					++count;
				}
				else
					break;
			}

			return items;
		}

		//public virtual void GetObjectData(SerializationInfo info, StreamingContext context) { /*base.GetObjectData(info, context);*/ } //only needed if i have additional data to serialize
	}
}

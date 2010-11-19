using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//this may be removed

namespace CommonLib.Collections
{
	[Serializable]
	public class FlexDictionary<TKey, TValue> : Dictionary<TKey, TValue>
	{
		/*public FlexDictionary() {
			CreationFactory = () => { return default(TValue); };
		}*/

		public FlexDictionary(Func<TValue> creationFactory) {
			this.CreationFactory = creationFactory;
		}

		public Func<TValue> CreationFactory { get; set; }

		public new TValue this[TKey key]{ //hide parent []
			get {
				if (!base.ContainsKey(key))
					base.Add(key, this.CreationFactory.Invoke());

				return base[key];
			}

			set {
				if (!base.ContainsKey(key))
					base.Add(key, value);
				else
					base[key] = value;
			}
		}

		public new void Add(TKey key, TValue value){
			this[key] = value;
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;

namespace CommonLib.Collections
{
	public class ValueDictionary<V> : Dictionary<int, V>
	{
		private static Type _type = typeof(V);
		private static TypeConverter _converter = TypeDescriptor.GetConverter(_type);
		
		public ValueDictionary(V defaultReturn) : base() {
			this.DefaultReturn = defaultReturn;

			if (_converter == null)
				throw new Exception("Can't use ValueDictionary with " + _type);
		}

		public ValueDictionary(IDictionary<int, V> vdict, V defaultReturn) : base(vdict) {
			this.DefaultReturn = defaultReturn;

			if (_converter == null)
				throw new Exception("Can't use ValueDictionary with " + _type);
		}

		public V DefaultReturn { get; set; }

		//public int ActualCount { get { return base.Count; } }

		/*private int _desiredCount = -1;
		public int DesiredCount { //used to iterate over large set and get default values for those not present in the dictionary
			get {
				if (_desiredCount < 0)
					return this.ActualCount;
				else
					return _desiredCount;
			}
			set {
				_desiredCount = value;
			}
		}*/

		public new V this[int index] {
			set {
				if (!this.ContainsKey(index))
					this.Add(index, value);
				else
					base[index] = value;

				//address negative case
				if (index < 0) {
					if (!_negativeIndices.ContainsKey(index))
						_negativeIndices.Add(index, value);
					else
						_negativeIndices[index] = value;
				}
			}
			get {
				if (!this.ContainsKey(index))
					return this.DefaultReturn;
				else
					return base[index];
			}
		}

		private Dictionary<int, V> _negativeIndices = new Dictionary<int, V>();
		public Dictionary<int, V> NegativeIndices {
			get {
				return new Dictionary<int, V>(_negativeIndices);
			}
		}

		public static ValueDictionary<V> Load(string file) {
			var sr = new StreamReader(file);
			
			V defaultReturn = (V)_converter.ConvertFromString(sr.ReadLine());
			var bd = new ValueDictionary<V>(defaultReturn);

			while (!sr.EndOfStream) {
				var line = sr.ReadLine();
				var data = line.Split(',');
				bd.Add(Convert.ToInt32(data[0]), (V)_converter.ConvertFromString(data[1]));
			}
			sr.Close();

			return bd;
		}

		public new void Remove(int index){
			base.Remove(index);

			if (index < 0)
				if (_negativeIndices.ContainsKey(index))
					_negativeIndices.Remove(index);
		}

		public void Save(string file) {
			var fs = new FileStream(file, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
			var sw = new StreamWriter(fs);
			sw.WriteLine(this.DefaultReturn);

			foreach (var k in this.Keys)
				sw.WriteLine("{0},{1}", k, base[k].ToString());

			sw.Close();
		}
	}
}

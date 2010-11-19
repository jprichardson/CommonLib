using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;

//this may be renamed

namespace CommonLib.Collections
{
	[Serializable]
	public class BoolDictionary : Dictionary<int, bool>, ISerializable
	{
		public BoolDictionary() : base() { }

		public BoolDictionary(bool defaultReturn) : base() {
			this.DefaultReturn = defaultReturn;
		}

		public BoolDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }

		public bool DefaultReturn { get; set; } 

		public new bool this[int index] {
			set {
				if (!this.ContainsKey(index))
					this.Add(index, value);
				else
					base[index] = value;
			}
			get {
				if (!this.ContainsKey(index))
					return this.DefaultReturn;
				else
					return base[index];
			}
		}

		public virtual void GetObjectData(SerializationInfo info, StreamingContext context) { base.GetObjectData(info, context); }

		public static BoolDictionary Load(string file) {
			var sr = new StreamReader(file);

			bool defaultReturn = Convert.ToBoolean(sr.ReadLine());
			var bd = new BoolDictionary(defaultReturn);

			while (!sr.EndOfStream) {
				var line = sr.ReadLine();
				var data = line.Split(',');
				bd.Add(Convert.ToInt32(data[0]), Convert.ToBoolean(data[1]));
			}

			sr.Close();

			return bd;
		}

		public void Save(string file) {
			var sw = new StreamWriter(file);
			sw.WriteLine(this.DefaultReturn);

			foreach (var k in this.Keys)
				sw.WriteLine("{0},{1}", k, base[k]);

			sw.Close();
		}
	}
}

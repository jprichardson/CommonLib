using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace CommonLib.Collections
{
	[XmlRoot("ValueDictionary")]
	public class ValueDictionary<V> : Dictionary<int, V>, IXmlSerializable
	{
		private static Type _type = typeof(V);
		private static TypeConverter _converter = TypeDescriptor.GetConverter(_type);

		public ValueDictionary() : base() {
			this.AllowNegativeIndices = true;
			if (_converter == null)
				throw new Exception("Can't use ValueDictionary with " + _type);
		}
		
		public ValueDictionary(V defaultReturn) : base() {
			this.AllowNegativeIndices = true;
			this.DefaultReturn = defaultReturn;

			if (_converter == null)
				throw new Exception("Can't use ValueDictionary with " + _type);
		}

		public ValueDictionary(IDictionary<int, V> vdict, V defaultReturn) : base(vdict) {
			this.DefaultReturn = defaultReturn;
			this.AllowNegativeIndices = true;

			if (_converter == null)
				throw new Exception("Can't use ValueDictionary with " + _type);
		}

		//public new object Count { get { return null; } }

		
		public V DefaultReturn { get; set; }
		public bool AllowNegativeIndices { get; set; }

		

		public new V this[int index] {
			set {
				if (!this.ContainsKey(index))
					this.Add(index, value);
				else
					base[index] = value;

				//address negative case
				if (index < 0) {
					if (this.AllowNegativeIndices) {
						if (!_negativeIndices.ContainsKey(index))
							_negativeIndices.Add(index, value);
						else
							_negativeIndices[index] = value;
					}
					else
						throw new Exception("AllowNegativeIndices set to false. Please set to true if you want to allow negative indices.");
				}
			}
			get {
				if (!this.ContainsKey(index))
					return this.DefaultReturn;
				else
					return base[index];
			}
		}

		public XmlSchema GetSchema() {
			return null;
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

		public void ReadXml(XmlReader reader) {
			XmlSerializer keySerializer = new XmlSerializer(typeof(int));
			XmlSerializer valueSerializer = new XmlSerializer(typeof(V));

			bool wasEmpty = reader.IsEmptyElement;
			reader.Read();

			if (wasEmpty)
				return;

			reader.ReadStartElement("DefaultReturn");
			this.DefaultReturn = (V)valueSerializer.Deserialize(reader);
			reader.ReadEndElement();

			reader.ReadStartElement("AllowNegativeIndices");
			this.AllowNegativeIndices = (bool)valueSerializer.Deserialize(reader);
			reader.ReadEndElement();

			while (reader.NodeType != System.Xml.XmlNodeType.EndElement) {
				reader.ReadStartElement("Item");

				reader.ReadStartElement("Index");
				int key = (int)keySerializer.Deserialize(reader);
				reader.ReadEndElement();

				reader.ReadStartElement("Value");
				V value = (V)valueSerializer.Deserialize(reader);
				reader.ReadEndElement();

				this.Add(key, value);

				reader.ReadEndElement();
				reader.MoveToContent();
			}
			
			reader.ReadEndElement();
		}

		public new void Remove(int index) {
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

		public void WriteXml(XmlWriter writer) {
			XmlSerializer keySerializer = new XmlSerializer(typeof(int));
			XmlSerializer valueSerializer = new XmlSerializer(typeof(V));

			writer.WriteStartElement("DefaultReturn");
			valueSerializer.Serialize(writer, this.DefaultReturn);
			writer.WriteEndElement();

			writer.WriteStartElement("AllowNegativeIndices");
			valueSerializer.Serialize(writer, this.AllowNegativeIndices);
			writer.WriteEndElement();

			foreach (int key in this.Keys) {
				writer.WriteStartElement("Item");

				writer.WriteStartElement("Index");
				keySerializer.Serialize(writer, key);
				writer.WriteEndElement();

				writer.WriteStartElement("Value");
				V value = this[key];
				valueSerializer.Serialize(writer, value);
				writer.WriteEndElement();

				writer.WriteEndElement();
			}
		}
	}
}

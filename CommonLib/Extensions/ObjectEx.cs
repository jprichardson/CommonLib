using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace CommonLib.Extensions
{
	public static class ObjectEx
	{
		public enum SerializationStrategy { Xml, Binary }

		public static T DeepClone<T>(this T self) {
			return self.DeepClone<T>(SerializationStrategy.Binary);
		}

		public static T DeepClone<T>(this T self, SerializationStrategy strategy) {
			switch (strategy) {
				case SerializationStrategy.Xml:
					XmlSerializer xs = new XmlSerializer(typeof(T));
					StringWriter os = new StringWriter();
					xs.Serialize(os, self);

					StringReader sr = new StringReader(os.ToString());
					T retXml = (T)xs.Deserialize(sr);

					os.Close();
					sr.Close();

					return retXml;
				case SerializationStrategy.Binary:
					var bf = new BinaryFormatter();
					var ms = new MemoryStream();
					bf.Serialize(ms, self);
					ms.Position = 0;

					T retBin = (T)bf.Deserialize(ms);

					ms.Close();
					return retBin;
			}

			return default(T);
		}
	}
}

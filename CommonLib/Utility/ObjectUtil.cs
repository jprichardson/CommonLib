using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace CommonLib.Utility
{
	public static class ObjectUtil
	{
		public enum SerializationStrategy { Xml, Binary }

		public static T DeepClone<T>(this T self) {
			return self.DeepClone<T>(SerializationStrategy.Xml);
		}

		public static T DeepClone<T>(this T self, SerializationStrategy strategy) {
			switch (strategy) {
				case SerializationStrategy.Xml:
					XmlSerializer xs = new XmlSerializer(typeof(T));
					StringWriter os = new StringWriter();
					xs.Serialize(os, self);

					StringReader sr = new StringReader(os.ToString());
					T ret = (T)xs.Deserialize(sr);

					os.Close();
					sr.Close();

					return ret;
				case SerializationStrategy.Binary:
					/*var bf = new BinaryFormatter();
					var ms = new MemoryStream();
					bf.Serialize(ms, self);*/
					throw new NotImplementedException("Binary not implemented.");
			}

			return default(T);
		}
	}
}

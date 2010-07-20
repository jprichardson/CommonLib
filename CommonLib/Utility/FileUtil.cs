using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace CommonLib.Utility
{
	public static class FileUtil
	{
		public static T DeserializeFromBytes<T>(byte[] bytes) {
			var bf = new BinaryFormatter();
			Stream s = new MemoryStream(bytes);
			T ret = (T)bf.Deserialize(s);
			s.Close();
			return ret;
		}

		public static T ReadFromBinFile<T>(string file) {
			BinaryFormatter bf = new BinaryFormatter();
			Stream s = new FileStream(file, FileMode.Open);
			T ret = (T)bf.Deserialize(s);
			s.Close();
			return ret;
		} 

		public static T ReadFromXmlFile<T>(string file) {
			XmlSerializer xs = new XmlSerializer(typeof(T));
			StreamReader sr = new StreamReader(file);
			T ret = (T)xs.Deserialize(sr);
			sr.Close();
			return ret;
		}

		public static void WriteToBinFile<T>(T obj, string file) {
			BinaryFormatter bf = new BinaryFormatter();
			Stream s = new FileStream(file, FileMode.Create);
			bf.Serialize(s, obj);
			s.Close();
		}

		public static void WriteToXmlFile<T>(T obj, string file) {
			XmlSerializer xs = new XmlSerializer(typeof(T));
			StreamWriter sw = new StreamWriter(file);
			xs.Serialize(sw, obj);
			sw.Close();
		}
}

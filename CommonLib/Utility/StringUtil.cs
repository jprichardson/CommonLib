using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CommonLib.Extensions;
using System.Xml;
using System.Xml.Linq;
using CommonLib.IO;
using System.Globalization;

namespace CommonLib.Utility
{
	public static class StringUtil
	{

		public static string ByteArrayToHexString(byte[] bytes) {
			return String.Concat(Array.ConvertAll(bytes, x => x.ToString("X2")));
		}
		
		public static byte[] HexStringToByteArray(string hex) {
			return Enumerable.Range(0, hex.Length).
				   Where(x => 0 == x % 2).
				   Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).
				   ToArray();
		}

		public static string ToCsv(params string[] items) {
			var sb = new StringBuilder(255);
			foreach (var i in items) {
				sb.Append('"');
				sb.Append(i);
				sb.Append('"');
				sb.Append(',');
			}

			return sb.ToString().Chop();
		}

		public static string XElementToString(XElement xml){
			var sw = new StringWriterUTF8(CultureInfo.CurrentCulture);
			var writer = XmlWriter.Create(sw, new XmlWriterSettings() { Indent = true, IndentChars = "\t", Encoding = Encoding.UTF8 });
			xml.WriteTo(writer);
			writer.Flush();
			writer.Close();
			return sw.ToString();
		}
	}
}

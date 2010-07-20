using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CommonLib.Utility
{
	public static class StringUtil
	{
		//Monkey-Patching
		//public static string[] SplsitNewLine(this string s){
		//    return s.Split(new string[]{
		//}

		public static byte[] HexStringToByteArray(string hex) {
			return Enumerable.Range(0, hex.Length).
				   Where(x => 0 == x % 2).
				   Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).
				   ToArray();
		}

		public static string ByteArrayToHexString(byte[] bytes) {
			 return String.Concat(Array.ConvertAll(bytes, x => x.ToString("X2")));
		}

		//identical to Ruby's chop
		public static string Chop(this string self) {
			if (self.Length == 0)
				return self;

			if (self.Length == 1)
				return "";

			if (self.LastCharAt(-1) == '\n' && self.LastCharAt(-2) == '\r')
				return self.Substring(0, self.Length - 2);
			else
				return self.Substring(0, self.Length - 1);
		}

		public static string FirstLine(this string self) {
			int pos = self.IndexOf(Environment.NewLine);
			if (pos == -1)
				return self;

			return self.Substring(0, pos);
		}

		public static bool IsAlphaNumeric(this string s) {
			return !(new Regex("[^a-zA-Z0-9]")).IsMatch(s);
		}

		public static char LastCharAt(this string self, int pos) {
			return self[self.Length - pos];
		}

		public static string MakeProper(this string s) {
			return s[0].ToString().ToUpper() + s.Substring(1).ToLower();
		}
	}
}

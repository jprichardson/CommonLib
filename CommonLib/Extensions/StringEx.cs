using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CommonLib.Extensions
{
	public static class StringEx
	{
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

		public static int CountLines(this string self) {
			int pos = 0;
			int count = 1;
			do {
				pos = self.IndexOf(Environment.NewLine, pos);
				if (pos >= 0)
					++count;
				++pos;
			} while (pos > 0);

			return count;
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

		public static string TrimEnd(this string self, int len) {
			return self.Substring(0, self.Length - len);
		}
	}
}

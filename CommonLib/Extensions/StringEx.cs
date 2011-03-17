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
			return self[self.Length + pos];
		}

		public static string MakeProper(this string s) {
			return s[0].ToString().ToUpper() + s.Substring(1).ToLower();
		}

		public static string RemoveLastChar(this string self) {
			if (self.Length >= 1)
				return self.Remove(self.Length - 1, 1);
			else
				return self;
		}

		public static IEnumerable<string> SplitAt(this string self, params int[] positions) {
			var ret = new List<string>();

			if (positions == null) { ret.Add(self); return ret; }

			var poses = positions.Distinct().OrderBy(n => n).ToList();

			var indicesToRemove = new Queue<int>();
			var total = poses.Count();
			var i = 0;
			while (i < total){
				if ((poses[i] <= 0) || (poses[i] >= self.Length))
					indicesToRemove.Enqueue(poses[i]);
				++i;
			}

			while (indicesToRemove.Count > 0)
				poses.Remove(indicesToRemove.Dequeue());

			switch (poses.Count) {
				case 0:
					ret.Add(self); return ret;
				case 1:
					ret.Add(self.Substring(0, poses[0])); ret.Add(self.Substring(poses[0], self.Length - poses[0])); return ret;
				default:
					var sb = new StringBuilder(255);
					var pos1 = 0;
					var len = poses[0];
					ret.Add(self.Substring(pos1, len));

					pos1 = poses[0];
					for (int j = 1; j <= poses.Count; ++j) {
						if (j == poses.Count)
							len = self.Length - poses[j -1];
						else
							len = poses[j] - poses[j - 1];
						
						ret.Add(self.Substring(pos1, len));

						if (j < poses.Count)
							pos1 = poses[j];
					}
					return ret;
			}
		}

		public static string TrimEnd(this string self, int len) {
			return self.Substring(0, self.Length - len);
		}
	}
}

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

		
	}
}

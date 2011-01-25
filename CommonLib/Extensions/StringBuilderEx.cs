using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib.Extensions
{
	public static class StringBuilderEx
	{
		public static void RemoveLastChar(this StringBuilder sb) {
			if (sb.Length >= 1)
				sb.Remove(sb.Length - 1, 1);
		}
	}
}

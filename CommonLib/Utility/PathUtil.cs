using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CommonLib.Utility
{
	public static class PathUtil
	{
		public static string GetPathAndFileNameWithoutExtension(string file) {
			var fn = Path.GetFileNameWithoutExtension(file);
			var path = Path.GetDirectoryName(file);
			var ret = "";
			if (path != "")
				ret = path + Path.DirectorySeparatorChar + fn;
			else
				ret = fn;

			return ret;
		}
	}
}

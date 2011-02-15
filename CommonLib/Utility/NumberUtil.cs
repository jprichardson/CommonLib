using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace CommonLib.Utility
{
	public class NumberUtil
	{
		//I shoudl probably put an Endian specifier here...
		public static BitArray ConvertNibbleToBitarray(byte b) {
			int byt = b;
			byt &= 0xF;
			BitArray ba = new BitArray(4, false);
			for (int x = 3; x >= 0; --x) {
				var test = byt & 1;
				ba[x] = (test == 1);
				byt >>= 1;
			}
			return ba;
		}

		public static string TrimZeros(string num) {
			var d = Convert.ToDecimal(num);
			string ret = null;

			int pos = num.IndexOf('.');
			if (pos > 0) {
				var data = num.Split('.');
				data[1] = data[1].TrimEnd('0');
				if (data[1].Length == 0) //they were all zeros
					data[1] = "0";
				ret = data[0] + '.' + data[1];
			}
			else
				ret = num;

			var dret = Convert.ToDecimal(ret);

			if (d != dret)
				throw new Exception(string.Format("Bug in TrimZeros. {0} != {1}", d, dret));

			return ret;
		}


	}
}

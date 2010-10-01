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
	}
}

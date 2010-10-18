using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CommonLib.Extensions
{
	public static class RandomEx
	{
		//may not have even distribution, must test
		public static long NextLong(this Random rnd, long min, long max) {
			if (max <= min)
				throw new Exception("Max must be less than min.");

			long dif = max - min;

			var bytes = new byte[8];
			rnd.NextBytes(bytes);
			bytes[7] &= 0x7f; //strip sign bit

			long posNum = BitConverter.ToInt64(bytes, 0);
			while (posNum > dif)
				posNum >>= 1;

			return min + posNum;
		}

		//didn't have even distribution, remove
		/*public static DateTime NextDateTime(this Random rnd, DateTime min, DateTime max) {
			if (max.Ticks <= min.Ticks)
				throw new Exception("Max must be less than min.");

			var ticks = rnd.NextLong(min.Ticks, max.Ticks);
			return new DateTime(ticks);
		}*/

		public static DateTime NextDateTime(this Random rnd, DateTime min, DateTime max) {
			var milliRange = (max - min).TotalMilliseconds;

			var millis = (long)milliRange;
			var num = (long)(rnd.NextDouble() * millis);
			return min.AddMilliseconds(num);
		}

		
	}
}

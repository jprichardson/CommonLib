using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib.Extensions
{
	public static class IEnumerableEx
	{
		/*public static double StandardDeviation<T>(this IEnumerable<T> values) {
			int count = values.Count();
			if (count > 1) {
				double avg = values.Average();
				return Math.Sqrt(values.Average(t => Math.Pow(t - avg, 2)));
			}
			else
				return 0;
		}*/

		public static double StandardDeviation(this IEnumerable<double> values) {
			return values.StandardDeviation(x => x);
		}

		public static double StandardDeviation<T>(this IEnumerable<T> values, Func<T,double> func) {
			int count = values.Count();
			if(count > 1){
				double mean = values.Average(func);
				var sum = values.Sum(x => Math.Pow(func(x) - mean, 2));
				var var = sum / (count - 1);
				return Math.Sqrt(var);
			} else 
				return 0;
		}
	}
}

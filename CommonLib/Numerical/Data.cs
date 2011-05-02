using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib.Numerical
{
	public class Data
	{
		public static void LeastSquaresCoef(double[] Y, double[] X, out double m, out double b, out double stdErrorM, out double stdErrorB) {
			int N = X.Length;
			
			//delta = N*sum(xi^2) - sum(xi)^2
			double delta = X.Length * SumInSquare(X) - Math.Pow(Sum(X), 2.0);

			//b = (1/delta)*(sum(xi^2)*sum(yi) - sum(xi)*sum(xi*yi))
			double bt = (1 / delta) * (SumInSquare(X) * Sum(Y) - Sum(X) * SumInMultiply(Y, X));

			//m = (1/delta)*(N*sum(xi*yi)-sum(xi)*sum(yi))
			double mt = (1 / delta) * (X.Length * SumInMultiply(Y, X) - Sum(X) * Sum(Y));

			m = mt;
			b = bt;

			double var_sum = 0.0;
			for (int i = 0; i < X.Length; i++){
				var_sum += Math.Pow(Y[i] - b - m*X[i],2);
			}

			double var = 1.0 / (N - 2.0) * var_sum;

			stdErrorB = Math.Sqrt(var / delta * SumInSquare(X));
			stdErrorM = Math.Sqrt(N / delta * var);
		}

		public static double LeastSquaresCoefThroughOrigin(double[] Y, double[] X) {
			//m = sum(xi*yi)/sum(xi^2)
			double m = SumInMultiply(Y, X) / SumInSquare(X);
			return m;
		}

		public static double[] LeastSquaresThroughOrigin(double[] Y, double[] X) {
			double m = LeastSquaresCoefThroughOrigin(Y, X);

			double[] newY = new double[Y.Length];
			for (int i = 0; i < Y.Length; i++)
				newY[i] = X[i] * m;
			return newY;
		}

		public static double PercentDifference(double x1, double x2) {
			var top = x1 - x2;
			var bot = (x1 + x2) / 2.0;
			return Math.Abs(top / bot);
		}

		//
		public static double PercentChange(double newX, double oldX) {
			return (newX - oldX) / oldX;
		}

		public static double PercentError(double experimental, double theoretical) {
			return (experimental - theoretical) / theoretical;
		}

		public static double PercentErrorAbs(double experimental, double theoretical) {
			return Math.Abs(PercentError(experimental, theoretical));
		}

		//http://en.wikipedia.org/wiki/Bessel's_correction
		public static double StandardDeviation(double[] X) {
			if (X.Length > 1) {
				var mean = X.Average();
				var sum = X.Sum(x => Math.Pow(x - mean, 2));
				var var = sum / (X.Length - 1);

				return Math.Sqrt(var);
			}
			else
				return 0.0;
		}

		/*public static double[] LeastSquaresLine(double[] Y, double[] x) {

		}*/

		private static double Sii(double[] i1, double[] i2) {
			return 0;
		}

		private static double Sum(double[] nums) {
			double d = 0;
			foreach (double n in nums)
				d += n;

			return d;
		}

		private static double SumInSquare(double[] nums) {
			double d = 0;

			for (int i = 0; i < nums.Length; i++)
				d += Math.Pow(nums[i], 2.0);

			return d;
		}

		private static double SumInMultiply(double[] Y, double[] X) {
			double d = 0;

			if (Y.Length != X.Length)
				throw new Exception("Lengths must be equal.");

			for (int i = 0; i < X.Length; i++)
				d += (X[i] * Y[i]);

			return d;
		}
	}
}

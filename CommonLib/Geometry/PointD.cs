using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib.Geometry
{
	public class PointD
	{
		public PointD() : this(0.0, 0.0) { }

		public PointD(double x, double y) { this.X = x; this.Y = y; }

		public double X;
		public double Y;
		public double Residual;

		public object Tag; //extra data

		public override bool Equals(object obj) {
			var p = obj as PointD;
			if (p == null)
				return false;
			return p.X == this.X && p.Y == this.Y;
		}

		public override int GetHashCode() {
			return this.X.GetHashCode() + this.Y.GetHashCode();
		}

		public override string ToString() {
			return string.Format("{0},{1}", X, Y);
		}


	}
}

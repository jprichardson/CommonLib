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
	}
}

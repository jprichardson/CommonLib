using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CommonLib.Geometry;

namespace CommonLib.Numerical
{
	public class XYDataSet : IList<PointD>
	{
		
		private List<PointD> _internalList = new List<PointD>();

		public XYDataSet() : this(null, null) { }

		public XYDataSet(IEnumerable<PointD> points) {
			ResetValues();

			foreach (var point in points)
				Add(point);
		}

		public XYDataSet(IEnumerable<double> Xs, IEnumerable<double> Ys) {
			ResetValues();
			
			if (Xs != null || Ys != null) {
				if (Xs.Count() != Ys.Count())
					throw new Exception("X count must be the same as the Y count.");

				for (int i = 0; i < Xs.Count(); ++i)
					Add(Xs.ElementAt(i), Ys.ElementAt(i));
			}
		}

		public int Count { get { return _internalList.Count; } }

		public bool IsReadOnly { get { return false; } }

		private double _maxX = Double.NegativeInfinity;
		public double XMax { get { return _internalList[XMaxIndex].X; } }
		private double _minX = Double.PositiveInfinity;
		public double XMin { get { return _internalList[XMinIndex].X; } }
		public int XMaxIndex { get; protected set; }
		public int XMinIndex { get; protected set; }

		private double _maxY = Double.NegativeInfinity;
		public double YMax { get { return _internalList[YMaxIndex].Y; } }
		private double _minY = Double.PositiveInfinity;
		public double YMin { get { return _internalList[YMinIndex].Y; } }
		public int YMaxIndex { get; protected set; }
		public int YMinIndex { get; protected set; }

		public double XMean { get { return XSum / Count; } }
		public double YMean { get { return YSum / Count; } }

		public double RSquare { get; protected set; }

		public PointD RegressionPoint0 { get; protected set; }
		public PointD RegressionPointN { get; protected set; }

		public double ResidualStandardDeviation { get; protected set; }

		public double Slope { get; protected set; }

		public double XSum { get; set; }
		public double YSum { get; set; }
		
		public double XSquaredSum { get; set; }
		public double YSquaredSum { get; set; }
		
		public double XYProductSum { get; set; }

		public double XIntercept { get { return -YIntercept / Slope; } }
		public double YIntercept { get; protected set; }


		public PointD this[int index] {
			get { return _internalList[index]; }
			set {
				var p = value;
				var old = _internalList[index];
				_internalList[index] = p;

				ComputeSums(old, SumMode.Subtract);
				ComputeSums(p, SumMode.Add);
				ComputeMinAndMax();
				ComputeSlopeAndYIntercept();
			}
		}

		public void Add(double x, double y) {
			Add(new PointD(x, y));
		}

		public void Add(PointD p) {
			_internalList.Add(p);
			RSquare = double.NaN;

			ComputeSums(p, SumMode.Add);
			ComputeMinAndMax(Count - 1, p);
			ComputeSlopeAndYIntercept();
		}

		public void Clear() {
			_internalList.Clear();
			ResetValues();
		}

		public void ComputeSlopeAndYIntercept() {
			double delta = Count * XSquaredSum - Math.Pow(XSum, 2.0);
			YIntercept = (1.0 / delta) * (XSquaredSum * YSum - XSum * XYProductSum);
			Slope = (1.0 / delta) * (Count * XYProductSum - XSum * YSum);

			RegressionPoint0.X = XMin;
			RegressionPoint0.Y = Slope * XMin + YIntercept;
			RegressionPointN.X = XMax;
			RegressionPointN.Y = Slope * XMax + YIntercept;
		}

		public void ComputeResiduals() {
			for (int i = 0; i < Count; ++i) {
				var yline = Slope * _internalList[i].X + YIntercept;
				var ydelta = _internalList[i].Y - yline;
				_internalList[i].Residual = ydelta;
			}
		}

		public double ComputeRSquared() {
			var SStot = _internalList.Sum(p => Math.Pow(p.Y - YMean, 2.0));
			var SSerr = _internalList.Sum(p => Math.Pow(p.Y - (Slope * p.X + YIntercept), 2.0));
			RSquare = 1.0 - SSerr / SStot;
			return RSquare;
		}

		public double ComputeStandardDeviationOnResiduals(){
			var res = _internalList.Select(p => p.Residual);
			ResidualStandardDeviation = Data.StandardDeviation(res.ToArray());
			return ResidualStandardDeviation;
		}

		public bool Contains(PointD p) {
			return _internalList.Contains(p);
		}

		public void CopyTo(PointD[] points, int index) {
			_internalList.CopyTo(points, index);
		}

		public IEnumerable<PointD> FilterByResidualStandardDeviation(double multiplier) {
			var filter = multiplier * ResidualStandardDeviation;
			var keep = new List<PointD>();
			var reject = new List<PointD>();

			foreach (var p in _internalList)
				if (Math.Abs(p.Residual) < filter)
					keep.Add(p);
				else
					reject.Add(p);

			Clear();
			foreach (var p in keep)
				Add(p);

			return reject;
		}

		public IEnumerator<PointD> GetEnumerator() {
			return _internalList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return _internalList.GetEnumerator();
		}

		public int IndexOf(PointD p) {
			return _internalList.IndexOf(p);
		}

		public void Insert(int index, PointD p) {
			_internalList.Insert(index, p);
			RSquare = double.NaN;

			ComputeSums(p, SumMode.Add);
			ComputeMinAndMax();
			ComputeSlopeAndYIntercept();
		}

		public bool Remove(PointD p) {
			var success = _internalList.Remove(p);
			if (success) {
				RSquare = double.NaN;
				ComputeSums(p, SumMode.Subtract);
				ComputeMinAndMax();
				ComputeSlopeAndYIntercept();
			}
			return success;
		}

		public void RemoveAt(int index) {
			var old = _internalList[index];
			_internalList.RemoveAt(index);
			RSquare = double.NaN;

			ComputeSums(old, SumMode.Subtract);
			ComputeMinAndMax();
			ComputeSlopeAndYIntercept();
		}

		protected void ComputeMinAndMax() { //methods that call this, Insert, 
			ResetMinAndMax();

			for (int i = 0; i < _internalList.Count; ++i)
				ComputeMinAndMax(i, _internalList[i]);
		}

		protected void ComputeMinAndMax(int index, PointD newPoint) {
			if (newPoint.X <= _minX) {
				_minX = newPoint.X;
				XMinIndex = index;
			}

			if (newPoint.X >= _maxX) {
				_maxX = newPoint.X;
				XMaxIndex = index;
			}

			if (newPoint.Y <= _minY) {
				_minY = newPoint.Y;
				YMinIndex = index;
			}

			if (newPoint.Y >= _maxY) {
				_maxY = newPoint.Y;
				YMaxIndex = index;
			}
		}

		protected enum SumMode { Add, Subtract };
		protected void ComputeSums(PointD p, SumMode mode) {
			if (mode == SumMode.Add) {
				XSum += p.X;
				YSum += p.Y;
				XSquaredSum += Math.Pow(p.X, 2.0);
				YSquaredSum += Math.Pow(p.Y, 2.0);
				XYProductSum += (p.X * p.Y);
			}
			else if (mode == SumMode.Subtract) {
				XSum -= p.X;
				YSum -= p.Y;
				XSquaredSum -= Math.Pow(p.X, 2.0);
				YSquaredSum -= Math.Pow(p.Y, 2.0);
				XYProductSum -= (p.X * p.Y);
			}
		}

		protected void ResetMinAndMax() {
			_maxX = double.NegativeInfinity;
			_maxY = double.NegativeInfinity;
			_minX = double.PositiveInfinity;
			_minY = double.PositiveInfinity;
		}

		protected void ResetValues() {
			ResetMinAndMax();

			RegressionPoint0 = new PointD();
			RegressionPointN = new PointD();

			RSquare = double.NaN;

			Slope = double.NaN;
			YIntercept = double.NaN;

			XSum = 0.0;
			YSum = 0.0;
			XSquaredSum = 0.0;
			XYProductSum = 0.0;

			XMaxIndex = -1;
			YMaxIndex = -1;
			XMinIndex = -1;
			YMinIndex = -1;
		}
		
	}
}

using CommonLib.Numerical;
using CommonLib.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TestCommonLib
{
 
	[TestClass()]
	public class XYDataSetTest
	{
		private TestContext testContextInstance;

		public TestContext TestContext {
			get {
				return testContextInstance;
			}
			set {
				testContextInstance = value;
			}
		}

		[TestMethod()]
		public void XYDataSetConstructorTest() {
			var ds = new XYDataSet();
			
			Assert.AreEqual(-1, ds.XMaxIndex);
			Assert.AreEqual(-1, ds.XMinIndex);
			Assert.AreEqual(-1, ds.YMaxIndex);
			Assert.AreEqual(-1, ds.YMinIndex);
			Assert.AreEqual(double.NaN, Math.Round(ds.Slope, 4));
			Assert.AreEqual(double.NaN, Math.Round(ds.YIntercept, 4));
			Assert.AreEqual(double.NaN, Math.Round(ds.XIntercept, 4));

			bool error = false;
			try { Assert.AreEqual(2000, ds.XMin); } catch { error = true; }
			Assert.IsTrue(error);

			error = false;
			try { Assert.AreEqual(2000, ds.XMax); } catch { error = true; }
			Assert.IsTrue(error);

			error = false;
			try { Assert.AreEqual(2000, ds.YMin); } catch { error = true; }
			Assert.IsTrue(error);

			error = false;
			try { Assert.AreEqual(2000, ds.YMin); } catch { error = true; }
			Assert.IsTrue(error);
		}

		[TestMethod()]
		public void XYDataSetConstructorTest2() {
			double[] X = {2000, 2001, 2002, 2003, 2004};
			double[] Y = {9.34, 8.50, 7.62, 6.93, 6.60};

			var ds = new XYDataSet(X, Y);
			Assert.AreEqual(2000, ds.XMin);
			Assert.AreEqual(2004, ds.XMax);
			Assert.AreEqual(6.60, ds.YMin);
			Assert.AreEqual(9.34, ds.YMax);
			Assert.AreEqual(4, ds.XMaxIndex);
			Assert.AreEqual(0, ds.XMinIndex);
			Assert.AreEqual(0, ds.YMaxIndex);
			Assert.AreEqual(4, ds.YMinIndex);
			Assert.AreEqual(-0.705, Math.Round(ds.Slope,4));
			Assert.AreEqual(1419.208, Math.Round(ds.YIntercept,4));
			Assert.AreEqual(2013.061, Math.Round(ds.XIntercept, 4));

			X = new[]{ 2003.0, 2001, 2004, 2000, 2002 };
			Y = new[]{ 6.93, 8.50, 6.60, 9.34, 7.62 };

			ds = new XYDataSet(X, Y);
			Assert.AreEqual(2000, ds.XMin);
			Assert.AreEqual(2004, ds.XMax);
			Assert.AreEqual(6.60, ds.YMin);
			Assert.AreEqual(9.34, ds.YMax);
			Assert.AreEqual(2, ds.XMaxIndex);
			Assert.AreEqual(3, ds.XMinIndex);
			Assert.AreEqual(3, ds.YMaxIndex);
			Assert.AreEqual(2, ds.YMinIndex);
			Assert.AreEqual(-0.705, Math.Round(ds.Slope, 4));
			Assert.AreEqual(1419.208, Math.Round(ds.YIntercept, 4));
			Assert.AreEqual(2013.061, Math.Round(ds.XIntercept, 4));
		}

		[TestMethod()]
		public void XYDataSetConstructorTest3() {
			var X = new[] { 2003.0, 2001, 2004, 2000, 2002 };
			var Y = new[] { 6.93, 8.50, 6.60, 9.34, 7.62 };

			var list = new List<PointD>();
			for (int i = 0; i < X.Length; ++i)
				list.Add(new PointD(X[i], Y[i]));

			var ds = new XYDataSet(list);
			Assert.AreEqual(2000, ds.XMin);
			Assert.AreEqual(2004, ds.XMax);
			Assert.AreEqual(6.60, ds.YMin);
			Assert.AreEqual(9.34, ds.YMax);
			Assert.AreEqual(-0.705, Math.Round(ds.Slope, 4));
			Assert.AreEqual(1419.208, Math.Round(ds.YIntercept, 4));
			Assert.AreEqual(2013.061, Math.Round(ds.XIntercept, 4));
		}

		[TestMethod()]
		public void AddTest() {
			var X = new[] { 2003.0, 2001, 2004, 2000, 2002 };
			var Y = new[] { 6.93, 8.50, 6.60, 9.34, 7.62 };

			var ds = new XYDataSet(X, Y);
			Assert.AreEqual(2000, ds.XMin);
			Assert.AreEqual(2004, ds.XMax);
			Assert.AreEqual(6.60, ds.YMin);
			Assert.AreEqual(9.34, ds.YMax);
			Assert.AreEqual(2, ds.XMaxIndex);
			Assert.AreEqual(3, ds.XMinIndex);
			Assert.AreEqual(3, ds.YMaxIndex);
			Assert.AreEqual(2, ds.YMinIndex);
			Assert.AreEqual(-0.705, Math.Round(ds.Slope, 4));
			Assert.AreEqual(1419.208, Math.Round(ds.YIntercept, 4));
			Assert.AreEqual(2013.061, Math.Round(ds.XIntercept, 4));
			Assert.AreEqual(2002, Math.Round(ds.XMean, 4));
			Assert.AreEqual(7.798, Math.Round(ds.YMean, 4));

			ds.Add(2005, 5.90);
			Assert.AreEqual(2000, ds.XMin);
			Assert.AreEqual(2005, ds.XMax);
			Assert.AreEqual(5.90, ds.YMin);
			Assert.AreEqual(9.34, ds.YMax);
			Assert.AreEqual(5, ds.XMaxIndex);
			Assert.AreEqual(3, ds.XMinIndex);
			Assert.AreEqual(3, ds.YMaxIndex);
			Assert.AreEqual(5, ds.YMinIndex);
			Assert.AreEqual(-0.674, Math.Round(ds.Slope, 4));
			Assert.AreEqual(1357.1667, Math.Round(ds.YIntercept, 4));
			Assert.AreEqual(2013.6004, Math.Round(ds.XIntercept, 4));
			Assert.AreEqual(2002.5, Math.Round(ds.XMean, 4));
			Assert.AreEqual(7.4817, Math.Round(ds.YMean, 4));
		}

		[TestMethod()]
		public void ThisIndexTest() {
			var X = new[] { 2003.0, 2001, 2004, 2000, 2002 };
			var Y = new[] { 6.93, 8.50, 6.60, 9.34, 7.62 };
			var ds = new XYDataSet(X, Y);

			ds[3] = new PointD(2005, 5.90);
			Assert.AreEqual(2001, ds.XMin);
			Assert.AreEqual(2005, ds.XMax);
			Assert.AreEqual(5.90, ds.YMin);
			Assert.AreEqual(8.50, ds.YMax);
			Assert.AreEqual(-0.622, Math.Round(ds.Slope, 4));
			Assert.AreEqual(1252.976, Math.Round(ds.YIntercept, 4));
			Assert.AreEqual(2014.4309, Math.Round(ds.XIntercept, 4));

			ds.Add(2000, 9.34);
			Assert.AreEqual(2000, ds.XMin);
			Assert.AreEqual(2005, ds.XMax);
			Assert.AreEqual(5.90, ds.YMin);
			Assert.AreEqual(9.34, ds.YMax);
			Assert.AreEqual(-0.674, Math.Round(ds.Slope, 4));
			Assert.AreEqual(1357.1667, Math.Round(ds.YIntercept, 4));
			Assert.AreEqual(2013.6004, Math.Round(ds.XIntercept, 4));
		}

		[TestMethod()]
		public void ClearAndCountTest() {
			var X = new[] { 2003.0, 2001, 2004, 2000, 2002 };
			var Y = new[] { 6.93, 8.50, 6.60, 9.34, 7.62 };
			var ds = new XYDataSet(X, Y);

			Assert.AreEqual(5, ds.Count);
			ds.Clear();
			Assert.AreEqual(0, ds.Count);

			Assert.AreEqual(-1, ds.XMaxIndex);
			Assert.AreEqual(-1, ds.XMinIndex);
			Assert.AreEqual(-1, ds.YMaxIndex);
			Assert.AreEqual(-1, ds.YMinIndex);
			Assert.AreEqual(double.NaN, Math.Round(ds.Slope, 4));
			Assert.AreEqual(double.NaN, Math.Round(ds.YIntercept, 4));
			Assert.AreEqual(double.NaN, Math.Round(ds.XIntercept, 4));

			bool error = false;
			try { Assert.AreEqual(2000, ds.XMin); }
			catch { error = true; }
			Assert.IsTrue(error);

			error = false;
			try { Assert.AreEqual(2000, ds.XMax); }
			catch { error = true; }
			Assert.IsTrue(error);

			error = false;
			try { Assert.AreEqual(2000, ds.YMin); }
			catch { error = true; }
			Assert.IsTrue(error);

			error = false;
			try { Assert.AreEqual(2000, ds.YMin); }
			catch { error = true; }
			Assert.IsTrue(error);
		}

		[TestMethod()]
		public void ContainsTest() {
			var X = new[] { 2003.0, 2001, 2004, 2000, 2002 };
			var Y = new[] { 6.93, 8.50, 6.60, 9.34, 7.62 };
			var ds = new XYDataSet(X, Y);

			Assert.IsFalse(ds.Contains(new PointD(2003, 10000.0)));
			Assert.IsTrue(ds.Contains(new PointD(2003, 6.93)));
		}

		[TestMethod()]
		public void IndexOfTest() {
			var X = new[] { 2003.0, 2001, 2004, 2000, 2002 };
			var Y = new[] { 6.93, 8.50, 6.60, 9.34, 7.62 };
			var ds = new XYDataSet(X, Y);

			Assert.AreEqual(-1, ds.IndexOf(new PointD(2022, 9.34)));
			Assert.AreEqual(3, ds.IndexOf(new PointD(2000, 9.34)));
		}

		[TestMethod()]
		public void InsertTest() {
			var X = new[] { 2003.0, 2001, 2004, 2000, 2002 };
			var Y = new[] { 6.93, 8.50, 6.60, 9.34, 7.62 };
			var ds = new XYDataSet(X, Y);

			ds.Insert(1, new PointD(2005, 5.90));
			Assert.AreEqual(2000, ds.XMin);
			Assert.AreEqual(2005, ds.XMax);
			Assert.AreEqual(5.90, ds.YMin);
			Assert.AreEqual(9.34, ds.YMax);
			Assert.AreEqual(-0.674, Math.Round(ds.Slope, 4));
			Assert.AreEqual(1357.1667, Math.Round(ds.YIntercept, 4));
			Assert.AreEqual(2013.6004, Math.Round(ds.XIntercept, 4));
		}

		[TestMethod()]
		public void RemoveTest() {			
			var X = new[] { 2003.0, 2001, 2004, 2000, 2002, 2005 };
			var Y = new[] { 6.93, 8.50, 6.60, 9.34, 7.62, 5.90 };
			var ds = new XYDataSet(X, Y);

			Assert.AreEqual(2000, ds.XMin);
			Assert.AreEqual(2005, ds.XMax);
			Assert.AreEqual(5.90, ds.YMin);
			Assert.AreEqual(9.34, ds.YMax);
			Assert.AreEqual(-0.674, Math.Round(ds.Slope, 4));
			Assert.AreEqual(1357.1667, Math.Round(ds.YIntercept, 4));
			Assert.AreEqual(2013.6004, Math.Round(ds.XIntercept, 4));

			ds.Remove(new PointD(2005, 5.90));

			Assert.AreEqual(2000, ds.XMin);
			Assert.AreEqual(2004, ds.XMax);
			Assert.AreEqual(6.60, ds.YMin);
			Assert.AreEqual(9.34, ds.YMax);
			Assert.AreEqual(-0.705, Math.Round(ds.Slope, 4));
			Assert.AreEqual(1419.208, Math.Round(ds.YIntercept, 4));
			Assert.AreEqual(2013.061, Math.Round(ds.XIntercept, 4));
		}

		[TestMethod()]
		public void RemoveAtTest() {
			var X = new[] { 2003.0, 2001, 2004, 2000, 2002, 2005 };
			var Y = new[] { 6.93, 8.50, 6.60, 9.34, 7.62, 5.90 };
			var ds = new XYDataSet(X, Y);

			Assert.AreEqual(2000, ds.XMin);
			Assert.AreEqual(2005, ds.XMax);
			Assert.AreEqual(5.90, ds.YMin);
			Assert.AreEqual(9.34, ds.YMax);
			Assert.AreEqual(-0.674, Math.Round(ds.Slope, 4));
			Assert.AreEqual(1357.1667, Math.Round(ds.YIntercept, 4));
			Assert.AreEqual(2013.6004, Math.Round(ds.XIntercept, 4));

			ds.RemoveAt(5);

			Assert.AreEqual(2000, ds.XMin);
			Assert.AreEqual(2004, ds.XMax);
			Assert.AreEqual(6.60, ds.YMin);
			Assert.AreEqual(9.34, ds.YMax);
			Assert.AreEqual(-0.705, Math.Round(ds.Slope, 4));
			Assert.AreEqual(1419.208, Math.Round(ds.YIntercept, 4));
			Assert.AreEqual(2013.061, Math.Round(ds.XIntercept, 4));
		}

		[TestMethod()]
		public void ComputeRSquaredTest() {
			double[] X = { 75.0, 83, 85, 85, 92, 97, 99 };
			double[] Y = { 16.0, 20, 25, 27, 32, 48, 48 };
			var ds = new XYDataSet(X, Y);

			var r2 = ds.ComputeRSquared();
			Assert.AreEqual(1.45, Math.Round(ds.Slope,2));
			Assert.AreEqual(-96.85, Math.Round(ds.YIntercept,2));
			Assert.AreEqual(0.927, Math.Round(r2, 3));

			Assert.AreEqual(0.927, Math.Round(ds.RSquare, 3));

			ds.Add(101, 51);

			Assert.AreEqual(double.NaN, Math.Round(ds.RSquare, 3));
		}

	}
}

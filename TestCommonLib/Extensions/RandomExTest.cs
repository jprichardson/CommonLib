using CommonLib.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TestCommonLib
{

	[TestClass()]
	public class RandomExTest
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
		public void NextDateTimeTest() {
			var rnd = new Random();
			var min = new DateTime(2009, 12, 30, 0, 0, 0);
			var max = new DateTime(2010, 1, 2, 0, 0, 0);

			double TRIALS = 100000;
			var dates = new List<DateTime>((int)TRIALS);
			for (int i = 0; i < TRIALS; ++i) {
				var dt = rnd.NextDateTime(min, max);
				dates.Add(dt);
			}

			//verify even distribution
			var avg = (max.Ticks + min.Ticks) / 2;
			var part = (max.Ticks - min.Ticks) / 4;

			var mid = avg; //					1/2
			var mid14 = avg - part; //			1/4
			var mid34 = avg + part;//			3/4
			
			double c1 = 0, c2 = 0, c3 = 0, c4 = 0;

			foreach (var dt in dates) {
				var t = dt.Ticks;
				if (t < mid14)
					++c1;
				else if (t >= mid14 && t < mid)
					++c2;
				else if (t >= mid && t < mid34)
					++c3;
				else if (t >= mid34)
					++c4;
			}

			double q1 = c1/TRIALS;
			double q2 = c2/TRIALS;
			double q3 = c3/TRIALS;
			double q4 = c4/TRIALS;

			Assert.AreEqual(0.25, q1, 0.01);
			Assert.AreEqual(0.25, q2, 0.01);
			Assert.AreEqual(0.25, q3, 0.01);
			Assert.AreEqual(0.25, q4, 0.01);
		}
	}
}

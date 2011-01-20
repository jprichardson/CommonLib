using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib.Utility
{
	

	public static class BenchmarkTimer
	{
		private static Stack<BenchmarkData> _startStack = new Stack<BenchmarkData>();

		public static void Start() {
			_startStack.Push(new BenchmarkData());
		}

		public static void Start(string label) {
			var bd = new BenchmarkData() { Label = label };
			_startStack.Push(bd);
		}

		public static TimeSpan Stop() {
			var stop = DateTime.Now;
			var startBD = _startStack.Pop();
			return stop - startBD.DateTime;
		}

		public static TimeSpan StopAndOutput() {
			var stop = DateTime.Now;
			var startBD = _startStack.Pop();

			var delta = stop - startBD.DateTime;

			var lbl = "{0}: {1} ms";
			var outp = String.Format(lbl, startBD.Label, delta.TotalMilliseconds);
			Console.WriteLine(outp);
			return delta;
		}

		private class BenchmarkData
		{
			public DateTime DateTime { get; set; }
			public string Label { get; set; }

			public BenchmarkData(){
				this.DateTime = DateTime.Now;
				this.Label = "";
			}
		}
	}
}

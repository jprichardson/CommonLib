using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;

namespace CommonLib.Mvvm.Progress
{
	public class ProgressWorkerViewModel : ProgressViewModel
	{
		private AutoResetEvent _resetEvent = new AutoResetEvent(false);
		private BackgroundWorker _bgw;

		public ProgressWorkerViewModel(string progressText) :base(progressText) {
			_bgw = new BackgroundWorker();
			_bgw.DoWork += new DoWorkEventHandler(_bgw_DoWork);
			_bgw.ProgressChanged += new ProgressChangedEventHandler(_bgw_ProgressChanged);
			_bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_bgw_RunWorkerCompleted);
		}

		public override bool ReportsProgress {
			get { return base.ReportsProgress; }
			set { _bgw.WorkerReportsProgress = base.ReportsProgress = value; }
		}

		public override void ReportProgress(int x) {
			if (this.ReportsProgress) {
				if (this.ActionProgressUpdate != null)
					_bgw.ReportProgress(x);
			}
			else
				throw new Exception("Must set ReportsProgress to true");
		}
		
		public override void Wait() {
			_resetEvent.WaitOne();
		}

		protected override void Run() {
			_bgw.RunWorkerAsync();
		}

		private void _bgw_DoWork(object sender, DoWorkEventArgs e) {
			this.ActionWork.Invoke(this);
		}

		private void _bgw_ProgressChanged(object sender, ProgressChangedEventArgs e) {
			if (this.ActionProgressUpdate != null)
				this.ActionProgressUpdate(e.ProgressPercentage);
		}

		private void _bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			if (this.ActionComplete != null)
				this.ActionComplete.Invoke();
			_resetEvent.Set();
		}
	}
}

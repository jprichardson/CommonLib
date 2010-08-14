using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CommonLib.Mvvm;

namespace CommonLib.Mvvm.Progress
{
	public abstract class ProgressViewModel : ViewModelBase
	{


/**************************************************
 * CONSTRUCTOR(S)
 **************************************************/

		protected ProgressViewModel() { }

		public ProgressViewModel(string progressText) {
			this.ProgressText = progressText;
		}

/**************************************************
 * PROPERTIES
 **************************************************/
		public Action ActionComplete { get; set; }

		public Action<int> ActionProgressUpdate { get; set; }

		public Action<ProgressViewModel> ActionWork { get; set; }

		protected bool _isIndeterminate = true;
		public bool IsIndeterminate { get { return _isIndeterminate; } set { _isIndeterminate = value; base.OnPropertyChanged("IsIndeterminate"); } }

		protected string _progressText;
		public string ProgressText { get { return _progressText; } set { _progressText = value; base.OnPropertyChanged("ProgressText"); } }

		protected bool _reportsProgress = false;
		public virtual bool ReportsProgress {
			get { return _reportsProgress; }
			set {
				_reportsProgress = value;
				this.IsIndeterminate = !_reportsProgress;
			}
		}

		//should either be Task or BackgroundWorker
		public object ThreadWorker { get; set; }

/**************************************************
* COMMANDS
**************************************************/

		private ICommand _executeCommand;
		public ICommand ExecuteCommand {
			get {
				if (_executeCommand == null)
					_executeCommand = new RelayCommand(param => this.Run());
				return _executeCommand;
			}
		}

/**************************************************
 * PUBLIC METHODS
 **************************************************/

		public override string ToString() {
			return this.ProgressText;
		}

		public abstract void ReportProgress(int x);

		public abstract void Wait();


/**************************************************
 * PROTECTED METHODS
 **************************************************/
		protected abstract void Run();
	}
}

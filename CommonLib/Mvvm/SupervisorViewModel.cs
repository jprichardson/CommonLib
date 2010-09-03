using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;
using System.Diagnostics;
using CommonLib.Mvvm;
using CommonLib.Mvvm.Progress;

namespace CommonLib.Mvvm
{
	public abstract class SupervisorViewModel : ViewModelBase
	{
		public event EventHandler RequestClose;

		protected Queue<Action> _finishedTaskQueue = new Queue<Action>();

/**********************************************************
 * COMMAND(S)
 **********************************************************/

		protected RelayCommand _closeCommand;
		public ICommand CloseCommand {
			get {
				if (_closeCommand == null)
					_closeCommand = new RelayCommand(param => this.OnRequestClose());

				return _closeCommand;
			}
		}

/**********************************************************
 * PROPERTIES
 **********************************************************/

		private ObservableCollection<ProgressViewModel> _progressViewModels;
		public ObservableCollection<ProgressViewModel> ProgressViewModels {
			get {
				if (_progressViewModels == null) {
					_progressViewModels = new ObservableCollection<ProgressViewModel>();
					_progressViewModels.CollectionChanged += this.OnProgressViewModelsChanged;
				}
				return _progressViewModels;
			}
		}

		private ObservableCollection<WorkspaceViewModel> _workspaces;
		public ObservableCollection<WorkspaceViewModel> Workspaces {
			get {
				if (_workspaces == null) {
					_workspaces = new ObservableCollection<WorkspaceViewModel>();
					_workspaces.CollectionChanged += this.OnWorkspacesChanged;
				}
				return _workspaces;
			}
		}

/**********************************************************
 * PUBLIC METHODS
 **********************************************************/
		public bool IsTaskRunningWithText(string taskText) {
			return (this.ProgressViewModels.Where(pvm => pvm.ProgressText == taskText).Count() > 0);
		}

		public void RunTask(string taskText, bool reportsProgress, Action<ProgressViewModel> task) {
			this.RunTask(taskText, reportsProgress, task, null);
		}

		public void RunTask(string taskText, bool reportsProgress, Action<ProgressViewModel> task, Action taskComplete) {
			ProgressViewModel pvm = new ProgressWorkerViewModel(taskText);
			pvm.ReportsProgress = reportsProgress;

			this.ProgressViewModels.Add(pvm);

			pvm.ActionComplete = () =>
			{
				if (taskComplete != null) { taskComplete.Invoke(); }
				this.ProgressViewModels.Remove(pvm);
			};

			pvm.ActionWork = task;
			pvm.ExecuteCommand.Execute(null);
		}

		public void RunTaskWhenAllFinished(Action task) {
			if (this.ProgressViewModels.Count > 0)
				_finishedTaskQueue.Enqueue(task);
			else
				task.Invoke();
		}

		public void WaitForTasks() {
			while (this.ProgressViewModels.Count > 0)
				this.ProgressViewModels[0].Wait();
		}

		public void AddWorkspace(WorkspaceViewModel workspace) {
			workspace.AttachCommand.Execute(this);
			this.Workspaces.Add(workspace);
		}

		public void AddWorkspaceAndSetActive(WorkspaceViewModel workspace) {
			this.AddWorkspace(workspace);
			this.SetActiveWorkspace(workspace);
		}

		public void SetActiveWorkspace(WorkspaceViewModel workspace) {
			//Debug.Assert(this.Workspaces.Contains(workspace));

			if (this.Workspaces.Contains(workspace)) {
				ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
				if (collectionView != null)
					collectionView.MoveCurrentTo(workspace);
			} else
				throw new Exception("Workspace: " + workspace.DisplayName + " not in Supervisor workspaces.");
		}

/**********************************************************
 * PRIVATE METHODS
 **********************************************************/

		private void OnProgressViewModelsChanged(object sender, NotifyCollectionChangedEventArgs e) {
			if (_progressViewModels.Count == 0) {
				while (_finishedTaskQueue.Count > 0) {
					//Console.WriteLine("Executing!!!");
					_finishedTaskQueue.Dequeue().Invoke();
				}
			}
		}
		
		private void OnRequestClose() {
			EventHandler handler = this.RequestClose;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		private void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e) {
			if (e.NewItems != null && e.NewItems.Count != 0)
				foreach (WorkspaceViewModel workspace in e.NewItems) {
					workspace.RequestClose += this.OnWorkspaceRequestClose;
					//workspace.RequestDetach += this.OnWorkspaceRequestDetach;
					workspace.ConfirmDialog = this.ConfirmDialog;
					workspace.PromptDialog = this.PromptDialog;
					workspace.InputDialog = this.InputDialog;
				}

			if (e.OldItems != null && e.OldItems.Count != 0)
				foreach (WorkspaceViewModel workspace in e.OldItems)
					workspace.RequestClose -= this.OnWorkspaceRequestClose;
		}

		private void OnWorkspaceRequestClose(object sender, EventArgs e) {
			WorkspaceViewModel workspace = sender as WorkspaceViewModel;
			workspace.Dispose();
			this.Workspaces.Remove(workspace);
		}

	}
}

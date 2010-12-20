using System;
using System.Windows.Input;
using  CommonLib.Mvvm;

namespace CommonLib.Mvvm
{
	/// <summary>
	/// This ViewModelBase subclass requests to be removed 
	/// from the UI when its CloseCommand executes.
	/// This class is abstract.
	/// </summary>
	/// 

	public abstract class WorkspaceViewModel : ViewModelBase
	{
		protected WorkspaceViewModel() {}

		protected bool _isDirty = false;
		public bool IsDirty {
			get { return _isDirty; } 
			set {
				if (value)
					if (!this.DisplayName.StartsWith("*")) {
						this.DisplayName = "* " + this.DisplayName;
						this.OnPropertyChanged("DisplayName");
					}
					else
						if (this.DisplayName.StartsWith("*"))
							this.DisplayName = this.DisplayName.TrimStart('*').Trim();

				_isDirty = value;
			} 
		}

		public SupervisorViewModel Supervisor { get; set; }

		protected RelayCommand _attachCommand;
		public ICommand AttachCommand {
			get {
				if (_attachCommand == null)
					_attachCommand = new RelayCommand(param => this.OnAttached(param));

				return _attachCommand;
			}
		}

		protected RelayCommand _closeCommand;
		public ICommand CloseCommand {
			get {
				if (_closeCommand == null)
					_closeCommand = new RelayCommand(param => this.OnRequestClose());

				return _closeCommand;
			}
		}

		protected RelayCommand _detachCommand;
		public ICommand DetachCommand {
			get {
				if (_detachCommand == null)
					_detachCommand = new RelayCommand(param => this.OnRequestDetach());

				return _detachCommand;
			}
		}

		protected RelayCommand _ConfirmCommand;
		public ICommand ConfirmCommand { get; set; }

		public string ConfirmText = "Changes have been made. Do you want to save before closing?";
		public string ConfirmTextTitle = "Save?";

		public event EventHandler RequestClose;
		public event EventHandler RequestDetach;

		protected virtual void OnAttached(object svm) {
			this.Supervisor = (SupervisorViewModel)svm;
			
		}

		private void OnRequestClose() {
			if (this.IsDirty) {
				if (this.ConfirmDialog != null)
					if (this.ConfirmDialog(this.ConfirmText, this.ConfirmTextTitle))
						if (this.ConfirmCommand != null) {
							this.ConfirmCommand.Execute(null);
							this.IsDirty = false;
						}
			}

			EventHandler handler = this.RequestClose;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		private void OnRequestDetach() {
			EventHandler handler = this.RequestDetach;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}
	}
}
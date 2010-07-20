using System;
using System.Diagnostics;
using System.Windows.Input;

namespace CommonLib.Mvvm
{

	public class RelayCommand : ICommand
	{
		readonly Action<object> _execute;
		readonly Func<bool> _canExecute;

		public RelayCommand(Action<object> execute) : this(execute, () => true) {}

		public RelayCommand(Action<object> execute, Func<bool> canExecute) {
			if (execute == null)
				throw new ArgumentNullException("execute");

			_execute = execute;
			_canExecute = canExecute;
		}

		[DebuggerStepThrough]
		public bool CanExecute(object parameter) {
			//return _canExecute == null ? true : _canExecute(parameter);
			return _canExecute();
		}

		public event EventHandler CanExecuteChanged {
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public void Execute(object parameter) {
			_execute(parameter);
		}

	}
}
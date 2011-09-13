using System;
using System.Windows.Input;

namespace SeriesSelector.Frame
{
    /// <summary>
    /// Make an ICommand based on methods passed in for CanExecvute and Execute
    /// </summary>
    public class AdHocCommand : ICommand
    {
        Func<object, bool> _canExecute;
        Action<object> _execute;

        /// <summary>
        /// Set the can execute lambda
        /// </summary>
        public Func<object, bool> SetCanExecute
        {
            set { _canExecute = value; }
        }

        /// <summary>
        /// Set the Execute action
        /// </summary>
        public Action<object> SetExecute
        {
            set { _execute = value; }
        }

        /// <summary>
        /// Instantiate a command that can always be executed and does nothing.
        /// </summary>
        public AdHocCommand()
            : this(_ => true, _ => { })
        {
        }

        /// <summary>
        /// Instantiate a command that can always be executed and executes the passed in delegate
        /// </summary>
        public AdHocCommand(Action<object> execute)
            : this(_ => true, execute)
        {
        }

        /// <summary>
        /// Instantiate a command with can execute and execute implementations
        /// </summary>
        public AdHocCommand(Func<object, bool> canExecute, Action<object> execute)
        {
            _canExecute = canExecute;
            _execute = execute;
        }

        /// <summary>
        /// Whether this command can execute
        /// </summary>
        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        /// <summary>
        /// Notify to some Command Manager that Can Execute may have changed
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Raise the CanExecuteChanged event from the outside
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
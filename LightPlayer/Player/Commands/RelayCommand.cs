using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Player.Commands
{
    class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public bool CanExecute(object parameter)
        {
            bool result = _canExecute == null ? true : _canExecute(parameter);
            return result;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}

using System;
using System.Windows.Input;

namespace MP3FileUpdater.UI.WPF.Command
{

    public class DelegateCommand : ICommand
    {
        private readonly Action _action;

        public DelegateCommand(Action action)
        {
            _action = action;
        }
        public bool CanExecute(object parameter)
       {
            return true;
        }

        public void Execute(object parameter)
        {
            _action();
        }

        public event EventHandler CanExecuteChanged;
    }
}

using FilesNamesChanger.ViewModels;
using System;
using System.Windows.Input;

namespace FilesNamesChanger.Commands
{
    class ProcessFilesCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        readonly FilesChangerViewModel parent;
        public ProcessFilesCommand(FilesChangerViewModel parent)
        {
            this.parent = parent;
            parent.PropertyChanged += delegate { CanExecuteChanged?.Invoke(this, EventArgs.Empty); };
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            
        }
    }
}

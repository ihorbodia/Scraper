using Scraper.ViewModel;
using Sraper.Common;
using System;
using System.Windows.Input;

namespace ScraperGUI.Commands
{
    class ChooseFileCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        readonly MainViewModel parent;
        public ChooseFileCommand(MainViewModel parent)
        {
            this.parent = parent;
            parent.PropertyChanged += delegate { CanExecuteChanged?.Invoke(this, EventArgs.Empty); };
        }
        public bool CanExecute(object parameter)
        {
            return !parent.FileProcessingLabelData.Equals(StringConsts.FileProcessingLabelData_Processing);
        }

        public void Execute(object parameter)
        {
            string chosenPath = FilesHelper.SelectFile();
            if (!string.IsNullOrEmpty(chosenPath.Trim()))
            {
                parent.FilePathLabelData = chosenPath;
                if (!string.IsNullOrEmpty(parent.CountryFolderPathLabelData) && !string.IsNullOrEmpty(parent.FilePathLabelData))
                {
                    parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_CanProcess;
                }
                if (string.IsNullOrEmpty(parent.CountryFolderPathLabelData))
                {
                    parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_ChooseFolder;
                }
                if (string.IsNullOrEmpty(parent.FilePathLabelData))
                {
                    parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_ChooseFile;
                }
            }
        }
    }
}

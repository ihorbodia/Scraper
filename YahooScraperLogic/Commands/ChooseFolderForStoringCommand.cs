using Sraper.Common;
using System;
using System.Windows.Input;
using YahooScraperLogic.ViewModels;

namespace YahooScraperLogic.Commands
{
    public class ChooseFolderForStoringCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        readonly YahooScraperViewModel parent;
        public ChooseFolderForStoringCommand(YahooScraperViewModel parent)
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
            string chosenPath = FilesHelper.SelectFolder();
            if (!string.IsNullOrEmpty(chosenPath.Trim()))
            {
                parent.FolderForStoringFilesLabelData = chosenPath;
                if (!string.IsNullOrEmpty(parent.FolderForStoringFilesLabelData) && !string.IsNullOrEmpty(parent.FilePathLabelData))
                {
                    parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_CanProcess;
                }
                if (string.IsNullOrEmpty(parent.FolderForStoringFilesLabelData))
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

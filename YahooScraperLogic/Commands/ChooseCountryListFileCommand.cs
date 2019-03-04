using Sraper.Common;
using System;
using System.Windows.Input;
using YahooScraperLogic.ViewModels;

namespace YahooScraperLogic.Commands
{
    class ChooseCountryListFileCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        readonly ScraperViewModel parent;
        public ChooseCountryListFileCommand(ScraperViewModel parent)
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
                parent.CountryListLabelData = chosenPath;
                if (!string.IsNullOrEmpty(parent.WSJCodesFileLabelData) && !string.IsNullOrEmpty(parent.CountryListLabelData))
                {
                    parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_CanProcess;
                }
                if (string.IsNullOrEmpty(parent.WSJCodesFileLabelData))
                {
                    parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_ChooseAnotherFile;
                }
                if (string.IsNullOrEmpty(parent.CountryListLabelData))
                {
                    parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_ChooseFile;
                }
            }
        }
    }
}

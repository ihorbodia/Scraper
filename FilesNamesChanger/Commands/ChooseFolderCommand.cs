using FilesNamesChanger.ViewModels;
using Sraper.Common;
using System;
using System.Windows.Input;

namespace FilesNamesChanger.Commands
{
    class ChooseFolderCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        readonly FilesChangerViewModel parent;
        public ChooseFolderCommand(FilesChangerViewModel parent)
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
            switch (parameter)
            {
                case "1":
                    parent.FolderPath_1 = chosenPath;
                    break;
                case "2":
                    parent.FolderPath_2 = chosenPath;
                    break;
                case "3":
                    parent.FolderPath_3 = chosenPath;
                    break;
                case "4":
                    parent.FolderPath_4 = chosenPath;
                    break;
                case "5":
                    parent.FolderPath_5 = chosenPath;
                    break;
                case "6":
                    parent.FolderPath_6 = chosenPath;
                    break;
                case "7":
                    parent.FolderPath_7 = chosenPath;
                    break;
                case "8":
                    parent.FolderPath_8 = chosenPath;
                    break;
                case "10":
                    parent.FolderPath_10 = chosenPath;
                    break;
                case "11":
                    parent.FolderPath_11 = chosenPath;
                    break;
                case "12":
                    parent.FolderPath_12 = chosenPath;
                    break;
                case "13":
                    parent.FolderPath_13 = chosenPath;
                    break;
                case "14":
                    parent.FolderPath_14 = chosenPath;
                    break;
                case "15":
                    parent.FolderPath_15 = chosenPath;
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(chosenPath.Trim()))
            {
                //parent.FolderForStoringFilesLabelData = chosenPath;
                //if (!string.IsNullOrEmpty(parent.FolderForStoringFilesLabelData) && !string.IsNullOrEmpty(parent.FilePathLabelData))
                //{
                //    parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_CanProcess;
                //}
                //if (string.IsNullOrEmpty(parent.FolderForStoringFilesLabelData))
                //{
                //    parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_ChooseFolder;
                //}
                //if (string.IsNullOrEmpty(parent.FilePathLabelData))
                //{
                //    parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_ChooseFile;
                //}
            }
        }
    }
}

using Sraper.Common;
using System;
using System.ComponentModel;
using System.Windows.Input;
using YahooScraperLogic.Commands;

namespace YahooScraperLogic.ViewModels
{
    public class YahooScraperViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _filePathLabel;
        private string _filePathLabelData;

        private string _dateFromlabel;
        private string _dateTolabel;

        private string _folderForStoringFilesLabel;
        private string _folderForStoringFilesLabelData;

        private string _fileProcessingLabel;
        private string _fileProcessingLabelData;

        private DateTime _selectedDateFrom;
        private DateTime _selectedDateTo;

        public YahooScraperViewModel()
        {
            ProcessFileCommand = new ProcessFileCommand(this);
            ChooseCountryFolderCommand = new ChooseFolderForStoringCommand(this);
            ChooseFileCommand = new ChooseFileCommand(this);
            FilePathLabel = StringConsts.FilePathLabelConst;
            FileProcessingLabel = StringConsts.FileProcessingLabelConst;
            FolderForStoringFilesLabel = StringConsts.CountryFolderPathLabelConst;
            FileProcessingLabelData = string.Empty;
            DateFromLabel = StringConsts.DateFromLabelConst;
            DateToLabel = StringConsts.DateToLabelConst;
            SelectedDateFrom = DateTime.Now;
            SelectedDateTo = DateTime.Now;
        }

        public ICommand ProcessFileCommand { get; private set; }
        public ICommand ChooseCountryFolderCommand { get; private set; }
        public ICommand ChooseFileCommand { get; private set; }

        public DateTime SelectedDateFrom
        {
            get
            {
                return _selectedDateFrom;
            }
            set
            {
                if (_selectedDateFrom != value)
                {
                    _selectedDateFrom = value;
                    RaisePropertyChanged(nameof(SelectedDateFrom));
                }
            }
        }
        public DateTime SelectedDateTo
        {
            get
            {
                return _selectedDateTo;
            }
            set
            {
                if (_selectedDateTo != value)
                {
                    _selectedDateTo = value;
                    RaisePropertyChanged(nameof(SelectedDateTo));
                }
            }
        }

        public string DateFromLabel
        {
            get
            {
                return _dateFromlabel;
            }
            private set
            {
                if (_dateFromlabel != value)
                {
                    _dateFromlabel = value;
                    RaisePropertyChanged(nameof(DateFromLabel));
                }
            }
        }
        public string DateToLabel
        {
            get
            {
                return _dateTolabel;
            }
            set
            {
                if (_dateTolabel != value)
                {
                    _dateTolabel = value;
                    RaisePropertyChanged(nameof(DateToLabel));
                }
            }
        }

        public string FilePathLabel
        {
            get
            {
                return _filePathLabel;
            }
            private set
            {
                if (_filePathLabel != value)
                {
                    _filePathLabel = value;
                    RaisePropertyChanged(nameof(FilePathLabel));
                }
            }
        }
        public string FilePathLabelData
        {
            get
            {
                return _filePathLabelData;
            }
            set
            {
                if (_filePathLabelData != value)
                {
                    _filePathLabelData = value;
                    RaisePropertyChanged(nameof(FilePathLabelData));
                }
            }
        }

        public string FileProcessingLabel
        {
            get
            {
                return _fileProcessingLabel;
            }
            private set
            {
                if (_fileProcessingLabel != value)
                {
                    _fileProcessingLabel = value;
                    RaisePropertyChanged(nameof(FileProcessingLabel));
                }
            }
        }
        public string FileProcessingLabelData
        {
            get
            {
                return _fileProcessingLabelData;
            }
            set
            {
                if (_fileProcessingLabelData != value)
                {
                    _fileProcessingLabelData = value;
                    RaisePropertyChanged(nameof(FileProcessingLabelData));
                }
            }
        }

        public string FolderForStoringFilesLabel
        {
            get
            {
                return _folderForStoringFilesLabel;
            }
            private set
            {
                if (_filePathLabel != value)
                {
                    _folderForStoringFilesLabel = value;
                    RaisePropertyChanged(nameof(FolderForStoringFilesLabel));
                }
            }
        }
        public string FolderForStoringFilesLabelData
        {
            get
            {
                return _folderForStoringFilesLabelData;
            }
            set
            {
                if (_folderForStoringFilesLabelData != value)
                {
                    _folderForStoringFilesLabelData = value;
                    RaisePropertyChanged(nameof(FolderForStoringFilesLabelData));
                }
            }
        }

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}

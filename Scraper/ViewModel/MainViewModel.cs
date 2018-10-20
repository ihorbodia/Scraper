using ScraperGUI.Commands;
using Sraper.Common;
using System.ComponentModel;
using System.Windows.Input;

namespace Scraper.ViewModel
{
    public class MainViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _filePathLabel;
        private string _filePathLabelData;

        private string _countryFolderPathLabel;
        private string _countryFolderPathLabelData;

        private string _fileProcessingLabel;
        private string _fileProcessingLabelData;

        public MainViewModel()
        {
            ProcessFileCommand = new ProcessFileCommand(this);
            ChooseCountryFolderCommand = new ChooseCountryFolderCommand(this);
            ChooseFileCommand = new ChooseFileCommand(this);
            FilePathLabel = StringConsts.FilePathLabelConst;
            FileProcessingLabel = StringConsts.FileProcessingLabelConst;
            CountryFolderPathLabel = StringConsts.CountryFolderPathLabelConst;
            FileProcessingLabelData = string.Empty;
        }

        public ICommand ProcessFileCommand { get; private set; }
        public ICommand ChooseCountryFolderCommand { get; private set; }
        public ICommand ChooseFileCommand { get; private set; }

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
        public string CountryFolderPathLabel
        {
            get
            {
                return _countryFolderPathLabel;
            }
            private set
            {
                if (_filePathLabel != value)
                {
                    _countryFolderPathLabel = value;
                    RaisePropertyChanged(nameof(CountryFolderPathLabel));
                }
            }
        }
        public string CountryFolderPathLabelData
        {
            get
            {
                return _countryFolderPathLabelData;
            }
            set
            {
                if (_countryFolderPathLabelData != value)
                {
                    _countryFolderPathLabelData = value;
                    RaisePropertyChanged(nameof(CountryFolderPathLabelData));
                }
            }
        }

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}

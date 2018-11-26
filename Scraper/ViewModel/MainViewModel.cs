using ScraperGUI.Commands;
using Sraper.Common;
using System.ComponentModel;
using System.Windows.Input;

namespace Scraper.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _outputFolderLabel;
        private string _outputFolderLabelData;

        private string _countryFolderPathLabel;
        private string _countryFolderPathLabelData;

        private string _fileProcessingLabel;
        private string _fileProcessingLabelData;

        public MainViewModel()
        {
            ProcessFileCommand = new ProcessFileCommand(this);
            ChooseCountryFolderCommand = new ChooseCountryFolderCommand(this);
			ChooseOutputFileCommand = new ChooseOutputFileCommand(this);
			OutputFolderLabel = StringConsts.FileProcessingLabelData_ChooseFolder;
            FileProcessingLabel = StringConsts.FileProcessingLabelConst;
            CountryFolderPathLabel = StringConsts.CountryFolderPathLabelConst;
            FileProcessingLabelData = string.Empty;
        }

        public ICommand ProcessFileCommand { get; private set; }
        public ICommand ChooseCountryFolderCommand { get; private set; }
        public ICommand ChooseOutputFileCommand { get; private set; }

		public string OutputFolderLabel
		{
			get
			{
				return _outputFolderLabel;
			}
			private set
			{
				if (_outputFolderLabel != value)
				{
					_outputFolderLabel = value;
					RaisePropertyChanged(nameof(OutputFolderLabel));
				}
			}
		}
		public string OutputFolderLabelData
		{
			get
			{
				return _outputFolderLabelData;
			}
			set
			{
				if (_outputFolderLabelData != value)
				{
					_outputFolderLabelData = value;
					RaisePropertyChanged(nameof(OutputFolderLabelData));
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
                if (_countryFolderPathLabel != value)
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

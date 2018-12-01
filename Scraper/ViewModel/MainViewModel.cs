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

		private string _secondCountryFolderPathLabel;
		private string _secondCountryFolderPathLabelData;

		private string _fileProcessingLabel;
        private string _fileProcessingLabelData;

        public MainViewModel()
        {
            ProcessFileCommand = new ProcessFileCommand(this);
            ChooseCountryFolderCommand = new ChooseCountryFolderCommand(this);
			ChooseOutputFileCommand = new ChooseOutputFileCommand(this);
			ChooseSecondCountryFolderCommand = new ChooseSecondCountryFolderCommand(this);

			OutputFolderLabel = "Output folder:";
            CountryFolderPathLabel = "Country list folder:";
			SecondCountryFolderPathLabel = "Second country list folder:";
			FileProcessingLabelData = string.Empty;
			FileProcessingLabel = StringConsts.FileProcessingLabelConst;
			OutputFolderLabelData = Properties.Settings.Default.OutputFolderPath;
			CountryFolderPathLabelData = Properties.Settings.Default.CountryFolderPath;
			SecondCountryFolderPathLabelData = Properties.Settings.Default.SecondCountryFolderPath;
		}

        public ICommand ProcessFileCommand { get; private set; }
        public ICommand ChooseCountryFolderCommand { get; private set; }
        public ICommand ChooseOutputFileCommand { get; private set; }
		public ICommand ChooseSecondCountryFolderCommand { get; private set; }

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
					Properties.Settings.Default.OutputFolderPath = value;
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
					Properties.Settings.Default.CountryFolderPath = value;
					RaisePropertyChanged(nameof(CountryFolderPathLabelData));
                }
            }
        }
		public string SecondCountryFolderPathLabel
		{
			get
			{
				return _secondCountryFolderPathLabel;
			}
			private set
			{
				if (_secondCountryFolderPathLabel != value)
				{
					_secondCountryFolderPathLabel = value;
					RaisePropertyChanged(nameof(SecondCountryFolderPathLabel));
				}
			}
		}
		public string SecondCountryFolderPathLabelData
		{
			get
			{
				return _secondCountryFolderPathLabelData;
			}
			set
			{
				if (_secondCountryFolderPathLabelData != value)
				{
					_secondCountryFolderPathLabelData = value;
					Properties.Settings.Default.SecondCountryFolderPath = value;
					RaisePropertyChanged(nameof(SecondCountryFolderPathLabelData));
				}
			}
		}

		private void RaisePropertyChanged(string property)
        {
			Properties.Settings.Default.Save();
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}

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
        private string _japanListFileLabel;
        private string _japanListFileLabelData;

        private string _dateFromlabel;
        private string _dateTolabel;

        private string _wsjCodesListLabel;
        private string _wsjCodesListLabelData;

        private string _fileProcessingLabel;
        private string _fileProcessingLabelData;

		private bool _processingJapanFile;

		public YahooScraperViewModel()
        {
            ProcessFileCommand = new YahooJapanFinanceDataProcessingCommand(this);
            ChooseCountryFolderCommand = new ChooseWSJListFileCommand(this);
            ChooseFileCommand = new ChooseJapanListFileCommand(this);
            CountryListLabel = StringConsts.CountryListFileLabel;
            FileProcessingLabel = StringConsts.FileProcessingLabelConst;
            WSJCodesFileLabel = StringConsts.WSJListFileLabel;
            FileProcessingLabelData = string.Empty;
            DateFromLabel = StringConsts.DateFromLabelConst;
            DateToLabel = StringConsts.DateToLabelConst;
        }

        public ICommand ProcessFileCommand { get; private set; }
        public ICommand ChooseCountryFolderCommand { get; private set; }
        public ICommand ChooseFileCommand { get; private set; }

		public bool ProcessingJapanFile
		{
			get
			{
				return _processingJapanFile;
			}
			set
			{
				if (_processingJapanFile != value)
				{
					_processingJapanFile = value;
					RaisePropertyChanged(nameof(ProcessingJapanFile));
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

        public string CountryListLabel
		{
            get
            {
                return _japanListFileLabel;
            }
            private set
            {
                if (_japanListFileLabel != value)
                {
                    _japanListFileLabel = value;
                    RaisePropertyChanged(nameof(CountryListLabel));
                }
            }
        }
        public string CountryListLabelData
		{
            get
            {
                return _japanListFileLabelData;
            }
            set
            {
                if (_japanListFileLabelData != value)
                {
                    _japanListFileLabelData = value;
                    RaisePropertyChanged(nameof(CountryListLabelData));
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

        public string WSJCodesFileLabel
        {
            get
            {
                return _wsjCodesListLabel;
            }
            private set
            {
                if (_japanListFileLabel != value)
                {
                    _wsjCodesListLabel = value;
                    RaisePropertyChanged(nameof(WSJCodesFileLabel));
                }
            }
        }
        public string WSJCodesFileLabelData
        {
            get
            {
                return _wsjCodesListLabelData;
            }
            set
            {
                if (_wsjCodesListLabelData != value)
                {
                    _wsjCodesListLabelData = value;
                    RaisePropertyChanged(nameof(WSJCodesFileLabelData));
                }
            }
        }

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}

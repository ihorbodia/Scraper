using FilesNamesChanger.Commands;
using Sraper.Common;
using System.ComponentModel;
using System.Windows.Input;

namespace FilesNamesChanger.ViewModels
{
    class FilesChangerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _folderLabel;

        private string _fileProcessingLabel;
        private string _fileProcessingLabelData;

        private string _fileNameForSearching;
        private string _fileNameForChanging;

        private string _folderPath_1;
        private string _folderPath_2;
        private string _folderPath_3;
        private string _folderPath_4;
        private string _folderPath_5;
        private string _folderPath_6;
        private string _folderPath_7;
        private string _folderPath_8;
        private string _folderPath_9;
        private string _folderPath_10;
        private string _folderPath_11;
        private string _folderPath_12;
        private string _folderPath_13;
        private string _folderPath_14;
        private string _folderPath_15;

        public FilesChangerViewModel()
        {
            ProcessFileCommand = new ProcessFilesCommand(this);
            ChooseCountryFolderCommand = new ChooseFolderCommand(this);
            FileProcessingLabel = StringConsts.FileProcessingLabelConst;
            FileProcessingLabelData = string.Empty;
            FolderLabel = StringConsts.FileProcessingLabelData_ChooseFolder;
        }

        public ICommand ProcessFileCommand { get; private set; }
        public ICommand ChooseCountryFolderCommand { get; private set; }

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

        public string FileNameForSearching
        {
            get
            {
                return _fileNameForSearching;
            }
            private set
            {
                if (_fileNameForSearching != value)
                {
                    _fileNameForSearching = value;
                    RaisePropertyChanged(nameof(FileNameForSearching));
                }
            }
        }
        public string FileNameForChanging
        {
            get
            {
                return _fileNameForChanging;
            }
            set
            {
                if (_fileNameForChanging != value)
                {
                    _fileNameForChanging = value;
                    RaisePropertyChanged(nameof(FileNameForChanging));
                }
            }
        }

        public string FolderLabel
        {
            get
            {
                return _folderLabel;
            }
            set
            {
                if (_folderLabel != value)
                {
                    _folderLabel = value;
                    RaisePropertyChanged(nameof(FolderLabel));
                }
            }
        }

        public string FolderPath_1
        {
            get
            {
                return _folderPath_1;
            }
            set
            {
                if (_folderPath_1 != value)
                {
                    _folderPath_1 = value;
                    RaisePropertyChanged(nameof(FolderPath_1));
                }
            }
        }
        public string FolderPath_2
        {
            get
            {
                return _folderPath_2;
            }
            set
            {
                if (_folderPath_2 != value)
                {
                    _folderPath_2 = value;
                    RaisePropertyChanged(nameof(FolderPath_2));
                }
            }
        }
        public string FolderPath_3
        {
            get
            {
                return _folderPath_3;
            }
            set
            {
                if (_folderPath_3 != value)
                {
                    _folderPath_3 = value;
                    RaisePropertyChanged(nameof(FolderPath_3));
                }
            }
        }
        public string FolderPath_4
        {
            get
            {
                return _folderPath_4;
            }
            set
            {
                if (_folderPath_4 != value)
                {
                    _folderPath_4 = value;
                    RaisePropertyChanged(nameof(FolderPath_4));
                }
            }
        }
        public string FolderPath_5
        {
            get
            {
                return _folderPath_5;
            }
            set
            {
                if (_folderPath_5 != value)
                {
                    _folderPath_5 = value;
                    RaisePropertyChanged(nameof(FolderPath_5));
                }
            }
        }
        public string FolderPath_6
        {
            get
            {
                return _folderPath_6;
            }
            set
            {
                if (_folderPath_6 != value)
                {
                    _folderPath_6 = value;
                    RaisePropertyChanged(nameof(FolderPath_6));
                }
            }
        }
        public string FolderPath_7
        {
            get
            {
                return _folderPath_7;
            }
            set
            {
                if (_folderPath_7 != value)
                {
                    _folderPath_7 = value;
                    RaisePropertyChanged(nameof(FolderPath_7));
                }
            }
        }
        public string FolderPath_8
        {
            get
            {
                return _folderPath_8;
            }
            set
            {
                if (_folderPath_8 != value)
                {
                    _folderPath_8 = value;
                    RaisePropertyChanged(nameof(FolderPath_8));
                }
            }
        }
        public string FolderPath_9
        {
            get
            {
                return _folderPath_9;
            }
            set
            {
                if (_folderPath_9 != value)
                {
                    _folderPath_9 = value;
                    RaisePropertyChanged(nameof(FolderPath_9));
                }
            }
        }
        public string FolderPath_10
        {
            get
            {
                return _folderPath_10;
            }
            set
            {
                if (_folderPath_10 != value)
                {
                    _folderPath_10 = value;
                    RaisePropertyChanged(nameof(FolderPath_10));
                }
            }
        }
        public string FolderPath_11
        {
            get
            {
                return _folderPath_11;
            }
            set
            {
                if (_folderPath_11 != value)
                {
                    _folderPath_11 = value;
                    RaisePropertyChanged(nameof(FolderPath_11));
                }
            }
        }
        public string FolderPath_12
        {
            get
            {
                return _folderPath_12;
            }
            set
            {
                if (_folderPath_12 != value)
                {
                    _folderPath_12 = value;
                    RaisePropertyChanged(nameof(FolderPath_12));
                }
            }
        }
        public string FolderPath_13
        {
            get
            {
                return _folderPath_13;
            }
            set
            {
                if (_folderPath_13 != value)
                {
                    _folderPath_13 = value;
                    RaisePropertyChanged(nameof(FolderPath_13));
                }
            }
        }
        public string FolderPath_14
        {
            get
            {
                return _folderPath_14;
            }
            set
            {
                if (_folderPath_14 != value)
                {
                    _folderPath_14 = value;
                    RaisePropertyChanged(nameof(FolderPath_14));
                }
            }
        }
        public string FolderPath_15
        {
            get
            {
                return _folderPath_15;
            }
            set
            {
                if (_folderPath_15 != value)
                {
                    _folderPath_15 = value;
                    RaisePropertyChanged(nameof(FolderPath_15));
                }
            }
        }

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}

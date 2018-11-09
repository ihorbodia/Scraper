using FilesNamesChanger.ViewModels;
using System;
using System.IO;
using System.Windows.Input;

namespace FilesNamesChanger.Commands
{
    class ProcessFilesCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        readonly FilesChangerViewModel parent;
        public ProcessFilesCommand(FilesChangerViewModel parent)
        {
            this.parent = parent;
            parent.PropertyChanged += delegate { CanExecuteChanged?.Invoke(this, EventArgs.Empty); };
        }
        public bool CanExecute(object parameter)
        {
            return
                !string.IsNullOrEmpty(parent.FolderPath_1) &&
                !string.IsNullOrEmpty(parent.FolderPath_2) &&
                !string.IsNullOrEmpty(parent.FolderPath_3) &&
                !string.IsNullOrEmpty(parent.FolderPath_4) &&
                !string.IsNullOrEmpty(parent.FolderPath_5) &&
                !string.IsNullOrEmpty(parent.FolderPath_6) &&
                !string.IsNullOrEmpty(parent.FolderPath_7) &&
                !string.IsNullOrEmpty(parent.FolderPath_8) &&
                !string.IsNullOrEmpty(parent.FolderPath_10) &&
                !string.IsNullOrEmpty(parent.FolderPath_11) &&
                !string.IsNullOrEmpty(parent.FolderPath_12) &&
                !string.IsNullOrEmpty(parent.FolderPath_13) &&
                !string.IsNullOrEmpty(parent.FolderPath_14) &&
                !string.IsNullOrEmpty(parent.FolderPath_15) &&
                !string.IsNullOrEmpty(parent.FileNameForSearching) &&
                !string.IsNullOrEmpty(parent.FileNameForChanging);

        }

        public void Execute(object parameter)
        {
            processFiles(parent.FolderPath_1);
            processFiles(parent.FolderPath_2);
            processFiles(parent.FolderPath_3);
            processFiles(parent.FolderPath_4);
            processFiles(parent.FolderPath_5);
            processFiles(parent.FolderPath_6);
            processFiles(parent.FolderPath_7);
            processFiles(parent.FolderPath_8);
            processFiles(parent.FolderPath_9);
            processFiles(parent.FolderPath_10);
            processFiles(parent.FolderPath_11);
            processFiles(parent.FolderPath_12);
            processFiles(parent.FolderPath_13);
            processFiles(parent.FolderPath_14);
            processFiles(parent.FolderPath_15);
        }

        private void processFiles(string path)
        {
            if (path == null)
            {
                return;
            }
            DirectoryInfo countryDirs = new DirectoryInfo(path);
            foreach (var countryDir in countryDirs.EnumerateDirectories())
            {
                if (!countryDir.Equals(parent.CountryNameFolder))
                {
                    continue;
                }
                var files = countryDir.EnumerateFiles();
                foreach (var file in files)
                {
                    string extension = file.Extension;
                    string name = Path.GetFileNameWithoutExtension(file.Name);

                    string input = file.FullName;
                    int index = input.LastIndexOf("\\");
                    if (index > 0)
                        input = input.Substring(0, index);
                    if (name.Equals(parent.FileNameForSearching))
                    {
                        File.Move(file.FullName, input+"\\"+parent.FileNameForChanging + extension);
                    }
                }
            }
        }
    }
}

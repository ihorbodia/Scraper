using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Scraper.ViewModel;
using Sraper.Common;
using System.IO;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ScraperGUI.Commands
{
    internal class ProcessFileCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        readonly MainViewModel parent;
        public ProcessFileCommand(MainViewModel parent)
        {
            this.parent = parent;
            parent.PropertyChanged += delegate { CanExecuteChanged?.Invoke(this, EventArgs.Empty); };
        }
        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(parent.FileProcessingLabelData) &&
                    !string.IsNullOrEmpty(parent.CountryFolderPathLabelData) &&
                    !parent.FileProcessingLabelData.Equals(StringConsts.FileProcessingLabelData_Processing);
        }

		public async void Execute(object parameter)
		{
			string chosenPath = parent.CountryFolderPathLabelData;
			if (string.IsNullOrEmpty(chosenPath.Trim()))
			{
				return;
			}
			parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_Processing;

			var dir = new DirectoryInfo(chosenPath); //Unlisted list //Template 3
			var filesList = dir.GetFiles("*.xlsx").ToList();
			FileInfo template = filesList.FirstOrDefault(x => x.Name.ToLower().Contains("template"));
			if (template == null)
			{
				return;
			}
			filesList = filesList.Where(item => !item.Name.ToLower().Contains("template")).ToList();
			filesList = filesList.Where(item => !item.Name.ToLower().Contains("unlisted")).ToList();

			var templateData = FilesHelper.GetDataTableFromExcelHeaders(template.FullName, false);
			DataRow row = templateData.Rows[0];

			foreach (var file in filesList)
			{
				var data = FilesHelper.GetDataTableFromExcel(file.FullName, row);
				if (data == null)
				{
					return;
				}
				using (ExcelPackage pck = new ExcelPackage(new FileInfo(Path.Combine(parent.OutputFolderLabelData, file.Name))))
				{
					ExcelWorksheet ws = pck.Workbook.Worksheets.FirstOrDefault(x => x.Name.Equals("Sheet1"));
					if (ws == null)
					{
						ws = pck.Workbook.Worksheets.Add("Sheet1");
					}
					ws.Cells["A1"].LoadFromDataTable(data, true);
					pck.Save();
				}
			}
			parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_Finish;
			Console.WriteLine(StringConsts.FileProcessingLabelData_Finish);


			////var table = FilesHelper.GetDataTableFromExcel(parent.FilePathLabelData);
			//if (table != null)
			//{
			//	try
			//	{
			//		//await DownloadMultipleFilesAsync(table.AsEnumerable());
			//	}
			//	catch (Exception ex)
			//	{
			//		parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_ErrorMessage;
			//	}
			//}

		}
	}
}

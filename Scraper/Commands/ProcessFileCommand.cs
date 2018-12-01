using System;
using System.Windows.Input;
using Scraper.ViewModel;
using Sraper.Common;
using System.IO;
using OfficeOpenXml;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

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
			return !string.IsNullOrEmpty(parent.CountryFolderPathLabelData) &&
					!string.IsNullOrEmpty(parent.OutputFolderLabelData) &&
					!string.IsNullOrEmpty(parent.SecondCountryFolderPathLabelData) &&
					!parent.FileProcessingLabelData.Equals(StringConsts.FileProcessingLabelData_Processing);
		}

		private string getChangedName(string name)
		{
			var items = name.Split(' ');
			return items[0] + " 3.xlsx";
		}

		public async void Execute(object parameter)
		{
			string chosenPath = parent.CountryFolderPathLabelData;
			string chosenSecondCountryPath = parent.SecondCountryFolderPathLabelData;
			if (string.IsNullOrEmpty(chosenPath.Trim()) || string.IsNullOrEmpty(chosenSecondCountryPath.Trim()))
			{
				return;
			}
			parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_Processing;
			await Task.Factory.StartNew(() =>
			{
				var dir = new DirectoryInfo(chosenPath);
				var secondCountrDir = new DirectoryInfo(chosenSecondCountryPath);
				var secondCountryFilesList = secondCountrDir.GetFiles("*.xlsx").ToList();
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
					var secondCountryFile = secondCountryFilesList.FirstOrDefault(x => x.Name.Equals($"{file.Name.Split(' ')[0]} 2.xlsx"));
					if (data == null || secondCountryFile == null)
					{
						continue;
					}
					var secondCountryFileData = FilesHelper.GetDataTableFromExcelAllData(secondCountryFile.FullName, row);
					for (int i = 0; i < data.Rows.Count; i++)
					{
						if (i > secondCountryFileData.Rows.Count - 1)
						{
							break;
						}
						string yahooCode = data.Rows[i].ItemArray[2].ToString();
						string secondYahooCode = secondCountryFileData.Rows[i].ItemArray[2].ToString();

						if (yahooCode.Equals(secondYahooCode))
						{
							data.Rows[i].SetField(5, secondCountryFileData.Rows[i].ItemArray[5].ToString());
							data.Rows[i].SetField(6, secondCountryFileData.Rows[i].ItemArray[6].ToString());
						}
					}

					using (ExcelPackage pck = new ExcelPackage(new FileInfo(Path.Combine(parent.OutputFolderLabelData, getChangedName(file.Name)))))
					{
						ExcelWorksheet ws = pck.Workbook.Worksheets.FirstOrDefault(x => x.Name.Equals("Sheet1"));
						if (ws == null)
						{
							ws = pck.Workbook.Worksheets.Add("Sheet1");
						}
						ws.Cells["A1"].LoadFromDataTable(data, true);

						var end = ws.Dimension.End;
						for (int currRow = 2; currRow <= end.Row; currRow++)
						{

							if (int.TryParse(ws.Cells[currRow, 3].Text, out int v))
							{
								object firstValue = ws.Cells[currRow, 3].Text;
								ws.Cells[currRow, 3].Value = v;
							}

							if (int.TryParse(ws.Cells[currRow, 4].Text, out int n))
							{
								object firstValue = ws.Cells[currRow, 4].Text;
								ws.Cells[currRow, 4].Value = n;
							}

							if (int.TryParse(ws.Cells[currRow, 5].Text, out int s))
							{
								object firstValue = ws.Cells[currRow, 5].Text;
								ws.Cells[currRow, 5].Value = s;
							}
						}
						pck.Save();
					}
				}
			});
			parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_Finish;
			Console.WriteLine(StringConsts.FileProcessingLabelData_Finish);
		}
	}
}

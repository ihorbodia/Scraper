using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Sraper.Common
{
	public static class FilesHelper
	{
		public static string SelectFile()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();

			string selectedFileName = string.Empty;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				selectedFileName = openFileDialog.FileName;
			}
			else
			{
				selectedFileName = string.Empty;
			}
			return selectedFileName;
		}

		public static string SelectFolder()
		{
			FolderBrowserDialog openFolderDialog = new FolderBrowserDialog();

			string selectedFolderName = string.Empty;
			if (openFolderDialog.ShowDialog() == DialogResult.OK)
			{
				selectedFolderName = openFolderDialog.SelectedPath;
			}
			else
			{
				selectedFolderName = string.Empty;
			}
			return selectedFolderName;
		}

		public static DataTable GetDataTableFromExcel(string path, DataRow headerRow)
		{
			using (var pck = new OfficeOpenXml.ExcelPackage())
			{
				using (var stream = File.OpenRead(path))
				{
					pck.Load(stream);
				}
				var ws = pck.Workbook.Worksheets.First();
				DataTable tbl = new DataTable();
				for (int i = 0; i < headerRow.ItemArray.Length; i++)
				{
					var value = IsNullOrEmpty(headerRow[i].ToString().ToArray()) ? getSpaces(i) : headerRow[i].ToString();
					tbl.Columns.Add(value);
				}
				for (int rowNum = 2; rowNum <= ws.Dimension.End.Row; rowNum++)
				{
					var wsRow = ws.Cells[rowNum, 1, rowNum, 5];
					DataRow row = tbl.Rows.Add();
					foreach (var cell in wsRow)
					{
						row[cell.Start.Column - 1] = cell.Text;
					}
				}
				return tbl;
			}
		}

		public static bool IsNullOrEmpty<T>(T[] array)
		{
			return array == null || array.Length == 0;
		}

		private static string getSpaces(int counter)
		{
			return new String(' ', counter);
		}

		public static DataTable GetDataTableFromExcelHeaders(string path, bool hasHeader = true)
		{
			using (var pck = new OfficeOpenXml.ExcelPackage())
			{
				try
				{
					using (var stream = File.OpenRead(path))
					{
						pck.Load(stream);
					}
				}
				catch (IOException ex)
				{
					MessageBox.Show("Program cannot acces to excel file, try to close file and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				if (pck.Workbook.Worksheets.Count == 0)
				{
					return null;
				}
				var ws = pck.Workbook.Worksheets.First();
				DataTable tbl = new DataTable();
				for (int i = 1; i <= ws.Dimension.End.Column; i++)
				{
					tbl.Columns.Add();
				}
				var startRow = hasHeader ? 2 : 1;
				for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
				{
					var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
					if (string.IsNullOrEmpty(ws.Cells[rowNum, 1].Text))
					{
						break;
					}
					DataRow row = tbl.Rows.Add();
					foreach (var cell in wsRow)
					{
						row[cell.Start.Column - 1] = cell.Text;
					}
				}
				return tbl;
			}
		}


		public static string CleanCompanyName(string data)
		{
			if (string.IsNullOrEmpty(data))
			{
				return string.Empty;
			}
			string newChar = string.Empty;
			data =
				data.Replace("Inc.", newChar)
					.Replace("Ltd.", newChar)
					.Replace("Holdings", newChar)
					.Replace("HOLDINGS", newChar)
					.Replace("LIMITED", newChar)
					.Replace("Holding", newChar)
					.Replace("Group", newChar)
					.Replace("Plc", newChar)
					.Replace(",", newChar)
					.Replace(".", newChar);
			var res = data.Trim().Split(' ').AsEnumerable().Where(x => x.Length > 1);
			var resData = string.Join(" ", res);
			return resData;
		}

		public static string CleanName(string data)
		{
			if (string.IsNullOrEmpty(data))
			{
				return string.Empty;
			}
			string newChar = string.Empty;
			data =
				data.Replace("MBA", newChar)
					.Replace("PhD", newChar)
					.Replace("CPA", newChar)
					.Replace("Jr", newChar)
					.Replace("Sr", newChar)
					.Replace("MD", newChar)
					.Replace("family", newChar)
					.Replace("CFA", newChar)
					.Replace(",", newChar)
					.Replace(".", newChar);
			data = Regex.Replace(data, @"\s[I]{1,}\s", string.Empty);
			data = Regex.Replace(data, @"(\s)([IV]{1,})(\b|\s)", string.Empty);
			var res = data.Trim().Split(' ').AsEnumerable().Where(x => x.Length > 1);
			var resData = string.Join(" ", res);
			return resData;
		}
	}
}

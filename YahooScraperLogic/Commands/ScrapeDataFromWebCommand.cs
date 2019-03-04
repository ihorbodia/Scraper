using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.Style;
using Sraper.Common;
using Sraper.Common.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using YahooScraperLogic.ViewModels;

namespace YahooScraperLogic.Commands
{
	public class ScrapeDataFromWebCommand : ICommand
	{
		readonly ScraperViewModel parent;
		object lockObject = new object();

		DataTable CountryListData = new DataTable();
		DataTable CountryData = new DataTable();

		Dictionary<DataRow, string> nosValues = new Dictionary<DataRow, string>();

		public ScrapeDataFromWebCommand(ScraperViewModel parent)
		{
			this.parent = parent;
			parent.PropertyChanged += delegate { CanExecuteChanged?.Invoke(this, EventArgs.Empty); };
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return !string.IsNullOrEmpty(parent.FileProcessingLabelData) &&
					!string.IsNullOrEmpty(parent.WSJCodesFileLabelData) &&
					!parent.FileProcessingLabelData.Equals(StringConsts.FileProcessingLabelData_Processing);
		}

		public async void Execute(object parameter)
		{
			string chosenPath = parent.CountryListLabelData;
			if (string.IsNullOrEmpty(chosenPath.Trim()))
			{
				return;
			}
			parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_Processing;
			CountryListData = FilesHelper.GetDataTableFromExcel(parent.CountryListLabelData, true);
			CountryData = FilesHelper.GetDataTableFromExcel(parent.WSJCodesFileLabelData, true);
			if (CountryListData != null)
			{
				try
				{
					await Task.WhenAll(CountryListData.AsEnumerable().Select(row => DownloadFileAsync(row)));
				}
				catch (Exception ex)
				{
					parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_ErrorMessage;
				}
			}

			//AB -> 27

			if (nosValues.Count > 0)
			{
				using (var pck = new OfficeOpenXml.ExcelPackage(new FileInfo(parent.WSJCodesFileLabelData)))
				{
					try
					{
						var sheet = pck.Workbook.Worksheets.FirstOrDefault();
					
						foreach (var nosValue in nosValues)
						{
							int rowIndex = CountryData.Rows.IndexOf(nosValue.Key);
							if (rowIndex == -1)
							{
								continue;
							}
							var row = (rowIndex + 2);
							var cell = sheet.Cells[row, 28];
							cell.Value = Int64.Parse(nosValue.Value);
						}
						pck.Save();
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
						parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_ErrorMessage;
					}
				}
			}
			parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_Finish;
			Console.WriteLine(StringConsts.FileProcessingLabelData_Finish);
		}

		private async Task DownloadFileAsync(DataRow row)
		{

			string link = row[6].ToString();
			string companyName = row[1].ToString();

			DataRow companyRowData = null;
			foreach (DataRow rowItem in CountryData.Rows)
			{
				if (rowItem[1] != null && rowItem[1].ToString().Trim().Equals(companyName.Trim()))
				{
					companyRowData = rowItem;
					break;
				}
			}

			Regex regex = new Regex("Instrument=[^&]+");
			Match match = regex.Match(link);

			if (!match.Success)
			{
				throw new RegexMatchTimeoutException();
			}
			string code = match.Value.Replace("Instrument=", "");

			string webLink = $"http://www.nasdaqomxnordic.com/webproxy/DataFeedProxy.aspx?SubSystem=Prices&Action=GetInstrument&Instrument={code}&inst.an=nos&json=1&app=/shares/microsite-ShareInformation";

			string url = Uri.EscapeUriString(webLink);
			string doc = "";
			using (System.Net.WebClient client = new System.Net.WebClient()) // WebClient class inherits IDisposable
			{
				doc = await client.DownloadStringTaskAsync(new Uri(url));
			}

			JObject json = JObject.Parse(doc);
			string instDataArr = Convert.ToString(json["inst"]);
			var instDataJsonArr = JObject.Parse(instDataArr);
			string nosValue = Convert.ToString(instDataJsonArr["@nos"]);

			nosValues.Add(companyRowData, nosValue);
		}
	}
}

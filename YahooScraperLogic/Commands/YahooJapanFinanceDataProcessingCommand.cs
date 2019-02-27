using HtmlAgilityPack;
using OfficeOpenXml.Style;
using Sraper.Common;
using Sraper.Common.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using YahooFinanceApi;
using YahooScraperLogic.ViewModels;

namespace YahooScraperLogic.Commands
{
    public class YahooJapanFinanceDataProcessingCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        readonly YahooScraperViewModel parent;
        List<DataRow> errors = new List<DataRow>();
		DataTable JapanListTable = new DataTable();
		DataTable WSJListTable = new DataTable();
		List<int> errorRows = new List<int>();
		int counter = 0;
        object lockObject = new object();
        public YahooJapanFinanceDataProcessingCommand(YahooScraperViewModel parent)
        {
            this.parent = parent;
            parent.PropertyChanged += delegate { CanExecuteChanged?.Invoke(this, EventArgs.Empty); };
        }
        public bool CanExecute(object parameter)
        {
			return !string.IsNullOrEmpty(parent.FileProcessingLabelData) &&
					!string.IsNullOrEmpty(parent.WSJCodesFileLabelData) &&
					!parent.FileProcessingLabelData.Equals(StringConsts.FileProcessingLabelData_Processing);
        }

        public async void Execute(object parameter)
        {
			counter = 0;
			string chosenPath = parent.CountryListLabelData;
            if (string.IsNullOrEmpty(chosenPath.Trim()))
            {
                return;
            }
            parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_Processing;
            JapanListTable = FilesHelper.GetDataTableFromExcel(parent.CountryListLabelData);
			WSJListTable = FilesHelper.GetDataTableFromExcel(parent.WSJCodesFileLabelData, true);
			if (JapanListTable != null)
            {
				try
				{
					var symbols = JapanListTable.AsEnumerable().Select(x => x[2]?.ToString()).ToList();
					await Task.WhenAll(symbols.Select(symbol => DownloadDataAsync(symbol)));
				}
				catch (Exception ex)
				{
					parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_ErrorMessage;
				}
            }

			if (errorRows.Count > 0)
			{
                using (var pck = new OfficeOpenXml.ExcelPackage(new FileInfo(parent.CountryListLabelData)))
                {
                    try
                    {
                        var sheet = pck.Workbook.Worksheets.FirstOrDefault();
                        foreach (var item in errorRows)
                        {
                            var range = sheet.SelectedRange[item, sheet.Dimension.Start.Column, item, sheet.Dimension.End.Column];
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.IndianRed);
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

		private async Task DownloadDataAsync(string symbol)
		{
			string code = $"{symbol}";
			if (parent.ProcessingJapanFile)
			{
				code += ".T";
			}
			var result = await Yahoo.Symbols(code).Fields(Field.RegularMarketVolume).QueryAsync();
			if (result.FirstOrDefault().Value == null)
			{
				ExtractDataFromWSJpage(symbol);
			}
		}

		private void ExtractDataFromWSJpage(string symbol)
		{
			DataRow listRow = null;
			foreach (DataRow japanRowItem in JapanListTable.Rows)
			{
				if (japanRowItem[0] != null && japanRowItem[2].ToString().Trim().Equals(symbol.Trim()))
				{
					listRow = japanRowItem;
					break;
				}
			}

			string reductedCompanyName = listRow[10].ToString();

			DataRow wsjRow = null;
			foreach (DataRow wsjRowItem in WSJListTable.Rows)
			{
				if (wsjRowItem[0] != null && wsjRowItem[0].ToString().Trim().Equals(reductedCompanyName.Trim()))
				{
					wsjRow = wsjRowItem;
					break;
				}
			}

			string code = symbol.ToLower().Replace(".si", "");
			string bColumnWSJList = wsjRow[1]?.ToString();
			string cColumnWSJLIst = wsjRow[2]?.ToString();

			string url = $"https://quotes.wsj.com/{bColumnWSJList}/{cColumnWSJLIst}/{code}?mod=DNH_S_cq";
			HtmlDocument doc = WebHelper.GetPageData(url);
			var volume = doc.GetElementbyId("quote_volume");
			if (volume != null)
			{
				string strVolume = volume.InnerText.Replace(",", "").Replace(".", "");
				int vol = Int32.Parse(strVolume);
				if (vol <= 0)
				{
					errorRows.Add(JapanListTable.Rows.IndexOf(listRow));
				}
			}
            else
            {
                errorRows.Add(JapanListTable.Rows.IndexOf(listRow) + 2);
            }
		}
    }
}

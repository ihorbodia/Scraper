﻿using HtmlAgilityPack;
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
			string chosenPath = parent.JapanListLabelData;
            if (string.IsNullOrEmpty(chosenPath.Trim()))
            {
                return;
            }
            parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_Processing;
            JapanListTable = FilesHelper.GetDataTableFromExcel(parent.JapanListLabelData);
			WSJListTable = FilesHelper.GetDataTableFromExcel(parent.WSJCodesFileLabelData, true);
			if (JapanListTable != null)
            {
                try
                {
                    await DownloadMultipleFilesAsync(JapanListTable.AsEnumerable());
                }
                catch (Exception ex)
                {
                    parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_ErrorMessage;
                }
            }

			if (errorRows.Count > 0)
			{
				using (var pck = new OfficeOpenXml.ExcelPackage())
				{
					try
					{
						using (var stream = File.OpenRead(parent.JapanListLabelData))
						{
							pck.Load(stream);
						}

						var sheet = pck.Workbook.Worksheets.FirstOrDefault();

						foreach (var item in errorRows)
						{
							var range = sheet.SelectedRange[item, sheet.Dimension.Start.Column, item, sheet.Dimension.End.Column];
							range.Style.Fill.PatternType = ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
						}
						pck.Save();
					}
					catch (Exception e)
					{
						parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_ErrorMessage;
					}
				}
			}
			parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_Finish;
            Console.WriteLine(StringConsts.FileProcessingLabelData_Finish);
        }

        private async Task DownloadFileAsync(DataRow row)
        {
            try
            {
                Yahoo.IgnoreEmptyRows = true;

				string yahooCode = row[2] == null ? "" : row[2].ToString();
				var history = await Yahoo.GetHistoricalAsync($"{yahooCode}.T", DateTime.Today.AddDays(-1), DateTime.Today, Period.Daily);
				
				lock (lockObject)
                {
					var item = history.FirstOrDefault();
					if (item?.Volume > 0)
					{
						return;
					}
					string reductedCompanyName = row[10].ToString();
					var wsjRow = WSJListTable.Select($"listing = '{reductedCompanyName}'").FirstOrDefault();
					string code = row.Field<string>("WSJ code");
					string bColumnWSJList = wsjRow.Field<string>("WSJ code");
					string cColumnWSJLIst = wsjRow.Field<string>("WSJ prefix");

					string url = $"https://quotes.wsj.com/{bColumnWSJList}/{cColumnWSJLIst}/{code}?mod=DNH_S_cq";
					HtmlDocument doc = WebHelper.GetPageData(url);
					int volume = Int32.Parse(doc.GetElementbyId("quote_volume").InnerText.Replace(",", "").Replace(".", ""));
					if (volume <= 0)
					{
						errorRows.Add(JapanListTable.Rows.IndexOf(row));
					}
				}
            }
            catch (Exception ex)
            {
                lock (lockObject)
                {
                    if (!ex.Message.Contains("Invalid ticker or endpoint for symbol"))
                    {
                        errors.Add(row);
                    }
                }
                Console.WriteLine("Something wrong");
            }
        }

        private async Task DownloadMultipleFilesAsync(IEnumerable<DataRow> rows)
        {
			counter++;
            await Task.WhenAll(rows.Select(row => DownloadFileAsync(row)));
            if (errors.Any() && counter < 10)
            {
                List<DataRow> items = new List<DataRow>(errors);
                errors.Clear();
                await DownloadMultipleFilesAsync(items);
            }
        }
    }
}

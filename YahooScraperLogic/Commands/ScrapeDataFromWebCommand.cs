using HtmlAgilityPack;
using OfficeOpenXml.Style;
using Sraper.Common;
using Sraper.Common.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using YahooFinanceApi;
using YahooScraperLogic.ViewModels;

namespace YahooScraperLogic.Commands
{
    public class ScrapeDataFromWebCommand : ICommand
    {
        readonly ScraperViewModel parent;
        object lockObject = new object();

        DataTable JapanListTable = new DataTable();
        DataTable WSJListTable = new DataTable();

        List<int> errorRows = new List<int>();

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
            JapanListTable = FilesHelper.GetDataTableFromExcel(parent.CountryListLabelData);
            WSJListTable = FilesHelper.GetDataTableFromExcel(parent.WSJCodesFileLabelData, true);
            if (JapanListTable != null)
            {
                try
                {
                    await Task.WhenAll(JapanListTable.AsEnumerable().Select(row => DownloadFileAsync(row)));
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

        private async Task DownloadFileAsync(DataRow row)
        {
            IReadOnlyList<Candle> history = null;
            try
            {
                Yahoo.IgnoreEmptyRows = true;

                string yahooCode = row[2] == null ? "" : row[2].ToString();
                string code = $"{yahooCode}";
                if (parent.ProcessingJapanFile)
                {
                    code += ".T";
                }
                history = await Yahoo.GetHistoricalAsync(code, DateTime.Today, DateTime.Today, Period.Daily);
                if (history != null && history.FirstOrDefault().Volume == 0)
                {
                    ExtractDataFromWSJpage(row);
                }
            }
            catch (Exception ex)
            {
                lock (lockObject)
                {
                    if (ex.Message.Contains("Invalid ticker or endpoint for symbol"))
                    {
                        ExtractDataFromWSJpage(row);
                    }
                    
                }
            }
        }

        private void ExtractDataFromWSJpage(DataRow row)
        {
            string reductedCompanyName = row[10].ToString();
            DataRow wsjRow = null;
            foreach (DataRow wsjRowItem in WSJListTable.Rows)
            {
                if (wsjRowItem[0] != null && wsjRowItem[0].ToString().Trim().Equals(reductedCompanyName.Trim()))
                {
                    wsjRow = wsjRowItem;
                    break;
                }
            }

            string code = row[3]?.ToString();
            string bColumnWSJList = wsjRow[1]?.ToString();
            string cColumnWSJLIst = wsjRow[2]?.ToString();

            string url = $"https://quotes.wsj.com/{bColumnWSJList}/{cColumnWSJLIst}/{code}?mod=DNH_S_cq";
            HtmlDocument doc = WebHelper.GetPageData(url);
            var volume = doc.GetElementbyId("quote_volume");
            if (volume != null)
            {
                string test = volume.InnerText.Replace(",", "").Replace(".", "");
                int vol = Int32.Parse(test);
                if (vol <= 0)
                {
                    errorRows.Add(JapanListTable.Rows.IndexOf(row));
                }
            }
            else
            {
                errorRows.Add(JapanListTable.Rows.IndexOf(row) + 2);
            }
        }
    }
}

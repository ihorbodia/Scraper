using Sraper.Common;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using YahooFinanceApi;
using YahooScraperLogic.ViewModels;

namespace YahooScraperLogic.Commands
{
    public class ProcessFileCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        readonly YahooScraperViewModel parent;
        public ProcessFileCommand(YahooScraperViewModel parent)
        {
            this.parent = parent;
            parent.PropertyChanged += delegate { CanExecuteChanged?.Invoke(this, EventArgs.Empty); };
        }
        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(parent.FileProcessingLabelData) &&
                    !string.IsNullOrEmpty(parent.FolderForStoringFilesLabelData) &&
                    !parent.FileProcessingLabelData.Equals(StringConsts.FileProcessingLabelData_Processing)
                    && (parent.SelectedDateFrom.Date < parent.SelectedDateTo.Date);
        }

        public async void Execute(object parameter)
        {
            string chosenPath = parent.FilePathLabelData;
            if (string.IsNullOrEmpty(chosenPath.Trim()))
            {
                return;
            }
            parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_Processing;
            var table = FilesHelper.GetDataTableFromExcel(parent.FilePathLabelData).AsEnumerable();
            try
            {
                await DownloadMultipleFilesAsync(table);
                parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_Finish;
                Console.WriteLine(StringConsts.FileProcessingLabelData_Finish);
            }
            catch (Exception)
            {
                parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_ErrorMessage;
            }
        }

        private async Task DownloadFileAsync(DataRow row)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    var history = await Yahoo.GetHistoricalAsync(row[2].ToString(), new DateTime(
                        parent.SelectedDateFrom.Year,
                        parent.SelectedDateFrom.Month,
                        parent.SelectedDateFrom.Day), new DateTime(
                        parent.SelectedDateTo.Year,
                        parent.SelectedDateTo.Month,
                        parent.SelectedDateTo.Day), Period.Daily);
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Date,Open,High,Low,Close,Adj Close,Volume");
                    foreach (var item in history)
                    {
                        sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6}",
                            item.DateTime.ToString("yyyy-MM-dd"),
                            commaToDot(item.Open),
                            commaToDot(item.High),
                            commaToDot(item.Low),
                            commaToDot(item.Close),
                            commaToDot(item.AdjustedClose),
                            commaToDot(item.Volume)));
                    }
                    File.WriteAllText(Path.Combine(parent.FolderForStoringFilesLabelData, row[1]+".csv"), sb.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something wrong");
            }
        }

        private async Task DownloadMultipleFilesAsync(EnumerableRowCollection<DataRow> rows)
        {
            await Task.WhenAll(rows.Select(row => DownloadFileAsync(row)));
        }
        private string commaToDot(decimal value)
        {
            return value.ToString().Replace(',','.');
        }
    }
}

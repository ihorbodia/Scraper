﻿using Sraper.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
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
        object lockObject = new object();
        public YahooJapanFinanceDataProcessingCommand(YahooScraperViewModel parent)
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
            var table = FilesHelper.GetDataTableFromExcel(parent.FilePathLabelData);
            if (table != null)
            {
                try
                {
                    await DownloadMultipleFilesAsync(table.AsEnumerable());
                }
                catch (Exception ex)
                {
                    parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_ErrorMessage;
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
                var history = await Yahoo.GetHistoricalAsync(row[2].ToString() + ".T", new DateTime(
                    parent.SelectedDateFrom.Year,
                    parent.SelectedDateFrom.Month,
                    parent.SelectedDateFrom.Day), new DateTime(
                    parent.SelectedDateTo.Year,
                    parent.SelectedDateTo.Month,
                    parent.SelectedDateTo.Day), Period.Daily);
                StringBuilder sb = new StringBuilder();
                lock (lockObject)
                {
                    
                    sb.AppendLine("Date;Close;Volume");
                    foreach (var item in history)
                    {
                        sb.AppendLine(string.Format("{0};{1};{2}",
                        item.DateTime.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("fr-FR")),
                        item.Close,
                        item.Volume));
                    }
                }
                File.WriteAllText(Path.Combine(parent.FolderForStoringFilesLabelData, row[1] + ".csv"), sb.ToString());
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
            await Task.WhenAll(rows.Select(row => DownloadFileAsync(row)));
            if (errors.Any())
            {
                List<DataRow> items = new List<DataRow>(errors);
                errors.Clear();
                await DownloadMultipleFilesAsync(items);
            }
        }
    }
}

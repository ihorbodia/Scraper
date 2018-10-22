using HtmlAgilityPack;
using ScrapySharp.Network;
using Sraper.Common;
using Sraper.Common.Models;
using System;
using System.Data;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

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
                    !parent.FileProcessingLabelData.Equals(StringConsts.FileProcessingLabelData_Processing);
        }

        public async void Execute(object parameter)
        {
            string chosenPath = parent.FilePathLabelData;
            string chosenFodlerPath = parent.FolderForStoringFilesLabelData;
            //фывфывфывЫ
            if (string.IsNullOrEmpty(chosenPath.Trim()))
            {
                return;
            }
            parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_Processing;
            var table = FilesHelper.GetDataTableFromExcel(parent.FilePathLabelData).AsEnumerable();

            Encoding iso = Encoding.GetEncoding("iso-8859-1");
            HtmlWeb web = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = iso,
            };
            //https://finance.yahoo.com/quote/OMV.VI/history?period1=1490997600&period2=1522188000&interval=1d&filter=history&frequency=1d

            ScrapingBrowser Browser = new ScrapingBrowser();

            WebPage PageResult = await Browser.NavigateToPageAsync(new Uri("https://finance.yahoo.com/quote/OMV.VI/history?period1=1490997600&period2=1522188000&interval=1d&filter=history&frequency=1d"));
            HtmlNodeCollection rawHTML = PageResult.Html.SelectNodes("//span[@class='Fl(end) Pos(r) T(-6px)']");
            //Console.WriteLine(rawHTML.InnerHtml);

            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"https://finance.yahoo.com/quote/OTS.VI/history?period1=1490997600&period2=1522188000&interval=1d&filter=history&frequency=1d");
            //request.CookieContainer = new CookieContainer();

            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //Cookie cook = response.Cookies[0];

            //using (WebClient webClient = new WebClient())
            //{
            //    Uri url = new Uri(@"https://query1.finance.yahoo.com/v7/finance/download/OTS.VI?period1=1490997600&period2=1522188000&interval=1d&events=history&crumb=1RG5lY0Nras");
            //    string downloadToDirectory = parent.FolderForStoringFilesLabelData + "\\SEM.VI.csv";
            //    webClient.Headers.Add(HttpRequestHeader.Cookie, string.Format("{0}={1}", cook.Name, cook.Value));
            //    webClient.UseDefaultCredentials = true;
            //    webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            //    webClient.DownloadFile(url, downloadToDirectory);
            //}
            try
            {

                //await DownloadMultipleFilesAsync(table);
                //await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                //{
                //    parent.FileProcessingLabelData = StringConsts.FileProcessingLabelData_Finish;
                //    Console.WriteLine(StringConsts.FileProcessingLabelData_Finish);
                //}));
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
                    Uri url = new Uri(
                        string.Format(@"https://query1.finance.yahoo.com/v7/finance/download/{0}?period1=1490997600&period2=1522188000&interval=1d&events=history&crumb=1RG5lY0Nras",
                        row[2]));
                    string downloadToDirectory = parent.FolderForStoringFilesLabelData +"\\"+ row[1] + ".csv";
                    webClient.UseDefaultCredentials = true;
                    webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    webClient.DownloadFile(url, downloadToDirectory);
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
    }
}

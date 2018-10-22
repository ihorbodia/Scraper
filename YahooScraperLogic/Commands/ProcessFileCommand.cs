using HtmlAgilityPack;
using Sraper.Common;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using WatiN.Core;
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

            //IE ie = new IE();
            //ie.GoTo("https://finance.yahoo.com/quote/OMV.VI/history?period1=1490997600&period2=1522188000&interval=1d&filter=history&frequency=1d");
            //try
            //{
            //    ie.Button(Find.ByValue("OK")).Click();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}
            //ie.Link(Find.ByText("Download Data")).Click();

            var history = await Yahoo.GetHistoricalAsync("AAPL", new DateTime(2016, 1, 1), new DateTime(2016, 7, 1));
            foreach (var candle in history)
            {
                Console.WriteLine($"DateTime: {candle.DateTime}, Open: {candle.Open}, High: {candle.High}, Low: {candle.Low}, Close: {candle.Close}, Volume: {candle.Volume}, AdjustedClose: {candle.AdjustedClose}");
            }

            //LoadHtmlWithBrowser("https://finance.yahoo.com/quote/OMV.VI/history?period1=1490997600&period2=1522188000&interval=1d&filter=history&frequency=1d");


            //using (WebClient webClient = new WebClient())
            //{
            //    Uri url = new Uri(@"https://finance.yahoo.com/quote/OMV.VI/history?period1=1490997600&period2=1522188000&interval=1d&filter=history&frequency=1d");
            //    //string downloadToDirectory = parent.FolderForStoringFilesLabelData + "\\SEM.VI.csv";
            //    webClient.UseDefaultCredentials = true;
            //    webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            //    var test = webClient.DownloadString(url);
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



        private void LoadHtmlWithBrowser(String url)
        {
            WebBrowser webBrowser = new WebBrowser();
            
            webBrowser.ScriptErrorsSuppressed = true;
            webBrowser.Navigate(url);

            waitTillLoad(webBrowser);

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            var documentAsIHtmlDocument3 = (mshtml.IHTMLDocument3)webBrowser.Document.DomDocument;
            StringReader sr = new StringReader(documentAsIHtmlDocument3.documentElement.outerHTML);
            doc.Load(sr);
        }

        private void waitTillLoad(WebBrowser webBrControl)
        {
            WebBrowserReadyState loadStatus;
            int waittime = 100000;
            int counter = 0;
            while (true)
            {
                loadStatus = webBrControl.ReadyState;
                Application.DoEvents();
                if ((counter > waittime) || (loadStatus == WebBrowserReadyState.Uninitialized) || (loadStatus == WebBrowserReadyState.Loading) || (loadStatus == WebBrowserReadyState.Interactive))
                {
                    break;
                }
                counter++;
            }

            counter = 0;
            while (true)
            {
                loadStatus = webBrControl.ReadyState;
                Application.DoEvents();
                if (loadStatus == WebBrowserReadyState.Complete && webBrControl.IsBusy != true)
                {
                    break;
                }
                counter++;
            }
        }
    }
}

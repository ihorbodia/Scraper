using HtmlAgilityPack;
using System.Linq;

namespace Sraper.Common.Models.ShareholderModels
{
    public class ShareholderPage
    {
        HtmlDocument document;
        string shareholderName;
        public ShareholderPage(HtmlDocument HtmlDocument, string ShareholderName)
        {
            document = HtmlDocument;
            shareholderName = ShareholderName;
            parseHtmlDocument();

            var shareholdersTable = document.DocumentNode.SelectNodes("//table[@class='nfvtTab linkTabBl']")
                .FirstOrDefault(x => x.Attributes.Count > 5);

            var managersTable = document.DocumentNode.SelectNodes("//table[@class='nfvtTab linkTabBl']")
                .FirstOrDefault(x => x.Attributes.Count < 6)
                .ChildNodes.Where(x => x.Name == "tr" && x.PreviousSibling.Name == "tr")
                .SelectMany(x => x.ChildNodes.Where(y => y.Name == "td"));

            var data = shareholdersTable.ChildNodes.Where(x => x.Name == "tr" && x.PreviousSibling.Name == "tr")
                .Select(x => x.ChildNodes.Where(y => y.Name == "td").FirstOrDefault().InnerText.Trim());

            var linkToSummary = shareholdersTable.ChildNodes
                .Where(x => x.Name == "tr" && x.PreviousSibling.Name == "tr")
                .Where(x => x.FirstChild.Attributes["class"].Value == "nfvtL")
                .Where(x => x.FirstChild.FirstChild.Name == "a")
                .Where(x => x.InnerText.Trim().ToUpper().Contains(shareholderName))
                .Select(n => n.FirstChild.FirstChild.Attributes["href"].Value).FirstOrDefault();

            var doc = WebHelper.GetShareholderPage(shareholderName, linkToSummary).DocumentNode;
            var personPageTables = doc.SelectNodes("//table[@class='tabElemNoBor overfH']");

            var htmlPositionsTable = personPageTables.FirstOrDefault(x => x.InnerText.Contains("Current positions"));
            CurrentPositionTable = new CurrentPositionTable(htmlPositionsTable);

            var summaryText = personPageTables.FirstOrDefault(x => x.InnerText.Contains("Summary")).InnerText.Trim();
            SummaryText = summaryText;

            var holdingsTable = personPageTables.FirstOrDefault(x => x.InnerText.Contains("Holdings table"));
            HoldingsTable = new HoldingsTable(holdingsTable);
        }

        public readonly CurrentPositionTable CurrentPositionTable;
        public readonly HoldingsTable HoldingsTable;
        public readonly string SummaryText;

        private void parseHtmlDocument()
        {
            //IEnumerable<IList<HtmlNode>> currentPositionCompanies = null;
            //if (htmlPositionsTable != null)
            //{
            //    currentPositionCompanies = htmlPositionsTable
            //    .ChildNodes
            //    .FirstOrDefault(x => x.Name == "tr" && x.LastChild.Name == "td")
            //    .FirstChild
            //    .FirstChild
            //    .FirstChild
            //    .ChildNodes
            //    .Where(x => x.Name == "tr" && x.PreviousSibling.Name == "tr")
            //    .Select(x => x.ChildNodes.Where(y => y.Name == "td").ToList());
            //}
        }
    }
}

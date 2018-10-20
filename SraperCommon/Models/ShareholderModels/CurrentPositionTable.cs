using HtmlAgilityPack;
using System.Collections.Generic;

namespace Sraper.Common.Models.ShareholderModels
{
    public class CurrentPositionTable
    {
        private HtmlNode document;
        IEnumerable<CurrentPositionTableItem> Rows;
        public CurrentPositionTable(HtmlNode htmlDocument)
        {
            htmlDocument = document;
            Rows = new List<CurrentPositionTableItem>();
        }
    }

    public class CurrentPositionTableItem
    {
        public CurrentPositionTableItem()
        {

        }
        public string CompanyName { get; set; }
        public string JobTitle { get; set; }
    }
}

using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sraper.Common.Models.ShareholderModels
{
    public class HoldingsTable
    {
        HtmlNode document;
        IEnumerable<HoldingsTableItem> Rows;
        public HoldingsTable(HtmlNode htmlDocument)
        {
            document = htmlDocument;
            Rows = new List<HoldingsTableItem>();
        }
    }

    public class HoldingsTableItem
    {
        public string Name { get; set; }
        public string Equitities { get; set; }
        public string Percent { get; set; }
        public  string Valuation { get; set; }
        public HoldingsTableItem()
        {

        }
    }

}

using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YahooScrapper
{
    public class YahooScrapperLogic
    {
        readonly List<Task> tasks;
        public YahooScrapperLogic()
        {
            tasks = new List<Task>();
        }

        public void ProcessFile(string filePath, string countryFolderPathData)
        {
            string excelFilePath = new FileInfo(filePath).FullName;
            string countryFolderPath = new FileInfo(countryFolderPathData).FullName;
            FileInfo fi = new FileInfo(excelFilePath);
            if (!fi.Exists)
            {
                return;
            }
            using (ExcelPackage p = new ExcelPackage(fi))
            {
                ExcelWorksheet workSheet = p.Workbook.Worksheets[1];
                var start = workSheet.Dimension.Start.Row + 1;
                var end = workSheet.Dimension.End.Row;
                for (int row = start; row <= end; row++)
                {
                    string companyName = workSheet.Cells[row, 1].Text;
                    string fileName = workSheet.Cells[row, 2].Text;
                    string URL = workSheet.Cells[row, 6].Text;
                    if (string.IsNullOrEmpty(companyName))
                    {
                        break;
                    }
#if DEBUG
                    //var htmlDocument = WebHelper.GetPageData(URL);
                    //DirectoryInfo d = new DirectoryInfo(countryFolderPath);
                    //var file = d.GetFiles("*.xlsx").FirstOrDefault(x => x.Name.Contains(fileName));
                    //if (file == null)
                    //{
                    //    break;
                    //}
                    //using (ExcelPackage pck = new ExcelPackage(file))
                    //{
                    //    ExcelWorksheet ws = pck.Workbook.Worksheets.FirstOrDefault(x => x.Name.Equals("Feuil1"));
                    //    if (ws == null)
                    //    {
                    //        break;
                    //    }
                    //    ws.Cells["A1"].LoadFromDataTable(table.ShareholdersDataTable, false);
                    //    ws.Cells["C1:C" + table.ShareholdersDataTable.Rows.Count].Style.Numberformat.Format = "#0.##%";
                    //    ws.Cells.AutoFitColumns();
                    //    SaveFile(pck);
                    //}
#else
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        var htmlDocument = WebHelper.GetPageData(URL);
                        ShareholdersTable table = new ShareholdersTable();
                        table.InitTable(htmlDocument);
                        DirectoryInfo d = new DirectoryInfo(countryFolderPath);
                        var file = d.GetFiles("*.xlsx").FirstOrDefault(x => x.Name.Contains(fileName));
                        if (file == null)
                        {
                            return;
                        }
                        using (ExcelPackage pck = new ExcelPackage(file))
                        {
                            ExcelWorksheet ws = pck.Workbook.Worksheets.FirstOrDefault(x => x.Name.Equals("Feuil1"));
                            if (ws == null)
                            {
                                return;
                            }
                            ws.Cells["A1"].LoadFromDataTable(table.ShareholdersDataTable, false);
                            ws.Cells["C1:C" + table.ShareholdersDataTable.Rows.Count].Style.Numberformat.Format = "#0.0#%";
                            ws.Cells.AutoFitColumns();
                            SaveFile(pck);
                        }
                    }));
#endif
                }
            }
            if (tasks.Count > 0)
            {
                Task.WaitAll(tasks.ToArray());
            }
        }

        private void SaveFile(ExcelPackage pck)
        {
            try
            {
                pck.Save();
            }
            catch (InvalidOperationException ex)
            {
                DialogResult result = MessageBox.Show("Try to close excel file " + ex.Message.Substring(ex.Message.LastIndexOf('\\') + 1) + " and click OK.", "Error while saving file", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if (result == DialogResult.OK)
                {
                    SaveFile(pck);
                }
            }
        }
    }
}

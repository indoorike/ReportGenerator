using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml.Table;

namespace AccountingReportsGenerator
{
    public class AccountingExcel
    {
        private readonly string _activePath;
        private readonly string _archivePath;
        public AccountingExcel(string activepath, string archivepath)
        {
            _activePath = activepath;
            _archivePath = archivepath;
            CreateDirectories();
        }
        public void CreateExcels(dynamic[] sheetsToCreate) 
        {
            //For each collection of data passed in, we create an excel. 
            //We need to know what type of class type each collection holds, so that we can dynamically format the excel.
            foreach (var sheetToCreate in sheetsToCreate)
            {
                using (var package = new ExcelPackage())
                {
                    //If the collection is empty, create a blank excel
                    if(sheetsToCreate is null || sheetsToCreate.Length == 0)
                    {
                        Type blankType = sheetToCreate[0].GetType();
                        var blankSheet = package.Workbook.Worksheets.Add($"No data found - {blankType.Name}");
                        var blankCells = blankSheet.Cells["A1"].Value = "No data was returned for this report. Please contact the development team if you believe this is a mistake.";

                        SaveExcel(package, blankType.Name);
                        continue;
                    }
                    Type sheetType = sheetToCreate[0].GetType();
                    var sheetProperties = sheetType.GetProperties();
                    var newSheet = package.Workbook.Worksheets.Add($"{sheetType.Name}");
                    var range = newSheet.Cells.LoadFromCollection(sheetToCreate, true, TableStyles.Medium1);


                    FormatCells(newSheet, sheetToCreate.Count, sheetProperties);
                    SaveExcel(package, sheetType.Name);
                }
            } 
        }
        public void ArchiveCurrentFiles()
        {
            string[] reportsForArchive = Directory.GetFiles(_activePath);
            for (int i = 0; i < reportsForArchive.Length; i++)
            {
                FileInfo fileInfo = new FileInfo(reportsForArchive[i]);
                File.Copy(fileInfo.FullName, $"{_archivePath}\\{fileInfo.Name}", overwrite: true);

                fileInfo.Delete();
            }
        }
        public int DeleteOldArchives(bool testMode = false)
        {
            //Gets all file path strings, and turns them into FileInfo objects.
            int filesDeleted = 0;
            List<FileInfo> filesDetails = new List<FileInfo>();
            string[] files = Directory.GetFiles(_archivePath);

            foreach(string file in files)
            {
                filesDetails.Add(new FileInfo(file));
            }
            //Checking if each file is 30 days old, and deleting it if so. 
            foreach(FileInfo fileDetail in filesDetails)
            {
                if (testMode && fileDetail.Name.Contains("InvoiceChargeTransactionsToCapture")) fileDetail.CreationTime = DateTime.Today.AddDays(-31);
                if (fileDetail.CreationTime < DateTime.Today.AddDays(-30))
                {
                    fileDetail.Delete();
                    filesDeleted += 1;
                }
            }
            return filesDeleted;
        }
        private static void FormatCells(ExcelWorksheet newsheet, int newsheetlength, PropertyInfo[] properties)
        {
            //Loops through all of the properties of the collection type of the new sheet being created.
            //When a 'formattable' (is that a word?) property is found, we know that is where the excel needs to be formatted. 
            //We increment the 'cell to be formatted' by one because excel has 1 based indecies.
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].PropertyType == typeof(DateTime))
                {
                    newsheet.Cells[1, i + 1, newsheetlength, i + 1].Style.Numberformat.Format = "yyyy-mm-dd";
                }
                if (properties[i].PropertyType == typeof(decimal) || properties[i].PropertyType == typeof(double))
                {
                    newsheet.Cells[1, i + 1, newsheetlength, i + 1].Style.Numberformat.Format = "$0.00";
                }
            }
            newsheet.Cells.AutoFitColumns();
            newsheet.View.FreezePanes(2, 1);
        }
        private void SaveExcel(ExcelPackage package, string filename)
        {
            package.SaveAs($"{_activePath}\\{filename}_{DateTime.Today:yyyy-MM-dd}.xlsx");
        }
        private void CreateDirectories()
        {
            if (!Directory.Exists(_archivePath))
            {
                Directory.CreateDirectory(_archivePath);
            }
            if (!Directory.Exists(_activePath))
            {
                Directory.CreateDirectory(_activePath);
            }
        }
    }
}

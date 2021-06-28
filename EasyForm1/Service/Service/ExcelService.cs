using Common.CommonModel;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Repository;
using Service.Iservice;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Service.Service
{
    class ExcelService : IExcelService
    {
        private IFileRepository repository;
        public ExcelService(IFileRepository repository)
        {
            this.repository = repository;
        }
        
        public void ExcelAB(ExcelData excelData)
        {
            using (var package = new ExcelPackage())
            {
                
                // Add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Inventory");
            //    List<string> FileName = repository.GetFilesByForm(formID);//Get list of file name
               // List<List<string>> Text = GetText();// Get list of Text

                //Add the headers
                worksheet.Cells["A1"].Value = "שם השדה";
                worksheet.Cells["B1"].Value = "ערך השדה";
                for (int i = 0; i < excelData.filesList.Count; i++)
                {
                    worksheet.Cells["A" + (i+2)].Value = excelData.filesList[i];
                     worksheet.Cells["B" + (i+2)].Value = excelData.resultsList[i];
                }


                var xlFile = Utils.GetFileInfo(@"F:\ExcelFile.xlsx");
                package.SaveAs(xlFile);
            }
        }
       

    }
}



using Common.CommonModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Service.Iservice
{
    public interface IExcelService
    {
       void ExcelAB( ExcelData excelData);
    }
}

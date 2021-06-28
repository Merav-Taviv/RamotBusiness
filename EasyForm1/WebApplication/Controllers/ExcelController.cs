using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Common.CommonModel;
using Microsoft.AspNetCore.Mvc;
using Service.Iservice;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    public class ExcelController : ControllerBase
    {
        IExcelService ExcelService;
        IImageService imageService;

        public ExcelController(IExcelService service, IImageService ImgService)
        {
            ExcelService = service;
            imageService = ImgService;
        }

        [HttpPost]
        public void excel([FromBody] ExcelData excel)
        {
            ExcelService.ExcelAB(excel);
        }
       



    }
}

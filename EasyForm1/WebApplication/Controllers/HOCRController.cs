using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.Iservice;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]

    public class HOCRController : ControllerBase
    {
        IHOCRService HOCRService;
        IImageService ImageService;
        public HOCRController(IHOCRService service, IImageService imageService)
        {
            HOCRService = service;
            ImageService = imageService;
        }

        [HttpGet]
        public string HOCR()
        {
            string x = HOCRService.HOCR();
            return x;

        }
    }
}


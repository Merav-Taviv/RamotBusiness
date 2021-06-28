using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Mvc;
using Service.Iservice;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication.Controllers
{

    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {

      IFileService FileService;
        public FileController(IFileService service)
        {
            FileService = service;
        }
        [HttpGet("{formID}")]
        //public List<FileCommon> GetFilesByForm(int formID)
        //{
        //    return FileService.GetFilesByForm(formID);
        //}
        public List<string> GetFilesByForm(int formID)
        {
            int x = 0;
            List<ProviderCommon> ff = FileService.GetFilesByForm(formID);
            List<string> file = new List<string>();
            foreach (ProviderCommon item in ff)
            {
                file.Insert(x++,item.FileName);
            }
            return file;
        }
        [HttpPost("{files}")]
        public void AddFiles([FromBody] List<ProviderCommon> fileCommom)
        {
            foreach (var item in fileCommom)
            {
                FileService.AddFile(item);
            }
        }
    }
}

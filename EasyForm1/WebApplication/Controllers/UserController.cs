using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Mvc;
using Service.Iservice;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        IUserService Userservice;
        IFileService FileService;
        IImageService ims;
        public UserController(IUserService service, IFileService FService, IImageService i)
        {
            Userservice = service;
            FileService = FService;
            ims = i;
        }

        [HttpGet("{email}")]
        public UserCommon GetUserByID(string email/*,string password*/)

        {
            return Userservice.GetUserByID(email/*,password*/);
        }

        [HttpPost]
        public bool AddUser ([FromBody] UserCommon userCommon)
        {
           return  Userservice.AddUser(userCommon);
        }

        [HttpPut]
        public void UpdateUser([FromBody]UserCommon userCommon)
        {
            Userservice.UpdateUser(userCommon);


        }
        [HttpPost("{files}")]
        public void AddFiles([FromBody] List <ProviderCommon> fileCommom)
        {
            foreach (var item in fileCommom)
            {
                FileService.AddFile(item);
            }
        }
    }
}

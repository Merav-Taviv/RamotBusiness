using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Mvc;
using Service.Iservice;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    public class FormController : ControllerBase
    {
        IFormService FormService;

        public FormController(IFormService service)
        {
            FormService = service;
        }

        [HttpGet]
        public List<FormCommon> GetAllFormsSharing()
        {
            return FormService.GetAllFormsSharing();
        }
        

        [HttpGet("{UserID}")]
        public List<FormCommon> GetAllFormsByUser(int UserID)
        {
            return FormService.GetAllFormsByUser(UserID);
        }
        [HttpGet("GetFormImage/{FormID}")]
        public string GetFormImage(int FormID)
        {
            return FormService.GetFormImagePath(FormID);
        }

        [HttpPost ]
        public FormCommon AddForm([FromBody] FormCommon formCommon)
        {
            return FormService.AddForm(formCommon);
        }   

        [HttpPut("{sharing}")]
        public void UpdateForm(FormCommon formCommon, UserCommon userCommon)
        {
            FormService.UpdateForm(formCommon,userCommon);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void DeleteFormByUser(int userID)
        {
            FormService.DeleteFormByUser(userID);
        }
        //[HttpGet ("{UpdateAmontOfUsing}")]
        //public void UpdateAmontOfUsing(int formid)
        //{
        //    FormService.UpdateAmontOfUsing(formid);
        //}
    }
}

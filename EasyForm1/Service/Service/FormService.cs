using Repository;
using Repository.Models;
using Service.Iservice;
using System;
using System.Collections.Generic;
using System.Text;
using Common;
using System.IO;

namespace Service
{
    class FormService : IFormService
    {

        private IFormRepository repository;
        public FormService(IFormRepository repository)
        {
            this.repository = repository;
        }
        public FormCommon AddForm( FormCommon formCommon)
        {
            var memoryStream = new MemoryStream(Convert.FromBase64String(formCommon.ImageSrc));
            using (FileStream file1 = new FileStream(@"F:\Project\EasyForm1\WebApplication\Images\" + formCommon.FormName+".png", FileMode.Create, FileAccess.Write))
            {
                memoryStream.WriteTo(file1);
            }
            formCommon.ImagePath = @"F:\Project\EasyForm1\WebApplication\Images\" + formCommon.FormName + ".png";
            return repository.AddForm(FormMap.MapFormCommonToForms(formCommon));
        }
        //public void UpdateAmontOfUsing(int formid)
        //{
        //    repository.UpdateAmontOfUsing(formid);
        //}

        //לשים לב האם אפשר לעדכן מסמך או רק לעדכן אם שיתופי או לא, כי אז צריך לשלוח פרמטרים אחרים
        public void UpdateForm(FormCommon formCommon, UserCommon userCommon)
        {
            repository.UpdateForm(FormMap.MapFormCommonToForms(formCommon), UserMap.MapUserCommonToUsers(userCommon));
        }
        public void DeleteFormByUser(int userID)
        {
            repository.DeleteFormByUser(userID);
        }
        public List<FormCommon> GetAllFormsSharing()
        {
            return repository.GetAllFormsSharing();
        }
        public List<FormCommon> GetAllFormsByUser(int UserID)
        {
            return repository.GetAllFormsByUser(UserID);
        }
        public string GetFormImagePath(int FormID)
        {
            return repository.GetFormImagePath(FormID);
        }
    }
}
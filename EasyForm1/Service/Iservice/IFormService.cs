using Common;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Iservice
{
    public interface IFormService
    {
        FormCommon AddForm(FormCommon form);
        void UpdateForm(FormCommon form, UserCommon user);
        void DeleteFormByUser(int userID);
        List<FormCommon> GetAllFormsSharing();
        List<FormCommon> GetAllFormsByUser(int UserID);
        string GetFormImagePath(int FormID);
        //void UpdateAmontOfUsing(int formid);

    }
}

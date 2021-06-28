using Common;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public interface ICategoryRepository
    {
        FeedbackCommon AddForm(Forms form);
        void UpdateForm(Forms form, Users user);
        void DeleteForm(Forms form, Users user);
        void DeleteFormByUser(int userID);
        List<FeedbackCommon> GetAllFormsSharing();
        List<FeedbackCommon> GetAllFormsByUser(int UserID);
        void UpdateLastUsing(Forms form);
        //void UpdateAmontOfUsing(int form);
        void RemoveOldForm();
        string GetFormImagePath(int FormID);
    }
}

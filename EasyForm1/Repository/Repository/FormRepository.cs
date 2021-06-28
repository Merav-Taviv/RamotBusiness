using Common;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace Repository
{
    public class FormRepository : IFormRepository
    {
        FileRepository FileRepository = new FileRepository();
        private Context1 context;

        public FormRepository(Context1 context)
        {
            this.context = context;
        }

        public FormRepository()
        {
        }

        public FormCommon AddForm(Forms form)
        {
            var x=context.Forms.Add(form);
            context.SaveChanges();
            return FormMap.MapFormsToFormCommon(x.Entity);
        }

        public void UpdateForm(Forms form, Users user)
        {
            Users Y = context.Users.Where(a => a.UserId == user.UserId).First();
            Forms x = context.Forms.Where(a => a.FormId == form.FormId).First();
            if (Y.UserId == x.UserId)
            {
                x.FormId = form.FormId;
                x.FormName = form.FormName;
                x.LastUsing = form.LastUsing;
                x.AmountOfUsing = form.AmountOfUsing;
                x.UserId = form.UserId;
                x.Sharing = form.Sharing;
                context.Forms.Update(x);
                context.SaveChanges();
            }
        }
        //לבדוק למה יש שתי ונקציות למחיקה
        public void DeleteForm(Forms form, Users user)
        {
            Users Y = context.Users.Where(a => a.UserId == user.UserId).First();
            Forms x = context.Forms.Where(a => a.FormId == form.FormId).First();
            if (Y.UserId == x.UserId)
            {
                FileRepository.DeleteFile(x.FormId);
                context.Forms.Remove(x);
                context.SaveChanges();
            }
        }
        public void DeleteFormByUser(int userID)
        {
            List<Forms> x = context.Forms.Where(a => a.UserId == userID && a.Sharing == false).ToList();
            foreach (Forms item in x)
            {
                FileRepository.DeleteFile(item.FormId);
                context.Forms.Remove(item);
            }
            context.SaveChanges();
        }
        public List<FormCommon> GetAllFormsSharing()
        {
            List<Forms> x = context.Forms.Where(a => a.Sharing == true).ToList();
            return FormMap.MapListFormsToFormCommon(x);
        }
        public List<FormCommon> GetAllFormsByUser(int UserID)
        {
            List<Forms> x = context.Forms.Where(a => a.UserId == UserID && a.Sharing == false).ToList();
            return FormMap.MapListFormsToFormCommon(x);
        }
        public void UpdateLastUsing(Forms form)
        {
            Forms x = context.Forms.Where(a => a.FormId == form.FormId).First();
            x.LastUsing = DateTime.Today;
            context.SaveChanges();
        }
        //public void UpdateAmontOfUsing(int form)
        //{
        //    Forms x = context.Forms.Where(a => a.FormId == form).FirstOrDefault();
        //    x.AmountOfUsing++;
        //    context.Update(x);
        //    context.SaveChanges();
        //}
        public void RemoveOldForm()
        {

            foreach (Forms item in context.Forms)
            {
                if ((DateTime.Today.Year) - item.LastUsing.Year > 3)// לעשות בדיקה לפי נגידי יומיים,, רק בשביל לראות שזה עובד
                {
                    Forms x = context.Forms.Where(a => a.FormId == item.FormId).First();
                    context.Forms.Remove(x);
                }
            }
            context.SaveChanges();
        }
        public string GetFormImagePath(int FormID)
        {
            string path=context.Forms.Where(a => a.FormId == FormID).FirstOrDefault().ImagePath;
            return  path;
        }

    }
}




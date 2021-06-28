using Common;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private Context1 context;
        FormRepository FormRepository = new FormRepository();
        public UserRepository(Context1 context)
        {
            this.context = context;
        }
        public bool AddUser(Users user)
        {
            var x = context.Users.Where(a => a.Email == user.Email).FirstOrDefault();
            if (x == null)
            {
                context.Users.Add(user);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public void UpdateUser(Users user)
        {
            Users x = context.Users.Where(ca => ca.UserId == user.UserId).First();
            x.UserId = user.UserId;
            x.UserName = user.UserName;
            x.Email = user.Email;
            x.Password = user.Password;
            context.Users.Update(x);
            context.SaveChanges();
        }

        public void DeleteUser(Users user)
        {
            Users x = context.Users.Where(a => a.UserId == user.UserId).First();
            FormRepository.DeleteFormByUser(x.UserId);
            context.Users.Remove(x);
            context.SaveChanges();
        }

        public UserCommon GetUserByID(string email/*,string password*/)
        {
            Users x = context.Users.Where(a => a.Email == email /*&& a.Password==password*/).FirstOrDefault();
            if(x!=null)
            return UserMap.MapUsersToUserCommon(x);
            return null;
        }

    }
}


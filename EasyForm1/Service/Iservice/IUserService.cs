using Common;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Iservice
{
    public interface IUserService
    {
        bool AddUser(UserCommon userCommon);
        void UpdateUser(UserCommon userCommon);
        UserCommon GetUserByID(string email/*,string password*/);
    }
}

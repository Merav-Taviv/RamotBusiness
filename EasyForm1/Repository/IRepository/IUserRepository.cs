using Common;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public interface IUserRepository
    {
        bool AddUser(Users user);
        void UpdateUser(Users user);
        void DeleteUser(Users user);
        CustomerCommon GetUserByID(string email/*,string password*/);
    }
}

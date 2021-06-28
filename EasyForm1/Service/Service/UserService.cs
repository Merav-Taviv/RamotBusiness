using Common;
using Repository;
using Repository.Models;
using Service.Iservice;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    class UserService : IUserService
    {
        private IUserRepository repository;
        public UserService(IUserRepository repository)
        {
            this.repository = repository;
        }
        public bool AddUser(UserCommon userCommon)
        {
            return repository.AddUser(UserMap.MapUserCommonToUsers(userCommon));
        }
        public void UpdateUser(UserCommon userCommon)
        {
            repository.UpdateUser(UserMap.MapUserCommonToUsers(userCommon));
        }
        
        public UserCommon GetUserByID(string email/*,string password*/)

        {
            return repository.GetUserByID(email/*,password*/);
        }

    }
}

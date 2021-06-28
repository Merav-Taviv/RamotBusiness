using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace Repository
{
    public static class UserMap
    {
        public static UserCommon MapUsersToUserCommon(Users user)
        {
            UserCommon userCommon = new UserCommon();
            if (user != null)
            {
                userCommon.UserID = user.UserId;
                userCommon.UserName = user.UserName;
                userCommon.Email = user.Email;
                userCommon.Password = user.Password;
            }
            return userCommon;
        }
        public static List<UserCommon> MapListUsersToUserCommon(List<Users> userList)
        {
            List<UserCommon> userCommonList = new List<UserCommon>();
            if (userList != null)
            {
                foreach (Users item in userList)
                {
                    userCommonList.Add(MapUsersToUserCommon(item));
                }
            }
            return userCommonList;
        }

        public static Users MapUserCommonToUsers(UserCommon userCommon)
        {
            Users user = new Users();
            if (userCommon != null)
            {
                userCommon.UserID = user.UserId;
                user.UserName = userCommon.UserName;
                user.Email = userCommon.Email;
                user.Password = userCommon.Password;
            }
            return user;
        }
        public static List<Users> MapListUserCommonToUsers(List<UserCommon> userCommonList)
        {
            List<Users> userList = new List<Users>();
            if (userCommonList != null)
            {
                foreach (UserCommon item in userCommonList)
                {
                    userList.Add(MapUserCommonToUsers(item));
                }
            }
            return userList;
        }
    }
}

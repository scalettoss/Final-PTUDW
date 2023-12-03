using Microsoft.Ajax.Utilities;
using MyClass.DAO;
using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BCrypt;
namespace MyStore.Services
{
    public class AuthenticationService
    {
        private MyDBContext db = new MyDBContext();
        UsersDAO usersDAO = new UsersDAO();
        public bool RegisterUser(string username, string password, string email, string img, string role)
        {

            var us = usersDAO.getList().Where(u => u.Username == username).ToList();
            if (us.Count > 0)
            {
                return false;
            }
            var newUser = new Users
            {
                Username = username,
                Password = password,
                Email = email,
                Img = img,
                Role = role,
                Status = 1,
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
            };
            usersDAO.Insert(newUser);
            return true;
        }

        //public bool UpdateAccount()
        //{
        //    usersDAO.Update(user);
        //    return true;
        //}
    }
}

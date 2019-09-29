using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Email_MVC.Controllers;
using Project_Email_MVC.Models;

namespace Project_Email_MVC.Service
{
    public class UserService
    {
        private MyDbContext dbContext;
        private HashPassword hashPassword;
        public UserService(MyDbContext context, HashPassword hashPassword)
        {
            this.dbContext = context;
            this.hashPassword = hashPassword;
        }
        public Users Register(string username, string password)
        {
            dbContext.Database.EnsureCreated();
            Users user = new Users(username, hashPassword.GetMd5Hash(password));
            dbContext.SaveChanges();
            return user;
        }
        public bool checkLogin(string username, string password)
        {
            var user = new Users(username, password);
            user = dbContext.Users.FirstOrDefault(x => x.Username == username);
            return hashPassword.VerifyMd5Hash(password, user.Password);
        }
        public Users GetUserById(int id)
        {
            return dbContext.Users.FirstOrDefault(x => x.Id == id);
        }
    }
}
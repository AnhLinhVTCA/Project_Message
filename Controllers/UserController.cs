using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Email_MVC.Models;
using Project_Email_MVC.Service;

namespace Project_Email_MVC.Controllers
{
    public class UserController : Controller
    {
        private MyDbContext dbContext;
        private HashPassword hashPassword;
        private UserService userService;
        public UserController(MyDbContext dbContext, HashPassword hashPassword,UserService userService)
        {
            this.dbContext = dbContext;
            this.hashPassword = hashPassword;
            this.userService = userService;
            dbContext.Database.EnsureCreated();
        }
        [HttpGet("/Login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost("/Login")]
        public IActionResult Login([FromForm]string username,[FromForm]string password)
        {
            // if(userService.checkLogin(username, password) == true)
            // {
            //     return Json("/received");
            // }
            return Json(username + "userService.checkLogin(username, password)");
        }
        
        [HttpPost]
        public IActionResult Register(string username, string password)
        {
            var user = new Users(username, password);
            user = dbContext.Users.FirstOrDefault(x => x.Username == username);
            if (user == null)
            {
                string hash;
                hash = hashPassword.GetMd5Hash(password);
                var user1 = new Users(username, password);
                user1.Username = username;
                user1.Password = hash;
                dbContext.Add(user1);
                dbContext.SaveChanges();
                return Redirect("/?Register=" + true);
            }
            else
            {
                return Redirect("/?Register=" + false);
            }
        }
        // public IActionResult Register()
        // {
        //     return View();
        // }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
    }
}
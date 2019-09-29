using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Project_Email_MVC.ActionFilter;
using Project_Email_MVC.Models;
using Project_Email_MVC.Service;

namespace Project_Email_MVC.Controllers
{
    // [Authentication]
    public class HomeController : Controller
    {
        private MyDbContext dbContext;
        private UserService userService;
        public HomeController(MyDbContext Context, UserService userService)
        {
            this.dbContext = Context;
            this.userService = userService;
        }
        public IActionResult Index(bool Error, bool Register)
        {
            // if (HttpContext.Session.GetInt32("Id") != null)
            // {
            //     return Redirect("/Received");
            // }
            // if (Error == true)
            // {
            //     ViewBag.Error = true;
            // }
            // if (Register == true)
            // {
            //     ViewBag.Register = true;
            // }
            // if (Register == false)
            // {
            //     ViewBag.Register = false;
            // }
            // return Redirect("/");
            string username = "Linh";
            string password = "1";
            Users user = userService.Register(username, password);
            return Json(user.Username);
        }
    }
}
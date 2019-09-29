using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Email_MVC.ActionFilter;
using Project_Email_MVC.Models;

namespace Project_Email_MVC.Controllers
{
    [Authentication]
    public class MessageController : Controller
    {
        private MyDbContext dbContext;
        public MessageController(MyDbContext context)
        {
            this.dbContext = context;
        }
        public IActionResult Compose()
        {
            int? Id = HttpContext.Session.GetInt32("Id");
            var users = dbContext.Users.FirstOrDefault(x => x.Id == Id);
            ViewBag.Username = users.Username;
            return View();
        }
        [HttpPost]
        public IActionResult SendMess(string receiver, string title, string content)
        {
            string[] receivers;
            if (receiver != null && title != null && content != null)
            {
                receivers = receiver.Split(',');
                Message mess = new Message();
                mess.Title = title;
                mess.Content = content;
                mess.SendTime = DateTime.Now;
                mess.SenderId = HttpContext.Session.GetInt32("Id");
                dbContext.Add(mess);
                dbContext.SaveChanges();
                Outbox ob = new Outbox();
                ob.MessageId = mess.Id;
                ob.IsDeleted = 0;
                ob.SenderId = HttpContext.Session.GetInt32("Id");
                dbContext.Add(ob);
                dbContext.SaveChanges();
                foreach (var item in receivers)
                {
                    if (item == "")
                    {
                        break;
                    }
                    Users user = dbContext.Users.FirstOrDefault(x => x.Username == item);
                    if (user == null)
                    {
                        return Redirect("/Message/Compose/?sendmail=" + false);
                    }
                    Inbox ib = new Inbox();
                    ib.MessageId = mess.Id;
                    ib.IsDeleted = 0;
                    ib.ReceiverId = user.Id;
                    dbContext.Add(ib);
                    dbContext.SaveChanges();
                }
            }
            return Redirect("/Message/Compose/?sendmail=" + true);
        }
        public IActionResult Sent()
        {
            int? Id = HttpContext.Session.GetInt32("Id");
            var user = dbContext.Users.FirstOrDefault(x => x.Id == Id);
            ViewBag.Username = user.Username;
            List<Outbox> ListOb = dbContext.Outbox.Where(ob => ob.SenderId == Id && ob.IsDeleted == 0).ToList();
            foreach (var item in ListOb)
            {
                item.Message = dbContext.Message.FirstOrDefault(x => x.Id == item.MessageId);
                List<Users> listUsers = dbContext.Users.FromSql(@"select u.id, group_concat(Username) as Username, u.password from Users u inner join Inbox ib on ib.receiverid = u.id where ib.messageid = " + item.Id + " and IsDeleted = 0;").ToList();
                // item.Inbox = dbContext.Inbox.Where(x => x.MessageId == item.Id).ToList();
                foreach (var item1 in listUsers)
                {
                    item.Message.User = dbContext.Users.FirstOrDefault(x => x.Id == item1.Id);
                }
            }
            ViewBag.ListMessSend = ListOb;
            return View();
        }
        public IActionResult Received()
        {
            int? Id = HttpContext.Session.GetInt32("Id");
            var user = dbContext.Users.FirstOrDefault(x => x.Id == Id);
            ViewBag.Username = user.Username;
            List<Inbox> ListIb = dbContext.Inbox.Where(ib => ib.ReceiverId == Id && ib.IsDeleted == 0).ToList();
            foreach (var item in ListIb)
            {
                item.Message = dbContext.Message.FirstOrDefault(x => x.Id == item.MessageId);
                item.Message.User = dbContext.Users.FirstOrDefault(x => x.Id == item.Message.SenderId);
            }
            ViewBag.ListMessReceive = ListIb;
            return View();
        }
        public IActionResult RemoveByReceived(int MessageId)
        {
            Inbox Ib = dbContext.Inbox.FirstOrDefault(x => x.MessageId == MessageId);
            Ib.IsDeleted = 1;
            dbContext.SaveChanges();
            return Redirect("/Message/Received");
        }
        public IActionResult RemoveBySender(int MessageId)
        {
            Outbox Ob = dbContext.Outbox.FirstOrDefault(x => x.MessageId == MessageId);
            Ob.IsDeleted = 1;
            dbContext.SaveChanges();
            return Redirect("/Message/Sent");
        }
        public IActionResult RecycleBin()
        {
            return View();
        }
    }
}
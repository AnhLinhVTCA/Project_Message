using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Project_Email_MVC.Controllers;

namespace Project_Email_MVC.ActionFilter
{
    public class Authentication : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.Session.GetInt32("Id");
            if (userId == null)
            {
                var controller = (Controller)context.Controller;
                context.Result = controller.Redirect("/Login");
            }
        }
    }
}
using Codesanook.Examples.AspNetSignalR.Hubs;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Codesanook.Examples.AspNetSignalR.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            //https://stackoverflow.com/a/11098832/1872200
            //https://docs.microsoft.com/en-us/aspnet/signalr/overview/guide-to-the-api/hubs-api-guide-server
            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<CoffeeHub>();
            //hubContext.Clients.All.man("");
            return View();
        }
    }
}

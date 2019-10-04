using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Codesanook.Examples.AspNetSignalR.Hubs
{
    public class VotingHubController : Hub 
    {
        public static Dictionary<string, int> poll = new Dictionary<string, int>()
        {
            { "Samsung", 20 },
            { "Apple", 18 },
            { "Huawei", 16 },
            { "Oppo", 14 },
            { "Vivi", 12 },
            { "Xiaomi", 10 },
            { "LG", 8 },
            { "Lenovo", 8 }
        };

        // GET: VotingHub
        public ActionResult Index()
        {
            return View();
        }
    }
}
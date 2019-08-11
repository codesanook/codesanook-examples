using Codesanook.Examples.AspNetSignalR.Hubs;
using Codesanook.Examples.AspNetSignalR.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Codesanook.Examples.AspNetSignalR.Controllers
{
    public class CoffeeController : ApiController
    {
        private static int OrderId;

        public CoffeeController()
        {

        }

        public int OrderCoffee(Order order)
        {
            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<CoffeeHub>();
            //hubContext.Clients.All.NewOrder(order);
            OrderId++;
            return OrderId;
        }
    }
}
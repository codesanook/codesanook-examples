using Codesanook.Examples.AspNetSignalR.Helpers;
using Codesanook.Examples.AspNetSignalR.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Codesanook.Examples.AspNetSignalR.Hubs
{
    public class CoffeeHub:Hub<ICoffeeClient>
    {

        private static readonly OrderChecker orderChecker = new OrderChecker(new Random());
        const string GroupName = "allUpdateReceivers";

        public async Task GetUpdateForOrder(Order order)
        {
            await Clients.Others.NewOrder(order);
            UpdateInfo result;
            do
            {
                result = orderChecker.GetUpdate(order);
                await Task.Delay(500);
                if (!result.New) continue;

                await Clients.Caller.ReceiveOrderUpdate(result);
                await Clients.Group(GroupName).ReceiveOrderUpdate(result);

            } while (!result.Finished);
            await Clients.Caller.Finished(order);
        }

        public override Task OnConnected()
        {
            if(Context.QueryString["group"] == "allUpdates")
            {
                Groups.Add(Context.ConnectionId, GroupName);
            }
            return base.OnConnected();
        }

    }
}
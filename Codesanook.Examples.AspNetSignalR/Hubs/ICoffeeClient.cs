using Codesanook.Examples.AspNetSignalR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Codesanook.Examples.AspNetSignalR.Hubs
{
    public interface ICoffeeClient
    {
        Task NewOrder(Order order);
        Task ReceiveOrderUpdate(UpdateInfo info);
        Task Finished(Order order);
    }
}
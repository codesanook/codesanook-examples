using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Codesanook.Examples.AspNetMvc.Hubs
{
    [HubName("pushMessageHub")]
    public class PushMessageHub : Hub<IPushMessageOperation>
    {
        [HubMethodName("sendMessage")]
        public void SendMessage(string message) => 
            Clients.All.AddNewMessage(message);
    }
}

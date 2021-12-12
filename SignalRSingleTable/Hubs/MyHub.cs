using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRSingleTable.Hubs
{
    public class MyHub : Hub
    {
        [HubMethodName("sendMessages")]
        public static void SendMessages()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
            context.Clients.All.updateMessages();
        }


        public static void SendNotifications()
        {
            // from here we will send notification message to clients
            IHubContext notificationHub = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
            notificationHub.Clients.All.notify("added");
        }

    }
}
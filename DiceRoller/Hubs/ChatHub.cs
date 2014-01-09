using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace DiceRoller.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string name, string msg)
        {
            Clients.All.broadcastMessage(name, msg);
        }
    }
}
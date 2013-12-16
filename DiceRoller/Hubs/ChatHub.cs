using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace DiceRoller.Hubs
{
    public class ChatHub : Hub
    {
        private static ChatHub _instance;
        public static ChatHub Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                _instance = new ChatHub();
            }

            return _instance;
        }
        IHubContext _context = GlobalHost.ConnectionManager.GetHubContext<DiceHub>();

        private Hub hub { get; set; }

        public void Send(string name, string msg)
        {
            _context.Clients.All.broadcastMessage(name, msg);
        }
    }
}
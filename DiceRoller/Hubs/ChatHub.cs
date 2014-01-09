using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiceRoller.Annotations;
using Microsoft.AspNet.SignalR;

namespace DiceRoller.Hubs
{
    [UsedImplicitly]
    public class ChatHub : Hub
    {
        private readonly static Lazy<ChatHub> _instance = new Lazy<ChatHub>(
            () => new ChatHub(GlobalHost.ConnectionManager.GetHubContext<ChatHub>()));

        private IHubContext _context;
        public static ChatHub Instance { get { return _instance.Value; } }
        private ChatHub(IHubContext context)
        {
            _context = context;
        }
        public ChatHub()
        {
        }

        public void Send(string name, string msg)
        {
            Clients.All.broadcastMessage(name, msg);
        }
    }
}
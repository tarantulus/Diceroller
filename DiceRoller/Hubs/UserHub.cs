using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace DiceRoller.Hubs
{
    public class UserHub
    {
        IHubContext _context = GlobalHost.ConnectionManager.GetHubContext<DiceHub>();
        UserCollection users = UserCollection.Instance();

        public string ConnectionId { get; set; }

        public void Login()
        {
            if (!users.Where(user => user.ClientId == ConnectionId).Any())
            {
                User thisUser = new User();
                thisUser.ClientId = ConnectionId;
                users.Add(thisUser);
                User currentUser = thisUser;
            }
        }

        public void Logout()
        {
            if (users.Where(user => user.ClientId == ConnectionId).Any())
            {
                users.RemoveAll(user => user.ClientId == ConnectionId);
            }
        }

        public User CurrentUser
        {
            get
            {
                return users.Where(user => user.ClientId == ConnectionId).First();
            }
        }

        public void UpdateUsers()
        {
            _context.Clients.All.UpdateUsers(users);
        }
    }
}
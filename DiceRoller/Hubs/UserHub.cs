using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace DiceRoller.Hubs
{
    public class UserHub : Hub
    {
        
        UserCollection users = UserCollection.Instance();

        public override Task OnConnected()
        {
            Login();
            UpdateUsers();
            return base.OnDisconnected();
        }
        

        public override Task OnDisconnected()
        {
            Logout();
            UpdateUsers();
            return base.OnDisconnected();
        }
        


        public void Login()
        {
            if (users.All(user => user.ClientId != Context.ConnectionId))
            {
                User thisUser = new User();
                thisUser.ClientId = Context.ConnectionId;
                users.Add(thisUser);                
            }
        }

        public void Logout()
        {
            if (users.Any(user => user.ClientId == Context.ConnectionId))
            {
                users.RemoveAll(user => user.ClientId == Context.ConnectionId);
            }
        }

        public User CurrentUser
        {
            get { return users.FirstOrDefault(user => user.ClientId == Context.ConnectionId); }
        }

        public void UpdateUsers()
        {
            Clients.All.UpdateUsers(users);
        }

        public void SetName(string name)
        {
            CurrentUser.Name = name;
            UpdateUsers();
        }
    }
}
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
        public static UserHub Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                _instance = new UserHub();
            }

            return _instance;
        }
        private static UserHub _instance;        

        public void Login(string connectionId)
        {
            if (!users.Where(user => user.ClientId == connectionId).Any())
            {
                User thisUser = new User();
                thisUser.ClientId = connectionId;
                users.Add(thisUser);
            }
        }

        public void Logout(string connectionId)
        {
            if (users.Where(user => user.ClientId == connectionId).Any())
            {
                users.RemoveAll(user => user.ClientId == connectionId);
            }
        }

        public User CurrentUser (string connectionId)
        { 
            return users.Where(user => user.ClientId == connectionId).FirstOrDefault();
        }

        public void UpdateUsers()
        {
            _context.Clients.All.UpdateUsers(users);
        }

        public void SetName(string name, string connectionId)
        {
            CurrentUser(connectionId).Name = name;
        }
    }
}
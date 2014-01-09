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

        private readonly static Lazy<UserHub> _instance = new Lazy<UserHub>(
            () => new UserHub(GlobalHost.ConnectionManager.GetHubContext<DiceHub>()));

        private IHubContext _context;
        public static UserHub Instance { get { return _instance.Value; } }
        private UserHub(IHubContext context)
        {
            _context = context;
        }
        public UserHub()
        {
        }

        public Task OnConnected(string connectionId)
        {
            Login(connectionId);
            UpdateUsers();
            return base.OnDisconnected();
        }


        public Task OnDisconnected(string connectionId)
        {
            Logout(connectionId);
            UpdateUsers();
            return base.OnDisconnected();
        }



        public void Login(string connectionId)
        {
            if (users.All(user => user.ClientId != connectionId))
            {
                User thisUser = new User();
                thisUser.ClientId = connectionId;
                users.Add(thisUser);
                UpdateUsers();
            }
        }

        public void Logout(string connectionId)
        {
            if (users.Any(user => user.ClientId == connectionId))
            {
                users.RemoveAll(user => user.ClientId == connectionId);
            }
            UpdateUsers();
        }

        public User CurrentUser
        {
            get { return users.FirstOrDefault(user => user.ClientId == Context.ConnectionId); }
        }

        public void UpdateUsers()
        {
            _context.Clients.All.UpdateUsers(users);
        }

        public void SetName(string name)
        {
            CurrentUser.Name = name;
            UpdateUsers();
        }
    }
}
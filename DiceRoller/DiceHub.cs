using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Data;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace DiceRoller
{
    
    public class DiceHub : Hub
    {
        UserCollection users = UserCollection.Instance();
        Log _log = Log.Instance();
        Helpers.HTMLhelper _htmlHelper = new Helpers.HTMLhelper();
        
        public override Task OnConnected()
        {
            if (!users.Where(user => user.ClientId == Context.ConnectionId).Any())
            {
                User thisUser = new User();
                thisUser.ClientId = Context.ConnectionId;
                users.Add(thisUser);
                User currentUser = thisUser;
            }
            Clients.Caller.clearImg();
            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            if (users.Where(user => user.ClientId == Context.ConnectionId).Any())
            {
                users.RemoveAll(user => user.ClientId == Context.ConnectionId);
            }
            Clients.All.UpdateUsers(users);
            return base.OnConnected();
        }

        public void SetName(string userName)
        {
            users.First(user => user.ClientId == Context.ConnectionId).Name=userName;
            Clients.All.UpdateUsers(users);
        }

        public void GetLog()
        {
            foreach (KeyValuePair<string, object> entry in _log)
            {
                string name = entry.Key;
                object msg = entry.Value;                
                Clients.Caller.broadcastMessage(name, msg);
            }
            SendCanvas(_log.LastImg);
        }

        public void Send(string name, string msg, string die, int rolls)
        {
            // Call the broadcastMessage method to update clients.

            if (!string.IsNullOrEmpty(die))
            {
                SendDie(name, msg, die, rolls);                
            }
            else
            {
                _log.Add(new KeyValuePair<string,object>(name,msg));
                Clients.All.broadcastMessage(name, msg);
            }
        }

        public void Send(string name, string msg)
        {
            // Call the broadcastMessage method to update clients.


                _log.Add(new KeyValuePair<string, object>(name, msg));
                Clients.All.broadcastMessage(name, msg);
        }

        public void SendDie(string name, string msg, string die, int numRolls)
        {
            Dice roller = new Dice(); 
            // Call the broadcastMessage method to update clients.
            int runningTotal = 0;
            for (int i = 0; i < numRolls; i++)
            {
                    var parsedRolls = roller.Parse(die);
                    runningTotal = runningTotal + parsedRolls[0].Sum(); 
                    string counter = (numRolls - i).ToString();
                    Clients.All.broadcastMessage(counter,parsedRolls);                    
                    _log.Add(new KeyValuePair<string, object>(counter,parsedRolls));
                
            }
            int avg = runningTotal / numRolls;
            Clients.All.broadcastMessage("Avg", avg);
            Clients.All.broadcastMessage("Sum", runningTotal);
            Clients.All.broadcastMessage(name, msg);
            _log.Add(new KeyValuePair<string, object>(name, msg));
            
        }

        public void SendCanvas(string img)
        {
            _log.LastImg = img;
            Clients.All.broadcastImg(img);
        }

        public void ClearImg()
        {
           Clients.All.clearImg();
        }

        public void userIsDrawing()
        {
            User currentUser = users.Where(user => user.ClientId == Context.ConnectionId).First();
            Clients.All.userIsDrawing(currentUser.Name);
        }
        public void userStoppedDrawing()
        {
            User currentUser = users.Where(user => user.ClientId == Context.ConnectionId).First();
            Clients.All.userStoppedDrawing(currentUser.Name);
        }

        public void setInit(int mod)
        {
            User currentUser = users.Where(user => user.ClientId == Context.ConnectionId).First();
            Dice roller = new Dice();
            string initRoll = "1d20+" + mod;
            var result = roller.Parse(initRoll);
            currentUser.Init = result[0][0] + result[0][1];
            Clients.All.UpdateUsers(users);
            Clients.All.broadcastMessage(currentUser.Name, "init - " + currentUser.Init);
            _log.Add(new KeyValuePair<string, object>(currentUser.Name, "init - "+currentUser.Init));
            
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiceRoller.Classes;
using Microsoft.AspNet.SignalR;
using System.Data;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.SessionState;
using DiceRoller.Hubs;
namespace DiceRoller
{
    
    public class DiceHub : Hub
    {
        #region setup

        private Log _log = Log.Instance();
        Helpers.HTMLhelper _htmlHelper = new Helpers.HTMLhelper();
        static HttpSessionState session = HttpContext.Current.Session;
        private ChatHub chatHub = ChatHub.Instance();
        private CanvasHub canvasHub = CanvasHub.Instance();        
        private UserHub userHub = UserHub.Instance();
        #endregion

        #region ConnectionActions
        public override Task OnConnected()
        {
            userHub.Login(Context.ConnectionId);
            Clients.Caller.clearImg();
            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            userHub.Logout(Context.ConnectionId);
            userHub.UpdateUsers();
            return base.OnDisconnected();
        }
        
        
        #endregion


        #region prepActions

        public void SetName(string userName)
        {
            userHub.SetName(userName, Context.ConnectionId);            
            userHub.UpdateUsers();
        }

        public string GetName()
        {
            return Clients.Caller.userName;
        }

        public void GetLog()
        {
            foreach (KeyValuePair<string, object> entry in _log)
            {
                string name = entry.Key;
                var msg = entry.Value;
                if (entry.Value is String)
                {
                    Clients.Caller.broadcastMessage(name, msg);
                }
                else 
                {
                    Clients.Caller.broadcastDice(name, msg);
                }
            }
            SendCanvas(_log.LastImg);
        }

        #endregion 

        #region userMethods

        public void CreateRoom(string name, string password)
        {
            string roomId = Guid.NewGuid().ToString();
            Classes.Room room = new Classes.Room(roomId,name,password);
            DataStore store = new DataLayer.JsonStore("rooms");
            Groups.Add(Context.ConnectionId, roomId.ToString());
            if (!store.Exists()){
            store.Create();            
            }
            store.Update<Room>(room);
        }

        public void JoinRoom(string id)
        {
            DataStore store = new DataLayer.JsonStore("rooms");
            store.GetData<List<Room>>();
        }

        #endregion

        #region Send/Set Methods

        public void Send(string name, string msg, string die, int rolls)
        {
                SendDie(name, msg, die, rolls);            
        }
        public void Send(string name, string msg)
        {
            chatHub.Send(name, msg);
            _log.Add(new KeyValuePair<string, object>(name, msg));
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
                    Clients.All.broadcastDice(counter,parsedRolls);                    
                    _log.Add(new KeyValuePair<string, object>(counter,parsedRolls.ToArray()));
                
            }
            int avg = runningTotal / numRolls;
            Clients.All.broadcastMessage("Avg", avg);
            Clients.All.broadcastMessage("Sum", runningTotal);
            Clients.All.broadcastMessage(name, msg);
            _log.Add(new KeyValuePair<string, object>(name, msg));
            
        }

        public void userIsDrawing()
        {
            canvasHub.userIsDrawing(Clients.Caller.userName);
        }
        public void userStoppedDrawing()
        {
            canvasHub.userStoppedDrawing(Clients.Caller.userName);
        }

        public void SendCanvas(object img)
        {
            _log.LastImg = img;
            canvasHub.SendCanvas(img,Context.ConnectionId);
        }

        public void ClearImg()
        {
            canvasHub.ClearImg();
        }

        public void setInit(int mod)
        {
            Dice roller = new Dice();
            string initRoll = "1d20+" + mod;
            var result = roller.Parse(initRoll);
            userHub.CurrentUser(Context.ConnectionId).Init = result[0][0] + result[0][1];
            userHub.UpdateUsers();
            Clients.All.broadcastMessage(Clients.Caller.userName, "init - " + userHub.CurrentUser(Context.ConnectionId).Init);
            _log.Add(new KeyValuePair<string, object>(Clients.Caller.userName, "init - " + userHub.CurrentUser(Context.ConnectionId).Init));

        }

        #endregion
    }
}
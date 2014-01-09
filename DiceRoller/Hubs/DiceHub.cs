using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiceRoller.Annotations;
using Microsoft.AspNet.SignalR;
using System.Data;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.SessionState;
using DiceRoller.Hubs;
namespace DiceRoller
{
    [UsedImplicitly]
    public class DiceHub : Hub
    {
        #region setup

        private Log _log = Log.Instance();
        Helpers.HTMLhelper _htmlHelper = new Helpers.HTMLhelper();
        static HttpSessionState session = HttpContext.Current.Session;
        private ChatHub _chatHub = ChatHub.Instance;
        private CanvasHub _canvasHub = CanvasHub.Instance;
        private UserHub _userHub = UserHub.Instance; 
        public DiceHub()
        {
        }
        #endregion

        #region ConnectionActions
        public override Task OnConnected()
        {   Clients.Caller.clearImg();
            _userHub.Login(Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            Clients.Caller.clearImg();
            _userHub.Logout(Context.ConnectionId);
            return base.OnConnected();
        }


        
        #endregion


        #region prepActions



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
            store.Update(room);
        }

        public void JoinRoom(string id)
        {
            DataStore store = new DataLayer.JsonStore("rooms");
            store.GetData(typeof(Classes.RoomCollection));
        }

        #endregion

        #region Send/Set Methods

        public void Send(string name, string msg, string die, int rolls)
        {
                SendDie(name, msg, die, rolls);            
        }
        public void Send(string name, string msg)
        {
            _chatHub.Send(name, msg);
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
            _canvasHub.userIsDrawing(Clients.Caller.userName);
        }
        public void userStoppedDrawing()
        {
            _canvasHub.userStoppedDrawing(Clients.Caller.userName);
        }

        public void SendCanvas(object img)
        {
            _log.LastImg = img;
            _canvasHub.SendCanvas(img);
        }

        public void ClearImg()
        {
            _canvasHub.ClearImg();
        }

        public void setInit(int mod)
        {
            Dice roller = new Dice();
            string initRoll = "1d20+" + mod;
            var result = roller.Parse(initRoll);
            _userHub.CurrentUser.Init = result[0][0] + result[0][1];
            _userHub.UpdateUsers();
            Clients.All.broadcastMessage(Clients.Caller.userName, "init - " + _userHub.CurrentUser.Init);
            _log.Add(new KeyValuePair<string, object>(Clients.Caller.userName, "init - " + _userHub.CurrentUser.Init));

        }

        #endregion
    }
}
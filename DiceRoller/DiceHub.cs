﻿using System;
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
            }
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
                    _log.Insert(0,new KeyValuePair<string, object>(counter,parsedRolls));
                
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
            _log.LastImg = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAyAAAAJYCAYAAACadoJwAAAgAElEQVR4Xu3cPYumdxmH4QRBFC0S0F6xEEUxWPuCnZWFleAnsRERxU8iWGlrJ2ovEUWxEO1FkkIRxLd7YBd8QZbd7Px/z8x5BB4iZmeu+zquuzmZSV59xV8ECBAgQIAAAQIECBA4JPDqoTnGECBAgAABAgQIECBA4BUB4iUgQIAAAQIECBAgQOCYgAA5Rm0QAQIECBAgQIAAAQICxDtAgAABAgQIECBAgMAxAQFyjNogAgQIECBAgAABAgQEiHeAAAECBAgQIECAAIFjAgLkGLVBBAgQIECAAAECBAgIEO8AAQIECBAgQIAAAQLHBATIMWqDCBAgQIAAAQIECBAQIN4BAgQIECBAgAABAgSOCQiQY9QGESBAgAABAgQIECAgQLwDBAgQIECAAAECBAgcExAgx6gNIkCAAAECBAgQIEBAgHgHCBAgQIAAAQIECBA4JiBAjlEbRIAAAQIECBAgQICAAPEOECBAgAABAgQIECBwTECAHKM2iAABAgQIECBAgAABAeIdIECAAAECBAgQIEDgmIAAOUZtEAECBAgQIECAAAECAsQ7QIAAAQIECBAgQIDAMQEBcozaIAIECBAgQIAAAQIEBIh3gAABAgQIECBAgACBYwIC5Bi1QQQIECBAgAABAgQICBDvAAECBAgQIECAAAECxwQEyDFqgwgQIECAAAECBAgQECDeAQIECBAgQIAAAQIEjgkIkGPUBhEgQIAAAQIECBAgIEC8AwQIECBAgAABAgQIHBMQIMeoDSJAgAABAgQIECBAQIB4BwgQIECAAAECBAgQOCYgQI5RG0SAAAECBAgQIECAgADxDhAgQIAAAQIECBAgcExAgByjNogAAQIECBAgQIAAAQHiHSBAgAABAgQIECBA4JiAADlGbRABAgQIECBAgAABAgLEO0CAAAECBAgQIECAwDEBAXKM2iACBAgQIECAAAECBASId4AAAQIECBAgQIAAgWMCAuQYtUEECBAgQIAAAQIECAgQ7wABAgQIECBAgAABAscEBMgxaoMIECBAgAABAgQIEBAg3gECBAgQIECAAAECBI4JCJBj1AYRIECAAAECBAgQICBAvAMECBAgQIAAAQIECBwTECDHqA0iQIAAAQIECBAgQECAeAcIECBAgAABAgQIEDgmIECOURtEgAABAgQIECBAgIAA8Q4QIECAAAECBAgQIHBMQIAcozaIAAECBAgQIECAAAEB4h0gQIAAAQIECBAgQOCYgAA5Rm0QAQIECBAgQIAAAQICxDtAgAABAgQIECBAgMAxAQFyjNogAgQIECBAgAABAgQEiHeAAAECBAgQIECAAIFjAgLkGLVBBAgQIECAAAECBAgIEO8AAQIECBAgQIAAAQLHBATIMWqDCBAgQIAAAQIECBAQIN4BAgQIECBAgAABAgSOCQiQY9QGESBAgAABAgQIECAgQLwDBAgQIECAAAECBAgcExAgx6gNIkCAAAECBAgQIEBAgHgHCBAgQIAAAQIECBA4JiBAjlEbRIAAAQIECBAgQICAAPEOECBAgAABAgQIECBwTECAHKM2iAABAgQIECBAgAABAeIdIECAAAECBAgQIEDgmIAAOUZtEAECBAgQIECAAAECAsQ7QIAAAQIECBAgQIDAMQEBcozaIAIECBAgQIAAAQIEBIh3gAABAgQIECBAgACBYwIC5Bi1QQQIECBAgAABAgQICBDvAAECBAgQIECAAAECxwQEyDFqgwgQIECAAAECBAgQECDeAQIECBAgQIAAAQIEjgkIkGPUBhEgQIAAAQIECBAgIEC8AwQIECBAgAABAgQIHBMQIMeoDSJAgAABAgQIECBAQIB4BwgQIECAAAECBAgQOCYgQI5RG0SAAAECBAgQIECAgADxDhAgQIAAAQIECBAgcExAgByjNogAAQIECBAgQIAAAQHiHSBAgAABAgQIECBA4JiAADlGbRABAgQIECBAgAABAgLEO0CAAAECBAgQIECAwDEBAXKM2iACBAgQIECAAAECBASId4AAAQIECBAgQIAAgWMCAuQYtUEECBAgQIAAAQIECAgQ7wABAgQIECBAgAABAscEBMgxaoMIECBAgAABAgQIEBAg3gECBAgQIECAAAECBI4JCJBj1AYRIECAAAECBAgQICBAvAMECBAgQIAAAQIECBwTECDHqA0iQIAAAQIECBAgQECAeAcIECBAgAABAgQIEDgmIECOURtEgAABAgQIECBAgIAA8Q78t8Bfrv/jvVgIECBAgAABAgQI3IeAALkP1Yf7Pe/i4z3X58/X5/0Pdw1PToAAAQIECBAgcKsCAuRWL7N7rj9do98nQnYHMJkAAQIECBAg8JgF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";
            Clients.All.clearImg();
        }
    }
}
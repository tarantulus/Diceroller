using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Data;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;


namespace DiceRoller
{
    
    public class DiceHub : Hub
    {
        Log _log = Log.Instance();
        Helpers.HTMLhelper _htmlHelper = new Helpers.HTMLhelper();

        public void GetLog()
        {
            foreach (KeyValuePair<string, string> entry in _log)
            {
                string name = entry.Key;
                string msg = entry.Value;
                Clients.Caller.broadcastMessage(name, msg);
            }
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
                _log.Add(new KeyValuePair<string,string>(name,msg));
                Clients.All.broadcastMessage(name, msg);
            }
        }

        public void SendDie(string name, string msg, string die, int numRolls)
        {
            Dice roller = new Dice(); 
            Clients.All.broadcastMessage(name, msg);
            _log.Add(new KeyValuePair<string,string>(name,msg));
            // Call the broadcastMessage method to update clients.
            for (int i = 0; i < numRolls; i++)
            {
                    var parsedRolls = roller.Parse(die);
                    string html =_htmlHelper.Roll2HTML(parsedRolls);
                    Clients.All.broadcastMessage(i + 1,html);
                    string counter = (i+1).ToString();
                    _log.Add(new KeyValuePair<string, string>(counter,html));
                
            }
            
        }
    }
}
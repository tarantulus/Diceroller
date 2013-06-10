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
        Log _log;
        public void LoadLog()
        {
            if (_log != null)
            {
                foreach (KeyValuePair<string, string> entry in _log)
                {
                    Clients.Caller.broadcastMessage(entry.Key, entry.Value);
                }
            }
        }

        Helpers.HTMLhelper _htmlHelper = new Helpers.HTMLhelper();
        public void Send(string name, string msg, string die, int rolls)
        {
            // Call the broadcastMessage method to update clients.

            if (!string.IsNullOrEmpty(die))
            {
                SendDie(name, msg, die, rolls);
            }
            else
            {
                Clients.All.broadcastMessage(name, msg);
            }

            SaveToLog(name, msg);
        }

        public void SendDie(string name, string msg, string die, int numRolls)
        {
            Dice roller = new Dice(); 
            Clients.All.broadcastMessage(name, msg);
            // Call the broadcastMessage method to update clients.
            for (int i = 0; i < numRolls; i++)
            {
                    var parsedRolls = roller.Parse(die);
                    string html =_htmlHelper.Roll2HTML(parsedRolls);
                    Clients.All.broadcastMessage(i + 1,html);
                
            }
            
        }

        public void SaveToLog(string name, string msg)
        {
            _log.Add(new KeyValuePair<string, string> (name, msg) );
        }
    }
}
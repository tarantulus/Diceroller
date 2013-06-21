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
                _log.Add(new KeyValuePair<string,string>(name,msg));
                Clients.All.broadcastMessage(name, msg);
            }
        }

        public void SendDie(string name, string msg, string die, int numRolls)
        {
            Dice roller = new Dice(); 
            // Call the broadcastMessage method to update clients.
            for (int i = 0; i < numRolls; i++)
            {
                    var parsedRolls = roller.Parse(die);

                //ewwww! this whole block needs to be fixed! make it JSON!
                    string html =_htmlHelper.Roll2HTML(parsedRolls);
                    html = die + "<br/>" +html + "<br/><br/>";
                    string counter = (numRolls - i).ToString();
                    Clients.All.broadcastMessage(counter,html);                    
                    _log.Insert(0,new KeyValuePair<string, string>(counter,html));
                
            }
            Clients.All.broadcastMessage(name, msg);
            _log.Add(new KeyValuePair<string, string>(name, msg));
            
        }

        public void SendCanvas(string img)
        {
            _log.LastImg = img;
            Clients.All.broadcastImg(img);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Web.Script.Serialization;


namespace DiceRoller
{
    public class DiceHub : Hub
    {

        Random _rand = new Random();

        public void Send(string name, string msg, int die = 0 , int rolls = 0)
        {
            // Call the broadcastMessage method to update clients.
            if (die>0)
            {
                Roll(name, msg, die, rolls);
            }
            else
            {
                Clients.All.broadcastMessage(name, msg);
            }
        }

        public void Roll(string name, string msg, int die, int numRolls)
        {
            Clients.All.broadcastMessage(name, msg);
            List<int> rolls = new List<int>();
            // Call the broadcastMessage method to update clients.
            for (int i = 0; i < numRolls; i++)
            {
                Clients.All.broadcastMessage(i + 1,_rand.Next(1, die));
            }
            
        }
    }
}
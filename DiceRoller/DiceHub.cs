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

        Random _rand = new Random();

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
        }

        public void SendDie(string name, string msg, string die, int numRolls)
        {
            Regex rgx = new Regex(@"((?<=\+)|(?=\+))");
            string[] splitDie = rgx.Split(die, 2).Where(s => s != String.Empty).ToArray(); ;
            string[] toRoll = splitDie[0].Split('d');
            Clients.All.broadcastMessage(name, msg);
            List<int> parsedRolls = new List<int>();
            // Call the broadcastMessage method to update clients.
            for (int i = 0; i < numRolls; i++)
            {
                for (int roll = 0; roll < numRolls; roll++)
                {
                    if (splitDie[1] != null)
                    {
                        parsedRolls = Roll(int.Parse(toRoll[1]), int.Parse(toRoll[0]), splitDie[1]);
                    }
                    else
                    {
                        parsedRolls = Roll(int.Parse(toRoll[1]), int.Parse(toRoll[0]));
                    }
                    Clients.All.broadcastMessage(roll + 1,parsedRolls[roll]);
                }
            }
            
        }

        public List<int> Roll(int die, int numRolls, string mod = "")
        {
            List<int> parsedRolls = new List<int>();
            for (int i = 0; i < numRolls; i++)
            {
                parsedRolls.Add(Evaluate(_rand.Next(1,die)+mod));
            }
            return parsedRolls;
        }

        public int Evaluate(string expression)
        {
            DataTable table = new DataTable();
            table.Columns.Add("expression", typeof(string), expression);
            DataRow row = table.NewRow();
            table.Rows.Add(row);
            return int.Parse((string)row["expression"]);
        }
    }
}
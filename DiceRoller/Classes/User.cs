using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace DiceRoller
{
    public class User
    {
        public User()
        {
        }

        public User(string name, string password)
        {
            this.Name = name;
            this.SaltedHash = new SaltedHash(password);
        }
        public string ClientId { get; set; }
        public string Name { get; set; }
        public int Init { get; set; }
        public SaltedHash SaltedHash { get; set; }

    }
}
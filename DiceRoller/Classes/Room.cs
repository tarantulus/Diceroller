using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiceRoller.Classes
{
    public class Room
    {
        private string _password;
        private string _salt;
        public string Id { get; set; }
        public string name { get; set; }
        public string salt { 
            get
            { 
                return _salt;
            }
        }
        public string hash
        {
            get
            {
                return _password;
            }
            set
            {
                SaltedHash hashValue = new SaltedHash(value);
                _password = hashValue.Hash;
                _salt = hashValue.Salt;
            }
        }

        public Room(string id, string name) 
        {
            this.Id = id;
            this.name = name;
        }
        public Room(string id, string name, string password)
        {
            this.hash = password;
            this.Id = id;
            this.name = name;
        }
    
    }
}
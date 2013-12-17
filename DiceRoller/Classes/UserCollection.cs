using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiceRoller
{
    public class UserCollection : List<User>
    {
        private static UserCollection _instance;

        protected UserCollection()
        {
        }

        public static UserCollection Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                _instance = new UserCollection();
            }

            return _instance;
        }        
    }
}
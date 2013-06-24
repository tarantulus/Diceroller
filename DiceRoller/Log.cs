using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiceRoller
{
    public class Log : List<KeyValuePair<string, object>>
    {
        private static Log _instance;

        protected Log()
        {
        }

        public static Log Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                _instance = new Log();
            }

            return _instance;
        }

        public string LastImg { get; set; }
    }
}
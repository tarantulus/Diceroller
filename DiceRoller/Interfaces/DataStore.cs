using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceRoller
{
    interface DataStore
    {
        void Create();
        bool Exists();
        Object GetData<T>();
        void Update<T>(Object data);
        bool Delete();
    }
}

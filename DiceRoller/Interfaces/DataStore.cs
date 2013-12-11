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
        Object GetData(Type type);
        void Update(Object data);
        bool Delete();
    }
}

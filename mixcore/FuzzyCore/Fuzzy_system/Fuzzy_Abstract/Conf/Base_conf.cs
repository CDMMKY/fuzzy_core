using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuzzy_system.Fuzzy_Abstract.conf
{
    public interface Base_conf
    {
        void Init(int countVars);
        void loadParams(string param);
    }
}

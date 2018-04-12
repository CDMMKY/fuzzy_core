using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fuzzy_system.Approx_Singletone.add_generators.conf;
using Fuzzy_system.Approx_Singletone;
using Fuzzy_system.Fuzzy_Abstract.add_generators.conf;

namespace Fuzzy_system.Approx_Singletone.add_generators
{
    public abstract class Abstract_generator
    {
        abstract public a_Fuzzy_System Generate(a_Fuzzy_System Approximate, Abstract_generator_conf config);
        abstract public string ToString(bool with_param=false);
       


    }
}

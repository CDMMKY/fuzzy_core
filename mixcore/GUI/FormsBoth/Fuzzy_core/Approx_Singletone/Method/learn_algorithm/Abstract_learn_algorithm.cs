using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fuzzy_system.Approx_Singletone;
using Fuzzy_system.Approx_Singletone.learn_algorithm.conf;
using Fuzzy_system.Fuzzy_Abstract.learn_algorithm.conf;

namespace Fuzzy_system.Approx_Singletone.learn_algorithm
{
    public  abstract class Abstract_learn_algorithm
    {
       public abstract a_Fuzzy_System TuneUpFuzzySystem(a_Fuzzy_System Approximate, Abstract_learn_algorithm_conf conf);
       abstract public string ToString(bool with_param = false);

    }
}

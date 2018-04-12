using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fuzzy_system.Class_Pittsburgh;
using Fuzzy_system.Approx_Singletone.learn_algorithm.conf;
using Fuzzy_system.Fuzzy_Abstract.learn_algorithm.conf;

namespace Fuzzy_system.Class_Pittsburgh.learn_algorithm
{
    public abstract class Abstract_term_config
    {
       public abstract c_Fuzzy_System TuneUpFuzzySystem(c_Fuzzy_System Classifier, Abstract_learn_algorithm_conf conf);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fuzzy_system.Approx_Singletone.add_generators.conf;
using Fuzzy_system.Class_Pittsburgh;
using Fuzzy_system.Fuzzy_Abstract.add_generators.conf;

namespace Fuzzy_system.Class_Pittsburgh.add_generators
{
    public abstract class Abstract_generator
    {
        abstract public c_Fuzzy_System Generate(c_Fuzzy_System Classifier, Abstract_generator_conf config);
    }
}

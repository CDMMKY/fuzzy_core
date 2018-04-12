using Fuzzy_system.Approx_Singletone.add_generators.conf;
using Fuzzy_system.Approx_Singletone.learn_algorithm.conf;
using Fuzzy_system.Fuzzy_Abstract.add_generators.conf;
using System.ComponentModel;
using Fuzzy_system.Fuzzy_Abstract.learn_algorithm.conf;


namespace Fuzzy_system.Approx_Singletone
{
    class Null_conf_for_all:Abstract_generator_conf,Abstract_learn_algorithm_conf
    {
        [Description("У данного метода нет настраиваемых параметров"), Category("Методы")]
        public string  Нет_настраиваемых_параметров { get { return ""; } }
    }
}

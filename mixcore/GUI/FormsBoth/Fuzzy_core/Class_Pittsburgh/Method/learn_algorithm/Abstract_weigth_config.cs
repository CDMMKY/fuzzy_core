using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fuzzy_system.Class_Pittsburgh.wegthconfig.conf;
using Fuzzy_system.Class_Pittsburgh;

namespace Fuzzy_system.Class_Pittsburgh.wegthconfig
{
    public abstract class Abstract_weigth_config
    {
        public abstract c_Fuzzy_System TuneUpWeigth(Fuzzy_system.Class_Pittsburgh.c_Fuzzy_System Classifier, Abstract_weigth_config_conf conf);
    
    }

    }


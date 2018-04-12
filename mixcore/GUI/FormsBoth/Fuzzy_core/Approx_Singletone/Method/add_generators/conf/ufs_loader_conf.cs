using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Fuzzy_system.Approx_Singletone;
using Mix_core.Properties;
using Fuzzy_system.Fuzzy_Abstract.add_generators.conf;
namespace Fuzzy_system.Approx_Singletone.add_generators.conf
{
    internal class Load_from_UFS_conf : Abstract_generator_conf
    {



        public Load_from_UFS_conf(string file_name)
        {
            Settings.Default.Load_UFS_file_name = file_name;
            Settings.Default.Save();
          
        }
    
                [Description("Источник базы правил"), Category("Файл")]

        public string Файл_UFS
        {
            get { return Settings.Default.Load_UFS_file_name; }
            set
            {
                Settings.Default.Load_UFS_file_name=value;
                Settings.Default.Save();
            }


        }

        

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Fuzzy_system.Approx_Singletone;
using Mix_core.Properties;

namespace Fuzzy_system.Approx_Singletone.add_generators.conf
{
    internal class Term_shrink_and_rotate_conf : init_everyone_with_everyone
    {

     
        readonly int max_count_shrink_vars;
        readonly int min_count_shrink_vars=1;
     
        
        public Term_shrink_and_rotate_conf(int count_vars):base(count_vars)
    {max_count_shrink_vars=count_vars-1;
    
    }

    
        [Description("По скольки входным параметрам будем уменьшено количество термов "), Category("Параметры НС")]
        public int Число_параметров_для_уменьшения_термов
        {
            get { return Settings.Default.Term_shrink_and_rotate_conf_count_shrink = Settings.Default.Term_shrink_and_rotate_conf_count_shrink < max_count_shrink_vars ? Settings.Default.Term_shrink_and_rotate_conf_count_shrink > min_count_shrink_vars ? Settings.Default.Term_shrink_and_rotate_conf_count_shrink : min_count_shrink_vars : max_count_shrink_vars; }
            set
            {
                Settings.Default.Term_shrink_and_rotate_conf_count_shrink = value < max_count_shrink_vars ? value : max_count_shrink_vars;
                Settings.Default.Term_shrink_and_rotate_conf_count_shrink = Settings.Default.Term_shrink_and_rotate_conf_count_shrink < min_count_shrink_vars ? min_count_shrink_vars : Settings.Default.Term_shrink_and_rotate_conf_count_shrink;
                Settings.Default.Save();
            }
    

        }

        [Description("Максимальная количество параметров которое вы можете уменьшить "), Category("Параметры НС")]

        public int Максимально_параметров_для_уменьшения_термов
        {
            get { return max_count_shrink_vars; }



        }

                [Description("Насколько будет уменьшено количеств термов для каждой"), Category("Параметры НС")]

        public int Значение_уменьшения_термов
        {
            get { return Settings.Default.Term_shrink_and_rotate_conf_size_of_shrink; }
            set
            {
                Settings.Default.Term_shrink_and_rotate_conf_size_of_shrink=value;
                Settings.Default.Save();
            }


        }

        

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fuzzy_system.Fuzzy_Abstract.add_generators.conf;
using Fuzzy_system.Fuzzy_Abstract.learn_algorithm.conf;

namespace Fuzzy_system.Fuzzy_FrontEnd
{
  public  interface IFuzzy_System_FroentEnd
    {
        object[] add_generator_algorithm { get; }
        object[] learn_algorithm { get; }
        bool Load_learn_set(string file_name);
        void Load_test_set(string file_name);
        void Set_count_add_generator(int count_used_Algorithm);
        Abstract_generator_conf Set_add_generator_algorothm(int numAlgorithm, int typeAlgorithm);
        void Set_count_learn_algorithm(int count_used_Algorithm);
        Abstract_learn_algorithm_conf Set_learn_algorothm(int numAlgorithm, int typeAlgorithm);
        void Set_count_repeate_into_cirlucle(int numRepeate);
        void Set_count_repeate_renew(int numRepeate);
        void Calc();
        string get_log{get;}
        int get_global_completed(); 
        bool Is_UFS{get ;}
        bool is_autosave_FS { set; }
        string path_to_save { set; }
        int max_sections_of_Calc { get; }
        System.ComponentModel.BackgroundWorker ProgressSource{set ;}
    

    }
}

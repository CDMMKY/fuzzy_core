using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fuzzy_system.Class_Pittsburgh.wegthconfig.conf;
using Fuzzy_system.Class_Pittsburgh;

namespace Fuzzy_system.Class_Pittsburgh.wegthconfig
{
    class Weigth_Config_Random_Search : Abstract_weigth_config
    {
        Random rand = new Random();
        public override Fuzzy_system.Class_Pittsburgh.c_Fuzzy_System TuneUpWeigth(Fuzzy_system.Class_Pittsburgh.c_Fuzzy_System Classifier, Abstract_weigth_config_conf conf)
        {
            int count_iteration = ((Weigth_Config_Random_Search_conf)conf).Количество_итераций;
            int count_generation_by_iteration =
                ((Weigth_Config_Random_Search_conf)conf).Количество_генерируемых_векторов_веса_за_итерацию;

            c_Fuzzy_System result = Classifier;


            for (int i = 0; i < count_iteration; i++)
            { 
                double [][] weigth = new double[count_generation_by_iteration+1][];
                weigth[0] = Classifier.Rulles_Database_Set[0].Weigth;
                double best_result = result.Classify_Learn_Samples();
                
                int best_index = 0;
                for (int j = 1; j < count_generation_by_iteration+1; j++)
                {  
                    weigth[j]= new double[weigth[0].Count()];
                    for (int k=0; k<weigth[0].Count();k++)
                    {
                        weigth[j][k] = rand.NextDouble();
                    }

                    result.Rulles_Database_Set[0].Weigth = weigth[j];
                    double current_result = result.Classify_Learn_Samples();
                    if (current_result > best_result)
                    {
                        best_result = current_result;
                        best_index = j;
                    }
                    result.Rulles_Database_Set[0].Weigth = weigth[best_index];
                }
            }

            return result;
        }
    }
}

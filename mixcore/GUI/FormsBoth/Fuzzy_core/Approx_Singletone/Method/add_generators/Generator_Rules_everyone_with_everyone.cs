using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fuzzy_system.Approx_Singletone.add_generators.conf;
using Fuzzy_system.Approx_Singletone;
using Fuzzy_system.Fuzzy_Abstract.add_generators.conf;
using Linglib;

namespace Fuzzy_system.Approx_Singletone.add_generators
{
    public class Generator_Rules_everyone_with_everyone : Abstract_generator
    {
        private Type_Term_Func_Enum type_func;
        int[] count_slice_vars =null;
        public override a_Fuzzy_System Generate(a_Fuzzy_System Approximate, Abstract_generator_conf config)
        {
            a_Fuzzy_System result = Approximate;

            init_everyone_with_everyone config1 = config as init_everyone_with_everyone;
             type_func = config1.Функция_принадлежности;
             count_slice_vars = config1.Количество_термов_для_каждого_признака;
            result.Init_Rules_everyone_with_everyone(type_func, count_slice_vars);



            return result;
        }


        public override string ToString(bool with_param = false)
        { if(with_param)
        {
            string result = "перебор с равномерным разбиением {";
            result += "Функции принадлежности= " +Member_Function.ToString(type_func) +" ;"+Environment.NewLine;
            for (int i = 0; i < count_slice_vars.Count(); i++)
            {
                result +=" "+count_slice_vars[i].ToString()+" "+ pluralform.nobot(count_slice_vars[i], new string[3] {"терм","терма","термов"}) +" по " +(i+1).ToString()+ " "+ " параметру ;" + Environment.NewLine; 

            }
            result +="}";
            return result; 
            }
        return "перебор с равномерным разбиением";
        }
    }
}

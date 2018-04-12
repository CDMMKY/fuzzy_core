using System;
using System.Linq;
using FuzzySystem.FuzzyAbstract.conf;
using Linglib;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract;

namespace FuzzySystem.SingletoneApproximate.AddGenerators
{
    public class GeneratorRulesEveryoneWithOptimal : AbstractNotSafeGenerator
    {
        public int[] count_slice_vars =null;

        List<KnowlegeBaseSARules> Systems_ready_to_test;
        List<double> errors_of_systems ;
        TypeTermFuncEnum type_func;

        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.Singletone };
            }
        }

        List<List<int>> Pull_of_systems ;
       
        public IFuzzySystem Generate(IFuzzySystem Approximate, IGeneratorConf config)
        {
            SAFuzzySystem toRunFuzzySystem = Approximate as SAFuzzySystem;
            return Generate(toRunFuzzySystem, config);
        }

        private void Generate_all_variant_in_pool(List<int> Bool_struct)
        {
            Pull_of_systems = new List<List<int>>();
            int pos = 0;
            do
            {
                Pull_of_systems.Add(new List<int>(Bool_struct));

                pos = Bool_struct.Count - 2;
                for (int i = Bool_struct.Count - 1; pos >= 0 && Bool_struct[pos] >= Bool_struct[i]; i--) pos--;
                int j = Bool_struct.Count - 1;
                while (pos >= 0 && Bool_struct[pos] >= Bool_struct[j]) j--;
                //j++;
                if (pos >= 0)
                {
                    int temp = Bool_struct[pos];
                    Bool_struct[pos] = Bool_struct[j];

                    Bool_struct[j] = temp;

                }
                int l = pos + 1, r = Bool_struct.Count - 1;
                while (l < r)
                {
                    int temp = Bool_struct[l];

                    Bool_struct[l] = Bool_struct[r];
                    Bool_struct[r] = temp;
                    l++;
                    r--;
                }
            } while (pos >= 0);




        }






        public override SAFuzzySystem Generate(SAFuzzySystem Approximate, IGeneratorConf config)
        {
            SAFuzzySystem result = Approximate;

            Systems_ready_to_test = new List<KnowlegeBaseSARules>();
            errors_of_systems = new List<double>();

            InitEveryoneWithOptimal config1 = config as InitEveryoneWithOptimal;
             type_func = config1.IEWOTypeFunc;
             count_slice_vars = config1.count_terms;


             List<int> Varians_of_run_system = new List<int>();
             for (int i = 0; i < Approximate.CountFeatures; i++)
             {
                 int count_terms_for_var = count_slice_vars[i];
                  Varians_of_run_system.Add(count_terms_for_var); 
             }

             Varians_of_run_system.Sort();
             Generate_all_variant_in_pool(Varians_of_run_system);

             for (int i = 0; i < Pull_of_systems.Count; i++)
             {
                 Approximate.RulesDatabaseSet.Clear();

                 GeneratorRulesEveryoneWithEveryone.InitRulesEveryoneWithEveryone(Approximate,type_func, Pull_of_systems[i].ToArray());
                 Systems_ready_to_test.Add(Approximate.RulesDatabaseSet[0]);
                 errors_of_systems.Add(result.approxLearnSamples(result.RulesDatabaseSet[ 0]));


             }

             int best_index = errors_of_systems.IndexOf(errors_of_systems.Min());
             result.RulesDatabaseSet.Clear();
             result.RulesDatabaseSet.Add(Systems_ready_to_test[best_index]);
             for (int i = 0; i < count_slice_vars.Count(); i++)
             {

                 count_slice_vars[i] = result.RulesDatabaseSet[0].TermsSet.Count(x => x.NumVar == i);
             }
            Console.WriteLine(Pull_of_systems.Count());
            result.RulesDatabaseSet[0].TermsSet.Trim();
             GC.Collect();

            return result;
        }


        public override string ToString(bool with_param = false)
        { if(with_param)
        {
            string result = "Перебор с подбором разбиения {";
            result += "Функции принадлежности= " +Term.ToStringTypeTerm(type_func) +" ;"+Environment.NewLine;
            for (int i = 0; i < count_slice_vars.Count(); i++)
            {
                result +=" "+count_slice_vars[i].ToString()+" "+ pluralform.nobot(count_slice_vars[i], new string[3] {"терм","терма","термов"}) +" по " +(i+1).ToString()+ " "+ " параметру ;" + Environment.NewLine; 

            }
            result +="}";
            return result; 
            }
        return "Перебор с подбором разбиения";
        }

        public override IGeneratorConf getConf(int CountFeatures)
        {
            IGeneratorConf result = new InitEveryoneWithOptimal();
            result.Init(CountFeatures);
            return result;
        }

    }
}

using System;
using System.Linq;
using FuzzySystem.FuzzyAbstract.conf;
using Linglib;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract;
using DynamicTune;

namespace FuzzySystem.TakagiSugenoApproximate.AddGenerators
{
    public class GeneratorRulesEveryoneWithOptimal : AbstractNotSafeGenerator
    {
        public int[] count_slice_vars =null;

        List<KnowlegeBaseTSARules> Systems_ready_to_test;
        List<double> errors_of_systems ;
        TypeTermFuncEnum type_func;
        double maxError;
        int RuleCount;
        int TryCount;

        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate };
            }
        }

        List<List<int>> Pull_of_systems ;
       
        public IFuzzySystem Generate(IFuzzySystem Approximate, IGeneratorConf config)
        {
            TSAFuzzySystem toRunFuzzySystem = Approximate as TSAFuzzySystem;
            return Generate(toRunFuzzySystem, config);
        }

        private void Generate_all_variant_in_pool(List<int> Bool_struct)
        {
            if (Pull_of_systems == null) { Pull_of_systems = new List<List<int>>(); }

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






        public override TSAFuzzySystem Generate(TSAFuzzySystem Approximate, IGeneratorConf config)
        {
            TSAFuzzySystem result = Approximate;
            Pull_of_systems = Pull_of_systems = new List<List<int>>();
            Systems_ready_to_test = new List<KnowlegeBaseTSARules>();
            errors_of_systems = new List<double>();

            DynamicTuneConfGenerator config1 = config as DynamicTuneConfGenerator;
             type_func = config1.IEWOTypeFunc;
          
            
            


             List<int> Varians_of_run_system = new List<int>();


             List<int> allOne = new List<int>();
             for (int i = 0; i < Approximate.CountFeatures; i++)
             {
                 allOne.Add( 1);
             }
             Pull_of_systems.Add(new List<int>(allOne));

             for (int j = 0; j < Approximate.CountFeatures-1;j++ )
             {
                 Varians_of_run_system.Clear();
                    allOne[j]=2;
                     Varians_of_run_system.AddRange(new List<int> (allOne));
                 Varians_of_run_system.Sort();
                 Generate_all_variant_in_pool(Varians_of_run_system);
             }
             allOne[Approximate.CountFeatures - 1] = 2;
             Pull_of_systems.Add(new List <int> (allOne));

             for (int i = 0; i < Pull_of_systems.Count; i++)
             {
                 Approximate.RulesDatabaseSet.Clear();

                 GeneratorRulesEveryoneWithEveryone.InitRulesEveryoneWithEveryone(Approximate,type_func, Pull_of_systems[i].ToArray());
                 Systems_ready_to_test.Add(Approximate.RulesDatabaseSet[0]);
                 errors_of_systems.Add(result.approxLearnSamples(result.RulesDatabaseSet[ 0]));
             }

             DynamicTuneClass dt = new DynamicTuneClass();
          Approximate=   dt.TuneUpFuzzySystem(Approximate, config1);
          Systems_ready_to_test.Add(Approximate.RulesDatabaseSet[0]);
          errors_of_systems.Add(result.approxLearnSamples(result.RulesDatabaseSet[ 0]));
            

             int best_index = errors_of_systems.IndexOf(errors_of_systems.Min());
             result.RulesDatabaseSet.Clear();
             result.RulesDatabaseSet.Add(Systems_ready_to_test[best_index]);
             count_slice_vars = new int[Approximate.CountFeatures];
             for (int i = 0; i < count_slice_vars.Count(); i++)
             {

                 count_slice_vars[i] = result.RulesDatabaseSet[0].TermsSet.Count(x => x.NumVar == i);
             }
             maxError = config1.MaxError;
             TryCount = config1.TryCount;
             RuleCount = result.RulesDatabaseSet[0].RulesDatabase.Count;
            Console.WriteLine(Pull_of_systems.Count());
            result.RulesDatabaseSet[0].TermsSet.Trim();
             GC.Collect();

            return result;
        }


        public override string ToString(bool with_param = false)
        { if(with_param)
        {
            string result = "Перебор и динамика {";
            result += "Функции принадлежности= " +Term.ToStringTypeTerm(type_func) +" ;"+Environment.NewLine;
            for (int i = 0; i < count_slice_vars.Count(); i++)
            {
                result +=" "+count_slice_vars[i].ToString()+" "+ pluralform.nobot(count_slice_vars[i], new string[3] {"терм","терма","термов"}) +" по " +(i+1).ToString()+ " "+ " параметру ;" + Environment.NewLine; 

            }
            result += "Максимальная ошибка: " + maxError + Environment.NewLine;
            result += "Число правил: " + RuleCount + Environment.NewLine;
            result += "Число попыток: " + TryCount + Environment.NewLine;
            result +="}";
            return result; 
            }
        return "Перебор и динамика";
        }

        public override IGeneratorConf getConf(int CountFeatures)
        {
            IGeneratorConf result = new DynamicTuneConfGenerator();
            result.Init(CountFeatures);
            return result;
        }

    }
}

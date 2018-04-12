using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySystem.FuzzyAbstract.conf;
using Linglib;
using FuzzySystem.FuzzyAbstract;

namespace FuzzySystem.TakagiSugenoApproximate.AddGenerators
{
    public class GeneratorTermShrinkAndRotate : AbstractNotSafeGenerator
    {
        List< List<int>> Pull_of_systems = new List< List<int>>();
        List <KnowlegeBaseTSARules> Systems_ready_to_test = new List<KnowlegeBaseTSARules>();
        List<double> errors_of_systems = new List<double>();
        int count_shrink = 0;
        int size_shrink = 0;
        TypeTermFuncEnum type_func;
        int[] count_slices = null;

        public IFuzzySystem Generate(IFuzzySystem Classifier, IGeneratorConf config)
        {
            TSAFuzzySystem toRunFuzzySystem = Classifier as TSAFuzzySystem;
            return Generate(toRunFuzzySystem, config);
        }

        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate};
            }
        }
     
        private void Generate_all_variant_in_pool(List<int> Bool_struct)
        {
                        int pos =0;
            do
            {Pull_of_systems.Add( new  List<int> (Bool_struct) );
                
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
                  
                    Bool_struct[l]=Bool_struct[r];
                    Bool_struct[r] = temp;
                    l++;
                    r--;
                }
            } while (pos >= 0);




        }


        public override TSAFuzzySystem Generate(TSAFuzzySystem Approximate, IGeneratorConf config)
        {
            TSAFuzzySystem result = Approximate;
            if (result.RulesDatabaseSet.Count == 0)
            {
                AbstractNotSafeGenerator tempGen = new GeneratorRulesEveryoneWithEveryone();
                result = tempGen.Generate(result, config);
                GC.Collect(); 
            }

            count_shrink = ((TermShrinkAndRotateConf)config).TSARCShrinkVars;
            size_shrink = ((TermShrinkAndRotateConf)config).TSARCShrinkTerm;
            type_func=((TermShrinkAndRotateConf)config).IEWEFuncType;
            count_slices = ((TermShrinkAndRotateConf)config).IEWECountSlice;

            

            List <int> Varians_of_run_system = new List<int>();
            for (int i=0; i<Approximate.CountFeatures;i++)
             {
                 int count_terms_for_var = Approximate.RulesDatabaseSet[0].TermsSet.FindAll(x => x.NumVar == i).Count;
                 if (i < count_shrink)
                 {
                     Varians_of_run_system.Add(count_terms_for_var-size_shrink);
                 }
                 else { Varians_of_run_system.Add(count_terms_for_var); }
                }

            Varians_of_run_system.Sort();
           TypeTermFuncEnum type_of_term =  Approximate.RulesDatabaseSet[0].TermsSet[0].TermFuncType;
            Generate_all_variant_in_pool(Varians_of_run_system);
            
            for (int i = 0; i < Pull_of_systems.Count;i++ )
            {
                Approximate.RulesDatabaseSet.Clear();

               GeneratorRulesEveryoneWithEveryone.InitRulesEveryoneWithEveryone(Approximate, type_of_term, Pull_of_systems[i].ToArray());
                Systems_ready_to_test.Add(Approximate.RulesDatabaseSet[0]);
                errors_of_systems.Add(result.approxLearnSamples(result.RulesDatabaseSet[ 0]));


            }

            int best_index = errors_of_systems.IndexOf(errors_of_systems.Min());
           result.RulesDatabaseSet.Clear();
           result.RulesDatabaseSet.Add(Systems_ready_to_test[best_index]);
                Console.WriteLine(Pull_of_systems.Count());



                GC.Collect();
//            result.UnlaidProtectionFix();
                result.RulesDatabaseSet[0].TermsSet.Trim();
             return result;
            
        }




        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "отсечение термов {";
                result += "Функции принадлежности= " + Term.ToStringTypeTerm(type_func) + " ;" + Environment.NewLine;
                result += "По скольким параметрам уменьшать  =" + this.count_shrink.ToString() + " ; " + Environment.NewLine;
                result += "На сколько уменьшать термов =" + this.size_shrink.ToString() + " ; " + Environment.NewLine;
                for (int i = 0; i < count_slices.Count(); i++)
                {
                    result += " " + count_slices[i].ToString() + pluralform.nobot(count_slices[i], new string[3] { "терм", "терма", "термов" }) + " по " + (i + 1).ToString() + " " + " параметру ;" + Environment.NewLine;

                }
                result += "}";
                return result;
            }
            return "отсечение термов";
        }


        public override IGeneratorConf getConf(int CountFeatures)
        {
            IGeneratorConf result = new TermShrinkAndRotateConf();
            result.Init(CountFeatures);
            return result;
        }
    }
}

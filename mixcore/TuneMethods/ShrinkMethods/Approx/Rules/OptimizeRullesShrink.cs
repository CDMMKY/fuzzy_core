using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;

namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm
{
    public class OptimizeRullesShrink : AbstractNotSafeLearnAlgorithm
    {
        List< List<bool>> Pull_of_systems = new List< List<bool>>();
        List<double> errors_of_systems = new List<double>();
        int start_add_rules;
        int count_Shrink_rule;     
    
        public static bool BytetoBool(byte value)
        {
            return !value.Equals(0); 
        }
        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.Singletone };
            }
        }
       

        private void Generate_all_variant_in_pool(List<byte> Bool_struct)
        {
                        int pos =0;
            do
            {Pull_of_systems.Add( Bool_struct.ConvertAll<bool>(BytetoBool) );
                
                pos = Bool_struct.Count - 2;
                for (int i = Bool_struct.Count - 1; pos >= 0 && Bool_struct[pos] >= Bool_struct[i]; i--) pos--; 
                int j = Bool_struct.Count - 1; 
                while (pos >= 0 && Bool_struct[pos] >= Bool_struct[j]) j--;
                //j++;
                if (pos >= 0)
                {
                    byte temp = Bool_struct[pos];
                    Bool_struct[pos] = Bool_struct[j];

                    Bool_struct[j] = temp;
                 
                }
                int l = pos + 1, r = Bool_struct.Count - 1;
                while (l < r)
                {
                    byte temp = Bool_struct[l];
                  
                    Bool_struct[l]=Bool_struct[r];
                    Bool_struct[r] = temp;
                    l++;
                    r--;
                }
            } while (pos >= 0);




        }


        public override SAFuzzySystem TuneUpFuzzySystem(FuzzySystem.SingletoneApproximate.SAFuzzySystem Approximate, ILearnAlgorithmConf config)
        {
            start_add_rules = Approximate.RulesDatabaseSet.Count;
            SAFuzzySystem result = Approximate;
            if (result.RulesDatabaseSet.Count == 0)
            {

                throw new System.FormatException("Что то не то с входными данными");
            }




            OptimizeRullesShrinkConf Config = config as OptimizeRullesShrinkConf;
            count_Shrink_rule = Config.ORSCCountShrinkRules;

            int count_of_swith_off = count_Shrink_rule;
            List <byte> Varians_of_run_system = new List<byte>();
            for (int i=0; i<Approximate.RulesDatabaseSet[0].RulesDatabase.Count;i++)
            {
                Varians_of_run_system.Add(1);
            }
              for (int i=0; i<count_of_swith_off;i++)
            {
                Varians_of_run_system[i]=0;
            }
            Generate_all_variant_in_pool(Varians_of_run_system);
            for (int i = 0; i < Pull_of_systems.Count;i++ )
            {
                KnowlegeBaseSARules temp_rules = new  KnowlegeBaseSARules(result.RulesDatabaseSet[0],Pull_of_systems[i]);
                temp_rules.TrimTerms();
                
                result.RulesDatabaseSet.Add(temp_rules);
                result.UnlaidProtectionFix(result.RulesDatabaseSet[ start_add_rules + i]);
                errors_of_systems.Add(result.approxLearnSamples(result.RulesDatabaseSet[ start_add_rules+i]));


            }

            int best_index = errors_of_systems.IndexOf(errors_of_systems.Min());
           KnowlegeBaseSARules best = result.RulesDatabaseSet[start_add_rules + best_index];
           result.RulesDatabaseSet.Clear();
           result.RulesDatabaseSet.Add(best);
                Console.WriteLine(Pull_of_systems.Count());



                result.RulesDatabaseSet[0].TermsSet.Trim();
//            result.UnlaidProtectionFix();
             return result;
            
        }
        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "отсечение правил {";
                result += "Требуется удалить правил правил =" + count_Shrink_rule.ToString() + " ; " + Environment.NewLine;
                result += "}";
                return result;
            }
            return "отсечение правил";
        }

        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new OptimizeRullesShrinkConf();
            result.Init(CountFeatures);
            return result;
        }


    }
}

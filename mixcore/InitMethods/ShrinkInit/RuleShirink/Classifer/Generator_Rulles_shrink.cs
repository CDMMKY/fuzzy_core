using System;
using System.Collections.Generic;
using System.Linq;

using Linglib;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;

namespace FuzzySystem.PittsburghClassifier.add_generators
{
    public class GeneratorRullesShrink : AbstractNotSafeGenerator
    {
        List< List<bool>> Pull_of_systems = new List< List<bool>>();
        List<double> errors_of_systems = new List<double>();
        int start_add_rules;
        int Request_count_rules = 0;
        int max_count_rules = 0;
        int min_count_rules = 0;
        int[] count_slices = null;
        TypeTermFuncEnum type_term;

        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.PittsburghClassifier };
            }
        }

        public static bool BytetoBool(byte value)
        {
            return !value.Equals(0); 
        }

        public IFuzzySystem Generate(IFuzzySystem Classifier, IGeneratorConf config)
        {
            PCFuzzySystem toRunFuzzySystem = Classifier as PCFuzzySystem;
            return Generate(toRunFuzzySystem, config);
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


        public override PCFuzzySystem Generate(PCFuzzySystem Classifier, IGeneratorConf config)
        {
            start_add_rules = Classifier.RulesDatabaseSet.Count;
            PCFuzzySystem result = Classifier;
            if (result.RulesDatabaseSet.Count == 0)
            {
                AbstractNotSafeGenerator tempGen = new GeneratorRulesEveryoneWithEveryone();
                result = tempGen.Generate(result,config);
                GC.Collect(); 
            }





            Request_count_rules = ((RullesShrinkConf)config).RSCCountRules;
            max_count_rules = ((RullesShrinkConf)config).RSCMaxRules;
            count_slices = ((RullesShrinkConf)config).IEWECountSlice;
            min_count_rules = ((RullesShrinkConf)config).RSCMinRules;
            type_term = ((RullesShrinkConf)config).IEWEFuncType;

            int count_of_swith_off = ((RullesShrinkConf)config).RSCMaxRules - Request_count_rules;
            List <byte> Varians_of_run_system = new List<byte>();
            for (int i=0; i<Classifier.RulesDatabaseSet[0].RulesDatabase.Count;i++)
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
                KnowlegeBasePCRules temp_rules = new  KnowlegeBasePCRules(result.RulesDatabaseSet[0],Pull_of_systems[i]);
                temp_rules.TrimTerms();
                
                result.RulesDatabaseSet.Add(temp_rules);
                result.UnlaidProtectionFix(result.RulesDatabaseSet[ start_add_rules + i]);
                errors_of_systems.Add(result.ErrorLearnSamples(result.RulesDatabaseSet[start_add_rules + i]));


            }

            int best_index = errors_of_systems.IndexOf(errors_of_systems.Min());
           KnowlegeBasePCRules best = result.RulesDatabaseSet[start_add_rules + best_index];
           result.RulesDatabaseSet.Clear();
           result.RulesDatabaseSet.Add(best);
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
                string result = "отсечение правил {";
                result += "Функции принадлежности= " + Term.ToStringTypeTerm(type_term) + " ;" + Environment.NewLine;
                result += "Требуется правил =" + Request_count_rules.ToString() + " ; " + Environment.NewLine;
                for (int i = 0; i < count_slices.Count(); i++)
                {
                    result += " " + count_slices[i].ToString() + pluralform.nobot(count_slices[i], new string[3] { "терм", "терма", "термов" }) + " по " + (i + 1).ToString() + " " + " параметру ;" + Environment.NewLine;

                }
                result += "}";
                return result;
            }
            return "отсечение правил";
        }


        public override IGeneratorConf getConf(int CountFeatures)
        {
            IGeneratorConf result = new RullesShrinkConf();
            result.Init(CountFeatures);
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract.AddGenerators.conf;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.SingletoneApproximate.AddGenerators.I_k_mean;



namespace FuzzySystem.TakagiSugenoApproximate.AddGenerators
{
    public class Approxk_mean_rules_generator : AbstractNotSafeGenerator
    {
        Type_k_mean_algorithm type_alg;
        int count_rules = 0;
        TypeTermFuncEnum type_func ;
        double nebulisation_factor =0;
        int Max_iteration =0;
        double need_precision =0;
        public IFuzzySystem Generate(IFuzzySystem Approximate, IGeneratorConf config)
        {
            TSAFuzzySystem toRunFuzzySystem = Approximate as TSAFuzzySystem;
            return Generate(toRunFuzzySystem, config);
        }

        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate };
            }
        }

        private double Calc_distance_for_member_ship_function_for_Clust(int number_cluster,int number_var,Approxk_mean_base Alg)
        {double nominator =0;
            double denominator=0;
            for (int e=0;e<Alg.Learn_table.CountSamples;e++)
            {nominator+=Math.Pow (Alg.U_matrix[number_cluster][e],2)*Math.Pow(Alg.Centroid_cordinate_S[number_cluster][number_var]-Alg.Learn_table.DataRows[e].InputAttributeValue[number_var],2);
            denominator+=Math.Pow (Alg.U_matrix[number_cluster][e],2);
            
            }
            return nominator/denominator;
        
            }





        public override TSAFuzzySystem Generate(TSAFuzzySystem Approximate, IGeneratorConf config)
        {
            type_alg = ((kMeanRulesGeneratorConfig)config).KMRGTypeAlg;
            count_rules = ((kMeanRulesGeneratorConfig)config).KMRGCountRules;
            type_func = ((kMeanRulesGeneratorConfig)config).KMRGTypeFunc;
            nebulisation_factor = ((kMeanRulesGeneratorConfig)config).KMRGExponentialWeight;
            Max_iteration = ((kMeanRulesGeneratorConfig)config).KMRGIteraton;
             need_precision = ((kMeanRulesGeneratorConfig)config).KMRGAccuracy;


                Approxk_mean_base K_Agl= null;

                switch (type_alg)
                {
                    case Type_k_mean_algorithm.GathGeva: K_Agl = new Approxk_mean_Gath_Geva(Approximate.LearnSamplesSet, Max_iteration,need_precision, count_rules,nebulisation_factor); break;
                    case Type_k_mean_algorithm.GustafsonKessel: K_Agl = new Approxk_mean_Gustafson_kessel(Approximate.LearnSamplesSet, Max_iteration, need_precision, count_rules,nebulisation_factor); break;
                    case Type_k_mean_algorithm.FCM: K_Agl = new Approxk_mean_base(Approximate.LearnSamplesSet, Max_iteration, need_precision, count_rules,nebulisation_factor); break;

                }
                K_Agl.Calc();
          
            KnowlegeBaseTSARules New_Rules= new KnowlegeBaseTSARules();
            for(int i=0;i<count_rules;i++)
            { int [] order_terms = new int [Approximate.LearnSamplesSet.CountVars];
            List<Term> term_set = new List<Term>();
                for (int j=0;j<Approximate.LearnSamplesSet.CountVars;j++)
            {
               Term temp_term= Term.MakeTerm(K_Agl.Centroid_cordinate_S[i][j], Math.Sqrt( Calc_distance_for_member_ship_function_for_Clust(i, j, K_Agl))*3, type_func,j);
               term_set.Add(temp_term);
            }
                New_Rules.ConstructNewRule(term_set,Approximate);
            }
          
            TSAFuzzySystem Result = Approximate;
            if (Result.RulesDatabaseSet.Count > 0)
            {
                Result.RulesDatabaseSet[0] = New_Rules;
            }
            else { Result.RulesDatabaseSet.Add (New_Rules); }
            Result.UnlaidProtectionFix(Result.RulesDatabaseSet[0]);
            GC.Collect();
            Result.RulesDatabaseSet[0].TermsSet.Trim();
            return Result;
        }

        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Модификация c-средних {";
               result += "Тип модификции = ";
                switch(this.type_alg)
                {
                    case Type_k_mean_algorithm.FCM: result += "FCM"; break;
                    case Type_k_mean_algorithm.GathGeva: result += "Gath-Geva"; break;
                    case Type_k_mean_algorithm.GustafsonKessel: result += "Guthstafson Kessel"; break; 
           
                }
                result += " ; " + Environment.NewLine;

                result += "Функции принадлежности= " + Term.ToStringTypeTerm(type_func) + " ;" + Environment.NewLine;
               
                result += "Генерируется правил= "+this.count_rules.ToString()+ " ;" + Environment.NewLine;
                 result += "KMRGIteraton = "+this.Max_iteration.ToString()+ " ;" + Environment.NewLine;
                 result += "Экспоненциальный вес = "+this.nebulisation_factor.ToString()+ " ;" + Environment.NewLine;
                   
                            
                result += "}";
                return result;
            }
            return "Модификация c-средних";
        }


        public override IGeneratorConf getConf(int CountFeatures)
        {
            IGeneratorConf result = new kMeanRulesGeneratorConfig();
            result.Init(CountFeatures);
            return result;
        }

    }
}

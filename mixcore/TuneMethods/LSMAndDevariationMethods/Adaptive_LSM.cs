using System;
using System.Linq;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;
using System.Collections.Generic;

namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm
{
    public class Adaptive_LSM : AbstractNotSafeLearnAlgorithm
    {

        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.Singletone };
            }
        }

        public  IFuzzySystem TuneUpFuzzySystem(IFuzzySystem Approximate, ILearnAlgorithmConf conf)
        {
            SAFuzzySystem toRunSystem = Approximate as SAFuzzySystem;
            return TuneUpFuzzySystem(toRunSystem, conf);
        }


        private double[, ,] extract_Rules(KnowlegeBaseSARules rules_Database)
        { 
            int count_rules= rules_Database.RulesDatabase.Count;
            int count_vars = rules_Database.RulesDatabase[0].ListTermsInRule.Count;
            int count_term_params =rules_Database.TermsSet[0].CountParams;

            double [,,] Result = new double[count_rules,count_vars,count_term_params];

            for (int i = 0; i < count_rules; i++)
            {
                for (int j = 0; j < count_vars; j++)
                {

                    Term temp_term = rules_Database.RulesDatabase[i].ListTermsInRule.First(x => x.NumVar == j);
                    for (int k = 0; k < count_term_params; k++)
                    {
                        Result[i, j, k] = temp_term.Parametrs[k];
                    }
                }
            }

            return Result;
        }
        private double[,] extract_Sample_table(SampleSet learn_Set)
        {
            double[,] Result = new double[learn_Set.CountSamples, learn_Set.CountVars];
            for (int i = 0; i < learn_Set.CountSamples; i++)
            {
                for (int j = 0; j < learn_Set.CountVars; j++)
                {
                    Result[i, j] = learn_Set.DataRows[i].InputAttributeValue[j]; 
                }
            }
            return Result;

        }

       

        private double[] extract_Sample_table_Out(SampleSet learn_Set)
        {
            double[] Result = new double[learn_Set.CountSamples];
            for (int i = 0; i < learn_Set.CountSamples; i++)
            {
                Result[i] = learn_Set.DataRows[i].DoubleOutput; 
            }
            return Result;
        }

        public override FuzzySystem.SingletoneApproximate.SAFuzzySystem TuneUpFuzzySystem(FuzzySystem.SingletoneApproximate.SAFuzzySystem Approximate, ILearnAlgorithmConf conf)
        {

            Mnk_lib.Mnk_class Mnk_me = new Mnk_lib.Mnk_class();
           
            double [,,] Extracted_rules= extract_Rules(Approximate.RulesDatabaseSet[0]);
            double [,] Extracted_Samples = extract_Sample_table(Approximate.LearnSamplesSet);
            double [] Extracted_Samples_out = extract_Sample_table_Out(Approximate.LearnSamplesSet);
            int count_rules= Approximate.RulesDatabaseSet[0].RulesDatabase.Count;
            int count_samples = Approximate.LearnSamplesSet.CountSamples;
            int count_Vars = Approximate.LearnSamplesSet.CountVars;
            double [] New_consq = new double[count_rules];
            TypeTermFuncEnum type_Func= Approximate.RulesDatabaseSet[0].TermsSet[0].TermFuncType;
            int type_func= (int) type_Func;
            
            Mnk_me.mnk_R(Extracted_rules,count_rules,type_func,Extracted_Samples,Extracted_Samples_out,count_samples,count_Vars,out New_consq);
           
            SAFuzzySystem Result = Approximate;
  
            double result_before = Result.approxLearnSamples(Result.RulesDatabaseSet[ 0]);
           double [] Back_consq  = Result.RulesDatabaseSet[0].all_conq_of_rules;
            Result.RulesDatabaseSet[0].all_conq_of_rules = New_consq;
            double result_after =  Result.approxLearnSamples(Result.RulesDatabaseSet[ 0]);
            if (result_before<result_after)
            { Result.RulesDatabaseSet[0].all_conq_of_rules = Back_consq;
            }
            GC.Collect();
            Result.RulesDatabaseSet[0].TermsSet.Trim();
            return Result;
          
                 }
        public override string ToString(bool with_param = false)
        {

            if (with_param)
            {
                string result = "Адаптивный МНК {";
                result += "}";
                return result;
            }
            return "Адаптивный МНК";
        }

        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new NullConfForAll();
            result.Init(CountFeatures);
            return result;
        }
    }
}

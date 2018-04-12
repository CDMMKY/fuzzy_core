using System;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;

namespace FuzzySystem.SingletoneApproximate.AddGenerators
{
    public class GeneratorRuleesSimpleRandom : AbstractNotSafeGenerator
    {
        TypeTermFuncEnum type_term ;
        int stable_terms = 0;
        int count_rules = 0;
        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.Singletone };
            }
        }

        public IFuzzySystem Generate(IFuzzySystem Approximate, IGeneratorConf config)
        {
            SAFuzzySystem toRunFuzzySystem = Approximate as SAFuzzySystem;
            return Generate(toRunFuzzySystem,config);
        }


        private TypeTermFuncEnum Generator_type_term()
        {
            int min = (int)TypeTermFuncEnum.Треугольник;
            int max = (int)TypeTermFuncEnum.Трапеция;
            return (TypeTermFuncEnum)(new Random()).Next(min, max);
        }

        public override SAFuzzySystem Generate(FuzzySystem.SingletoneApproximate.SAFuzzySystem Approximate, IGeneratorConf config)
        {
            Random rand = new Random();
            SAFuzzySystem result = Approximate;
            if (result.RulesDatabaseSet.Count == 0)
            {
                KnowlegeBaseSARules temp_rules = new KnowlegeBaseSARules();
                result.RulesDatabaseSet.Add(temp_rules);
            }

            type_term = ((GeneratorRullesSimpleRandomConfig)config).RSRTypeFunc;
            stable_terms = (int)((GeneratorRullesSimpleRandomConfig)config).RSRConstant;
            count_rules = ((GeneratorRullesSimpleRandomConfig)config).RSRCountRules;
            

            for (int j = 0; j < count_rules; j++)
            {
                int[] order = new int[result.CountFeatures];
                TypeTermFuncEnum temp_type_term;
                if (stable_terms == 0)
                {
                    temp_type_term = type_term;
                }
                else
                {
                    temp_type_term = Generator_type_term();
                }

                List< Term> temp_term_list = new List<Term>();
                for (int k = 0; k < result.CountFeatures; k++)
                {
                    double[] parametrs = new double[Term.CountParamsinSelectedTermType(temp_type_term)];
                
                    switch (temp_type_term)
                    {
                        case TypeTermFuncEnum.Треугольник:
                            parametrs[0] = result.LearnSamplesSet.InputAttributes[k].Min + rand.NextDouble() * (result.LearnSamplesSet.InputAttributes[k].Max - result.LearnSamplesSet.InputAttributes[k].Min);
                            parametrs[1] = result.LearnSamplesSet.InputAttributes[k].Min + rand.NextDouble() * (result.LearnSamplesSet.InputAttributes[k].Max - result.LearnSamplesSet.InputAttributes[k].Min);
                            parametrs[2] = result.LearnSamplesSet.InputAttributes[k].Min + rand.NextDouble() * (result.LearnSamplesSet.InputAttributes[k].Max - result.LearnSamplesSet.InputAttributes[k].Min);
                            Array.Sort(parametrs);
                            break;
                        case TypeTermFuncEnum.Гауссоида: parametrs[0] = result.LearnSamplesSet.InputAttributes[k].Min + rand.NextDouble() * (result.LearnSamplesSet.InputAttributes[k].Max - result.LearnSamplesSet.InputAttributes[k].Min);
                            parametrs[1] = (rand.NextDouble() + 0.01) * 0.5 *
                                           (result.LearnSamplesSet.InputAttributes[k].Max -
                                            result.LearnSamplesSet.InputAttributes[k].Min);
                            break;
                        case TypeTermFuncEnum.Парабола: parametrs[0] = result.LearnSamplesSet.InputAttributes[k].Min + rand.NextDouble() * (result.LearnSamplesSet.InputAttributes[k].Max - result.LearnSamplesSet.InputAttributes[k].Min);
                            parametrs[1] = result.LearnSamplesSet.InputAttributes[k].Min + rand.NextDouble() * (result.LearnSamplesSet.InputAttributes[k].Max - result.LearnSamplesSet.InputAttributes[k].Min);
                            Array.Sort(parametrs);
                            break;
                        case TypeTermFuncEnum.Трапеция: parametrs[0] = result.LearnSamplesSet.InputAttributes[k].Min + rand.NextDouble() * (result.LearnSamplesSet.InputAttributes[k].Max - result.LearnSamplesSet.InputAttributes[k].Min);
                            parametrs[1] = result.LearnSamplesSet.InputAttributes[k].Min + rand.NextDouble() * (result.LearnSamplesSet.InputAttributes[k].Max - result.LearnSamplesSet.InputAttributes[k].Min);
                            parametrs[2] = result.LearnSamplesSet.InputAttributes[k].Min + rand.NextDouble() * (result.LearnSamplesSet.InputAttributes[k].Max - result.LearnSamplesSet.InputAttributes[k].Min);
                            parametrs[3] = result.LearnSamplesSet.InputAttributes[k].Min + rand.NextDouble() * (result.LearnSamplesSet.InputAttributes[k].Max - result.LearnSamplesSet.InputAttributes[k].Min);

                            Array.Sort(parametrs);

                            break;

                    }
                    Term temp_term = new Term(parametrs, temp_type_term, k);
                    result.RulesDatabaseSet[0].TermsSet.Add(temp_term);
                    temp_term_list.Add(temp_term);
                    order[k] = result.RulesDatabaseSet[0].TermsSet.Count - 1;
                }
                double DoubleOutput = KNNConsequent.NearestApprox(result,temp_term_list);
                SARule temp_Rule = new SARule(result.RulesDatabaseSet[0].TermsSet,order,DoubleOutput);
                result.RulesDatabaseSet[0].RulesDatabase.Add(temp_Rule);
               
            }
            result.RulesDatabaseSet[0].TermsSet.Trim();


            result.UnlaidProtectionFix(result.RulesDatabaseSet[0]);

            GC.Collect();
            return result;
            
        }


        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Случайная генерация {";
                result += "Функции принадлежности= " + Term.ToStringTypeTerm(type_term) + " ;" + Environment.NewLine;
                result += "Генерируется правил =" + this.count_rules.ToString() + " ; "+Environment.NewLine;
                result += "Генерация правил случайной функции принадлежности =";
                if (stable_terms == 0) { result += "Нет"; }
                     else {result +="Да";}

                result += " ; " + Environment.NewLine;

                result += "}";
                return result;
            }
            return "Случайная генерация";
        }

        public override IGeneratorConf getConf(int CountFeatures)
        {
            IGeneratorConf result = new GeneratorRullesSimpleRandomConfig();
            result.Init(CountFeatures);
            return result;
        }

    }
}

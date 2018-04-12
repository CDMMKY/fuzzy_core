using System;
using System.Linq;
using FuzzySystem.FuzzyAbstract.conf;
using Linglib;
using FuzzySystem.FuzzyAbstract;
using System.Collections.Generic;

namespace FuzzySystem.SingletoneApproximate.AddGenerators
{

    public class GeneratorRulesEveryoneWithEveryone : AbstractNotSafeGenerator
    {
        private TypeTermFuncEnum type_func;
        int[] count_slice_vars = null;

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
            return Generate(toRunFuzzySystem, config);
        }

        public override SAFuzzySystem Generate(SAFuzzySystem Approximate, IGeneratorConf config)
        {
            SAFuzzySystem result = Approximate;
            InitEveryoneWithEveryone config1 = config as InitEveryoneWithEveryone;
            type_func = config1.IEWEFuncType;
            count_slice_vars = config1.IEWECountSlice;
            InitRulesEveryoneWithEveryone(Approximate, type_func, count_slice_vars);



            return result;
        }


        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Перебор с равномерным разбиением {";
                result += "Функции принадлежности= " + Term.ToStringTypeTerm(type_func) + " ;" + Environment.NewLine;
                for (int i = 0; i < count_slice_vars.Count(); i++)
                {
                    result += " " + count_slice_vars[i].ToString() + " " + pluralform.nobot(count_slice_vars[i], new string[3] { "терм", "терма", "термов" }) + " по " + (i + 1).ToString() + " " + " параметру ;" + Environment.NewLine;

                }
                result += "}";
                return result;
            }
            return "Перебор с равномерным разбиением";
        }

        public override IGeneratorConf getConf(int CountFeatures)
        {
            IGeneratorConf result = new InitEveryoneWithEveryone();
            result.Init(CountFeatures);
            return result;
        }


        public static void InitRulesEveryoneWithEveryone(SAFuzzySystem Approximate, TypeTermFuncEnum typeFunc, int[] countSliceForVar)
        {
            if ((Approximate.RulesDatabaseSet == null) || (Approximate.RulesDatabaseSet.Count == 0))
            {
                KnowlegeBaseSARules temp_rules = new KnowlegeBaseSARules();
                Approximate.RulesDatabaseSet.Add(temp_rules);
            }

            int[][] position_of_terms = new int[Approximate.CountFeatures][];
            for (int i = 0; i < Approximate.CountFeatures; i++)
            {

                if (Approximate.AcceptedFeatures[i] == false)
                {
                    countSliceForVar[i] = 0;
                    continue;
                }

                position_of_terms[i] = new int[countSliceForVar[i]];
                double current_value = Approximate.LearnSamplesSet.InputAttributes[i].Min;
                double coeef = (Approximate.LearnSamplesSet.InputAttributes[i].Scatter);

                if (countSliceForVar[i] > 1)
                {
                    coeef = coeef / (countSliceForVar[i] - 1);
                }
                if (countSliceForVar[i] <= 1)
                {
                    current_value = current_value + coeef * 0.5;
                    coeef *= 1.000000001 / 2;
                }
                for (int j = 0; j < countSliceForVar[i]; j++)
                {
                    double[] parametrs = new double[Term.CountParamsinSelectedTermType(typeFunc)];
                    switch (typeFunc)
                    {
                        case TypeTermFuncEnum.Треугольник:
                            parametrs[1] = current_value;
                            parametrs[0] = parametrs[1] - coeef;
                            parametrs[2] = parametrs[1] + coeef;
                            break;
                        case TypeTermFuncEnum.Гауссоида:
                            parametrs[0] = current_value;
                            parametrs[1] = coeef / 3;
                            break;
                        case TypeTermFuncEnum.Парабола:
                            parametrs[0] = current_value - coeef;
                            parametrs[1] = current_value + coeef;
                            break;
                        case TypeTermFuncEnum.Трапеция:
                            parametrs[0] = current_value - coeef;
                            parametrs[3] = current_value + coeef;
                            parametrs[1] = parametrs[0] + 0.4 * (parametrs[3] - parametrs[0]);
                            parametrs[2] = parametrs[0] + 0.6 * (parametrs[3] - parametrs[0]);
                            break;
                    }
                    Term temp_term = new Term(parametrs, typeFunc, i);
                    if (countSliceForVar[i] > 1)
                    {

                        if ((j == 0) && (typeFunc != TypeTermFuncEnum.Гауссоида))
                        {
                            temp_term.Min -= 0.00000001 * (temp_term.Max - temp_term.Min);
                        }
                        if ((j == countSliceForVar[i] - 1) && (typeFunc != TypeTermFuncEnum.Гауссоида))
                        {
                            temp_term.Max += 0.0000001 * (temp_term.Max - temp_term.Min);
                        }
                    }
                    Approximate.RulesDatabaseSet[0].TermsSet.Add(temp_term);
                    position_of_terms[i][j] = Approximate.RulesDatabaseSet[0].TermsSet.Count - 1;
                    current_value += coeef;
                }
            }
            int first_notNull = -1;
            int[] counter = new int[Approximate.CountFeatures];
            for (int i = 0; i < Approximate.CountFeatures; i++)
            {

                if (Approximate.AcceptedFeatures[i] == false)
                { continue; }

                counter[i] = countSliceForVar[i] - 1;
                if ((counter[i] != -1) && first_notNull == -1)
                {
                    first_notNull = i;
                }
            }
            while (counter[first_notNull] >= 0 && counter[0] >= -1)
            {
                List<Term> temp_term_set = new List<Term>();
                int[] order = new int[Approximate.CountFeatures];
                for (int i = 0; i < Approximate.CountFeatures; i++)
                {

                    if ((counter[i] == -1) || (Approximate.AcceptedFeatures[i] == false))
                    {
                        order[i] = -1;
                        continue;
                    }

                    temp_term_set.Add(Approximate.RulesDatabaseSet[0].TermsSet[position_of_terms[i][counter[i]]]);
                    order[i] = position_of_terms[i][counter[i]];
                }
                double approx_Values = KNNConsequent.NearestApprox(Approximate, temp_term_set);

                SARule temp_rule = new SARule(Approximate.RulesDatabaseSet[0].TermsSet, order, approx_Values);
                Approximate.RulesDatabaseSet[0].RulesDatabase.Add(temp_rule);


                counter = dec_count(counter, countSliceForVar, Approximate.CountFeatures);
            }
            Approximate.RulesDatabaseSet[0].TermsSet.Trim();
        }

        protected static int[] dec_count(int[] counter, int[] slice_count, int CountVars)
        {
            int[] result = counter;

            int j = CountVars - 1;
            result[j]--;
            while ((result[j] < 0) && (j > 0))
            {
                result[j] = slice_count[j] - 1;
                j--;
                result[j]--;
            }
            return result;
        }

    }
}

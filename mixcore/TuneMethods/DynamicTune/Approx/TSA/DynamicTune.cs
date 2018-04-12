
namespace DynamicTune
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FuzzySystem.FuzzyAbstract;
    
    using FuzzySystem.FuzzyAbstract.conf;
    using FuzzySystem.TakagiSugenoApproximate;


    public static class CartesianProduct
    {
        public static IEnumerable<IEnumerable<T>> Get<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
            IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
            return sequences.Aggregate(
                emptyProduct,
                (accumulator, sequence) =>
                    from accseq in accumulator
                    from item in sequence
                    select accseq.Concat(new[] { item }));
        }
    }

    public class DynamicTuneClass:AbstractNotSafeLearnAlgorithm
    {
        public double maxError;
        public int RuleCount;
        public int TryCount;
        TSAFuzzySystem result;
  
        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>
                           {
                               FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate
                           };
            }
        }





     
        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string res = "Динамическое разбиение{";
                res += "Максимальная ошибка: " + maxError +Environment.NewLine;
                res += "Число правил: " + RuleCount + Environment.NewLine;
                res += "Число попыток: " + TryCount + Environment.NewLine;

                for (int i = 0; i < result.CountFeatures; i++)
                {
                    int tmp = result.RulesDatabaseSet[0].TermsSet.Count(x => x.NumVar == i);
                    res += tmp + " термов по " + i + "-му параметру" + Environment.NewLine;

                }
                // result+= param1+Environment.NewLine;
                // result+= param1+Environment.NewLine;
                res += "}";
                return res;
            }
            return "Динамическое разбиение";
        }

        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new DynamicTuneConf();
            result.Init(CountFeatures);
            return result;
        }


        public override TSAFuzzySystem TuneUpFuzzySystem(TSAFuzzySystem Approximate, ILearnAlgorithmConf conf) // + override
        {
            result = Approximate;


            List<KnowlegeBaseTSARules> Archive = new List<KnowlegeBaseTSARules>();
            List<double> ErrorsArchive = new List<double>();

            var config = (DynamicTuneConf)conf;
            maxError = config.MaxError;
            RuleCount = config.RulesCount;
            TryCount = config.TryCount;
            double error = result.RMSEtoMSEdiv2forLearn(result.approxLearnSamples(result.RulesDatabaseSet[0]));
            var kbToOptimize = new KnowlegeBaseTSARules(result.RulesDatabaseSet[0]);
            var kbBest = new KnowlegeBaseTSARules(kbToOptimize);
            double errorBefore = Double.MaxValue;

            result.UnlaidProtectionFix(kbToOptimize);

            List<input_space> variable_spaces = new List<input_space>();

            for (int i = 0; i < result.LearnSamplesSet.InputAttributes.Count; i++)
            {
                List<Term> terms_of_variable = new List<Term>();
                terms_of_variable = kbToOptimize.TermsSet.Where(term => term.NumVar == i).ToList();
                variable_spaces.Add(new input_space(terms_of_variable, i));
            }

            int indexRegion = -1,
                indexVar = -1,
               number_of_input_variables = variable_spaces.Count;

            int tryCount = 0;



            while (error > maxError)
            {
                if (Double.IsInfinity(error))
                {
                    throw new Exception("Something went wrong, error is Infinity, region: " + indexRegion);
                }
                if (Double.IsNaN(error))
                {
                    throw new Exception("Something went wrong, error is NaN, region: " + indexRegion);
                }

                region_side[][] sides = new region_side[number_of_input_variables][];
                for (int i = 0; i < number_of_input_variables; i++)
                {
                    sides[i] = variable_spaces[i].get_region_sides();
                }
                var cartresult = CartesianProduct.Get(sides);

                List<region2> regions = new List<region2>();

                foreach (var x in cartresult)
                {
                    regions.Add(new region2(x.ToList(), result, variable_spaces));
                }

                List<double> region_errors = regions.Select(x => x.region_error()).ToList();
                indexRegion = region_errors.IndexOf(region_errors.Max());

                for (int i = 0; i < region_errors.Count; i++)
                {
                    if (Double.IsNaN(region_errors[i]) || Double.IsInfinity(region_errors[i]) ||
                        Double.IsNegativeInfinity(region_errors[i]) || Double.IsPositiveInfinity(region_errors[i]))
                    {
                        region_errors[i] = 0;
                    }
                }

                List<double> variable_errors = regions[indexRegion].variable_errors();
                bool check1 = false;
                for (int i = 1; i < variable_errors.Count; i++)
                {
                    if (variable_errors[i - 1] != variable_errors[i])
                    {
                        check1 = true;
                        break;
                    }
                }
                if (!check1) { indexVar = StaticRandom.Next(variable_errors.Count - 1); }
                else
                {
                    indexVar = variable_errors.IndexOf(variable_errors.Max());
                }

                Term new_term = regions[indexRegion].new_term(indexVar);
                result.RulesDatabaseSet[0] = kbToOptimize;
                kbToOptimize.TermsSet.Add(new_term);

                // Rules (CHECK REFERENCE TYPES)
                int @var = indexVar;

                var rulesLeft = kbToOptimize.RulesDatabase.Where(
                    rule => rule.ListTermsInRule.Contains(regions[indexRegion].sides[indexVar].left)).ToList();
                var rulesRight = kbToOptimize.RulesDatabase.Where(
                    rule => rule.ListTermsInRule.Contains(regions[indexRegion].sides[indexVar].right)).ToList();
                for (int j = 0; j < rulesLeft.Count; j++)
                {
                    int[] order = new int[rulesLeft[j].ListTermsInRule.Count];
                    for (int k = 0; k < rulesLeft[j].ListTermsInRule.Count; k++)
                    {
                        Term temp_term = rulesLeft[j].ListTermsInRule[k];
                        if (temp_term == regions[indexRegion].sides[indexVar].left)
                        {
                            temp_term = new_term;
                        }
                        order[k] = kbToOptimize.TermsSet.FindIndex(x => x == temp_term);
                    }
                    double temp_approx_Values = kbToOptimize.RulesDatabase[j].IndependentConstantConsequent;
                    double[] temp_approx_RegressionConstantConsequent =
                        kbToOptimize.RulesDatabase[j].RegressionConstantConsequent.Clone() as double[];
                    TSARule temp_rule = new TSARule(
                        kbToOptimize.TermsSet, order, temp_approx_Values, temp_approx_RegressionConstantConsequent);

                    double[] dC = null;
                    temp_rule.IndependentConstantConsequent = LSMWeghtReqursiveSimple .EvaluteConsiquent(
                        result, temp_rule.ListTermsInRule.ToList(), out dC);
                    temp_rule.RegressionConstantConsequent = (double[])dC.Clone();

                    kbToOptimize.RulesDatabase.Add(temp_rule);



                    rulesLeft[j].IndependentConstantConsequent = LSMWeghtReqursiveSimple.EvaluteConsiquent(
                        result, rulesLeft[j].ListTermsInRule.ToList(), out dC);
                    rulesLeft[j].RegressionConstantConsequent = (double[])dC.Clone();
                }

                foreach (var rule in rulesRight)
                {
                    double[] dC = null;
                    rule.IndependentConstantConsequent = LSMWeghtReqursiveSimple.EvaluteConsiquent(
                        result, rule.ListTermsInRule.ToList(), out dC);
                    rule.RegressionConstantConsequent = dC;
                }

                variable_spaces[indexVar].terms.Add(new_term);
                variable_spaces[indexVar].terms.Sort(new CompararerByPick());

                // Re-evaluate the system's error
                error = result.RMSEtoMSEdiv2forLearn(result.ErrorLearnSamples(kbToOptimize));

                if ((kbToOptimize.RulesDatabase.Count > config.RulesCount))
                { break; }

#if Console
                Console.WriteLine(error + " " + kbToOptimize.TermsSet.Count + " terms\n");
                for (int i = 0; i < variable_spaces.Count; i++)
                {
                    Console.WriteLine(variable_spaces[i].terms.Count + " термов по " + i + "му параметру\n");
                }
#endif
                result.RulesDatabaseSet[0] = kbToOptimize;
                // Get the best knowledge base on the 1st place
                if (error < errorBefore)
                {
                    kbBest = new KnowlegeBaseTSARules(kbToOptimize);
                    errorBefore = error;
                    tryCount = 0;
                }
                else
                {
                    tryCount++;
                }
                if (tryCount > TryCount) { break; }

            }


            result.RulesDatabaseSet[0] = kbBest;
            RuleCount = kbBest.RulesDatabase.Count;
            TryCount = tryCount;

            return result;

        }










        public class region_side
        {
            public Term left, right;
            public int variable_index;
            public double L, l;
            public bool one_term = false;

            public region_side(Term left, Term right, int index, double L)
            {
                this.left = left;
                this.right = right;
                variable_index = index;
                this.L = L;
                l = Math.Abs(left.Pick - right.Pick);
            }
            public region_side(Term left, int index, double L)
            {
                this.left = left;
                this.one_term = true;
                variable_index = index;
                this.L = L;
                l = Math.Abs(left.Parametrs[2] - left.Parametrs[0]);
            }

        }

        public class region2
        {
            TSAFuzzySystem approx;
            List<input_space> variable_spaces;
            public List<region_side> sides;
            List<SampleSet.RowSample> dots;

            public region2(List<region_side> sides, TSAFuzzySystem approx, List<input_space> variable_spaces)
            {
                this.sides = new List<region_side>(sides.Count);
                this.sides.AddRange(sides);
                this.approx = approx;
                get_dots(approx.LearnSamplesSet.DataRows);
                this.variable_spaces = variable_spaces;
            }

            void get_dots(List<SampleSet.RowSample> data)
            {
                dots = new List<SampleSet.RowSample>();
                foreach (SampleSet.RowSample x in data)
                {
                    bool accepted = true;
                    for (int i = 0; i < x.InputAttributeValue.Count(); i++)
                    {
                        if (sides[i].one_term)
                        {
                            if (x.InputAttributeValue[i] > sides[i].left.Parametrs[2] ||
                                x.InputAttributeValue[i] < sides[i].left.Parametrs[0])
                            {
                                accepted = false;
                                break;
                            }
                        }
                        else
                        {
                            if (x.InputAttributeValue[i] > sides[i].right.Pick || x.InputAttributeValue[i] < sides[i].left.Pick)
                            {
                                accepted = false;
                                break;
                            }
                        }
                    }
                    if (accepted) { dots.Add(x); }
                }
            }

            double PointYDifference(SampleSet.RowSample x)
            {
                return Math.Abs(x.DoubleOutput - approx.approx_base(x.InputAttributeValue, approx.RulesDatabaseSet[0]));
            }

            public double dot_count()
            {
                return Convert.ToDouble(dots.Count);
            }

            double error_base()
            {
                if (dots.Count == 0)
                {
                    return 0;
                }
                else
                {
                    return dots.Select(x => PointYDifference(x)).ToList().Sum() / dot_count();
                }
            }

            public Term new_term(int indexVar)
            {
                double a = dots.Select(x => PointYDifference(x) * x.InputAttributeValue[indexVar]).ToList().Sum(),
                       b = dots.Select(x => PointYDifference(x)).ToList().Sum();
                double pick = a / b, left, right;
                if (sides[indexVar].one_term)
                {
                    left = sides[indexVar].left.Parametrs[0];
                    right = sides[indexVar].left.Parametrs[2];
                }
                else
                {
                    left = sides[indexVar].left.Pick;
                    right = sides[indexVar].right.Pick;
                }
                var test = variable_spaces[indexVar].terms.Select(x => x.Pick);
                if (pick > approx.LearnSamplesSet.InputAttributes[indexVar].Max ||
                    pick < approx.LearnSamplesSet.InputAttributes[indexVar].Min ||
                    (variable_spaces[indexVar].terms.Select(x => x.Parametrs[1]).Contains(pick)))
                {
                    pick = StaticRandom.NextDouble(left, right);
                }
                double[] parameters = new double[3] { left, pick, right };
                Term result = new Term(parameters, sides[indexVar].left.TermFuncType, indexVar);
                return result;
            }

            public double region_error()
            {
                double result = error_base();
                foreach (region_side x in sides) { result *= x.l / x.L; }
                return result;
            }

            public List<double> variable_errors()
            {
                double error = error_base();
                List<double> result = new List<double>();
                foreach (region_side x in sides) { result.Add(error * x.l / x.L); }
                return result;
            }

        }

        public class input_space
        {
            public List<Term> terms;
            int variable_index;

            public input_space(List<Term> terms_to_add, int index)
            {
                terms = terms_to_add;
                terms.Sort(new CompararerByPick());
                variable_index = index;
            }

            public region_side[] get_region_sides()
            {
                double L;
                List<region_side> result = new List<region_side>();
                if (terms.Count == 1)
                {
                    L = Math.Abs(terms[0].Parametrs[2] - terms[0].Parametrs[0]);
                    region_side[] testc = new region_side[] { new region_side(terms[0], variable_index, L) };
                    return testc;
                }
                else
                {
                    L = Math.Abs(terms.Last().Pick - terms[0].Pick);
                    for (int i = 0; i < terms.Count - 1; i++)
                    {
                        result.Add(new region_side(terms[i], terms[i + 1], variable_index, L));
                    }
                    return result.ToArray();
                }
            }
        }



    }
}

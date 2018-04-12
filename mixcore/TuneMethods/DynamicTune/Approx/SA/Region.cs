using System.Collections.Generic;

namespace DynamicTuneSA
{
    using System;
    using System.Linq;

    using FuzzySystem.FuzzyAbstract;
    using FuzzySystem.SingletoneApproximate;

    public class Region
    {
        #region Fields

        private List<Term> terms;

        private List<List<Term>> termsByVar;

        private List<double> scatters;

        private List<double> mins;

        private List<double> maxs;

        private double error;

        private SAFuzzySystem system;

        #endregion

        #region Private methods

        private bool InBetweenTheLimits(double[] array)
        {
            if (array.Length != maxs.Count)
            {
                throw new ArgumentException("Wrong size of input sample!\nRegion.cs");
            }

            return !array.Where((t, i) => ((termsByVar[i].Count > 1) && (t < mins[i] || t > maxs[i]))).Any();
        }

        private List<bool> GetRules(KnowlegeBaseSARules knowlegeBase)
        {
          
            List<bool> answer = new List<bool>();
            foreach (var rule in knowlegeBase.RulesDatabase)
            {
                bool tmp = true;
                foreach (var term in rule.ListTermsInRule)
                {
                    if (!terms.Contains(term))
                        tmp = false;
                }

                answer.Add(tmp);
            }

            return answer;
        }

        #endregion

        #region Constructors

        public Region()
        {
            terms = new List<Term>();
            termsByVar = new List<List<Term>>();
        }

        public Region(Region source)
        {
            terms = new List<Term>(source.terms);

            termsByVar = new List<List<Term>>();
            foreach (var termList in source.termsByVar)
            {
                termsByVar.Add(new List<Term>(termList));
            }
        }

        //public Region(TSAFuzzySystem approx, List<Term> initialTerms)
        //{
        //    // Terms
        //    terms = initialTerms;

        //    GetLimits(approx);

        //    GetDots(approx);
            
        //    EvaluateError(approx);
        //}

        #endregion

        #region Properties

        public double Error{ get{ return error; }}

        public List<double> Scatters { get { return scatters; }}

        public int CountSamples {get { return system.LearnSamplesSet.CountSamples; }}

        public List<Term> Terms
        {
            get { return terms; }
            set { terms = value; }
        }

        public List<SampleSet.RowSample> Dots{ get { return system.LearnSamplesSet.DataRows; } }

        #endregion

        #region Public methods

        public void GetLimits(SAFuzzySystem approx)
        {
            termsByVar = new List<List<Term>>(approx.CountFeatures);
            for (int i = 0; i < approx.CountFeatures; i++)
            {
                termsByVar.Add(terms.Where(term => term.NumVar == i).ToList());
            }
            
            mins = termsByVar.Select((termsVar, i) => 
                (termsVar.Count > 1) ? termsVar[0].Pick : 
                approx.LearnSamplesSet.InputAttributes[i].Min).ToList();
            maxs = termsByVar.Select((termsVar, i) =>
                (termsVar.Count > 1) ? termsVar.Last().Pick :
                approx.LearnSamplesSet.InputAttributes[i].Max).ToList();
            scatters = maxs.Select((max, i) => max - mins[i]).ToList();
        }

        public void GetDots(SAFuzzySystem approx, KnowlegeBaseSARules knowlegeBase)
        {
            // Dots (COMPLETE, BUT DOUBLECHECK WHEN DEBUGGING)
            var inputs = approx.LearnSamplesSet.DataRows.AsParallel().AsOrdered()
                .Select(dataRow => dataRow.InputAttributeValue).ToList();
            var localDots = inputs.AsParallel().AsOrdered()
                .Where(InBetweenTheLimits).ToList();
          //  var strs = new List<string[]>(localDots.Count);
          //  for (int i = 0; i < strs.Capacity; i++)
            //    strs.Add(new[] { string.Empty });
            
            var rezs = approx.LearnSamplesSet.DataRows.AsParallel().AsOrdered()
                .Where(row => localDots.Contains(row.InputAttributeValue))
                    .Select(dataRow => dataRow.DoubleOutput).ToList();
            
            List<SampleSet.RowSample> rows = localDots.Select((t, i) => new SampleSet.RowSample(t, null , rezs[i],null)).ToList();

            var samples = new SampleSet("1.dat", rows, approx.LearnSamplesSet.InputAttributes, approx.LearnSamplesSet.OutputAttribute);

            system = new SAFuzzySystem(samples, samples);

            var usedRules = GetRules(knowlegeBase);

            system.RulesDatabaseSet.Add(new KnowlegeBaseSARules(knowlegeBase, null));
        }
        
        public void EvaluateError()
        {
          //  error = system.RMSEtoMSEdiv2forLearn(system.approxLearnSamples(system.RulesDatabaseSet[0]));
            error = 0;
            if (Dots.Count > 0)
            {
                foreach (var dot in Dots)
                {
                    error += PointYDifference(dot, system);
                }
                error /= Dots.Count;
            }
        }



        public double PointYDifference(SampleSet.RowSample Point, SAFuzzySystem Approx)
        {
            return Math.Abs(Point.DoubleOutput - Approx.approx_base(Point.InputAttributeValue, Approx.RulesDatabaseSet[0]));
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm.Term_config_AcoD
{
    public class Decision
    {

        #region Constructor & initialization

        public Decision(int position)
        {
            Position = position;
        }
        #endregion Constructor & initialization

        #region Properties

        public int Position { get; protected set; }
        public double Precision { get; protected set; }
        public double Weight { get; protected set; }
        public bool Value { get; protected set; }

        private bool backup;

        #endregion Properties

        public void setWeight(double weight)
        {
            Weight = weight;
        }

        public void setValue(bool value)
        {
            Value = value;
        }

        public virtual void CalcPecission(PCFuzzySystem FS)
        {
            backup = FS.AcceptedFeatures[Position];
            FS.AcceptedFeatures[Position] = Value;
            try
            {
                Precision = FS.ClassifyLearnSamples(FS.RulesDatabaseSet[0]);
            }
            catch (Exception)
            {
                Precision = double.PositiveInfinity;
            }
            FS.AcceptedFeatures[Position] = backup;
        }

        public void invert()
        {
            Value = !Value;
        }

        public void CalcWeigths(double q, int decisionArchiveCount, int l)
        {
            double pow_numenator = (-1 * Math.Pow(((l + 1) - 1), 2));
            double pow_denumenator = (2 * Math.Pow(q, 2) * Math.Pow(decisionArchiveCount, 2));
            double numenator = Math.Pow(Math.E, pow_numenator / pow_denumenator);
            double denumenator = (q * decisionArchiveCount * Math.Sqrt(2 * Math.PI));
            Weight = numenator / denumenator;

        }

        public void updateDecision(Decision decision)
        {
            Value = decision.Value;
            Precision = decision.Precision;
        }

        public void updateFS(PCFuzzySystem FS)
        {
            backup = FS.AcceptedFeatures[Position];
            FS.AcceptedFeatures[Position] = Value;
            try
            {
                Precision = FS.ClassifyLearnSamples( FS.RulesDatabaseSet[0]);
            }
            catch (Exception)
            {
                FS.AcceptedFeatures[Position] = backup;
            }
        }



        internal class DecisionComparer : IComparer<Decision>
        {
            public int Compare(Decision decision1, Decision decision2)
            {
                if (decision1.Precision > decision2.Precision)
                    return -1;
                if (decision2.Precision > decision1.Precision)
                    return 1;
                return 0;
            }
        }
    }
}
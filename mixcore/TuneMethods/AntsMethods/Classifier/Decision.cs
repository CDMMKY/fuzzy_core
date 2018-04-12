using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySystem.FuzzyAbstract;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm.Term_config_Aco
{
    public class Decision
    {
        protected Base_ACO parrent;

            #region Constructor & initialization

        public Decision( Base_ACO algorithm)
        {
            TermOrWeghtClassParams = new List<double>();
            parrent = algorithm;
        }
            #endregion Constructor & initialization

            #region Properties

            public double Precision { get; set; }
            public double Weight { get; set; }
            public List<double> TermOrWeghtClassParams { get; set; }

        private double[] backup;

            #endregion Properties

        public void FillRandom(double [] Params, Random rand, bool is_rand=true)
        {
            
                for (int i = 0; i < Params.Count(); i++)
                {
                    if (is_rand)
                    {
                        TermOrWeghtClassParams.Add(Base_ACO.BoxMullerTransform(0.1 * Params[i], Params[i],rand));
                    }
                    else {
                        TermOrWeghtClassParams.Add(Params[i]);
                    }
                }
        }

        public virtual void CalcPecission(Term Term)
        {
            setDecision(Term);
            Precision = parrent.getPrecission();
            backupDecision(Term);
        }

        public void CalcWeigths(double q, int decisionArchiveCount, int l)
        {
            double pow_numenator =(-1 * Math.Pow(((l+1) - 1),2));
            double pow_denumenator = (2 * Math.Pow(q,2) * Math.Pow(decisionArchiveCount,2));
            double numenator = Math.Pow(Math.E,  pow_numenator /pow_denumenator ) ;
            double denumenator = (q * decisionArchiveCount * Math.Sqrt(2 * Math.PI));

            Weight = numenator / denumenator ;
          
        }

        public void updateDecision(Decision decision)
        { 


            TermOrWeghtClassParams.Clear();
            TermOrWeghtClassParams.AddRange(decision.TermOrWeghtClassParams);
            Precision = decision.Precision;
            
        }

        public void updateTermByDecision(Term Term )
        {
            setDecision(Term);
        }


        protected void setDecision(Term Term)
        {
            backup = Term.Parametrs.Clone() as double [];
            for (int i = 0; i < Term.Parametrs.Count(); i++)
            {
                Term.Parametrs[i] = TermOrWeghtClassParams[i];
            }
        }

        protected void backupDecision(Term Term)
        {
            for (int i = 0; i < Term.Parametrs.Count(); i++)
            {
                Term.Parametrs[i] = backup[i];
            }
            backup = null;
            
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
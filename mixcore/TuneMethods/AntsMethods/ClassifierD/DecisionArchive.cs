using System;
using System.Collections.Generic;
using System.Linq;


namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm.Term_config_AcoD
{
    public class DecisionArchive
    {
        protected List<Decision> the_decisionArchive = new List<Decision>();

        public readonly int Count;
        protected double[] Weigths_backup;
        protected double[] propability;
        protected int Position;

        public double[] Propability { get { return propability; } }



        protected void save_Weigths()
        {
            for (int i = 0; i < Count; i++)
            {
                Weigths_backup[i] = the_decisionArchive[i].Weight;
            }
        }

        protected void calcPropability()
        {
            double denominator = Weigths_backup.Sum();
            propability = Weigths_backup.Select(weight => weight / denominator).ToArray();

        }
        public Decision this[int index]
        {
            get { return the_decisionArchive[index]; }
        }

        protected void load_Weigths()
        {
            for (int i = 0; i < Count; i++)
            {
                the_decisionArchive[i].setWeight(Weigths_backup[i]);
            }
        }

        public DecisionArchive(int decisionArchiveCount, int position)
        {
            Position = position;
            Count = decisionArchiveCount;
            for (int i = 0; i < decisionArchiveCount; i++)
            {
                Decision the_decision = new Decision(Position);
                the_decisionArchive.Add(the_decision);
            }
            Weigths_backup = new double[Count];
        }

        public void Add(Decision newElem)
        {
            the_decisionArchive.Add(newElem);
        }

        public virtual void FillRandom(PCFuzzySystem FS, bool Various, Random rand)
        {
            if (Count < 2)
            {
                the_decisionArchive[0].setValue(rand.NextDouble() >= 0.5);

                the_decisionArchive[0].CalcPecission(FS);
            }
            else
            {
                the_decisionArchive[0].setValue(false);

                the_decisionArchive[0].CalcPecission(FS);
                the_decisionArchive[1].setValue(true);

                the_decisionArchive[1].CalcPecission(FS);
            }


            the_decisionArchive.Sort(new Decision.DecisionComparer());


        }

        public void calcDecisionWeight(double q)
        {
            for (int i = 0; i < Count; i++)
            {
                the_decisionArchive[i].CalcWeigths(q, Count, i);
            }
            save_Weigths();
            calcPropability();
        }


        public void update(Decision newdecision)
        {
            the_decisionArchive.Add(newdecision);
            the_decisionArchive.Sort(new Decision.DecisionComparer());
            the_decisionArchive.RemoveAt(the_decisionArchive.Count - 1);
            load_Weigths();
        }



        public Decision getBestDecision()
        {
            the_decisionArchive.Sort(new Decision.DecisionComparer());
            if (the_decisionArchive.Count > 0)
            {
                return the_decisionArchive[0];
            }
            return null;
        }

        public List<Decision> getEliteDicision(int CountEliteDecision, Random rand)
        {
            the_decisionArchive.Sort(new Decision.DecisionComparer());
            int maxindex = (int)(CountEliteDecision * 1.5);

            maxindex = maxindex > the_decisionArchive.Count ? (the_decisionArchive.Count - 1) : maxindex;
            List<Decision> result = new List<Decision>();
            for (int i = 0; i < CountEliteDecision; i++)
            {
                result.Add(the_decisionArchive[rand.Next(maxindex)]);
            }
            return result;
        }

    }
}
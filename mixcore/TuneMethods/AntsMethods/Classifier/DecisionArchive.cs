using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySystem.FuzzyAbstract;


namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm.Term_config_Aco
{
   public class DecisionArchive
    {
       protected List<Decision> the_decisionArchive= new List<Decision>();

        public readonly int Count;
        protected double[] Weigths_backup;
        protected double[] propability;

        public double [] Propability{get {return propability;}}

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
         public Decision this [int index] {
         get {return the_decisionArchive[index];}
         }

        protected void load_Weigths()
        {
            for (int i = 0; i < Count; i++)
            {
               the_decisionArchive[i].Weight= Weigths_backup[i];
            }
  
        }

        public DecisionArchive(int decisionArchiveCount, Base_ACO algorithm)
        {
            Count = decisionArchiveCount;
            for (int i = 0; i < decisionArchiveCount; i++)
            {
                Decision the_decision = new Decision(algorithm);
                the_decisionArchive.Add(the_decision);
            }
            Weigths_backup = new double[Count];
        }

        public void Add(Decision newElem)
        {
            the_decisionArchive.Add(newElem);
        }

        public virtual void FillRandom(Term Term, Random rand)
        {
            for (int i = 0; i < Count; i++)
            {
                the_decisionArchive[i].FillRandom(Term.Parametrs, rand, i != 0);

                the_decisionArchive[i].CalcPecission(Term);

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
            int maxindex =(int) (CountEliteDecision*1.5);

            maxindex = maxindex>the_decisionArchive.Count?(the_decisionArchive.Count-1): maxindex;
            List<Decision> result = new List<Decision>();
            for (int i =0; i < CountEliteDecision ; i++)
            {
                result.Add(the_decisionArchive[rand.Next(maxindex)]);
            }
            return result;
        }

        public  void refillArchive(Decision bestDecision, List<Decision> EliteDecision, Base_ACO algorithm, Term Term, Random rand)
        {
            int size = the_decisionArchive.Count;
            the_decisionArchive.Clear();
            the_decisionArchive.Add(bestDecision);
            the_decisionArchive.AddRange(EliteDecision);
            int countRandomSolution = size - EliteDecision.Count-1; 
            for (int i = 0; i <countRandomSolution ; i++)
            {
                Decision the_decision = new Decision(algorithm);
                the_decisionArchive.Add(the_decision);
            
                the_decisionArchive[the_decisionArchive.Count-1].FillRandom(Term.Parametrs, rand, true);

                the_decisionArchive[the_decisionArchive.Count - 1].CalcPecission(Term);

            }
            the_decisionArchive.Sort(new Decision.DecisionComparer());
        }
    }
}
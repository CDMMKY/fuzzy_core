using System;
using System.Linq;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm.Term_config_AcoD
{
    public class Ant
    {
        public int l { get; set; }
        public int position { get; set; }
        public Decision decision { get; set; }

        public Ant(int position)
        {
           this.position = position;
        }

        

        private int getOneOfArchive(double[] propability, Random rand)
        {
            double chance = rand.NextDouble();
            double end = 0;
            double start = end;
            for (int i = 0; i < propability.Count(); i++)
            {
                end += propability[i];
                if ((chance >= start) && (chance < end))
                {
                    return i;
                }
                start = end;
            }
            return 0;
        }

        private double calcSigma(DecisionArchive archive, double xi)
        {
            double  sigma =0;
                     for(int i =0; i< archive.Count;i++){
                  int differ = 0;
                    if (archive[i].Value != archive[l].Value) { differ = 1; }
                        sigma = (differ) / archive.Count;
                       }
                sigma *= xi;
            return sigma;
        }
        public void getL(DecisionArchive archive, Random rand)
        {
            double[] propability = archive.Propability;
            l = getOneOfArchive(propability, rand);
        }

        public void makeNewDecision( DecisionArchive archive,Random rand, double xi)
      {
          double sigma = calcSigma(archive,xi);
      
            decision = new Decision(position);
            decision.updateDecision(archive[l]);
           
            if (rand.NextDouble() > sigma) { decision.invert(); }
            
     }

        public double checkDesicision(PCFuzzySystem FS, double basePrecission )
        {

            decision.CalcPecission(FS);
            if (basePrecission < decision.Precision)
            {
                decision.updateFS(FS);
                return decision.Precision;
            }

            return basePrecission;
        }

    }

}

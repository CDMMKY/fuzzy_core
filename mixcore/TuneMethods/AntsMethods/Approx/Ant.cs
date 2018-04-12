using System;
using System.Linq;
using FuzzySystem.FuzzyAbstract;

namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm.Term_config_Aco
{
    public class Ant
    {


        public int l { get; set; }
//        public int Order { get; set; }
        // public Ant NextAnt { get; set; }
        public Decision decision { get; set; }

      
       

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

        private double[] calcSigma(DecisionArchive archive, double xi)
        {
            double [] sigma = new double [archive[l].TermOrWeghtClassParams.Count];
 
              for (int j=0;j< archive[l].TermOrWeghtClassParams.Count;j++)
               {sigma[j]=0;
                     for(int i =0; i< archive.Count;i++){
                        sigma[j] += (Math.Abs(archive[i].TermOrWeghtClassParams[j] - archive[l].TermOrWeghtClassParams[j])) / archive.Count;
                       }
                sigma[j] *= xi;

                }
            return sigma;
        }
        public void getL(DecisionArchive archive, Random rand)
        {
            double[] propability = archive.Propability;
            l = getOneOfArchive(propability, rand);
        }

        public void makeNewDecision(Base_ACO algorithm , DecisionArchive archive,Random rand, double xi)
      {
          double [] sigma = calcSigma(archive,xi);
      
            decision = new Decision(algorithm);
            decision.TermOrWeghtClassParams.AddRange(archive[l].TermOrWeghtClassParams);
            for (int j=0;j<archive[l].TermOrWeghtClassParams.Count;j++)
            { decision.TermOrWeghtClassParams[j] = Base_ACO.BoxMullerTransform(sigma[j],decision.TermOrWeghtClassParams[j],rand); 
            }
     }

        public double checkDesicision(Term Term, double basePrecission )
        {

            decision.CalcPecission(Term);
            if (basePrecission > decision.Precision)
            {
                decision.updateTermByDecision(Term);
                return decision.Precision;
            }

            return basePrecission;
        }

    }

}

using System;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract;

namespace FuzzySystem.TakagiSugenoApproximate.LearnAlgorithm.Term_config_Aco
{
    public class Colony
    {   protected List<Ant> theAnts= new List<Ant>();
        protected DecisionArchive theArchive;
        protected Term theTerm;
       protected Base_ACO parrent;
       
        
        #region Constructor
       
       public Colony(int antCount, int decisionArchiveCount, int numTerm, Term Term, Base_ACO algorithm)
        {
            for (int i = 0; i < antCount; i++)
            {
                Ant the_Ant = new Ant();
                theAnts.Add(the_Ant);
            }
            theArchive = new DecisionArchive(decisionArchiveCount, algorithm);
            theTerm = Term;
            parrent = algorithm;
       }

  



       public void FillRandomArchiveDecision( Random rand)
       { 
           theArchive.FillRandom(theTerm, rand );
       }

       public void FillDecisionArchiveWeight(double q)
       {
           theArchive.calcDecisionWeight(q);
       }

       public void runAnt(int num_ant, Random rand, double xi)
       {
           theAnts[num_ant].getL(theArchive,rand);
          theAnts[num_ant].makeNewDecision(parrent,theArchive,rand,xi);

       }

       public double checkAntDecision(int num_ant,  double basePrecission)
       {
           double resultPrecission = theAnts[num_ant].checkDesicision(theTerm, basePrecission);
           return resultPrecission;
       }

       public void updateDecisionArchive(int num_ant)
       {
           theArchive.update(theAnts[num_ant].decision); 
       }


       public void refillDesicionArchive(int CountEliteDecision, Random rand, Base_ACO algorithm)
       {
           Decision bestDecision = theArchive.getBestDecision();
           List<Decision> EliteDecision = theArchive.getEliteDicision(CountEliteDecision, rand);
           theArchive.refillArchive(bestDecision,EliteDecision,algorithm,theTerm,rand);
       }

       public DecisionArchive Archive
       {
           get { return theArchive; }
       }

        #endregion 
    }
}

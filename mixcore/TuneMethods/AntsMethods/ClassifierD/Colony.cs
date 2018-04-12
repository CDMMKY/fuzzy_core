using System;
using System.Collections.Generic;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm.Term_config_AcoD
{
    public class Colony
    {   protected List<Ant> theAnts= new List<Ant>();
        protected DecisionArchive theArchive;
        protected int Position;
        protected bool Value;


        #region Constructor

        public Colony(int antCount, int decisionArchiveCount, int numPosition, bool Various)
        {
            Position = numPosition;
            for (int i = 0; i < antCount; i++)
            {
                Ant the_Ant = new Ant(Position);
                theAnts.Add(the_Ant);
            }
            theArchive = new DecisionArchive(decisionArchiveCount, Position);

            Value = Various;
         
       }

  



       public void FillRandomArchiveDecision(PCFuzzySystem FS, Random rand)
       { 
           theArchive.FillRandom(FS, Value, rand );
       }

       public void FillDecisionArchiveWeight(double q)
       {
           theArchive.calcDecisionWeight(q);
       }

       public void runAnt(int num_ant, Random rand, double xi)
       {
           theAnts[num_ant].getL(theArchive,rand);
          theAnts[num_ant].makeNewDecision(theArchive,rand,xi);

       }

       public double checkAntDecision(PCFuzzySystem FS, int num_ant,  double basePrecission)
       {
           double resultPrecission = theAnts[num_ant].checkDesicision(FS, basePrecission);
           return resultPrecission;
       }

       public void updateDecisionArchive(int num_ant)
       {
           theArchive.update(theAnts[num_ant].decision); 
       }
  
        #endregion 
    }
}

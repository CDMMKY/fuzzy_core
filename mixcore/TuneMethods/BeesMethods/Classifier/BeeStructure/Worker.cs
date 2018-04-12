/*
using System;
using System.Linq;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm.Bee
{
    public class Worker:Bee
    {


       public  Worker(KnowlegeBasePCRules theSource, BeeStructureAlgorithm parrent)
            : base(theSource, parrent)
        { 
        }

        public void WorkerFly( int numOfRule, Random rand)
        {
            PCRule optimised = thePositionOfBee.RulesDatabase[numOfRule];
            double coust = 0;
        

            
            for (int i = 0; i < optimised.Term_of_Rule_Set.Count; i++)
            {
                int numOfFreature = optimised.Term_of_Rule_Set[i].NumberOfInputVar;
                for (int j = 0; j < optimised.Term_of_Rule_Set[i].Parametrs.Count(); j++)
                { coust = rand.Next(100) / 500.0;
                if (rand.Next(2) > 0)
                {
                    optimised.Term_of_Rule_Set[i].Parametrs[j] *=(1 - coust);
                }
                else {
                    optimised.Term_of_Rule_Set[i].Parametrs[j] *=(1 + coust);
           
                }
                }
            }
        }
    }
}

*/
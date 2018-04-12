
using System;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract;
namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm.BeeDis
{
    public class Scout : Bee
    {
        int[] numOfTerms;
        int numoFRules=0;
        public int NumOFRule { get { return numoFRules; } }
        List<Term> forClass = new List<Term>();

        public Scout(bool[] theSource) : base(theSource)
        { }
        

        public void generateNewVector(PCFuzzySystem FS, Random rand)
        {
            int CountVaribles = FS.CountFeatures;
            forClass.Clear();
            numOfTerms = new int[CountVaribles];
            for (int i = 0; i < CountVaribles; i++)
            {

                PositionOfBee[i] = rand.NextDouble() >= 0.5;
            }
          
        }

    }
}



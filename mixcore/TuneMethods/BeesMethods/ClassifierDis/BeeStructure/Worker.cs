
using System;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm.BeeDis
{
    public class Worker:Bee
    {
       public  Worker(bool [] theSource)
            : base(theSource)
        { 
        }
        public void WorkerFly(PCFuzzySystem FS, Random rand, int iterate, int maxiter)
        {
            double part = (double)iterate / (double)maxiter;
            int count =rand.Next(1, (int) Math.Ceiling ((FS.CountFeatures) * (1.0/2.0) *(1 -  part)));
            for (int i = 0; i < count; i++)
            {
                int position = rand.Next(FS.CountFeatures);
                thePositionOfBee[position] = !thePositionOfBee[position];
            }
        }
    }
}


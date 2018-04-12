

using System;
using BeesMethods.Base.Common;
using System.Collections.Generic;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm.BeeDis
{
    public class Bee:AbstractBeeStructure
    {
       
       protected bool [] thePositionOfBee;

       public bool[] PositionOfBee { get { return thePositionOfBee; } set {  thePositionOfBee = value; }  }
       protected double goods ;
       public double accuracy { get ; protected set; }


        override public double Goods { get {return goods; } }
      
        public Bee(bool [] theSource)
        {

            thePositionOfBee = (bool[])theSource.Clone() ;
        }

        public double getGoodsImproove(PCFuzzySystem FS, double baseValue)
        {
            thePositionOfBee = checkAndCorrect(thePositionOfBee);
            FS.AcceptedFeatures = thePositionOfBee;
            accuracy = FS.ClassifyLearnSamples(FS.RulesDatabaseSet[0] );
            goods = accuracy - baseValue;
            return goods  ;
        }

        bool[] checkAndCorrect(bool[] Source)
        { bool flag = false;
            for (int i = 0; i < Source.Length; i++)
            {
                flag |= Source[i];
            }
            if (flag == false)
            {
                Random rand = new Random(79979465);
                Source[rand.Next(Source.Length)] = true;
            }
            return Source;
        }
    }
    public class BeeComparerAccuracy : IComparer<Bee>
    {
        public int Compare(Bee s1, Bee s2)
        {
            if (s1.accuracy > s2.accuracy) { return 1; }
            if (s1.accuracy < s2.accuracy) { return -1; }

            return 0;
        }
    }


    public class BeeComparerEqual : EqualityComparer<Bee>
    {
        public override bool Equals(Bee x, Bee y)
        {
            if (x.PositionOfBee.Length != y.PositionOfBee.Length) return false;
            for (int i = 0; i < x.PositionOfBee.Length; i++)
            {if (x.PositionOfBee[i] != y.PositionOfBee[i]) return false;
            }
                return true;
           
        }

        public override int GetHashCode(Bee obj)
        {
            int result = obj.PositionOfBee.Length;
            for (int i = 0; i < obj.PositionOfBee.Length; i++)
            {
                result += obj.PositionOfBee[i] ? result ^ obj.PositionOfBee.Length : 100000;
            }
           return result;
        }
    }

}


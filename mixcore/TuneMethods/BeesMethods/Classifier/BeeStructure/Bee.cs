/*

using System;
using System.Collections.Generic;
using BeesMethods.Base.Common;



namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm.Bee
{
    public class Bee:AbstractBeeStructure
    {
       protected KnowlegeBasePCRules thePositionOfBee;

       public KnowlegeBasePCRules PositionOfBee { get { return thePositionOfBee; } set {  thePositionOfBee = value; }  }
       protected BeeStructureAlgorithm Parrent;
        protected double goods ;


      override  public double Goods { get {return goods; } }
      
        public Bee(KnowlegeBasePCRules theSource, BeeStructureAlgorithm parrent)
        {
            thePositionOfBee = new KnowlegeBasePCRules(theSource);
            
            Parrent = parrent;
        }

        public double getGoodsImproove(double baseValue)
        {
            goods = Parrent.CalcNewProfit(thePositionOfBee) - baseValue;
          return goods  ;
        }

   




    }
}

*/
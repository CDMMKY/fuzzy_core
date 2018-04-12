using System;
using System.Collections.Generic;



namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm.Bee
{
    public class BeeParams
    {
       protected KnowlegeBaseSARules thePositionOfBee;
       public KnowlegeBaseSARules PositionOfBee { get { return thePositionOfBee; } set { thePositionOfBee = new KnowlegeBaseSARules(value); } }
       protected BeeParamsAlgorithm Parrent;
        protected double goods ;
        

        public double Goods { get {return goods; } }
      
        public BeeParams(KnowlegeBaseSARules theSource, BeeParamsAlgorithm parrent)
        {
            thePositionOfBee = new KnowlegeBaseSARules(theSource);
            
            Parrent = parrent;
        }

        public double getGoodsImproove()
        {
            goods = Parrent.CalcNewProfit(thePositionOfBee);
          return goods  ;
        }

        protected double[] inValidateTriangle(double[] Params, double min, double max, double Scatter, Random rand)
        {
            double[] result = Params.Clone() as double [];
            if (Params[1] < Params[0])
            {
                result[1] = Params[0];
                result[0] = Params[1];

            }

            if (Params[1] > Params[2])
            {
                result[2] = Params[1];
                result[1] = Params[2];
            }


            if (result[0] < min)
            {
                result[0] = min - Scatter * 0.05; 
            }
            if (result[2] > max)
            {
                result[2] = max + Scatter * 0.05; 
           
            }

            if ((result[1] > result[2]) || (result[1] < result[0]))
            {
                result[1] = result[0] + (result[2]-result[0]) * rand.NextDouble(); 
            }

            return result;

        }



        protected double[] inValidateTrapec(double[] Params, double min, double max, double Scatter, Random rand)
        {
            double[] result = Params.Clone() as double[];
            if (Params[1] < Params[0])
            {
                result[1] = Params[0];
                result[0] = Params[1];

            }

            if (Params[1] > Params[2])
            {
                result[2] = Params[1];
                result[1] = Params[2];
            }

            if (Params[2] > Params[3])
            {
                result[3] = Params[2];
                result[2] = Params[3];
            }



            if (result[0] < min)
            {
                result[0] = min - Scatter * 0.05;
            }
            if (result[3] > max)
            {
                result[3] = max + Scatter * 0.05;

            }

            if ((result[1] > result[2]) || (result[1] < result[0]))
            {
                result[1] = result[0] + (result[2] - result[0]) * rand.NextDouble();
            }


            if ((result[2] > result[3]) || (result[2] < result[0]))
            {
                result[1] = result[1] + (result[3] - result[1]) * rand.NextDouble();
            }

            
            return result;

        }


        public class BeeParamsComparer : IComparer<BeeParams>
        {
            public int Compare(BeeParams s1, BeeParams s2)
            {
                        if (s1.Goods > s2.Goods) { return 1; }
                        if (s1.Goods < s2.Goods) { return -1; }

                        return 0;
                
            }
        }


    }
}

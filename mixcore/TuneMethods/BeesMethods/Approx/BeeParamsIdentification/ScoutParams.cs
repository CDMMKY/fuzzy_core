using System;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract;


namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm.Bee
{
    class ScoutParams : BeeParams
    {
    
       public ScoutParams(KnowlegeBaseSARules theSource, BeeParamsAlgorithm parrent, Random rand)
            : base(theSource, parrent)
        { 
           
            for (int i = 0; i < PositionOfBee.TermsSet.Count; i++)
            { 
                int Varrible = PositionOfBee.TermsSet[i].NumVar;
                double Scatter = Parrent.getCurrentNs().LearnSamplesSet.InputAttributes[Varrible].Scatter;
                    
                switch(PositionOfBee.TermsSet[i].TermFuncType)
                {
                    case TypeTermFuncEnum.Гауссоида: { PositionOfBee.TermsSet[i].Parametrs = optimizeGauss(Scatter, PositionOfBee.TermsSet[i].Parametrs, rand); } break;
                    case TypeTermFuncEnum.Парабола: { PositionOfBee.TermsSet[i].Parametrs = optimizeParabolic(Scatter, PositionOfBee.TermsSet[i].Parametrs, rand); } break;
                    case TypeTermFuncEnum.Трапеция: { PositionOfBee.TermsSet[i].Parametrs = optimizeTrapec(Scatter, PositionOfBee.TermsSet[i].Parametrs, rand); } break;
                    case TypeTermFuncEnum.Треугольник: { PositionOfBee.TermsSet[i].Parametrs = optimizeTrianlge(Scatter, PositionOfBee.TermsSet[i].Parametrs, rand); } break;
                }
            }

            getGoodsImproove();
         
      //      SAFuzzySystem ToOpintNS = Parrent.getCurrentNs();

      /*      lock (ToOpintNS)
            {
                Adaptive_LSM tryOpt = new Adaptive_LSM();

              


                double tempgood = getGoodsImproove();
                KnowlegeBaseSARules tempPositionofBee = PositionOfBee;
                KnowlegeBaseSARules zeroSolution = ToOpintNS.RulesDatabaseSet[0];
                ToOpintNS.RulesDatabaseSet[0] = PositionOfBee;

                tryOpt.TuneUpFuzzySystem(ToOpintNS, new NullConfForAll());
                PositionOfBee = ToOpintNS.RulesDatabaseSet[0];
                double newgood = getGoodsImproove();

                if (newgood > tempgood)
                {
                    PositionOfBee = tempPositionofBee;
                    getGoodsImproove();
                }
            } */
        }
        
            
       protected double[] optimizeTrianlge(double Scatter, double[] Params, Random rand)
        {
            double[] resultParams = new double[3];
            int Choose = rand.Next(2);
            switch (Choose)
            {
                case 0:
                    {
                        resultParams[1] = Params[1]+2* rand.NextDouble()*Scatter;
                        resultParams[0] = resultParams[1] - 2 * rand.NextDouble()*Scatter;
                        resultParams[2] = resultParams[1] + 2*rand.NextDouble()*Scatter;
                    } break;
                case 1:
                    {
                        resultParams[1] = Params[1]-2* rand.NextDouble()*Scatter;
                        resultParams[0] = resultParams[1] - 2 * rand.NextDouble()*Scatter;
                        resultParams[2] = resultParams[1] + 2*rand.NextDouble()*Scatter;
                   } break;
                   
                default: { Console.Write("{0}", Choose); } break;
            }
            return resultParams;
        }


        protected double[] optimizeGauss( double Scatter, double[] Params, Random rand)
        {
            double[] resultParams = new double[3];
            int Choose = rand.Next(2);
            switch (Choose)
            {
                case 0:
                    {
                        resultParams[0] = Params[0] + (Scatter) * rand.NextDouble() * 2;
                    }break;
                case 1:
                    {
                        resultParams[0] = Params[0] - (Scatter) * rand.NextDouble() * 2;
                        
                    }break;
                   
            }
            resultParams[1] = Params[1] * rand.NextDouble()*2;
                
            return resultParams;

        }


        protected double[] optimizeParabolic( double Scatter, double[] Params, Random rand)
        {
            double[] resultParams = new double[2];
            int choose = rand.Next(2);
            switch (choose)
            {
                case 0:
                    {
                        resultParams[0] = Params[0] + Scatter * 2 * rand.NextDouble();
                        resultParams[1] = Params[1] + Scatter * 2 * rand.NextDouble();
                    } break;
                case 1:
                    {
                        resultParams[0] = Params[0] + Scatter * 2 * rand.NextDouble();
                        resultParams[1] = Params[1] - Scatter * 2 * rand.NextDouble();
                    } break;
                case 2:
                    {
                        resultParams[0] = Params[0] - Scatter * 2 * rand.NextDouble();
                        Params[1] = Params[1] + Scatter * 2 * rand.NextDouble();
                    } break;

                case 3:
                    {
                        resultParams[0] = Params[0] - Scatter * 2 * rand.NextDouble();
                        resultParams[1] = Params[1] - Scatter * 2 * rand.NextDouble();
                    } break;

            }
            if (resultParams[0] > resultParams[1])
            {

                double Temp = resultParams[1];
                resultParams[1] = resultParams[0];
                resultParams[0] = Temp;

            }
            return resultParams;
        }

        protected double[] optimizeTrapec( double Scatter,double [] Params, Random rand)
        {
            double[] resultParams = new double[4];
            int Choose = rand.Next(4);
            
            switch(Choose)
            {
                case 0:
                    {
                        resultParams[1] = Params[1] + (Scatter) * rand.NextDouble();
                        resultParams[2] = Params[2] + (Scatter) * rand.NextDouble();
                    } break;
                case 1: {
                    resultParams[1] = Params[1] + (Scatter) * rand.NextDouble();
                    resultParams[2] = Params[2] - (Scatter) * rand.NextDouble();
                } break;
                case 2:
                    {
                        resultParams[1] = Params[1] - (Scatter) * rand.NextDouble();
                        resultParams[2] = Params[2] + (Scatter) * rand.NextDouble();
                    } break;
                case 3:
                    {
                        resultParams[1] = Params[1] - (Scatter) * rand.NextDouble();
                        resultParams[2] = Params[2] - (Scatter) * rand.NextDouble();
                    } break; 
                   
        }
            
            if (resultParams[1] > resultParams[2])
            {
                double Temp = resultParams[2];
                resultParams[2] = resultParams[1];
                resultParams[1] = Temp;
            }

                resultParams[0] = resultParams[1] - Scatter*2 * rand.NextDouble();
                resultParams[3] = resultParams[2] + Scatter*2 * rand.NextDouble();
            return Params;
        }



        public static List <BeeParams> FlyScout(int countofBee, Random rand, KnowlegeBaseSARules Source,BeeParamsAlgorithm parrent)
        {
            List <BeeParams> result = new List<BeeParams>(); 
            for (int i =0;i<countofBee;i++)
            {
           result.Add(new ScoutParams(Source,parrent, rand));
            }

                return result;
        }

     

    }
}

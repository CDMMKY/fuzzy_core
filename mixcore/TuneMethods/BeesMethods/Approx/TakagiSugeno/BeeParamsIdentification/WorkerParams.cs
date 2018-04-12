using System;
using System.Linq;
using System.Collections.Generic;

namespace FuzzySystem.TakagiSugenoApproximate.LearnAlgorithm.Bee
{
    class WorkerParams : BeeParams
    {


        public WorkerParams(KnowlegeBaseTSARules theSource, KnowlegeBaseTSARules Best, BeeParamsAlgorithm parrent, Random rand)
            : base(theSource, parrent)
        {
            for (int i = 0; i < PositionOfBee.TermsSet.Count; i++)
            { double [] Params = PositionOfBee.TermsSet[i].Parametrs;
                for (int j=0; j<Params.Count();j++ )
                { int Choose = rand.Next(2);
                    switch(Choose)
                    {
                        case 0: { Params[j] += Math.Abs(Best.TermsSet[i].Parametrs[j] - Params[j]) * rand.NextDouble(); } break;
                        case 1: { Params[j] -= Math.Abs(Best.TermsSet[i].Parametrs[j] - Params[j]) * rand.NextDouble(); } break;
                    }
                }
                PositionOfBee.TermsSet[i].Parametrs =Params;
            }

            double[] consq = PositionOfBee.all_conq_of_rules;
            for (int i = 0; i < consq[i]; i++)
            {int choose = rand.Next(2);
                switch(choose)
                {
                    case 0: { consq[i] += Math.Abs(Best.all_conq_of_rules[i] - consq[i]) * rand.NextDouble(); } break;
                    case 1: {consq[i] -=Math.Abs(Best.all_conq_of_rules[i]- consq[i])*rand.NextDouble(); } break;
                }
                
            }
            PositionOfBee.all_conq_of_rules = consq;

            getGoodsImproove();
        }

        public static List<BeeParams> WorkersFly(BeeParams Best, List<BeeParams> Source, Random rand, BeeParamsAlgorithm parrent)
        {

            List<BeeParams> Result = new List<BeeParams>();
            for (int i = 0; i < Source.Count(); i++)
            {
                Result.Add( new WorkerParams(Source[i].PositionOfBee, Best.PositionOfBee, parrent, rand));
            }

            return Result;
        }
    }
}

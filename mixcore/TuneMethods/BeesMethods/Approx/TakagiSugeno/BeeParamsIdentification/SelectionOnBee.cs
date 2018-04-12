using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzySystem.TakagiSugenoApproximate.LearnAlgorithm.Bee
{
    class SelectionOnBee
    {
       protected BeeParams.BeeParamsComparer sorterBees = new BeeParams.BeeParamsComparer();
       protected List<int> TabuList =new List<int>();

        private bool notInTabuList(int Source)
        {
            return !TabuList.Contains(Source);
        }


        public List<BeeParams> selectRoulet(List <BeeParams> Source, int DividenSizeHive, Random rand)
        {
            TabuList.Clear();
            Source.Sort(sorterBees);
            if (DividenSizeHive >= Source.Count)
            {
                return Source;  
            }
           
            List<double> sortRules = new List<double>();
            double badresult = Source.Sum( x1 => x1.Goods);
            double badmax = Source[Source.Count - 1].Goods;
            sortRules.Add(0);
            double counter = 0;
            for (int i =0; i<Source.Count;i++)
            { 
            counter += (badmax-Source[i].Goods) / badresult;
                sortRules.Add(counter);
                
            }

            double toEqone = sortRules.Max();
            for (int i = 0; i < sortRules.Count(); i++)
            {
                sortRules[i] /= toEqone; 
            }

            int allowed_bee_pass=1;
            double choser=0;
            double Test=0;
            int index =0;
            List<BeeParams> result = new List<BeeParams>();
            result.Add(Source[0]);
            TabuList.Add(0);
            while (allowed_bee_pass < DividenSizeHive)
            { choser = rand.NextDouble();
              Test =   sortRules.FindLast(x => x < choser);
             index = sortRules.IndexOf(Test);
             if (notInTabuList(index))
             {
                 TabuList.Add(index);
                 result.Add(Source[index]);
                 allowed_bee_pass++;
             }

            }

            return result;
        }

        public List<BeeParams> selectElite(List<BeeParams> Source, int DividenSizeHive)
        {
            TabuList.Clear();
            Source.Sort(sorterBees);
            if (DividenSizeHive >= Source.Count)
            {
                return Source;
            }
         
            int allowed_bee_pass = 1;
            List<BeeParams> result = new List<BeeParams>();
            result.Add(Source[0]);
            TabuList.Add(0);
            while (allowed_bee_pass < DividenSizeHive)
            {
                result.Add(Source[allowed_bee_pass]);
                allowed_bee_pass++;
            }
            return result;
         
        }

        public List<BeeParams> selectBinaryTornament(List<BeeParams> Source, int DividenSizeHive, Random rand)
        {
            TabuList.Clear();
            Source.Sort(sorterBees);
        
               if (DividenSizeHive >= Source.Count)
            {
                return Source;  
            }
         

            int oneCandy = 0;
            int otherCandy = 0;
            int allowed_bee_pass = 1;
                 List<BeeParams> result = new List<BeeParams>();
                 result.Add(Source[0]);
                 TabuList.Add(0);
            while (allowed_bee_pass < DividenSizeHive)
            {       do
                    {    oneCandy = rand.Next(Source.Count);
                   }while (!notInTabuList(oneCandy));
            do {
            otherCandy = rand.Next(Source.Count);
            } while (!notInTabuList(otherCandy));

                int better =  Source[oneCandy].Goods>Source[otherCandy].Goods?oneCandy:otherCandy;
                result.Add( Source[better]);
                TabuList.Add(better);
                allowed_bee_pass++;
            }
            return result;

        }
        public List<BeeParams> selectRandom(List<BeeParams> Source, int DividenSizeHive, Random rand)
        {
            TabuList.Clear();
            Source.Sort(sorterBees);

            if (DividenSizeHive >= Source.Count)
            {
                return Source;
            }
            List<BeeParams> result = new List<BeeParams>();
            result.Add(Source[0]);
            TabuList.Add(0);
            int allowed_bee_pass = 1;
            int current = 0;
            while (allowed_bee_pass < DividenSizeHive)
            {
                current = rand.Next(Source.Count);
                if (notInTabuList(current))
                {
                    result.Add(Source[current]);
                    TabuList.Add(current);
                    allowed_bee_pass++;
                }
            }
            return result;
        
        }

        public virtual List<BeeParams> Regeration(List<BeeParams> Source, double Border1, int Repeat1, double Border2, int Repeat2, double Border3, int Repeat3, int DividenSizeHive)
        {
            TabuList.Clear();
            Source.Sort(sorterBees);
            List<double> badHistory = new List<double>();
            double maxError = Source.Sum(x => x.Goods);
            List <BeeParams> tempResult = new List<BeeParams>();
            int tempCounter;
            tempResult.Add(Source[0]);
           
            for (int i = 0; i <Source.Count; i++)
            {
                double relativeGoods = Source[i].Goods / maxError;
                badHistory.Add(relativeGoods);
                if ((relativeGoods>0)&& (relativeGoods <Border1))
                { tempCounter =0;
                    while(tempCounter<Repeat1)
                    {
                        tempResult.Add(Source[i]);
                        tempCounter++;
                    }
                    continue;
                }

                 if ((relativeGoods>Border1)&& (relativeGoods <Border2))
                 {
                     tempCounter = 0;
                    while(tempCounter<Repeat2)
                    {
                        tempResult.Add(Source[i]);
                        tempCounter++;
                    }
                    continue;
                }
               
                if ((relativeGoods>Border2)&& (relativeGoods <Border3))
                { tempCounter =0;
                    while(tempCounter<Repeat2)
                    {
                        tempResult.Add(Source[i]);
                        tempCounter++;
                    }
                    continue;
                }
            }
            if (DividenSizeHive < tempResult.Count)
            {
                tempResult.RemoveRange(DividenSizeHive, tempResult.Count -DividenSizeHive);
            }
                return tempResult;

            
        
        }
    }
}

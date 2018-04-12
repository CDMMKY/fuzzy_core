using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm.Bee
{
    class ParallelSelectorOnBee : SelectionOnBee
    {


        public override List<BeeParams> Regeration(List<BeeParams> Source, double Border1, int Repeat1, double Border2, int Repeat2, double Border3, int Repeat3, int DividenSizeHive)
        {
            TabuList.Clear();
            Source.Sort(sorterBees);
            double maxError = Source.Sum(x => x.Goods);
            List<BeeParams> tempResult = new List<BeeParams>();
            tempResult.Add(Source[0]);
            object Locker = new object();

            Parallel.For(0, Source.Count, (int i) =>
                  {
                      lock (Source[i]) {
                          int tempCounter=0;
          
                      double relativeGoods = Source[i].Goods / maxError;
                      if ((relativeGoods > 0) && (relativeGoods < Border1))
                      {
                          
                          while (tempCounter < Repeat1)
                          {
                              lock (Locker)
                              {
                                  tempResult.Add(Source[i]);
                              }
                                  tempCounter++;
                          }

                      }
                      else
                      {
                          if ((relativeGoods > Border1) && (relativeGoods < Border2))
                          {
                              
                              while (tempCounter < Repeat2)
                              {
                                  lock (Locker)
                                  {
                                      tempResult.Add(Source[i]);
                                  }
                                  tempCounter++;
                              }

                          }
                          else
                          {

                              if ((relativeGoods > Border2) && (relativeGoods < Border3))
                              {
                                 
                                  while (tempCounter < Repeat2)
                                  {
                                      lock (Locker)
                                      {
                                          tempResult.Add(Source[i]);
                                      }
                                      tempCounter++;
                                  }

                              }
                          }
                      }
                      }
                  });
            if (DividenSizeHive < tempResult.Count)
            {
                tempResult.RemoveRange(DividenSizeHive, tempResult.Count - DividenSizeHive);
            }
            return tempResult;



        }

    }
}

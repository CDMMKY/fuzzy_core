using System.Collections.Generic;
using System.Linq;

using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.Mesure;


namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm
{
   public class BreakTheCrossByLinds : AbstractNotSafeLearnAlgorithm
   {

       public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
       {
           get
           {
               return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.Singletone };
           }
       }

        public override SAFuzzySystem TuneUpFuzzySystem(SAFuzzySystem Approx, ILearnAlgorithmConf conf)
        {
            SAFuzzySystem result = Approx;
                  double minValue =5;
               int minFeature =0;
               int minATerm=0;
               int minBTerm=1;
               int indexDatabase = 0;

               for (int i = 0; i < result.CountFeatures; i++)
               {
                   List<Term> soureceByFeature = result.RulesDatabaseSet[indexDatabase].TermsSet.Where(x => x.NumVar == i).ToList();
                   for (int j = 0; j < soureceByFeature.Count(); j++)
                   {
                       for (int k = j + 1; k < soureceByFeature.Count(); k++)
                       {
                           double temp = TermOnterpreting.getIndexByLinds(soureceByFeature[j], soureceByFeature[k], soureceByFeature);
                           if (temp < minValue)
                           {
                               minValue = temp;
                               minATerm = j;
                               minBTerm = k;
                               minFeature = i;
                           }

                       }
                   }
               }
               result = BreakCrossTerm(result, minFeature, minATerm, minBTerm, indexDatabase);
               result.RulesDatabaseSet[0].TermsSet.Trim();
            return result; 
                 
        }

        private SAFuzzySystem BreakCrossTerm(SAFuzzySystem Approx, int Feature, int indexATerm, int indexBterm, int dataBase)
        {
            SAFuzzySystem result = Approx;
            KnowlegeBaseSARules DataSet = result.RulesDatabaseSet[dataBase];
            List<Term> soureTerms = DataSet.TermsSet.Where(x => x.NumVar == Feature).ToList();
            Term ATerm = soureTerms[indexATerm];
            Term BTerm = soureTerms[indexBterm];

            Term Left = ATerm;
            Term Right = BTerm;
            if (ATerm.Pick > BTerm.Pick)
            {
                Left = BTerm;
                Right = ATerm;
            }



            double border = (Left.Max + Right.Min) / 2;
            Left.Max = border;
            Right.Min = border;
      
            result.RulesDatabaseSet[dataBase] = DataSet;


            return result;
        }




        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Разрыв лексически далеких термов {";
                result += "}";
                return result;
            }
            return "Разрыв лексически далеких термов";
        }

        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new NullConfForAll();
            result.Init(CountFeatures);
            return result;
        }


    }
}

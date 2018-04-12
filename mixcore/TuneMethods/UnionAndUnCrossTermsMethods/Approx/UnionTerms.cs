using System;
using System.Collections.Generic;
using System.Linq;

using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.Mesure;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;

namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm

{
    public class UnionTerms : AbstractNotSafeLearnAlgorithm
    {


        double allowbyBorder = 0.4;
        double allowbySqare = 0.6;

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
            double minValue =1;
               int minFeature =0;
               int minATerm=0;
               int minBTerm=0;
               int indexDatabase = 0;
               UnionTermsConf Config = conf as UnionTermsConf;
               allowbyBorder = Config.UTCPercentCrossBorder/100;
               allowbySqare = Config.UTCPercentCrossSquare / 100;


           for (int i = 0; i < result.CountFeatures; i++)
           {
               List<Term> soureceByFeature = result.RulesDatabaseSet[indexDatabase].TermsSet.Where(x => x.NumVar == i).ToList();
               for (int j = 0; j < soureceByFeature.Count(); j++)
               {
                   for (int k=j+1; k<soureceByFeature.Count();k++)
                   {
                       double temp = TermOnterpreting.getG3(soureceByFeature[j], soureceByFeature[k], soureceByFeature.Count(), result.LearnSamplesSet.InputAttributes[i].Scatter, allowbySqare, allowbyBorder);
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

           result = UniTerm(result, minFeature, minATerm, minBTerm,indexDatabase);
           result.RulesDatabaseSet[0].TermsSet.Trim();
           return result;
        }


        private SAFuzzySystem UniTerm(SAFuzzySystem Approx, int Feature, int indexATerm, int indexBterm, int dataBase)
        {
            SAFuzzySystem result = Approx;
           KnowlegeBaseSARules DataSet = result.RulesDatabaseSet[dataBase];
            List<Term> soureTerms = DataSet.TermsSet.Where(x=>x.NumVar==Feature).ToList();
            Term ATerm = soureTerms[indexATerm];
            Term BTerm = soureTerms[indexBterm];
            double newPick = (ATerm.Pick + BTerm.Pick) / 2;
            double newMin = ATerm.Min;
            if (BTerm.Min < newMin) { newMin = BTerm.Min; }
            double newMax = ATerm.Max;
            if (BTerm.Max > newMax) { newMax = BTerm.Max; }

            Term uniTerm = new Term(ATerm);

            uniTerm.Pick = newPick;
            uniTerm.Min = newMin;
            uniTerm.Max = newMax;

            DataSet.TermsSet.Add(uniTerm);

           List <SARule> toChangeArules =  DataSet.RulesDatabase.Where(x => x.ListTermsInRule.Contains(ATerm)).ToList();
           for (int i = 0; i < toChangeArules.Count(); i++)
           {
               int indexofA = toChangeArules[i].ListTermsInRule.IndexOf(ATerm);
               toChangeArules[i].ListTermsInRule[indexofA] = uniTerm;
            }
           DataSet.TermsSet.Remove(ATerm);


           List<SARule> toChangeBrules = DataSet.RulesDatabase.Where(x => x.ListTermsInRule.Contains(BTerm)).ToList();
           for (int i = 0; i < toChangeBrules.Count(); i++)
           {
               int indexofB = toChangeBrules[i].ListTermsInRule.IndexOf(BTerm);
               toChangeBrules[i].ListTermsInRule[indexofB] = uniTerm;
           }
           DataSet.TermsSet.Remove(BTerm);

           result.RulesDatabaseSet[dataBase] = DataSet;


                return result;
        }


        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Объединение термов {";
                result += "Допустимый процент перекрытия по границам = " + allowbyBorder + ";" + Environment.NewLine;
                result += "Допустимый процент перекрытия по площади = " + allowbyBorder + ";" + Environment.NewLine;
                 result += "}";
                return result;
            }
            return "Объединение термов";
        }

        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new UnionTermsConf();
            result.Init(CountFeatures);
            return result;
        }
    }
}

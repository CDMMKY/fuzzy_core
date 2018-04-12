using FuzzySystem.PittsburghClassifier.Hybride;
using FuzzySystem.FuzzyAbstract.conf;
using System;
using System.Collections.Generic;
using System.Linq;
using FuzzyCoreUtils;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm
{
    public class PSOHybrideOcean:Term_Config_PSO,ILearnHybrideAvalibleToUse
    {
        protected int BorderGet;
        protected int BorderSend;

        protected int counterGet = 0;
        protected int countterSend = 0;

        protected List<KnowlegeBasePCRules> Outsiders;

        protected int countOutsiders;
        protected int countDiscovers;
        protected PittsburgHybride HybrideOcean;
        public PCFuzzySystem TuneUpFuzzySystem(PittsburgHybride Ocean, PCFuzzySystem Approximate, ILearnAlgorithmConf conf)
        {
            HybrideOcean = Ocean;
            base.TuneUpFuzzySystem(Approximate, conf);
            Ocean.Store(chooseDiscovers(1), this.ToString());
            result.RulesDatabaseSet[0].TermsSet.Trim();
            return result;
        }

        public List<KnowlegeBasePCRules> chooseDiscovers(int count)
        {
            List<KnowlegeBasePCRules> discovers = new List<KnowlegeBasePCRules>();
            discovers.AddRange(Pi.SelectBest(result,count));
            return discovers;
        }

        public void assimilateOutSiders()
        {
           
            X.Inject((X.Count()-1)/2, Outsiders, 0, Outsiders.Count, result);
           
        }

        public override void Init(ILearnAlgorithmConf conf)
        {
            base.Init(conf);

            PSOHybrideOceanConf config = conf as PSOHybrideOceanConf;
            BorderGet = config.PSOHOGetEach;
            BorderSend = config.PSOHOSendEach;
            countOutsiders = config.PSOSCPopulationSize / 2;
            countDiscovers = config.PSOSCPopulationSize / 2;

        }
        public override void oneIterate(PCFuzzySystem result)
        {
            if (HybrideOcean == null)
            {
                HybrideOcean = new PittsburgHybride(result);
            }

            base.oneIterate(result);

            counterGet++;
            countterSend++;


            if (countterSend == BorderSend)
            {
                countterSend = 0;
                HybrideOcean.Store(chooseDiscovers(countDiscovers), this.ToString());
            }

            if (counterGet == BorderGet)
            {
                counterGet = 0;
                Outsiders = HybrideOcean.Get(countOutsiders, PittsburgHybride.goodness.best, PittsburgHybride.islandStrategy.All);
                assimilateOutSiders();
            }
        }
        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new PSOHybrideOceanConf();
            result.Init(CountFeatures);
            return result;
        }
        public override string ToString(bool with_param = false)
        {

            if (with_param)
            {
                string result = "роящиеся частицы (острова) {";
                result += "Итераций= " + count_iteration.ToString() + " ;" + Environment.NewLine;
                result += "Коэффициент_c1= " + c1.ToString() + " ;" + Environment.NewLine;
                result += "Коэффициент_c2= " + c2.ToString() + " ;" + Environment.NewLine;
                result += "Особей в популяции= " + count_particle.ToString() + " ;" + Environment.NewLine;

                result += "}";
                return result;
            }
            return "роящиеся частицы (острова)";
        }

    }
}

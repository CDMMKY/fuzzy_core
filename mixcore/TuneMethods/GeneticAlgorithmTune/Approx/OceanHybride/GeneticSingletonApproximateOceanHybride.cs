using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySystem.SingletoneApproximate.Hybride;
using FuzzySystem.SingletoneApproximate.LearnAlgorithm;
using FuzzySystem.SingletoneApproximate;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzyCoreUtils;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;


namespace GeneticAlgorithmTune
{
    public class GeneticSingletonApproximateOceanHybride:GeneticApprox, ILearnHybrideAvalibleToUse
    {

       protected int BorderGet;
        protected int BorderSend;

        protected int counterGet = 0;
        protected int countterSend = 0;

        protected List<KnowlegeBaseSARules> Outsiders;
    
        protected int countOutsiders;
        protected int countDiscovers;
        protected SingletonHybride HybrideOcean;
        public SAFuzzySystem TuneUpFuzzySystem(SingletonHybride Ocean, SAFuzzySystem Approximate, ILearnAlgorithmConf conf)
        {
            HybrideOcean = Ocean;
            base.TuneUpFuzzySystem(Approximate, conf);
            Ocean.Store(chooseDiscovers(1), this.ToString());
            result.RulesDatabaseSet[0].TermsSet.Trim();
            return result;
        }

        public List<KnowlegeBaseSARules> chooseDiscovers(int count)
        {
            List<KnowlegeBaseSARules> discovers = populationMassive.SelectBest(result,currentConf.GENCPopulationSize/2).ToList();
            return discovers;
        }

        public void assimilateOutSiders()
        {
            populationMassive.Inject((int)Math.Floor(currentConf.GENCPopulationSize / 2.0), Outsiders, 0, Outsiders.Count, result);
        }

        public override void Init(ILearnAlgorithmConf conf)
        {
            base.Init(conf);

         GeneticHybrideOceanConfig config = conf as GeneticHybrideOceanConfig;
            BorderGet = config.GENCHOGetEach;
            BorderSend = config.GENCHOSendEach;
            countOutsiders = currentConf.GENCPopulationSize / 2;
            countDiscovers = (int)Math.Floor(currentConf.GENCPopulationSize / 2.0);
     
        }
        public override void oneIterate(SAFuzzySystem result)
        {
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
                Outsiders = HybrideOcean.Get(countOutsiders, SingletonHybride.goodness.best, SingletonHybride.islandStrategy.All);
                assimilateOutSiders();
            }
        }


        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Генетический алгоритм (острова){";
                result += "Количество итераций " + currentConf.GENCCountIteration + Environment.NewLine;
                result += "Вероятность скрещивания " + currentConf.GENCPopabilityCrossover + Environment.NewLine;
                result += "Доля отклонения при инициализации" + currentConf.GENCScateDeverceInit + Environment.NewLine;
                result += "Доля отклонения при мутации " + currentConf.GENCScateDeverceMutate + Environment.NewLine;
                result += "Количество генерируемых потомков " + currentConf.GENCCountChild + Environment.NewLine;
                result += "Особей в популяции " + currentConf.GENCPopulationSize + Environment.NewLine;
                result += "Тип инициализации " + currentConf.GENCTypeInit + Environment.NewLine;
                result += "Тип селекции " + currentConf.GENCTypeSelection + Environment.NewLine;
                result += "Тип скрещивания " + currentConf.GENCTypeCrossover + Environment.NewLine;
                result += "Точка деления " + currentConf.GENCPointCrossover + Environment.NewLine;
                result += "}";
                return result;
            }
            return "Генетический алгоритм (острова)";
        }


    }
}

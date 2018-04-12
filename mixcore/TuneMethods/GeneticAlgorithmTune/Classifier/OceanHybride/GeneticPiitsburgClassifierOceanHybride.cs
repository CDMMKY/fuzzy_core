using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySystem.PittsburghClassifier;
using FuzzySystem.PittsburghClassifier.Hybride;
using FuzzySystem.PittsburghClassifier.LearnAlgorithm;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzyCoreUtils;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;


namespace GeneticAlgorithmTune
{
    public class GeneticPiitsburgClassifierOceanHybride:GeneticClassifier, ILearnHybrideAvalibleToUse
    {

        protected int BorderGet;
        protected int BorderSend;

        protected int counterGet = 0;
        protected int countterSend = 0;

        protected List<KnowlegeBasePCRules> Outsiders;
    
        protected int countOutsiders;
        protected int countDiscovers;
        protected PittsburgHybride HybrideOcean;
        public PCFuzzySystem TuneUpFuzzySystem(PittsburgHybride Ocean, PCFuzzySystem Classifier, ILearnAlgorithmConf conf)
        {
            HybrideOcean = Ocean;
            base.TuneUpFuzzySystem(Classifier, conf);
            Ocean.Store(chooseDiscovers(1), this.ToString());
            return fullFuzzySystem;
        }

        public List<KnowlegeBasePCRules> chooseDiscovers(int count)
        {
            List<KnowlegeBasePCRules> discovers = populationMassive.SelectBest(fullFuzzySystem,currentConf.GENCPopulationSize/2).ToList();
            return discovers;
        }

        public void assimilateOutSiders()
        {
            populationMassive.Inject((int)Math.Floor(currentConf.GENCPopulationSize / 2.0), Outsiders, 0, Outsiders.Count, fullFuzzySystem);
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

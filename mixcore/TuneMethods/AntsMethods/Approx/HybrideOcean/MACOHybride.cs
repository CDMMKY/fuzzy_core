﻿
using FuzzySystem.SingletoneApproximate.Hybride;
using FuzzySystem.FuzzyAbstract.conf;
using System;
using System.Collections.Generic;
using FuzzyCoreUtils;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;



namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm.Term_config_Aco
{ 
    public class MACOHybride:Modified_ACO, ILearnHybrideAvalibleToUse
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
            return theFuzzySystem;
        }

        public List<KnowlegeBaseSARules> chooseDiscovers(int count)
        {
            List<KnowlegeBaseSARules> discovers = new List<KnowlegeBaseSARules>();
            
            discovers.AddRange(result.RulesDatabaseSet.SelectBest(theFuzzySystem,count));   
            return discovers;
        }

        public void assimilateOutSiders()
        {
            double CurrentErr = result.approxLearnSamples(newSolution);
            if (Outsiders.Count > 1)
            {
                double outSiderErr = result.approxLearnSamples(Outsiders[0]);
                if (CurrentErr > outSiderErr)
                {
                    newSolution = Outsiders[1];
                }
            }
        }
        public override void Init(ILearnAlgorithmConf conf)
        {
            base.Init(conf);

            MACOHybrideConfig config = conf as MACOHybrideConfig;
            BorderGet = config.MACOHOGetEach;
            BorderSend = config.MACOHOSendEach;
            countOutsiders = config.MACOCountElite / 2;
            countDiscovers = 1;
     
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
        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new MACOHybrideConfig();
            result.Init(CountFeatures);
            return result;
        }

        public override string ToString(bool with_param = false)
        {

            if (with_param)
            {
                string result = "Алгоритм муравьиной колонии (острова) {";
                result += "Итераций= " + ACO_iterationCount.ToString() + " ;" + Environment.NewLine;
                result += "Количество муравьев= " + ACO_antCount.ToString() + " ;" + Environment.NewLine;
                result += "Размер архива решений= " + ACO_decisionArchiveCount.ToString() + " ;" + Environment.NewLine;
                result += "Коэффицент q= " + ACO_q.ToString() + " ;" + Environment.NewLine;
                result += "Коэффицент xi= " + ACO_xi.ToString() + " ;" + Environment.NewLine;
                result += "Количество колоний= " + colonyCount.ToString() + " ;" + Environment.NewLine;
                result += "}";
                return result;
            }
            return "Алгоритм муравьиной колонии  (острова)";
        }

    }
}

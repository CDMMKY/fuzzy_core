/*
using FuzzySystem.SingletoneApproximate.Hybride;
using FuzzySystem.FuzzyAbstract.conf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FuzzyCoreUtils;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;



namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm.Bee
{
public class HybrideBeeStructure:BeeStructureAlgorithm, ILearnHybrideAvalibleToUse
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
            discovers.Add(Best);   
            return discovers;
        }

        public void assimilateOutSiders()
        {
            List<KnowlegeBaseSARules> Source = new List<KnowlegeBaseSARules>();

            for (int i = 0; i < theWorkers.Count; i++)
            {
                Source.Add(theWorkers[i].PositionOfBee);
            }

            Source.Inject(countWorkers / 2, Outsiders, 0, Outsiders.Count, theFuzzySystem);
                for (int i = 0; i < theWorkers.Count; i++)
                {
                   theWorkers[i].PositionOfBee = Source[i];
                   theWorkers[i].getGoodsImproove(baseLine);
                }

        }

        public override void Init(ILearnAlgorithmConf conf)
        {
            base.Init(conf);

            BeeStructureOceabHybrideConfig config = conf as BeeStructureOceabHybrideConfig;
            BorderGet = config.ABCSHOGetEach;
            BorderSend = config.ABCSHOSendEach;
            countOutsiders = countWorkers / 2;
            countDiscovers =1;
     
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
            ILearnAlgorithmConf result = new BeeStructureOceabHybrideConfig();
            result.Init(CountFeatures);
            return result;
        }


        public override string ToString(bool with_param = false)
        {

            if (with_param)
            {
                string result = "Алгоритм пчелинной колонии для структурной оптимизации (острова) {";
                result += "Количество разведчиков= " + countScouts.ToString() + " ;" + Environment.NewLine;
                result += "Количество рабочих пчел= " + countWorkers.ToString() + " ;" + Environment.NewLine;
                result += "Количество генерируемых правил= " + countRules.ToString() + " ;" + Environment.NewLine;
                //  result += "Вид функции принадлежности= " + Term.ToStringTypeTerm(typeTerm) + " ;" + Environment.NewLine;
                result += "}";
                return result;
            }
            return "Алгоритм пчелинной колонии для структурной оптимизации  (острова)";
        }
    }
}
*/
using System;
using System.Collections.Generic;

using FuzzySystem.SingletoneApproximate.LearnAlgorithm;
using GeneticAlgorithmTune;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using FuzzySystem.SingletoneApproximate;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.SingletoneApproximate.Hybride;
using System.Threading.Tasks;
using FuzzyCoreUtils;
using FuzzySystem.SingletoneApproximate.LearnAlgorithm.Term_config_Aco;
using FuzzySystem.FuzzyAbstract;

namespace HybrideWrappers
{
    public class HybrideOcean : AbstractNotSafeLearnAlgorithm
    {

        List<ILearnHybrideAvalibleToUse> Algorithms = new List<ILearnHybrideAvalibleToUse>();
        List<ILearnAlgorithmConf> Configs = new List<ILearnAlgorithmConf>();

        SingletonHybride Ocean;
        SAFuzzySystem result;



        HybrideOceanConfig Config;


        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.Singletone };
            }
        }

        public override FuzzySystem.SingletoneApproximate.SAFuzzySystem TuneUpFuzzySystem(FuzzySystem.SingletoneApproximate.SAFuzzySystem Approximate, ILearnAlgorithmConf conf)
        {
            result = Approximate;
            Init(conf);
           
           Ocean = new SingletonHybride(new SAFuzzySystem(result));

            
            List<Task> AlgTasks = new List<Task>();
      


            Parallel.For(0, Configs.Count, magic =>

         
            {

                Task CurrentTask = new Task(() => { Algorithms[magic].TuneUpFuzzySystem(Ocean, new SAFuzzySystem(result), Configs[magic]); });
                AlgTasks.Add(CurrentTask);

                AlgTasks[magic].Start();
            }
                );
            Task.WaitAll(AlgTasks.ToArray());
           List<KnowlegeBaseSARules> ListSystems= Ocean.Get(1, FuzzySystem.FuzzyAbstract.Hybride.FuzzyHybrideBase.goodness.best, FuzzySystem.FuzzyAbstract.Hybride.FuzzyHybrideBase.islandStrategy.All);

           ListSystems.Add(result.RulesDatabaseSet[0]);

           result.RulesDatabaseSet[0] = ListSystems.SelectBest(result, 1)[0];
           result.RulesDatabaseSet[0].TermsSet.Trim();
            return result;
        }

        void Init(ILearnAlgorithmConf Conf)
        {
            Config = Conf as HybrideOceanConfig;
            if (Config.ИспользоватьЭС)
            {
                Configs.Add(Config.НастройкиES);
                Algorithms.Add(new HybrideOceanESMethod());
            }

            if (Config.ИспользоватьГА)
            {
                Configs.Add(Config.НастройкиGA);
                Algorithms.Add(new GeneticAlgorithmTune.GeneticSingletonApproximateOceanHybride());
            }

          /*  if (Config.ИспользоватьABCS)
            {
                Configs.Add(Config.НастройкиABCS);
                Algorithms.Add(new HybrideBeeStructure ());
            }
            */
            if (Config.ИспользоватьPSO)
            {
                Configs.Add(Config.НастройкиPSO);
                Algorithms.Add(new PSOHybrideOcean());
            }


            if (Config.ИспользоватьMACO)
            {
                Configs.Add(Config.НастройкиMACO);
                Algorithms.Add(new MACOHybride());
            }


            for (int i = 0; i < Configs.Count; i++)
            {
                Configs[i].Init(result.CountFeatures);
            }



        }





        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Гибриды на основе островов {";
                if (Config.ИспользоватьГА)
                {
                   result += "Использовать ГА= Да ;" + Environment.NewLine;
                   result += "[" + Environment.NewLine + Algorithms.Find(x => x is GeneticSingletonApproximateOceanHybride).ToString(true) + Environment.NewLine+ "]";
                }
                else
                {
                    result += "Использовать ГА= Нет ;" + Environment.NewLine;
                }

                if (Config.ИспользоватьЭС)
                {
                    result += "Использовать ЭС= Да ;" + Environment.NewLine;
                    result += "[" + Environment.NewLine + Algorithms.Find(x => x is HybrideOceanESMethod).ToString(true) + Environment.NewLine + "]";
                }
                else
                {
                    result += "Использовать ЭС= Нет ;" + Environment.NewLine;
                }

/*
                if (Config.ИспользоватьABCS)
                {
                    result += "Использовать структурных пчел= = Да ;" + Environment.NewLine;
                    result += "[" + Environment.NewLine + Algorithms.Find(x => x is HybrideBeeStructure ).ToString(true) + Environment.NewLine + "]";
                }
                else
                {
                    result += "Использовать  структурных пчел = Нет ;" + Environment.NewLine;
                }
                */
                if (Config.ИспользоватьPSO)
                {
                    result += "Использовать АРЧ= = Да ;" + Environment.NewLine;
                    result += "[" + Environment.NewLine + Algorithms.Find(x => x is PSOHybrideOcean).ToString(true) + Environment.NewLine + "]";
                }
                else
                {
                    result += "Использовать АРЧ = Нет ;" + Environment.NewLine;
                }

                if (Config.ИспользоватьMACO)
                {
                    result += "Использовать НАМК  = Да ;" + Environment.NewLine;
                    result += "[" + Environment.NewLine + Algorithms.Find(x => x is MACOHybride).ToString(true) + Environment.NewLine + "]";
                }
                else
                {
                    result += "Использовать НАМК = Нет ;" + Environment.NewLine;
                }


                result += "}";
                return result;
            }




            return "Гибриды на основе островов";
        }


        public void Final()
        {
            //  result.RulesDatabaseSet[0] = main_pop.get_best_database();
            GC.Collect();

        }


        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new HybrideOceanConfig();
            result.Init(CountFeatures);
            return result;
        }

       




    }
}

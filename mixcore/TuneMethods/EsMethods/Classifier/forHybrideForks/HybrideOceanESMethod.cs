using System;
using System.Collections.Generic;

using FuzzySystem.PittsburghClassifier.Hybride;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm
{
    public  class HybrideOceanESMethod : ESMethod, ILearnHybrideAvalibleToUse
    {

      protected int BorderGet;
      protected int BorderSend;
        protected List<KnowlegeBasePCRules> Outsiders;
        protected List<KnowlegeBasePCRules> Discovers;
        protected int countOutsiders;
        protected int countDiscovers;
        protected int counterGet = 0;
       protected int counterSend = 0;
            
      protected PittsburgHybride  HybrideOcean;

      public   PCFuzzySystem TuneUpFuzzySystem(PittsburgHybride Ocean, PCFuzzySystem Approximate, ILearnAlgorithmConf conf)
        {
            HybrideOcean = Ocean;
            base.TuneUpFuzzySystem(Approximate, conf);
            Ocean.Store(chooseDiscovers(1), this.ToString());
            return result;
            
        }

        public List<KnowlegeBasePCRules> chooseDiscovers(int count)
        { List <KnowlegeBasePCRules> discovers= new List<KnowlegeBasePCRules>();
            discovers.Add(new KnowlegeBasePCRules(main_pop.get_best_database()));
            for (int i =0;i<countDiscovers-1;i++)
            {
                discovers.Add(new KnowlegeBasePCRules(main_pop.ThePopulate[i].hrom_vector.Core_Check));
            }
            return discovers;
        }

        public void assimilateOutSiders()
        {
            for (int i = 0; i < Outsiders.Count; i++)
            {
                main_pop.ThePopulate.Add(new ES.Individ(Outsiders[i], result.LearnSamplesSet, result.CountFeatures, true, rand, type_init));
            }
        }

        public override void Init(ILearnAlgorithmConf Conf)
        {
            base.Init(Conf);
            counterGet = 0;
            counterSend = 0;
            countOutsiders = count_populate / 2;
            countDiscovers = count_populate / 2;
            ESOceanHybrideConfig Confg = Conf as ESOceanHybrideConfig;
            BorderGet = Confg.ESCHOGetEach;
            BorderSend = Confg.ESCHOSendEach;
        }

        public override void oneIterate(PCFuzzySystem result)
        {
            base.oneIterate(result);
            HybrideOcean = new PittsburgHybride(result);
            counterGet++;
            counterSend++;
            if (counterGet == BorderGet)
            {
                counterGet = 0;
                Outsiders = HybrideOcean.Get(countOutsiders, PittsburgHybride.goodness.best, PittsburgHybride.islandStrategy.All);
                assimilateOutSiders();
            }
            if (counterSend == BorderSend)
            {
                counterSend = 0;
                HybrideOcean.Store(chooseDiscovers(count_populate / 2), this.ToString());
            }
        }
        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new ESOceanHybrideConfig();
            result.Init(CountFeatures);
            return result;
        }



        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Эволюционная стратегия (острова) {";
                result += "Итераций= " + count_iterate.ToString() + " ;" + Environment.NewLine;
                result += "Особей в популяции= " + count_populate.ToString() + " ;" + Environment.NewLine;
                result += "Потомков= " + count_child.ToString() + " ;" + Environment.NewLine;
                result += "Коэффицент t1= " + coef_t1.ToString() + " ;" + Environment.NewLine;
                result += "Коэффицент t2= " + coef_t2.ToString() + " ;" + Environment.NewLine;
                result += "Вероятность скрещивания= " + param_crossover.ToString() + " ;" + Environment.NewLine;
                result += "Точек скрещивания= " + count_Multipoint.ToString() + " ;" + Environment.NewLine;
                result += "Изменение РО= " + b_ro.ToString() + " ;" + Environment.NewLine;
                result += "Алгоритм инициализации= ";
                switch (type_init)
                {
                    case FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Type_init.Ограниченная: { result += "Ограниченная"; break; }
                    case FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Type_init.Случайная: { result += "Случайная"; break; }
                }

                result += " ;" + Environment.NewLine;

                result += "Алгоритм скрещивания= ";

                switch (alg_cross)
                {
                    case FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Alg_crossover.Многоточечный: { result += "Многоточечный"; break; }
                    case FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Alg_crossover.Унифицированный: { result += "Унифицированный"; break; }
                }
                result += " ;" + Environment.NewLine;

                result += "Алгоритм мутации= ";

                switch (type_mutate)
                {
                    case FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Type_Mutate.СКО: { result += "Простое ско"; break; }
                    case FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Type_Mutate.СКО_РО: { result += "На основе матрицы ковариации СКО РО"; break; }
                }
                result += " ;" + Environment.NewLine;



                result += "}";
                return result;
            }
            return "Эволюционная стратегия (острова)";
        }
    
    }
}

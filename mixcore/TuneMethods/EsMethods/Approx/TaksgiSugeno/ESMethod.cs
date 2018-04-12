using System;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.TakagiSugenoApproximate.LearnAlgorithm.ES;
using FuzzySystem.FuzzyAbstract;
using System.Collections.Generic;

namespace FuzzySystem.TakagiSugenoApproximate.LearnAlgorithm
{
    public class ESMethod : AbstractNotSafeLearnAlgorithm
    {
       protected Random rand = new Random();
       protected int count_populate;
       protected int count_child;
       protected int count_iterate;
       protected double coef_t1;
       protected double coef_t2;
       protected double param_crossover;
       protected int count_Multipoint;
       protected FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Alg_crossover alg_cross;
       protected FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Type_init type_init;
       protected FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Type_Mutate type_mutate;
       protected double b_ro;
       protected Population main_pop;
       protected ESConfig Config;
       protected TSAFuzzySystem result;

       public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
       {
           get
           {
               return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate };
           }
       }
     
        public override TSAFuzzySystem TuneUpFuzzySystem(TSAFuzzySystem Approximate, ILearnAlgorithmConf conf)
        {
            result = Approximate;
            Init(conf);
         
            for (int i = 0; i < count_iterate; i++)
            {
                oneIterate(result);
            }
            Final();
            result.RulesDatabaseSet[0].TermsSet.Trim();
            return result;
        }

        public virtual void Init(ILearnAlgorithmConf Conf)
        {
            Config = Conf as ESConfig;
            count_populate = Config.ESCPopulationSize;
            count_child = Config.ESCCountChild;
            count_iterate = Config.ESCCountIteration;
            coef_t1 = Config.ESCT1;
            coef_t2 = Config.ESCT2;
            param_crossover = Config.ESCCrossoverPropability;
            alg_cross = Config.ESCCrossoverType;
            type_init = Config.ESCInitType;
            count_Multipoint = Config.ESCCountCrossoverPoint;
            type_mutate = Config.ESCMutateAlg;
            b_ro = Config.ESCAngleRotateB;
            main_pop = new Population(count_populate, count_child, result.CountFeatures, result.LearnSamplesSet);
            main_pop.init_first(result.RulesDatabaseSet[0], rand, type_init);
        }

        public virtual void oneIterate(TSAFuzzySystem result)
        {
            double inparam = 0;
            switch (alg_cross)
            {
                case FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Alg_crossover.Унифицированный: inparam = param_crossover; break;
                case FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Alg_crossover.Многоточечный: inparam = count_Multipoint; break;
            }

            main_pop.select_parents_and_crossover(rand, alg_cross, inparam);
            main_pop.mutate_all(rand, coef_t1, coef_t2, type_mutate, b_ro);
            main_pop.union_parent_and_child();
            main_pop.Calc_Error(result);
            main_pop.select_global();
        }

        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Эволюционная стратегия {";
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
            return "Эволюционная стратегия";
        }


        public virtual void Final()
        {
            result.RulesDatabaseSet[0] = main_pop.get_best_database();
            GC.Collect();
        }


        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new ESConfig();
            result.Init(CountFeatures);
            return result;
        }

    }
}

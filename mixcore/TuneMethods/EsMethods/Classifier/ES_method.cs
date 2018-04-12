using System;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.PittsburghClassifier.LearnAlgorithm.ES;
using FuzzySystem.FuzzyAbstract;
using System.Collections.Generic;


namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm
{
   public  class ESMethod : AbstractNotSafeLearnAlgorithm
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
        protected PCFuzzySystem result;

        protected Population main_pop;



        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.PittsburghClassifier};
            }
        }

        public override PCFuzzySystem TuneUpFuzzySystem(PCFuzzySystem Classifier, ILearnAlgorithmConf conf)
        {
            result = Classifier;
            Init(conf);
            for (int i = 0; i < count_iterate; i++)
            {
                oneIterate(result);
                                
            }
            Final();
            result.RulesDatabaseSet[0].TermsSet.Trim();
               return result;
           }

        public override string ToString(bool with_param = false)
        {if (with_param)
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
                
                result+=" ;" + Environment.NewLine;
                
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


        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new ESConfig();
            result.Init(CountFeatures);
            return result;
        }


      
        public virtual void oneIterate(PCFuzzySystem result)
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

        public virtual void Init(ILearnAlgorithmConf Config)
        {
            ESConfig conf = Config as ESConfig;
            count_populate = conf.ESCPopulationSize;
            count_child = conf.ESCCountChild;
            count_iterate = conf.ESCCountIteration;
            coef_t1 = conf.ESCT1;
            coef_t2 = conf.ESCT2;
            param_crossover = conf.ESCCrossoverPropability;
            alg_cross = conf.ESCCrossoverType;
            type_init = conf.ESCInitType;
            count_Multipoint = conf.ESCCountCrossoverPoint;
            type_mutate = conf.ESCMutateAlg;
            b_ro = conf.ESCAngleRotateB;
            main_pop = new Population(count_populate, count_child, result.CountFeatures, result.LearnSamplesSet);
            main_pop.init_first(result.RulesDatabaseSet[0], rand, type_init);
       
        }

        public virtual void Final()
        {
            KnowlegeBasePCRules best = main_pop.get_best_database();
            double errorBest = result.ErrorLearnSamples(best);
            double currentError = result.ErrorLearnSamples(result.RulesDatabaseSet[0]);
            if (currentError >errorBest)
            {
                result.RulesDatabaseSet[0] = main_pop.get_best_database();
            }
            result.RulesDatabaseSet[0] = main_pop.get_best_database();
            GC.Collect();
         
        }
    }
}

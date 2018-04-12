using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using FuzzySystem.FuzzyAbstract;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm.Term_config_Aco
{
  public  class Base_ACO : AbstractNotSafeLearnAlgorithm
    {
        protected readonly Random rand = new Random();
        protected int ACO_iterationCount;
        protected int ACO_antCount;
        protected int ACO_decisionArchiveCount;
        protected double ACO_q;
        protected double ACO_xi;
        protected PCFuzzySystem result;
        protected int colonyCount;
        protected List<Colony> colonyList;
        protected double basePrecission;
        
        protected KnowlegeBasePCRules newSolution;


        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.PittsburghClassifier };
            }
        }

        public static double BoxMullerTransform(double disp, double mathExp, Random rnd)
        {
            var x1 = rnd.NextDouble();
            var x2 = rnd.NextDouble();
            var z = Math.Sin(2 * Math.PI * x1) * Math.Sqrt(-2 * Math.Log(x2));
            return mathExp + disp * z;
        }

        public override PCFuzzySystem TuneUpFuzzySystem(PCFuzzySystem Classifier, ILearnAlgorithmConf conf)
           {
            try
            {
                result = Classifier;
                Init(conf);
                for (int iterNum = 0; iterNum < ACO_iterationCount; iterNum++)
                {
                    oneIterate(result);

                }

                Final();
                result.RulesDatabaseSet[0].TermsSet.Trim();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// step 0
        /// </summary>
        /// <param name="Classifier"></param>
        /// <returns></returns>

        protected virtual bool preCheck(PCFuzzySystem Classifier)
        {
            if (Classifier.RulesDatabaseSet == null)
            {
                return false;
            }
            if (Classifier.RulesDatabaseSet.Count < 0)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Step 1
        /// </summary>
        /// <param name="Classifier"></param>
        /// <param name="config"></param>

        protected virtual void init(PCFuzzySystem Classifier, ACOSearchConf config)
        {
            ACO_iterationCount = config.ACOCountIteration;
            ACO_antCount = config.ACOCountAnt;
            ACO_decisionArchiveCount = config.ACODescisionArchiveSize;
            ACO_q = config.ACOQ;
            ACO_xi = config.ACOXi;
            result = Classifier;
            colonyCount = result.RulesDatabaseSet[0].TermsSet.Count;
            colonyList = new List<Colony>();
            newSolution = new KnowlegeBasePCRules(result.RulesDatabaseSet[0]);
            
            basePrecission = result.ClassifyLearnSamples(newSolution);

           
        }

        /// <summary>
        /// step 2
        /// </summary>

        protected virtual void colonyGenerate()
        {
            for (int i = 0; i < colonyCount; i++)
            {
                Colony the_Colony = new Colony(ACO_antCount, ACO_decisionArchiveCount, i, result.RulesDatabaseSet[0].TermsSet[i], this);
                colonyList.Add(the_Colony);
            }
        }

        /// <summary>
        /// step 3
        /// </summary>
        /// <param name="the_colony"></param>
        protected virtual void randomDecisionsGenerate(Colony the_colony)
        {
            the_colony.FillRandomArchiveDecision(rand);
        }
        /// <summary>
        /// step 4
        /// </summary>
        /// <param name="the_colony"></param>

        protected virtual void calc_decisions_Weight(Colony the_colony)
        {
            the_colony.FillDecisionArchiveWeight(ACO_q);
        }

        /// <summary>
        /// Last step
        /// </summary>
        protected virtual void prepareFinalFuzzySystem()
        {
            double startPrecission = result.ClassifyLearnSamples(result.RulesDatabaseSet[ 0]);
            double afterPrecission = result.ClassifyLearnSamples(newSolution);
            
            if (startPrecission < afterPrecission)
            {
                result.RulesDatabaseSet[0] = newSolution;
            }
          
        }

        public double  getPrecission ()
        {
            return result.ClassifyLearnSamples(newSolution);
        }



        public override string ToString(bool with_param = false)
        {

            if (with_param)
            {
                string result = "Алгоритм муравьиной колонии {";
                result += "Итераций= " + ACO_iterationCount.ToString() + " ;" + Environment.NewLine;
                result += "Количество муравьев= " + ACO_antCount.ToString() + " ;" + Environment.NewLine;
                result += "Размер архива решений= " + ACO_decisionArchiveCount.ToString() + " ;" + Environment.NewLine;
                result += "Коэффицент q= " + ACO_q.ToString() + " ;" + Environment.NewLine;
                result += "Коэффицент xi= " + ACO_xi.ToString() + " ;" + Environment.NewLine;
                result += "Количество колоний= " + colonyCount.ToString() + " ;" + Environment.NewLine;
                result += "}";
                return result;
            }
            return "Алгоритм муравьиной колонии";
        }




        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new ACOSearchConf();
            result.Init(CountFeatures);
            return result;
        }







        public virtual void oneIterate(PCFuzzySystem result)
        {
            foreach (Colony colony in colonyList)    //Шаг 9. Если имеется следующая колония, то сделать текущим первого муравья в этой колонии и перейти на шаг 5, иначе перейти на шаг 10.
            {
                for (int i = 0; i < ACO_antCount; i++) //Шаг 8. Если в текущей колонии имеется следующий муравей, то сделать его текущим и перейти к шагу 5, иначе перейти на шаг 9.
                {
                    // Шаг 5. Для текущего муравья текущей колонии вычислить номер l, используемой функции Гаусса по формуле 2.14. Определить  l i для i = 1, ..., N по формуле 2.15. Сгенерировать N случайных величин {θl*1, θl*2,…, θl*N} на основе полученных функций gl i(x). 
                    colony.runAnt(i, rand, ACO_xi);

                    //Шаг 6.  Найти ошибку вывода нечеткой системы при параметрах {θ1,…, θN }, если ошибка меньше текущей, то сохранить новые параметры.
                    basePrecission = colony.checkAntDecision(i, basePrecission);

                    // Шаг 7. Добавить в архив новое решение, ранжировать архив, удалить из архива худшее решение.
                    colony.updateDecisionArchive(i);
                }

            }
        }

        public virtual void Init(ILearnAlgorithmConf Config)
        {
            ACOSearchConf config = Config as ACOSearchConf;

            if (preCheck(result) == false)
            {
                throw new ArgumentNullException("Не правильно инициализированная нечеткая система");
            }

            // Шаг 1. Задать начальные параметры.
            init(result, config);

            // Шаг 2. Сгенерировать популяцию муравьев в колониях
            colonyGenerate();


            Parallel.ForEach(colonyList, colony =>
            {
                // Шаг 3. Сгенерировать k случайных решений, для всех архивов решений с последующим  оцениванием и ранжированием. 
                randomDecisionsGenerate(colony);

                //Шаг 4. Найти значения вектора весов.
                calc_decisions_Weight(colony);
            }
            );
        }

        public virtual void Final()
        {

            prepareFinalFuzzySystem();

        }
    }
}

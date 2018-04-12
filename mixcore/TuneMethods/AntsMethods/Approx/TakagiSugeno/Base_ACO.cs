using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using FuzzySystem.FuzzyAbstract;

namespace FuzzySystem.TakagiSugenoApproximate.LearnAlgorithm.Term_config_Aco
{
    public class Base_ACO :AbstractNotSafeLearnAlgorithm
    {
        protected readonly Random rand = new Random();
        protected int ACO_iterationCount;
        protected int ACO_antCount;
        protected int ACO_decisionArchiveCount;
        protected double ACO_q;
        protected double ACO_xi;
        protected TSAFuzzySystem result;
        protected int colonyCount;
        protected List<Colony> colonyList;
        protected double baseError;
       // protected int current_database;
        protected KnowlegeBaseTSARules newSolution;
        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
              return  new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate };
            }
        }

       public static double BoxMullerTransform(double disp, double mathExp, Random rnd)
        {
            var x1 = rnd.NextDouble();
            var x2 = rnd.NextDouble();
            var z = Math.Sin(2 * Math.PI * x1) * Math.Sqrt(-2 * Math.Log(x2));
            return mathExp + disp * z;
        }

      

        public override TSAFuzzySystem TuneUpFuzzySystem(TSAFuzzySystem Approx, ILearnAlgorithmConf conf)
           {
            try
            {




                ACOSearchConf config = conf as ACOSearchConf;

                if (preCheck(Approx) == false)
                {
                    throw new ArgumentNullException("Не правильно инициализированная нечеткая система");
                }

                // Шаг 1. Задать начальные параметры.
                init(Approx, config);

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



                for (int iterNum = 0; iterNum < ACO_iterationCount; iterNum++)
                {
                    foreach (Colony colony in colonyList)    //Шаг 9. Если имеется следующая колония, то сделать текущим первого муравья в этой колонии и перейти на шаг 5, иначе перейти на шаг 10.
                    {
                        for (int i = 0; i < ACO_antCount; i++) //Шаг 8. Если в текущей колонии имеется следующий муравей, то сделать его текущим и перейти к шагу 5, иначе перейти на шаг 9.
                        {
                            // Шаг 5. Для текущего муравья текущей колонии вычислить номер l, используемой функции Гаусса по формуле 2.14. Определить  l i для i = 1, ..., N по формуле 2.15. Сгенерировать N случайных величин {θl*1, θl*2,…, θl*N} на основе полученных функций gl i(x). 
                            colony.runAnt(i, rand, ACO_xi);
                            
                            //Шаг 6.  Найти ошибку вывода нечеткой системы при параметрах {θ1,…, θN }, если ошибка меньше текущей, то сохранить новые параметры.
                            baseError = colony.checkAntDecision(i, baseError);
                            
                            // Шаг 7. Добавить в архив новое решение, ранжировать архив, удалить из архива худшее решение.
                            colony.updateDecisionArchive(i);
                        }

                    }

                }



                prepareFinalFuzzySystem();
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

        protected virtual bool preCheck(TSAFuzzySystem Approx)
        {
            if (Approx.RulesDatabaseSet == null)
            {
                return false;
            }
            if (Approx.RulesDatabaseSet.Count < 0)
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

        protected virtual void init(TSAFuzzySystem Approx, ACOSearchConf config)
        {
            ACO_iterationCount = config.ACOCountIteration;
            ACO_antCount = config.ACOCountAnt;
            ACO_decisionArchiveCount = config.ACODescisionArchiveSize;
            ACO_q = config.ACOQ;
            ACO_xi = config.ACOXi;
            result = Approx;
            colonyCount = result.RulesDatabaseSet[0].TermsSet.Count;
            colonyList = new List<Colony>();
            newSolution = new KnowlegeBaseTSARules(result.RulesDatabaseSet[0]);
            
        //    current_database = result.RulesDatabaseSet.Count -1; 
           
            baseError = result.approxLearnSamples(newSolution);

           
        }

        /// <summary>
        /// step 2
        /// </summary>

        protected virtual void colonyGenerate()
        {
            for (int i = 0; i < colonyCount; i++)
            {
                Colony the_Colony = new Colony(ACO_antCount, ACO_decisionArchiveCount, i, newSolution.TermsSet[i], this);
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
            double startError = result.approxLearnSamples (result.RulesDatabaseSet[ 0]);
            double afterError = result.approxLearnSamples(newSolution);
            
            if (startError > afterError)
            {
                result.RulesDatabaseSet[0] = newSolution;
            }
            result.RulesDatabaseSet.RemoveRange(1, result.RulesDatabaseSet.Count - 1);
          
        }

        public virtual double  getError ()
        {
            return result.approxLearnSamples (newSolution);
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


    }
}

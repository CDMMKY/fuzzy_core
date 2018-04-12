using System;
using System.Threading.Tasks;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;

namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm.Term_config_Aco
{
   public class Modified_ACO:Base_ACO
    {
        protected double MACOCurrentError;
        protected int MACOCountRepeatError;
        protected int MACOCountBorderRepeat;
        protected int MACOCountEliteDecision;
        protected SAFuzzySystem theFuzzySystem;

        public override SAFuzzySystem TuneUpFuzzySystem(SAFuzzySystem Approx, ILearnAlgorithmConf conf)
        {
            result = Approx;
            theFuzzySystem = Approx;
            Init(conf);
            try
            {
                for (int iterNum = 0; iterNum < ACO_iterationCount; iterNum++)
                {
                    oneIterate(Approx);
                 }
                Final();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public override string ToString(bool with_param = false)
        {

            if (with_param)
            {
                string result = "Модифицированный алгоритм муравьиной колонии {";
                result += "Итераций= " + ACO_iterationCount.ToString() + " ;" + Environment.NewLine;
                result += "Количество муравьев= " + ACO_antCount.ToString() + " ;" + Environment.NewLine;
                result += "Размер архива решений= " + ACO_decisionArchiveCount.ToString() + " ;" + Environment.NewLine;
                result += "Коэффицент q= " + ACO_q.ToString() + " ;" + Environment.NewLine;
                result += "Коэффицент xi= " + ACO_xi.ToString() + " ;" + Environment.NewLine;
                result += "Количество колоний= " + colonyCount.ToString() + " ;" + Environment.NewLine;
                result += "Порог застревания архивов решений в экстремуме" + MACOCountBorderRepeat.ToString() + ";" + Environment.NewLine;
                result += "Элитных решений" + MACOCountEliteDecision.ToString() +";" + Environment.NewLine ;
                result += "}";
                return result;
            }
            return "Модифицированный алгоритм муравьиной колонии";
        }



        protected override void init(SAFuzzySystem Approx, ACOSearchConf config)
        {
            base.init(Approx, config);
            MACOSearchConf configNew = config as MACOSearchConf;
            MACOCountBorderRepeat = configNew.MACOCountExtremum;
            MACOCountRepeatError = 0;
            MACOCurrentError = getError();
            MACOCountEliteDecision = configNew.MACOCountElite;
        }

        protected virtual bool isInExtremum()
        { double currentError = getError();
        if (MACOCurrentError == currentError)
        {
            MACOCountRepeatError++;
            if (MACOCountRepeatError > MACOCountBorderRepeat)
            {
                MACOCountRepeatError = 0;
                return true;
            }
            return false;
        }
        MACOCurrentError = currentError;
        return false;
        }



        public virtual void oneIterate(SAFuzzySystem result)
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
            // Шаг. * Модифицированный алгоритм. Проверяем находиться ли в экстремуме алгоритм.
            if (isInExtremum())
            {
                foreach (Colony colony in colonyList)    //Шаг **. Обновляем архивы, заполняем их случайными решениями и элитными 
                {
                    colony.refillDesicionArchive(MACOCountEliteDecision - 1, rand, this);
                }
            }
        }

        public virtual void Init(ILearnAlgorithmConf Config)
        {
            ACOSearchConf config = Config as MACOSearchConf;

            if (preCheck(theFuzzySystem) == false)
            {
                throw new ArgumentNullException("Не правильно инициализированная нечеткая система");
            }

            // Шаг 1. Задать начальные параметры.
            init(theFuzzySystem, config);

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


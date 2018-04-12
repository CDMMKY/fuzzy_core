//#define CONTRACTS_FULL
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using static System.Diagnostics.Contracts.Contract;


namespace FuzzySystem.FuzzyAbstract
{ /// <summary>
/// Абстрактный класс представляющий любую нечеткую систему как совокупность обучающией и тестовой выборки, а также массива абстрактных правил баз правил.
/// </summary>
    public abstract class IFuzzySystem
    {

        #region Visible public methods
        /// <summary>
        /// Функция получающая сложность заданной базы правил как суммы количества правил и термов
        /// </summary>
        /// <param name="Source">База правил</param>
        /// <returns>Сумма количества правил и термов</returns>
        abstract public int ValueComplexity(KnowlegeBaseRules Source);
        /// <summary>
        /// Функция получающая количество правил в заданной базе правил
        /// </summary>
        /// <param name="Source">База правил</param>
        /// <returns>Количество правил</returns>

        abstract public int ValueRuleCount(KnowlegeBaseRules Source);

        /// <summary>
        /// Абстрактная функция требующая, чтобы для любой базы правил была вычислима ошибка на тестовой выборке.
        /// </summary>
        /// <param name="Source">База правил</param>
        /// <returns>Ошибка базы правил на тестовой выборке</returns>
        public abstract double ErrorTestSamples(KnowlegeBaseRules Source);
        /// <summary>
        /// Абстрактная функция требующая, чтобы для любой базы правил была вычислима ошибка на обучающей выборке.
        /// </summary>
        /// <param name="Source">База правил</param>
        /// <returns>Ошибка базы правил на обучающей выборке</returns>
        public abstract double ErrorLearnSamples(KnowlegeBaseRules Source);
        /// <summary>
        /// Функция преобразующая массив баз правил заданного у наследников типа в 
        /// </summary>
        /// <returns></returns>
        public abstract List<KnowlegeBaseRules> AbstractRulesBase();
        /// <summary>
        /// Обучающая выборка данных
        /// </summary>
        public SampleSet LearnSamplesSet
        { get; set; }
        /// <summary>
        /// Тестовая выборка данных
        /// </summary>
        public SampleSet TestSamplesSet
        { get; set; }

        /// <summary>
        /// Полное количество входных параметров доступных в обучающей выборке, без учета используемых или нет
        /// </summary>
        public int CountFeatures
        {
            get
            {

                return LearnSamplesSet.CountVars;
            }
        }
        /// <summary>
        /// Количество входных параметров используемых при вычислении ошибки. Данное значение меньше или равно количеству входных признаков в обучающей выборке. 
        /// </summary>
        public int CountUsedVars
        {
            get
            {

                return AcceptedFeatures.Where(x => x == true).Count();
            }
        }
        /// <summary>
        /// Вектор указывающий какие входные признаки используются в расчёте ошибки а какие нет. True значение на i-той позиции означает что i-тый входной признак используется
        /// </summary>
        public bool[] AcceptedFeatures { get; set; }


        #endregion

        #region constructor

        //    public System.Diagnostics.Stopwatch sw { get; set; }

        /// <summary>
        /// Конструктор создающий нечеткуюс систему без баз правил, но с заданной обучающей и тестовой выборкой 
        /// </summary>
        /// <param name="LearnSet">Обучающая выборка</param>
        /// <param name="TestSet">Тестовая выборка</param>
        public IFuzzySystem(SampleSet LearnSet, SampleSet TestSet)
        {


            Requires(LearnSet != null);
            // sw = new System.Diagnostics.Stopwatch();
            LearnSamplesSet = LearnSet;
            AcceptedFeatures = new bool[CountFeatures];

            if (TestSet != null)
            {
                TestSamplesSet = TestSet;
                for (int i = 0; i < CountFeatures; i++)
                {
                    AcceptedFeatures[i] = true;
                    if (
                        !LearnSamplesSet.InputAttributes[i].Name.Equals(TestSamplesSet.InputAttributes[i].Name,
                                                                          StringComparison.OrdinalIgnoreCase))
                    {
                        throw (new InvalidEnumArgumentException("Атрибуты обучающей таблицы и тестовой не совпадают"));
                    }
                }
            }
        }

        /// <summary>
        /// Клонирующий конструктор. Создает полную копию исходной нечеткой счистемы.
        /// </summary>
        /// <param name="Source">Исходная нечеткая система</param>
        public IFuzzySystem(IFuzzySystem Source)
        {
            Requires(Source != null);
            //       sw = new System.Diagnostics.Stopwatch();
            LearnSamplesSet = Source.LearnSamplesSet;
            AcceptedFeatures = Source.AcceptedFeatures.Clone() as bool[];
            TestSamplesSet = Source.TestSamplesSet;

        }

        #endregion

        /// <summary>
        /// Метод геометрической коррекции базы правил, гарантирует верное вычисление ошибки на обучающей и тестовой выборках за счёт разрешения ситуаций неопределенности (некоторые входные признаки неполностью покрыты функциями принадлежности)
        /// </summary>
        /// <param name="Source">База правил подлежащая исправлению методом геометрической проверки</param>
        public virtual void UnlaidProtectionFix(KnowlegeBaseRules Source)
        {
            Requires(Source != null);
            if (
                (double.IsInfinity(ErrorLearnSamples(Source)))
                || (double.IsNaN(ErrorLearnSamples(Source)))
                || (double.IsInfinity(ErrorTestSamples(Source)))
                || (double.IsNaN(ErrorTestSamples(Source)))
                )
            {
                lock (Source)
                {
                    UnlaidProtectionFixMaxMinBorder(Source);
                    UnlaidProtectionInMiddle(Source);
                    Source.TermsSet.Trim();
                }
            }
        }

        /// <summary>
        /// Часть метода геометрической коррекции базы правил, исправляет ситуацию когда для данных содержащих  минимальные или максимальные значения входящих признаков результат классификации или аппроксимации неопределен 
        /// </summary>
        /// <param name="Source">База правил подлежащая исправлению методом геометрической проверки</param>
        protected virtual void UnlaidProtectionFixMaxMinBorder(KnowlegeBaseRules Source)
        {
            Requires(Source != null);

            for (int i = 0; i < CountFeatures; i++)
            {
                if (AcceptedFeatures[i] == false)
                {
                    continue;
                }
                List<Term> termsForVar =
                      Source.TermsSet.FindAll(x => x.NumVar == i);
                if (termsForVar.Find(x => x.TermFuncType == TypeTermFuncEnum.Гауссоида) != null)
                {
                    continue;
                }
                else
                {
                    double min = termsForVar.Min(x => x.Min);
                    int min_index = termsForVar.FindIndex(x => (x.Min == min));
                    termsForVar[min_index].Min =
                        LearnSamplesSet.InputAttributes[i].Min - 0.001 * LearnSamplesSet.InputAttributes[i].Scatter;
                    double max = termsForVar.Max(x => x.Max);
                    int max_index = termsForVar.FindIndex(x => (x.Max == max));
                    termsForVar[max_index].Max = LearnSamplesSet.InputAttributes[i].Max +
                         0.001 * LearnSamplesSet.InputAttributes[i].Scatter;
                }
            }
        }

        /// <summary>
        /// Часть метода геометрической коррекции базы правил, исправляет ситуацию когда существую на входных признаках области не принадлежащие ни одному из множеств. 
        /// </summary>
        /// <param name="Source">База правил подлежащая исправлению методом геометрической проверки</param>
        protected virtual void UnlaidProtectionInMiddle(KnowlegeBaseRules Source)
        {
            Requires(Source != null);

            for (int i = 0; i < CountFeatures; i++)
            {
                if (AcceptedFeatures[i] == false)
                {
                    continue;
                }
                List<Term> TermsForVar = Source.TermsSet.FindAll(x => x.NumVar == i);
                if (TermsForVar.Exists(x => x.TermFuncType == TypeTermFuncEnum.Гауссоида)) { continue; }
                for (int j = 0; j < TermsForVar.Count - 1; j++)
                {

                    if ((TermsForVar[j].Max < TermsForVar[j + 1].Min))
                    {
                        double temp = TermsForVar[j].Max;
                        TermsForVar[j].Max = TermsForVar[j + 1].Min;
                        TermsForVar[j + 1].Min = temp;
                    }
                    if (TermsForVar[j].Max == TermsForVar[j + 1].Min)
                    {
                        TermsForVar[j].Max += LearnSamplesSet.InputAttributes[i].Scatter * 0.001;
                        TermsForVar[j + 1].Min -= LearnSamplesSet.InputAttributes[i].Scatter * 0.001;
                    }
                }
            }
        }

        #region  private interstruct
        /// <summary>
        /// Случайное имя нечеткой системы для отличия нечетких систем во время отладки
        /// </summary>
        protected string nameObj { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// Метод получения имени нечеткой системы. 
        /// </summary>
        /// <returns>Случайное и уникальное имя нечеткой системы</returns>
        public override string ToString()
        {
            return base.ToString() + " " + nameObj;
        }

        #endregion
        /// <summary>
        /// Общая формула пересчёта ошибки в RMSE (Корень квадратный из суммы квадратов невязок результатов аппроксимации и выходов таблицы наблюдения деленной на размер таблицы наблюдения) в MSE (отношение суммы квадратов невязок результатов аппроксимации с выходами таблицы наблюдения к количеству образцов в таблице наблюдения )
        /// </summary>
        /// <param name="Source">Ошибка в RMSE. Методы ErrorLearnSamples и ErrorTestSamples расчитывают ошибку аппроксимации в RMSE</param>
        /// <param name="CountSamples">Количество строк в таблице наблюдений</param>
        /// <returns>Ощибка аппроксимации в MSE </returns>
        public static double RMSEtoMSE(double Source, int CountSamples)
        {
            double result = Source;
            result *= result;
            result *= CountSamples;
            return result;
        }

        /// <summary>
        /// Метод пересчёта ошибки аппроксимации из RMSE в MSE для обучающей выборки.
        /// </summary>
        /// <param name="Source">Ошибка в RMSE. Метод ErrorLearnSamples расчитывает ошибку аппроксимации в RMSE</param>
        /// <returns>Ощибка аппроксимации в MSE </returns>

        public double RMSEtoMSEforLearn(double Source)
        {
            return RMSEtoMSE(Source, LearnSamplesSet.CountSamples);
        }

        /// <summary>
        /// Метод пересчёта ошибки аппроксимации из RMSE в MSE для тестовой выборки.
        /// </summary>
        /// <param name="Source">Ошибка в RMSE. Метод ErrorTestSamples расчитывает ошибку аппроксимации в RMSE</param>
        /// <returns>Ощибка аппроксимации в MSE </returns>

        public double RMSEtoMSEforTest(double Source)
        {
            return RMSEtoMSE(Source, TestSamplesSet.CountSamples);
        }

        /// <summary>
        /// Метод пересчёта ошибки аппроксимации из RMSE в MSE/2 для обучающей выборки.
        /// </summary>
        /// <param name="Source">Ошибка в RMSE. Метод ErrorLearnSamples расчитывает ошибку аппроксимации в RMSE</param>
        /// <returns>Ощибка аппроксимации в MSE/2 </returns>

        public double RMSEtoMSEdiv2forLearn(double Source)
        {

            return RMSEtoMSE(Source, LearnSamplesSet.CountSamples) / 2.0;
        }

        /// <summary>
        /// Метод пересчёта ошибки аппроксимации из RMSE в MSE/2 для тестовой выборки.
        /// </summary>
        /// <param name="Source">Ошибка в RMSE. Метод ErrorTestSamples расчитывает ошибку аппроксимации в RMSE</param>
        /// <returns>Ощибка аппроксимации в MSE/2 </returns>
        public double RMSEtoMSEdiv2forTest(double Source)
        {

            return RMSEtoMSE(Source, TestSamplesSet.CountSamples) / 2.0;
        }
    }
}
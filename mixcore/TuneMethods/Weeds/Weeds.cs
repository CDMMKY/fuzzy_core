using System;
using System.Collections.Generic;
using System.ComponentModel;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.SingletoneApproximate;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.TakagiSugenoApproximate;
using RecursiveLeastSquares.Approx;
using RecursiveLeastSquares.Base;
using Weeds.Properties;


namespace FuzzySystem.TSAApproximate.Weeds
{
    //    public class WeedSAFuzzySystem : TSAFuzzySystem
    //    {
    //        public double error { get; set; }
    //        public WeedSAFuzzySystem(TSAFuzzySystem Source)
    //            : base(Source)
    //        {
    //        }
    //    }

    public class Weeds : AbstractNotSafeLearnAlgorithm
    {
        private int maxiter;
        private int MaxChild = 5;
        private int MinChild = 1;
        private double originalDelta = 0.1;

        internal static List<KnowlegeBaseTSARules> WeedsRegenerateIteration(TSAFuzzySystem fuzzy ,KnowlegeBaseTSARules b, double delta, Random rand, int count)
        {
            var rez = new List<KnowlegeBaseTSARules>();
            
            for (int k = 0; k < count; k++)
            {
                var bforedit = new KnowlegeBaseTSARulesWithError(b);
                foreach (var rule in bforedit.RulesDatabase)
                {
                    for (int i = 0; i < rule.ListTermsInRule.Count; i++)
                    {
                        var term = rule.ListTermsInRule[i];
                        for (int index = 0; index < term.Parametrs.Length; index++)
                        {
                            term.Parametrs[index] += fuzzy.LearnSamplesSet.InputAttributes[i].Scatter * delta*Math.Sqrt(-2*Math.Log(rand.NextDouble()))
                                                     *Math.Cos(2*Math.PI*rand.NextDouble());
                        }
                    }
                }
                bforedit.error = fuzzy.ErrorLearnSamples(bforedit);
                rez.Add(bforedit);
            }
            return rez;

        }

        public override TSAFuzzySystem TuneUpFuzzySystem(TSAFuzzySystem Approx, ILearnAlgorithmConf conf)
        // Здесь ведется оптимизация вашим алгоритмом
        {
            
            Random rand = new Random();
            SAFuzzySystem result = Approx;
            var WeedOriginal = new TSAFuzzySystemWithErrorKnowledgeBase(Approx);
            ((KnowlegeBaseTSARulesWithError)WeedOriginal.RulesDatabaseSet[0]).error = WeedOriginal.ErrorLearnSamples(WeedOriginal.RulesDatabaseSet[0]);

            maxiter = ((Weeds_conf)conf).Количество_итераций;
            MaxChild = ((Weeds_conf)conf).Максимально_потомков;
            MinChild = ((Weeds_conf)conf).Минимально_потомков;
            originalDelta = ((Weeds_conf)conf).Дельта;

            for (int iter = 0; iter < maxiter; iter++)
            {
                List<KnowlegeBaseTSARules> newtstmp = new List<KnowlegeBaseTSARules>();
                for (int k = 0; k < WeedOriginal.RulesDatabaseSet.Count; k++)
                {
                    double fiBest = 1;

                    double fiWorst = (1 - ((KnowlegeBaseTSARulesWithError)WeedOriginal.RulesDatabaseSet[k]).error);
                    if (!Double.IsNaN(((KnowlegeBaseTSARulesWithError)WeedOriginal.RulesDatabaseSet[k]).error))
                    if (!Double.IsInfinity(((KnowlegeBaseTSARulesWithError)WeedOriginal.RulesDatabaseSet[k]).error))
                    {
                        var CurrertChild =
                            Convert.ToInt32((MaxChild - MinChild) / (fiBest - fiWorst) * (1 -  (((KnowlegeBaseTSARulesWithError)WeedOriginal.RulesDatabaseSet[k]).error)) +
                                            (fiBest * MinChild - fiWorst * MaxChild) / (fiBest - fiWorst));
                        double delta = originalDelta * (maxiter - iter) / maxiter;
                        newtstmp.AddRange(Weeds.WeedsRegenerateIteration(WeedOriginal, WeedOriginal.RulesDatabaseSet[k], delta, rand, CurrertChild));

                    }
                }
                foreach (var weedBase in newtstmp)
                {
                    RLS rls = new RLS();
                    TSAFuzzySystemWithErrorKnowledgeBase tmp = new TSAFuzzySystemWithErrorKnowledgeBase(WeedOriginal);
                    
                    tmp.RulesDatabaseSet.Clear();
                    tmp.RulesDatabaseSet.Add(weedBase);
                    var errormnk = tmp.approxLearnSamples(tmp.RulesDatabaseSet[0]);
                    var rezMnk = rls.TuneUpFuzzySystem(tmp, new RLSconfig());
                    tmp.reinit();
                    ((KnowlegeBaseTSARulesWithError) tmp.RulesDatabaseSet[0]).error = rezMnk.approxLearnSamples(rezMnk.RulesDatabaseSet[0]);
                    if (tmp.approxLearnSamples(tmp.RulesDatabaseSet[0]) < tmp.approxLearnSamples(tmp.RulesDatabaseSet[ 1]))
                    {
                        WeedOriginal.RulesDatabaseSet.Add(tmp.RulesDatabaseSet[0]);
                    }
                    else
                    WeedOriginal.RulesDatabaseSet.Add(tmp.RulesDatabaseSet[1]);
                }
                WeedOriginal.RulesDatabaseSet.RemoveAll(x => Double.IsInfinity(((KnowlegeBaseTSARulesWithError)x).error) || Double.IsInfinity(((KnowlegeBaseTSARulesWithError)x).error));
                WeedOriginal.RulesDatabaseSet.Sort((c1, c2) => ((KnowlegeBaseTSARulesWithError)c1).error.CompareTo(((KnowlegeBaseTSARulesWithError)c2).error));
                if (WeedOriginal.RulesDatabaseSet.Count>6) WeedOriginal.RulesDatabaseSet.RemoveRange(6, WeedOriginal.RulesDatabaseSet.Count-6);

            }

            return WeedOriginal;
        }
        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate };
            }
        }

        public override string ToString(bool with_param = false)// без параметров возвращает имя алгоритма, с параметров true возвращает имя алгоритма и значения его параметров
        {
            if (with_param)
            {
                string result = "Сорняковый алгоритм алгоритм{";
                result += "Итераций= " +  " ;" + Environment.NewLine;
//                result += "G0= " + G0.ToString() + " ;" + Environment.NewLine;
//                result += "Коэффициент альфа= " + alpha.ToString() + " ;" + Environment.NewLine;
//                result += "Коэффициент эпсилон= " + epsilon.ToString() + " ;" + Environment.NewLine;
//                result += "Особей в популяции= " + MCount.ToString() + " ;" + Environment.NewLine;
                result += "}";
                return result;
            }
            return "Сорняковый алгоритм";
        }
        public override ILearnAlgorithmConf getConf(int CountFeatures) // Создание класса конфигуратора для вашего метода
        {
            ILearnAlgorithmConf result = new Weeds_conf();
            result.Init(CountFeatures);
            return result;
        }
    }



    public class Weeds_conf : ILearnAlgorithmConf
    {
        [Description("Количество итераций"), Category("Итерации")]
        public int Количество_итераций
        {
            get { return Settings.Default.i; }
            set { Settings.Default.i = value; Settings.Default.Save(); }
        }

        [Description("Количество Максимально_потомков"), Category("Потомки")]
        public int Максимально_потомков
        {
            get { return Settings.Default.MaxChild; }
            set { Settings.Default.MaxChild = value; Settings.Default.Save(); }
        }

        [Description("Количество Минимально_потомков"), Category("Потомки")]
        public int Минимально_потомков
        {
            get { return Settings.Default.MinChild; }
            set { Settings.Default.MinChild = value; Settings.Default.Save(); }
        }

        [Description("Дельта"), Category("Дельта")]
        public double Дельта
        {
            get { return Settings.Default.originalDelta; }
            set { Settings.Default.originalDelta = value; Settings.Default.Save(); }
        }

        public void loadParams(string param)
        {
            throw (new NotImplementedException());
        }
        public void Init(int countVars)
        { }
    }
}

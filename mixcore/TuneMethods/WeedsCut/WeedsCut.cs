#if true

#endif


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.TakagiSugenoApproximate;
using KLI;
using RecursiveLeastSquares.Approx;
using WeedsCut.Properties;
using KnowlegeBaseTSARules = FuzzySystem.TSAApproximate.Weeds.KnowlegeBaseTSARulesWithError;
using FuzzySystem.TSAApproximate.Weeds;

namespace FuzzySystem.SingletoneApproximate.WeedsCut
{

    public class WeedsCut : AbstractNotSafeLearnAlgorithm
    {
        private int maxiter;
        private int MaxChild = 3;
        private int MinChild = 1;
        private double originalDelta = 0.1;

        internal static List<KnowlegeBaseTSARules> WeedsRegenerateIteration(TSAFuzzySystem fuzzy, KnowlegeBaseTSARules b, double delta, Random rand, int count)
        {
            var rez = new List<KnowlegeBaseTSARules>();

            for (int k = 0; k < count; k++)
            {
                var bforedit = new KnowlegeBaseTSARules(b);
                bforedit.error = fuzzy.ErrorLearnSamples(bforedit);
                rez.Add(bforedit);
            }
            return rez;

        }

        public override TSAFuzzySystem TuneUpFuzzySystem(TSAFuzzySystem Approx, ILearnAlgorithmConf conf)
        // Здесь ведется оптимизация вашим алгоритмом
        {
           
                var first = true;
            var WeedOriginal = new TSAFuzzySystemWithErrorKnowledgeBase(Approx);
            double KLIRange = ((WeedsCut_conf)conf).Граница; 
            maxiter = ((WeedsCut_conf)conf).Количество_итераций; 
            var MaxUsingRule = ((WeedsCut_conf)conf).Максимально_потомков; 
            var MinUsingRule = ((WeedsCut_conf)conf).Минимально_потомков; 
            Random r = new Random();

            while (first || Double.IsNaN((WeedOriginal.RulesDatabaseSet[0] as KnowlegeBaseTSARules).error))
            {
                
               
                int allUsingRuleCount = ((WeedsCut_conf)conf).Начальное_количество;
                WeedOriginal = new TSAFuzzySystemWithErrorKnowledgeBase(Approx);

                first = false;
                List<int> list = new List<int>();
                int counter = 0;
                for (int i = 0; i < allUsingRuleCount; i++)
                {
                    list.Add(counter);
                    counter++;
                }
                for (int i = 0; i < WeedOriginal.AcceptedFeatures.Count(); i++)
                {
                    WeedOriginal.AcceptedFeatures[i] = false;
                }
                for (int i = 0; i < allUsingRuleCount; i++)
                {
                    int index = r.Next(0, allUsingRuleCount - i);
                    int ddd = list[index];
                    WeedOriginal.AcceptedFeatures[ddd] = true;
                    list.RemoveAt(index);
                }
                
                global::KLI.KLI s = new global::KLI.KLI();
                WeedOriginal = new TSAFuzzySystemWithErrorKnowledgeBase (s.Generate(WeedOriginal as TSAFuzzySystem, new KLI_conf() { MaxValue = KLIRange} ));
                WeedOriginal.reinit();
                (WeedOriginal.RulesDatabaseSet[0] as KnowlegeBaseTSARules).error = WeedOriginal.ErrorLearnSamples(WeedOriginal.RulesDatabaseSet[0]);
                (WeedOriginal.RulesDatabaseSet[0] as KnowlegeBaseTSARules).RulesAcceptedFeatures = new List<bool>(WeedOriginal.AcceptedFeatures.ToList()); 
            }
            string added2 = (WeedOriginal.RulesDatabaseSet.Last() as  KnowlegeBaseTSARules).RulesAcceptedFeatures.Aggregate(String.Empty, (current, v) => current + (" " + (v ? "1" : "0")));
            File.AppendAllLines("./out2.txt", new List<string>() { WeedOriginal.RulesDatabaseSet.Last().RulesDatabase.Count +" " +
                             (WeedOriginal.RulesDatabaseSet.Last() as KnowlegeBaseTSARules).RulesAcceptedFeatures.Count(x=>x) + " " + WeedOriginal.RMSEtoMSEforLearn(WeedOriginal.ErrorLearnSamples(WeedOriginal.RulesDatabaseSet.Last())) / 2 + " " + WeedOriginal.RMSEtoMSEforTest(WeedOriginal.ErrorTestSamples(WeedOriginal.RulesDatabaseSet.Last())) / 2 +" "+ added2 });

            Random rand = new Random();
            for (int iter = 0; iter < maxiter; iter++)
            {
                List<KnowlegeBaseTSARules> newtstmp = new List<KnowlegeBaseTSARules>();
                for (int k = 0; k < WeedOriginal.RulesDatabaseSet.Count; k++)
                {
                    double fiBest = 1;

                    double fiWorst = (1 - (WeedOriginal.RulesDatabaseSet[k] as KnowlegeBaseTSARules).error);
                    if (!Double.IsNaN((WeedOriginal.RulesDatabaseSet[k] as KnowlegeBaseTSARules).error))
                        if (!Double.IsInfinity((WeedOriginal.RulesDatabaseSet[k] as KnowlegeBaseTSARules).error))
                        {
                            var CurrertChild =
                                Convert.ToInt32((MaxChild - MinChild) / (fiBest - fiWorst) * (1 - (WeedOriginal.RulesDatabaseSet[k] as KnowlegeBaseTSARules).error) +
                                                (fiBest * MinChild - fiWorst * MaxChild) / (fiBest - fiWorst));
                            double delta = originalDelta * (maxiter - iter) / maxiter;


                            newtstmp.AddRange(WeedsCut.WeedsRegenerateIteration(WeedOriginal, (WeedOriginal.RulesDatabaseSet[k] as KnowlegeBaseTSARules), delta, rand, CurrertChild));
                            
                            //
                            var item = newtstmp.Last();
                            item.RulesAcceptedFeatures = new List<bool>((WeedOriginal.RulesDatabaseSet[k] as KnowlegeBaseTSARules).RulesAcceptedFeatures);
                            for (int index = 0; index < item.RulesAcceptedFeatures.Count; index++)
                            {
                                if (Math.Sqrt(-2*Math.Log(rand.NextDouble()))*Math.Cos(2*Math.PI*rand.NextDouble()) > delta)
                                    item.RulesAcceptedFeatures[index] = !item.RulesAcceptedFeatures[index];
                            }

                            while (item.RulesAcceptedFeatures.Count(x => x) > MaxUsingRule)
                            {
                                item.RulesAcceptedFeatures[(rand.Next(item.RulesAcceptedFeatures.Count))] = false;
                            }

                            while (item.RulesAcceptedFeatures.Count(x => x) < MinUsingRule)
                            {
                                item.RulesAcceptedFeatures[(rand.Next(item.RulesAcceptedFeatures.Count))] = true;
                            }
                        }
                }

                foreach (var weedBase in newtstmp)
                {
                    RLS rls = new RLS();
                    var tmp = new TSAFuzzySystemWithErrorKnowledgeBase(WeedOriginal);

                    tmp.RulesDatabaseSet.Clear();
                    tmp.AcceptedFeatures = weedBase.RulesAcceptedFeatures.ToArray();
                    global::KLI.KLI s = new global::KLI.KLI();
                    tmp = s.Generate(tmp, new KLI_conf() { MaxValue = KLIRange }) as TSAFuzzySystemWithErrorKnowledgeBase;
                    tmp.reinit();
                    (tmp.RulesDatabaseSet[0] as KnowlegeBaseTSARules).error = tmp.ErrorLearnSamples(tmp.RulesDatabaseSet[0]);
                    (tmp.RulesDatabaseSet[0] as KnowlegeBaseTSARules).RulesAcceptedFeatures = new List<bool>(tmp.AcceptedFeatures.ToList());
                    if (!double.IsNaN((tmp.RulesDatabaseSet[0] as KnowlegeBaseTSARules).error))
                    {
                        WeedOriginal.RulesDatabaseSet.Add(tmp.RulesDatabaseSet[0]);
                        string added = (tmp.RulesDatabaseSet[0] as KnowlegeBaseTSARules).RulesAcceptedFeatures.Aggregate(String.Empty, (current, v) => current + (" " + (v ? "1" : "0")));
                        File.AppendAllLines("./out2.txt", new List<string>() { tmp.RulesDatabaseSet[0].RulesDatabase.Count +" " +
                             (tmp.RulesDatabaseSet[0] as KnowlegeBaseTSARules).RulesAcceptedFeatures.Count(x=>x) + " " + tmp.RMSEtoMSEforLearn(tmp.ErrorLearnSamples(tmp.RulesDatabaseSet[0])) / 2 + " " + tmp.RMSEtoMSEforTest(tmp.ErrorTestSamples(tmp.RulesDatabaseSet[0])) / 2 +" "+ added });
                    }

                }
                WeedOriginal.RulesDatabaseSet.RemoveAll(x => Double.IsInfinity((x as KnowlegeBaseTSARules).error) || Double.IsInfinity( (x as KnowlegeBaseTSARules).error));
                WeedOriginal.RulesDatabaseSet.Sort((c1, c2) => (c1 as KnowlegeBaseTSARules).error.CompareTo((c2 as KnowlegeBaseTSARules).error));
                if (WeedOriginal.RulesDatabaseSet.Count > 6) WeedOriginal.RulesDatabaseSet.RemoveRange(6, WeedOriginal.RulesDatabaseSet.Count - 6);

            }
            WeedOriginal.AcceptedFeatures = (WeedOriginal.RulesDatabaseSet[0] as  KnowlegeBaseTSARules).RulesAcceptedFeatures.ToArray();
            
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
                string result = "Сорняковый уменьшающий алгоритм алгоритм{";
                result += "Итераций= " + " ;" + Environment.NewLine;
                //                result += "G0= " + G0.ToString() + " ;" + Environment.NewLine;
                //                result += "Коэффициент альфа= " + alpha.ToString() + " ;" + Environment.NewLine;
                //                result += "Коэффициент эпсилон= " + epsilon.ToString() + " ;" + Environment.NewLine;
                //                result += "Особей в популяции= " + MCount.ToString() + " ;" + Environment.NewLine;
                result += "}";
                return result;
            }
            return "Сорняковый уменьшающий алгоритм";
        }
        public override ILearnAlgorithmConf getConf(int CountFeatures) // Создание класса конфигуратора для вашего метода
        {
            ILearnAlgorithmConf result = new WeedsCut_conf();
            result.Init(CountFeatures);
            return result;
        }
    }



    public class WeedsCut_conf : ILearnAlgorithmConf
    {
        [Description("Количество итераций"), Category("Итерации")]
        public int Количество_итераций
        {
            get { return Settings.Default.Iter; }
            set { Settings.Default.Iter = value; Settings.Default.Save(); }
        }

        [Description("Начальное количество"), Category("Начальное количество")]
        public int Начальное_количество
        {
            get { return Settings.Default.BeginCount; }
            set { Settings.Default.BeginCount = value; Settings.Default.Save(); }
        }

        [Description("Количество Максимально_потомков"), Category("Потомки")]
        public int Максимально_потомков
        {
            get { return Settings.Default.Max; }
            set { Settings.Default.Max = value; Settings.Default.Save(); }
        }

        [Description("Количество Минимально_потомков"), Category("Потомки")]
        public int Минимально_потомков
        {
            get { return Settings.Default.Min; }
            set { Settings.Default.Min = value; Settings.Default.Save(); }
        }

        [Description("Граница"), Category("Граница")]
        public double Граница
        {
            get { return Settings.Default.KLIRange; }
            set { Settings.Default.KLIRange = value; Settings.Default.Save(); }
        }

        public void loadParams(string param)
        {
            throw (new NotImplementedException());
        }
        public void Init(int countVars)
        { }
    }
}

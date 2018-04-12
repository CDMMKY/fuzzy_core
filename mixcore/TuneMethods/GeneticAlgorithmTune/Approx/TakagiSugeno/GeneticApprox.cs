using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySystem.TakagiSugenoApproximate;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using FuzzySystem.FuzzyAbstract;

namespace FuzzySystem.TakagiSugenoApproximate.GeneticAlgorithmTune
{
    public delegate double initFuncType(double inputVar, double scale, double max1, double min1);
    public delegate KnowlegeBaseTSARules crossoverFuncType(KnowlegeBaseTSARules parent1, KnowlegeBaseTSARules parent2);
    public delegate void selectionFuncType();
    public class GeneticApprox : AbstractNotSafeLearnAlgorithm
    {
        #region privateMembers
     protected   Random allRandom = new Random();
     protected GeneticConf currentConf;
     protected initFuncType initFunc;
     protected crossoverFuncType crossoverFunc;
     protected selectionFuncType selectionFunc;
     protected KnowlegeBaseTSARules[] populationMassive;
     protected KnowlegeBaseTSARules[] childrenMassive;
     protected TSAFuzzySystem fullFuzzySystem;
     protected TSAFuzzySystem result;
        int step = 0;
        double errorAfter;
        double errorBefore;
        KnowlegeBaseTSARules backUp;
        #endregion

        #region InitMethods
        double localInit(double inputVar, double scale, double max, double min)
        {
            double a = allRandom.NextDouble();
            double b = inputVar * (scale / 2) * a;
            if (allRandom.Next(2) == 1)
            {
                if ((inputVar - b) < min)
                {
                    return min;
                }
                return (inputVar - b);
            }

            if ((inputVar + b) > max)
            {
                return max;
            }
            return (inputVar + b);
        }

        double globalInit(double inputVar, double scale, double max, double min)
        {
            double a = min + ((max - min) * allRandom.NextDouble());
            if (a > max)
                return max;
            return a;
        }


        void fullInit()
        {
            populationMassive = new KnowlegeBaseTSARules[currentConf.GENCPopulationSize];
            childrenMassive = new KnowlegeBaseTSARules[currentConf.GENCCountChild];

            for (int i = 0; i < currentConf.GENCPopulationSize; i++)
            {
                populationMassive[i] = new KnowlegeBaseTSARules(fullFuzzySystem.RulesDatabaseSet[0]);
            }



            //Parallel.For(1, (currentConf.Особей_в_популяции - 1), i =>
            for (int i = 1; i < currentConf.GENCPopulationSize; i++)
            {
                double[] tempMassive = new double[populationMassive[i].all_conq_of_rules.Count()];
                //Parallel.For(0, (populationMassive[i].all_conq_of_rules.Count() - 1), j =>

                for (int j = 0; j < populationMassive[i].all_conq_of_rules.Count(); j++)
                {
                    tempMassive[j] = initFunc(populationMassive[i].all_conq_of_rules[j],
                        currentConf.GENCScateDeverceInit,
                        fullFuzzySystem.LearnSamplesSet.OutputAttribute.Max,
                        fullFuzzySystem.LearnSamplesSet.OutputAttribute.Min);
                }
                populationMassive[i].all_conq_of_rules = tempMassive;
                //});


                for (int j = 0; j < populationMassive[i].TermsSet.Count; j++)
                //Parallel.For(0, populationMassive[i].TermsSet.Count, j =>
                {
                    tempMassive = populationMassive[i].TermsSet[j].Parametrs.Clone() as double[];
                    for (int u = 0; u < populationMassive[i].TermsSet[j].Parametrs.Count(); u++)
                    //Parallel.For(0, populationMassive[i].TermsSet[j].Parametrs.Count(), u =>
                    {
                        tempMassive[u] = initFunc(tempMassive[u],
                            currentConf.GENCScateDeverceInit,
                            fullFuzzySystem.LearnSamplesSet.InputAttributes[populationMassive[i].TermsSet[j].NumVar].Max,
                            fullFuzzySystem.LearnSamplesSet.InputAttributes[populationMassive[i].TermsSet[j].NumVar].Min);
                        //    result.LearnSamplesSet.InputAttributeMax(fuzzyRulesDatabaseMassive[i].TermsSet[j].Max)

                        //  fuzzyRulesDatabaseMassive[i].TermsSet[j] .Parametrs[u]
                        //});
                    }
                    populationMassive[i].TermsSet[j].Parametrs = tempMassive;
                }
                //});
            }
            //});

        }



        #endregion

        #region mutation

        public void mutationAlg()
        {
            //Parallel.For(0, inputMassive.Count(), i =>
            for (int i = 0; i < childrenMassive.Count(); i++)
            {
                for (int j = 0; j < childrenMassive[i].TermsSet.Count; j++)
                //Parallel.For(0, inputMassive[i].TermsSet.Count, j =>
                {
                    double a = allRandom.NextDouble();
                    if (a <= currentConf.GENCScateDeverceMutate)
                    {
                        for (int u = 0; u < childrenMassive[i].TermsSet[j].Parametrs.Count(); u++)
                        //Parallel.For(0, inputMassive[i].TermsSet[j].Parametrs.Count(), u =>
                        {
                            childrenMassive[i].TermsSet[j].Parametrs[u] = globalInit(childrenMassive[i].TermsSet[j].Parametrs[u],
                            currentConf.GENCScateDeverceInit,
                            fullFuzzySystem.LearnSamplesSet.InputAttributes[childrenMassive[i].TermsSet[j].NumVar].Max,
                            fullFuzzySystem.LearnSamplesSet.InputAttributes[childrenMassive[i].TermsSet[j].NumVar].Min);
                        }
                        //});
                        Array.Sort(childrenMassive[i].TermsSet[j].Parametrs);
                    }
                }
                //});
                double[] tempMassive = childrenMassive[i].all_conq_of_rules.Clone() as double[];
                //Parallel.For(0, inputMassive[i].all_conq_of_rules.Count(), j =>

                for (int j = 0; j < tempMassive.Count(); j++)
                {
                    double a = allRandom.NextDouble();
                    if (a <= currentConf.GENCScateDeverceMutate)
                    {
                        tempMassive[j] = globalInit(tempMassive[j],
                            currentConf.GENCScateDeverceInit,
                            fullFuzzySystem.LearnSamplesSet.OutputAttribute.Max,
                            fullFuzzySystem.LearnSamplesSet.OutputAttribute.Min);
                    }
                }
                //});
                //lock (inputMassive[i].all_conq_of_rules)
                //{
                childrenMassive[i].all_conq_of_rules = tempMassive;
                //}
                //});
            }


        }

        #endregion


        #region crossover

        KnowlegeBaseTSARules unifiedCrossover(KnowlegeBaseTSARules parent1, KnowlegeBaseTSARules parent2)
        {
            double b = (currentConf.GENCPopabilityCrossover / 100);
            KnowlegeBaseTSARules child;
            child = new KnowlegeBaseTSARules(parent1);
            for (int i = 0; i < parent1.TermsSet.Count; i++)
            //Parallel.For(0, parent1.TermsSet.Count, i =>
            {
                if (b > allRandom.NextDouble())
                {
                    child.TermsSet[i].Parametrs = parent2.TermsSet[i].Parametrs;


                }
                //});
            }


            double[] tempMassiveCh = child.all_conq_of_rules.Clone() as double[];
            double[] tempMassiveP2 = parent2.all_conq_of_rules.Clone() as double[];
            for (int i = 0; i < tempMassiveCh.Count(); i++)
            //Parallel.For(0, parent1.all_conq_of_rules.Count(), i =>
            {
                if (b > allRandom.NextDouble())
                    tempMassiveCh[i] = tempMassiveP2[i];
            }
            child.all_conq_of_rules = tempMassiveCh;
            //});
            return child;
        }

        KnowlegeBaseTSARules pointsCrossover(KnowlegeBaseTSARules parent1, KnowlegeBaseTSARules parent2)
        {
            KnowlegeBaseTSARules child = new KnowlegeBaseTSARules(parent1);
            double controlPoint = Math.Round((((parent1.all_conq_of_rules.Count()) + (parent1.TermsSet.Count)))
                / (currentConf.GENCPointCrossover));
            int counter = 0;
            bool selector = false;
            for (int j = 0; j < parent1.TermsSet.Count; j++)
            {
                if (selector)
                {
                    child.TermsSet[j].Parametrs = parent2.TermsSet[j].Parametrs;
                }

                if (counter >= controlPoint)
                {
                    selector = !selector;
                    counter = 0;
                }
                counter++;

            }

            for (int j = 0; j < parent1.all_conq_of_rules.Count(); j++)
            {
                if (selector)
                {
                    child.all_conq_of_rules[j] = parent2.all_conq_of_rules[j];
                }

                if (counter >= controlPoint)
                {
                    selector = !selector;
                    counter = 0;
                }
                counter++;
            }
            return child;

        }


        public void fullCrossover()
        {
            //     childrenMassive = new KnowlegeBaseSARules[currentConf.Количество_генерируемых_потомков];


            //Parallel.For(0, currentConf.Количество_генерируемых_потомков, i =>
            for (int i = 0; i < currentConf.GENCCountChild; i++)
            {
                int a = allRandom.Next(populationMassive.Count());
                int b = allRandom.Next(populationMassive.Count());
                childrenMassive[i] = new KnowlegeBaseTSARules(crossoverFunc(populationMassive[a], populationMassive[b]));
            }
            //});

        }

        #endregion




        #region Selection
        public void randomSelection()
        {
            for (int i = 0; i < childrenMassive.Count(); i++)
            //Parallel.For(0, childrenMassive.Count(), i => 
            {
                fullFuzzySystem.RulesDatabaseSet.Add(childrenMassive[i]);
                fullFuzzySystem.UnlaidProtectionFix(childrenMassive[i]);
            }

            int a = allRandom.Next(childrenMassive.Count());
            List<int> indexMassive = new List<int>();
            //Parallel.For(0, current.Count(), i =>
            for (int i = 0; i < populationMassive.Count(); i++)
            {
                while (indexMassive.Contains(a))
                    a = allRandom.Next(childrenMassive.Count());
                populationMassive[i] = childrenMassive[a];
                indexMassive.Add(a);
                //});
            }
        }

        public void eliteSelection()
        {

            double[] currentError = new double[childrenMassive.Count()];
            for (int i = 0; i < childrenMassive.Count(); i++)
            //Parallel.For(0, childrenMassive.Count(), i => 
            {
                fullFuzzySystem.RulesDatabaseSet.Add(childrenMassive[i]);
                fullFuzzySystem.UnlaidProtectionFix(childrenMassive[i]);

                currentError[i] = fullFuzzySystem.approxLearnSamples(fullFuzzySystem.RulesDatabaseSet[ i + 1]);
            }
            //});
            Array.Sort(currentError, childrenMassive);
            populationMassive = childrenMassive.ToList().GetRange(0, populationMassive.Count()).ToArray();
            fullFuzzySystem.RulesDatabaseSet.RemoveRange(1, childrenMassive.Count());
        }

        public void rouletteSelection()
        {
            double[] currentError = new double[childrenMassive.Count()];
            double efficient = 0;
            for (int i = 0; i < childrenMassive.Count(); i++)
            //Parallel.For(0, childrenMassive.Count(), i =>
            {
                fullFuzzySystem.RulesDatabaseSet.Add(childrenMassive[i]);
                fullFuzzySystem.UnlaidProtectionFix(childrenMassive[i]);
                currentError[i] = fullFuzzySystem.approxLearnSamples(fullFuzzySystem.RulesDatabaseSet [i + 1]);

                efficient += currentError[i];
                //});
            }
            for (int i = 0; i < currentError.Count(); i++)
            //Parallel.For(0, currentError.Count(), i =>
            {
                currentError[i] = (1 - (currentError[i] / efficient));
                //});
            }
            Array.Sort(currentError, childrenMassive);
            for (int i = 0; i < populationMassive.Count(); i++)
            //Parallel.For(0, current.Count(), i =>
            {
                double a = allRandom.NextDouble();
                double summ = 0;
                for (int j = 0; j < currentError.Count(); j++)
                {
                    if ((a >= summ) && (a <= (summ + currentError[j])))
                    {

                        populationMassive[i] = childrenMassive[j];
                        break;
                    }
                    summ += currentError[j];
                }
            }
            //});
            fullFuzzySystem.RulesDatabaseSet.RemoveRange(1, childrenMassive.Count());
        }

        public void fullSelection()
        {

            for (int i = 0; i < populationMassive.Count(); i++)
            //Parallel.For(0, populationMassive.Count(), i =>
            {
                selectionFunc();
                fullFuzzySystem.RulesDatabaseSet.Add(populationMassive[i]);
                //});
            }



        }

        #endregion



        #region findBest

        public int findMinErrorElement()
        {
            int step = 0;
            double error = double.PositiveInfinity;
            for (int i = 0; i < fullFuzzySystem.RulesDatabaseSet.Count; i++)
            //Parallel.For(0, result.Count_Rulles_Databases, i =>
            {
                if (error > fullFuzzySystem.approxLearnSamples(result.RulesDatabaseSet [i]))
                {
                    step = i;
                    error = fullFuzzySystem.approxLearnSamples(result.RulesDatabaseSet [i]);
                }
            }
            //});
            return step;
        }


        #endregion



        #region intefaceImplement

        public virtual void Init(ILearnAlgorithmConf conf)
        {
            currentConf = conf as GeneticConf;
            fullFuzzySystem = result;
            step = 0;
            errorAfter = 0;
            errorBefore = result.approxLearnSamples(result.RulesDatabaseSet[ 0]);
             backUp = result.RulesDatabaseSet[0];

            initFunc = new initFuncType(localInit);
            if (currentConf.GENCTypeInit == GeneticConf.Alg_Init_Type.Глобальный)
            {
                initFunc = new initFuncType(globalInit);
            }

            crossoverFunc = new crossoverFuncType(unifiedCrossover);
            if (currentConf.GENCTypeCrossover == GeneticConf.Alg_Crossover_Type.Многоточечный)
            {
                crossoverFunc = new crossoverFuncType(pointsCrossover);
            }

            selectionFunc = new selectionFuncType(rouletteSelection);
            if (currentConf.GENCTypeSelection == GeneticConf.Alg_Selection_Type.Случайный)
            {
                selectionFunc = new selectionFuncType(randomSelection);
            }
            if (currentConf.GENCTypeSelection == GeneticConf.Alg_Selection_Type.Элитарный)
            {
                selectionFunc = new selectionFuncType(eliteSelection);
            }
            fullInit(); // Здесь проходит инициализация
        }


        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.Singletone };
            }
        }

        public override TSAFuzzySystem TuneUpFuzzySystem(TSAFuzzySystem Approx, ILearnAlgorithmConf conf)
        { result = Approx;
           Init(conf);
           
            
            for (int i = 0; i < currentConf.GENCCountIteration; i++)
            {
                oneIterate(result);
            }
            Final();
            Approx.RulesDatabaseSet[0].TermsSet.Trim();
            return result;
        }

        public virtual void oneIterate(TSAFuzzySystem result)
        {
            fullCrossover(); // здесь проходит скрещивание 
            mutationAlg();// здесь проходит мутация
            fullSelection(); //здесь проходит селекция
            step = findMinErrorElement(); // поиск лучшей базы знаний 
            result.RulesDatabaseSet[0] = new KnowlegeBaseTSARules(result.RulesDatabaseSet[step]);
            result.RulesDatabaseSet.RemoveRange(1, (result.RulesDatabaseSet.Count - 1));
         }

        public virtual void Final()
        {
            errorAfter = result.approxLearnSamples(result.RulesDatabaseSet[ 0]);
            if (errorAfter > errorBefore)
            {
                result.RulesDatabaseSet[0] = backUp;
            }
            GC.Collect();
        }


        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Генетический алгоритм{";
                result += "Количество итераций " + currentConf.GENCCountIteration + Environment.NewLine;
                result += "Вероятность скрещивания " + currentConf.GENCPopabilityCrossover + Environment.NewLine;
                result += "Доля отклонения при инициализации" + currentConf.GENCScateDeverceInit + Environment.NewLine;
                result += "Доля отклонения при мутации " + currentConf.GENCScateDeverceMutate + Environment.NewLine;
                result += "Количество генерируемых потомков " + currentConf.GENCCountChild + Environment.NewLine;
                result += "Особей в популяции " + currentConf.GENCPopulationSize + Environment.NewLine;
                result += "Тип инициализации " + currentConf.GENCTypeInit + Environment.NewLine;
                result += "Тип селекции " + currentConf.GENCTypeSelection + Environment.NewLine;
                result += "Тип скрещивания " + currentConf.GENCTypeCrossover + Environment.NewLine;
                result += "Точка деления " + currentConf.GENCPointCrossover + Environment.NewLine;
                result += "}";
                return result;
            }
            return "Генетический алгоритм";
        }

        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            GeneticConf currentGeneticConf = new GeneticConf();
            currentGeneticConf.Init(CountFeatures);
            return currentGeneticConf;
        }

        #endregion

    }
}

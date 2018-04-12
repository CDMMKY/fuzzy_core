using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySystem.PittsburghClassifier;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using FuzzySystem.FuzzyAbstract;

namespace GeneticAlgorithmTune
{
    public delegate double initFuncTypeClassifier(double inputVar, double scale, double max, double min);
    public delegate KnowlegeBasePCRules crossoverFuncTypeClassifier(KnowlegeBasePCRules parent1, KnowlegeBasePCRules parent2);
    public delegate void selectionFuncTypeClassifier();
   public class GeneticClassifier:AbstractNotSafeLearnAlgorithm
    {
        #region privateMembers
       protected Random allRandom = new Random();
       protected GeneticConf currentConf;
       protected initFuncTypeClassifier initFunc;
       protected crossoverFuncTypeClassifier crossoverFunc;
       protected selectionFuncTypeClassifier selectionFunc;
       protected KnowlegeBasePCRules[] populationMassive;
       protected KnowlegeBasePCRules[] childrenMassive;
       protected PCFuzzySystem fullFuzzySystem;
       protected int step;
       protected double errorAfter;
       protected double errorBefore;
          

        #endregion

        double localInit(double inputVar, double scale, double max, double min)
        {
            double a = allRandom.NextDouble();
            double b = inputVar * (scale / 2) * a;
            if (allRandom.Next(2) == 1)
            {
                if ((inputVar - b) < min)
                    return min;
                return (inputVar - b);
            }

            if ((inputVar + b) > max)
                return max;
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
            populationMassive = new KnowlegeBasePCRules[currentConf.GENCPopulationSize];
            childrenMassive = new KnowlegeBasePCRules[currentConf.GENCCountChild];

            for (int i = 0; i < currentConf.GENCPopulationSize; i++)
            {
                populationMassive[i] = new KnowlegeBasePCRules(fullFuzzySystem.RulesDatabaseSet[0]);
            }



            //Parallel.For(1, (currentConf.Особей_в_популяции - 1), i =>
            for (int i = 1; i < currentConf.GENCPopulationSize; i++)
            {
                double[] tempMassive = new double[populationMassive[i].Weigths.Count()];
                //Parallel.For(0, (populationMassive[i].all_conq_of_rules.Count() - 1), j =>

                for (int j = 0; j < populationMassive[i].Weigths.Count(); j++)
                {
                    tempMassive[j] = initFunc(populationMassive[i].Weigths[j],
                        currentConf.GENCScateDeverceInit,
                        fullFuzzySystem.LearnSamplesSet.OutputAttribute.Max,
                        fullFuzzySystem.LearnSamplesSet.OutputAttribute.Min);
                }
                populationMassive[i].Weigths = tempMassive;
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
                            fullFuzzySystem.LearnSamplesSet.InputAttributes [populationMassive[i].TermsSet[j].NumVar].Max,
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

        KnowlegeBasePCRules unifiedCrossover(KnowlegeBasePCRules parent1, KnowlegeBasePCRules parent2)
        {
            double b = (currentConf.GENCPopabilityCrossover / 100);
            KnowlegeBasePCRules child;
            child = new KnowlegeBasePCRules(parent1);
            for (int i = 0; i < parent1.TermsSet.Count; i++)
            //Parallel.For(0, parent1.TermsSet.Count, i =>
            {
                if (b > allRandom.NextDouble())
                {
                    child.TermsSet[i].Parametrs = parent2.TermsSet[i].Parametrs;


                }
                //});
            }


            double[] tempMassiveCh = child.Weigths.Clone() as double[];
            double[] tempMassiveP2 = parent2.Weigths.Clone() as double[];
            for (int i = 0; i < tempMassiveCh.Count(); i++)
            //Parallel.For(0, parent1.all_conq_of_rules.Count(), i =>
            {
                if (b > allRandom.NextDouble())
                    tempMassiveCh[i] = tempMassiveP2[i];
            }
            child.Weigths = tempMassiveCh;
            //});
            return child;
        }

        KnowlegeBasePCRules pointsCrossover(KnowlegeBasePCRules parent1, KnowlegeBasePCRules parent2)
        {
            KnowlegeBasePCRules child = new KnowlegeBasePCRules(parent1);
            double controlPoint = Math.Round((((parent1.Weigths.Count()) + (parent1.TermsSet.Count)))
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

            for (int j = 0; j < parent1.Weigths.Count(); j++)
            {
                if (selector)
                {
                    child.Weigths[j] = parent2.Weigths[j];
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
                childrenMassive[i] = new KnowlegeBasePCRules(crossoverFunc(populationMassive[a], populationMassive[b]));
            }
            //});

        }

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

                currentError[i] = ( fullFuzzySystem.ErrorLearnSamples(fullFuzzySystem.RulesDatabaseSet[ i + 1]));
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
                currentError[i] = fullFuzzySystem.ClassifyLearnSamples(fullFuzzySystem.RulesDatabaseSet[ i + 1]);

                efficient += currentError[i];
                //});
            }
            for (int i = 0; i < currentError.Count(); i++)
            //Parallel.For(0, currentError.Count(), i =>
            {
                currentError[i] = ((currentError[i] / efficient));
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
                double[] tempMassive = childrenMassive[i].Weigths.Clone() as double[];
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
                childrenMassive[i].Weigths = tempMassive;
                //}
                //});
            }


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

        public int findMinErrorElement()
        {
            int step = 0;
            double error = double.PositiveInfinity;
            for (int i = 0; i < fullFuzzySystem.RulesDatabaseSet.Count; i++)
            //Parallel.For(0, result.Count_Rulles_Databases, i =>
            {
                if (error > ( fullFuzzySystem.ErrorLearnSamples(fullFuzzySystem.RulesDatabaseSet[ i])))
                {
                    step = i;
                    error = (fullFuzzySystem.ErrorLearnSamples(fullFuzzySystem.RulesDatabaseSet [i]));
                }
            }
            //});
            return step;
        }

        public override PCFuzzySystem TuneUpFuzzySystem(PCFuzzySystem Classifier, ILearnAlgorithmConf conf)
        {
            PCFuzzySystem result = Classifier;
            fullFuzzySystem = result;

            Init(conf);

            for (int i = 0; i < currentConf.GENCCountIteration; i++)
            {
                oneIterate(fullFuzzySystem);

            }
            Final();
            result.RulesDatabaseSet[0].TermsSet.Trim();
            return result;
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
                result += "Точка деления " + currentConf.GENCPointCrossover + Environment.NewLine ;
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


        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.PittsburghClassifier };
            }
        }

        public virtual void oneIterate(PCFuzzySystem result)
        {
            fullCrossover(); // здесь проходит скрещивание 
            mutationAlg();// здесь проходит мутация
            fullSelection(); //здесь проходит селекция
            step = findMinErrorElement(); // поиск лучшей базы знаний 
            result.RulesDatabaseSet[0] = new KnowlegeBasePCRules(result.RulesDatabaseSet[step]);
            result.RulesDatabaseSet.RemoveRange(1, (result.RulesDatabaseSet.Count - 1));
            GC.Collect();
        }

        public virtual void Init(ILearnAlgorithmConf Config)
        {
            step = 0;
            errorAfter = 0;
            errorBefore = fullFuzzySystem.ClassifyLearnSamples(fullFuzzySystem.RulesDatabaseSet[ 0]);
            currentConf = Config as GeneticConf;

            initFunc = new initFuncTypeClassifier(localInit);
            if (currentConf.GENCTypeInit == GeneticConf.Alg_Init_Type.Глобальный)
            {
                initFunc = new initFuncTypeClassifier(globalInit);
            }

            crossoverFunc = new crossoverFuncTypeClassifier(unifiedCrossover);
            if (currentConf.GENCTypeCrossover == GeneticConf.Alg_Crossover_Type.Многоточечный)
            {
                crossoverFunc = new crossoverFuncTypeClassifier(pointsCrossover);
            }



            selectionFunc = new selectionFuncTypeClassifier(rouletteSelection);
            if (currentConf.GENCTypeSelection == GeneticConf.Alg_Selection_Type.Случайный)
            {
                selectionFunc = new selectionFuncTypeClassifier(randomSelection);
            }
            if (currentConf.GENCTypeSelection == GeneticConf.Alg_Selection_Type.Элитарный)
            {
                selectionFunc = new selectionFuncTypeClassifier(eliteSelection);
            }

            fullInit(); // Здесь проходит инициализация

        }

        public virtual void Final()
        {
            errorAfter = fullFuzzySystem.ClassifyLearnSamples(fullFuzzySystem.RulesDatabaseSet[ 0]);

        }
    }
}

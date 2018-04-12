using Fuzzy_system.Fuzzy_Abstract.learn_algorithm.conf;
using FuzzySystem.FuzzyAbstract.conf;
//using FuzzySystem.PittsburghClassifier;
using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySystem.FuzzyAbstract;


namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm
{

    public class GsaClass : AbstractNotSafeLearnAlgorithm
    {
        Random rand = new Random();
        // int number;
        double G, G0, alpha, epsilon;
        int iterMax, MCount;
        double[] Errors, mass;
        KnowlegeBasePCRules[] X;
        double[][, ,] R;
        double[,] RR;
        double[, ,] a, speed;
        PCFuzzySystem theFuzzySystem;

        double ErrorBest;
        KnowlegeBasePCRules BestSolution;
        double minValue;
        int iminIndex;
        double ErrorZero;
        KnowlegeBasePCRules temp_c_Rule;

        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.PittsburghClassifier};
            }
        }


        public override PCFuzzySystem TuneUpFuzzySystem(PCFuzzySystem Class, ILearnAlgorithmConf conf) // Здесь ведется оптимизация вашим алгоритмом
        {
            theFuzzySystem = Class;
        //    Console.WriteLine(theFuzzySystem.RulesDatabaseSet[0].TermsSet.Count);
                      iterMax = ((gsa_conf)conf).Количество_итераций;
            MCount = ((gsa_conf)conf).Количество_частиц;
            G0 = ((gsa_conf)conf).Гравитационная_постоянная;
            alpha = ((gsa_conf)conf).Коэффициент_уменьшения;
            epsilon = ((gsa_conf)conf).Малая_константа;
            X = new KnowlegeBasePCRules[MCount];
            Errors = new double[MCount];
            mass = new double[MCount];
           
            temp_c_Rule = new KnowlegeBasePCRules(theFuzzySystem.RulesDatabaseSet[0]);
            X[0] = temp_c_Rule;
            Errors[0] = theFuzzySystem.ErrorLearnSamples(X[0]); 
            
            ErrorZero = Errors[0];
            ErrorBest = Errors[0];
            BestSolution = new KnowlegeBasePCRules(theFuzzySystem.RulesDatabaseSet[0]);
            
            R = new double[MCount][, ,];
            speed = new double[MCount, X[0].TermsSet.Count, X[0].TermsSet[0].Parametrs.Count()];
            for (int i = 0; i < MCount; i++)
            {
                R[i] = new double[MCount, X[0].TermsSet.Count, X[0].TermsSet[0].Parametrs.Count()];
            }
            RR = new double[MCount, MCount];
            a = new double[MCount, X[0].TermsSet.Count, X[0].TermsSet[0].Parametrs.Count()];

            for (int i = 1; i < MCount; i++)
            {
                temp_c_Rule = new KnowlegeBasePCRules(theFuzzySystem.RulesDatabaseSet[0]);
                X[i] = temp_c_Rule;
                for (int j = 0; j < X[i].TermsSet.Count; j++)
                {
                    for (int k = 0; k < X[i].TermsSet[j].Parametrs.Count(); k++)
                        X[i].TermsSet[j].Parametrs[k] = GaussRandom.Random_gaussian(rand, X[i].TermsSet[j].Parametrs[k], 0.1 * (X[i].TermsSet[j].Parametrs[k])) + theFuzzySystem.LearnSamplesSet.InputAttributes[X[i].TermsSet[j].NumVar].Scatter * 0.05;
                }

                //theFuzzySystem.RulesDatabaseSet.Add(X[i]);
                //theFuzzySystem.UnlaidProtectionFix(theFuzzySystem.RulesDatabaseSet.Count - 1);
                //Errors[i] = theFuzzySystem.ErrorLearnSamples(X[i]);
                //X[i] = theFuzzySystem.RulesDatabaseSet[theFuzzySystem.RulesDatabaseSet.Count - 1];
                //theFuzzySystem.RulesDatabaseSet.Remove(X[i]);
                theFuzzySystem.UnlaidProtectionFix(X[i]);
                Errors[i] = theFuzzySystem.ErrorLearnSamples(X[i]);
            }

            for (int iter = 0; iter < iterMax; iter++)
            {
                //g(t) = G(0)*e^(-a*t/T);
                G = G0 * Math.Pow(Math.E, ((-1) * alpha * iter / iterMax));
                
                algorithm();

                for (int r = 0; r < MCount; r++)
                {
                    theFuzzySystem.UnlaidProtectionFix(X[r]);
                    Errors[r] = theFuzzySystem.  ErrorLearnSamples(X[r]);
                }

                minValue = Errors.Min();
                iminIndex = Errors.ToList().IndexOf(minValue);

                if (minValue < ErrorBest)
                {
                    ErrorBest = minValue;
                    BestSolution = new KnowlegeBasePCRules(X[iminIndex]);
                }
            }
            //theFuzzySystem.RulesDatabaseSet[0] = BestSolution;

            if (ErrorBest < ErrorZero)
            {
                theFuzzySystem.RulesDatabaseSet[0] = BestSolution;
            }

            return theFuzzySystem;
        }
        public override string ToString(bool with_param = false)// без параметров возвращает имя алгоритма, с параметров true возвращает имя алгоритма и значения его параметров
        {
            if (with_param)
            {
                string result = "Гравитационный алгоритм{";
                result += "Итераций = " + iterMax.ToString() + " ;" + Environment.NewLine;
                result += "G0 = " + G0.ToString() + " ;" + Environment.NewLine;
                result += "Коэффициент альфа = " + alpha.ToString() + " ;" + Environment.NewLine;
                result += "Коэффициент эпсилон = " + epsilon.ToString() + " ;" + Environment.NewLine;
                result += "Особей в популяции = " + MCount.ToString() + " ;" + Environment.NewLine;
                result += "}";
                return result;
            }
            return "Гравитационный алгоритм";
        }
        public override ILearnAlgorithmConf getConf(int CountFeatures) // Создание класса конфигуратора для вашего метода
        {
            ILearnAlgorithmConf result = new gsa_conf();
            result.Init(CountFeatures);
            return result;
        }
        public void algorithm()
        {
            //Вычисление масс
            weight();
            //Вычисление расстояний
            distance();
            //Ускорение
            acceleration();
            //Скорость и перемещение
            velocity();
        }
        private void weight()
        {
            double sum = 0;
            double best = mass[0];
            double worst = mass[0];
            int[] index = new Int32[MCount];
            int count = 0;
            for (int i = 1; i < MCount; i++)
            {
                mass[i] = Errors[i];
                if (mass[i] > best) best = mass[i];
                if (mass[i] < worst) worst = mass[i];
            }
            for (int i = 0; i < MCount; i++)
            {
                if (mass[i] == best)
                {
                    count++;
                    index[count - 1] = i;
                }
            }
            if (count > 1)
            {
                for (int i = 1; i < count; i++)
                {
                    ///X[index[i]] = ;
                    int f = index[i];
                    KnowlegeBasePCRules temp_c_Rule = new KnowlegeBasePCRules(theFuzzySystem.RulesDatabaseSet[0]);
                    temp_c_Rule = new KnowlegeBasePCRules(theFuzzySystem.RulesDatabaseSet[0]);
                    X[f] = temp_c_Rule;
                    for (int j = 0; j < X[f].TermsSet.Count; j++)
                    {
                        for (int k = 0; k < X[f].TermsSet[j].Parametrs.Count(); k++)
                            X[f].TermsSet[j].Parametrs[k] = GaussRandom.Random_gaussian(rand, X[f].TermsSet[j].Parametrs[k], 0.1 * (X[f].TermsSet[j].Parametrs[k])) + theFuzzySystem.LearnSamplesSet.InputAttributes[X[f].TermsSet[j].NumVar].Scatter * 0.05;
                    }
                    theFuzzySystem.UnlaidProtectionFix(X[f]);
                    Errors[f] = theFuzzySystem.ErrorLearnSamples(X[f]);
                    mass[f] = Errors[f];
                    if (mass[f] > best) i--;
                }
            }
            for (int i = 0; i < MCount; i++)
            {
                mass[i] = (mass[i] - worst) / (best - worst);
                sum = sum + mass[i];
            }
            for (int i = 0; i < MCount; i++)
            {
                mass[i] = mass[i] / sum;
            }

        }
        private void distance()
        {
            for (int i = 0; i < MCount; i++)
            {
                for (int j = 0; j < MCount; j++)
                {
                    double sum = 0;
                    for (int h = 0; h < X[j].TermsSet.Count; h++)
                    {
                        for (int t = 0; t < X[j].TermsSet[h].Parametrs.Count(); t++)
                        {
                            R[i][j, h, t] = X[j].TermsSet[h].Parametrs[t] - X[i].TermsSet[h].Parametrs[t];
                            sum = sum + R[i][j, h, t] * R[i][j, h, t];
                        }
                    }
                    RR[i, j] = Math.Sqrt(sum);
                }
            }
        }
        public void acceleration()
        {
            for (int i = 0; i < MCount; i++)
            {
                for (int j = 0; j < X[i].TermsSet.Count; j++)
                {
                    for (int t = 0; t < X[i].TermsSet[j].Parametrs.Length; t++)
                    {

                        a[i, j, t] = 0;
                        for (int h = 0; h < MCount; h++)
                        {
                            a[i, j, t] = a[i, j, t] + rand.NextDouble() * (mass[h] * R[i][h, j, t]) / (RR[i, h] + epsilon);
                        }
                        a[i, j, t] = a[i, j, t] * G;
                    }
                }
            }
        }

        public void velocity()
        {
            for (int i = 0; i < MCount; i++)
            {
                for (int j = 0; j < X[i].TermsSet.Count; j++)
                {
                    for (int t = 0; t < X[i].TermsSet[j].Parametrs.Length; t++)
                    {
                        speed[i, j, t] = rand.NextDouble() * speed[i, j, t] + a[i, j, t];
                    }
                }
            }
            for (int i = 0; i < MCount; i++)
            {
                for (int j = 0; j < X[i].TermsSet.Count; j++)
                {
                    for (int k = 0; k < X[i].TermsSet[j].Parametrs.Count(); k++)
                    {
                        X[i].TermsSet[j].Parametrs[k] = X[i].TermsSet[j].Parametrs[k] + speed[i, j, k];

                    }

                    //if (X[i].TermsSet[j].Min < theFuzzySystem.Learn_Samples_set.InputAttributeMin(X[i].TermsSet[j].NumberOfInputVar))
                    //{
                    //    X[i].TermsSet[j].Min = theFuzzySystem.Learn_Samples_set.InputAttributeMin(X[i].TermsSet[j].NumberOfInputVar) - theFuzzySystem.Learn_Samples_set.InputAttributeScatter(X[i].TermsSet[j].NumberOfInputVar) * 0.1;
                    //}

                    //if (X[i].TermsSet[j].Min > theFuzzySystem.Learn_Samples_set.InputAttributeMax(X[i].TermsSet[j].NumberOfInputVar))
                    //{
                    //    X[i].TermsSet[j].Min = theFuzzySystem.Learn_Samples_set.InputAttributeMax(X[i].TermsSet[j].NumberOfInputVar) - theFuzzySystem.Learn_Samples_set.InputAttributeScatter(X[i].TermsSet[j].NumberOfInputVar) * 0.1;
                    //}


                    //if (X[i].TermsSet[j].Max < theFuzzySystem.Learn_Samples_set.InputAttributeMin(X[i].TermsSet[j].NumberOfInputVar))
                    //{
                    //    X[i].TermsSet[j].Max = theFuzzySystem.Learn_Samples_set.InputAttributeMin(X[i].TermsSet[j].NumberOfInputVar) + theFuzzySystem.Learn_Samples_set.InputAttributeScatter(X[i].TermsSet[j].NumberOfInputVar) * 0.1;
                    //}


                    //if (X[i].TermsSet[j].Max > theFuzzySystem.Learn_Samples_set.InputAttributeMax(X[i].TermsSet[j].NumberOfInputVar))
                    //{
                    //    X[i].TermsSet[j].Max = theFuzzySystem.Learn_Samples_set.InputAttributeMax(X[i].TermsSet[j].NumberOfInputVar) + theFuzzySystem.Learn_Samples_set.InputAttributeScatter(X[i].TermsSet[j].NumberOfInputVar) * 0.1;
                    //}

                }
            }
        }
    }
}




﻿using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm
{
    class WCAClassifer : AbstractNotSafeLearnAlgorithm
    {
        Random rand = new Random();
        protected WCAConfig Config;
        protected bool flag;
        protected int MaxIter, NRivers, Npop, Nsr, Nraindrops;
        protected double Dmax, con;
        protected int[] NS;
        protected PCFuzzySystem result;
        protected KnowlegeBasePCRules[] Population;
        public override PCFuzzySystem TuneUpFuzzySystem(PCFuzzySystem Class, ILearnAlgorithmConf Conf)
        {
            result = Class;
            List<int[]> groups = new List<int[]>();
            Init(Conf);
            SetPopulation();
            Population = SortRules(Population);
            NS = new int[Nsr];
            NS = SetNS(Population, Nsr);
            groups = GroupStream();
            double BestMSETest = result.ErrorTestSamples(Population[0]);
            double BestMSELearn = result.ErrorLearnSamples(Population[0]);
            int BestIter = 0;
            for (int i = 1; i <= MaxIter; i++)
            {
                Console.Clear();
                Console.WriteLine((double)i * 100 / MaxIter + "%");
                Population = SetNextPosition(groups, Population);
                Population = Replacement(groups, Population);
                if (flag)
                {
                    Evaporation(groups.Last());//Испарение
                }
                if (BestMSETest > result.ErrorTestSamples(Population[0]))
                {
                    BestMSETest = result.ErrorTestSamples(Population[0]);
                    BestMSELearn = result.ErrorLearnSamples(Population[0]);
                    BestIter = i;
                }
            }
            Console.WriteLine(ToString(true));
            Console.WriteLine("Итер - " + BestIter + " MSET - " + BestMSETest + " MSEL - " + BestMSELearn);
            result.RulesDatabaseSet[0] = Population[0];
            return result;
        }

        private void Init(ILearnAlgorithmConf Conf)
        {
            Config = Conf as WCAConfig;
            MaxIter = ((WCAConfig)Conf).Количество_итераций;
            Dmax = ((WCAConfig)Conf).Dmax;
            Npop = ((WCAConfig)Conf).Количество_капель;
            NRivers = ((WCAConfig)Conf).Количество_рек;
            con = ((WCAConfig)Conf).Константа;
            flag = ((WCAConfig)Conf).Испарение;
            Nsr = NRivers + 1;
            Nraindrops = Npop - Nsr;
        }
        private void SetPopulation()
        {
            Population = new KnowlegeBasePCRules[Npop];
            KnowlegeBasePCRules TempRule = new KnowlegeBasePCRules(result.RulesDatabaseSet[0]);
            Population[0] = TempRule;
            for (int i = 1; i < Npop; i++)
            {
                TempRule = new KnowlegeBasePCRules(result.RulesDatabaseSet[0]);
                Population[i] = TempRule;
                for (int j = 0; j < Population[i].TermsSet.Count; j++)
                {
                    for (int k = 0; k < Population[i].TermsSet[j].Parametrs.Length; k++)
                    {
                        Population[i].TermsSet[j].Parametrs[k] = GaussRandom.Random_gaussian(rand, Population[i].TermsSet[j].Parametrs[k], 0.1 * Population[i].TermsSet[j].Parametrs[k]);
                    }
                }
            }
        }
        private KnowlegeBasePCRules[] SortRules(KnowlegeBasePCRules[] Source)
        {
            double[] keys = new double[Source.Count()];
            KnowlegeBasePCRules[] tempSol = Source.Clone() as KnowlegeBasePCRules[];
            for (int i = 0; i < Source.Count(); i++)
            {
                keys[i] = result.ErrorLearnSamples(Source[i]);

            }
            Array.Sort(keys, tempSol);
            return Source;
        }
        private int[] SetNS(KnowlegeBasePCRules[] Populationt, int Nsrt)
        {
            double Sum = 0;
            int[] NSt = new int[Nsrt];
            for (int i = 0; i < Nsrt; i++)
            {
                Sum += result.ErrorLearnSamples(Populationt[i]);
            }
            for (int i = 0; i < Nsrt; i++)
            {
                double tmp = result.ErrorLearnSamples(Populationt[i]);
                NSt[i] = (int)Math.Round((tmp / Sum) * Nraindrops);
            }
            NSt[Nsrt - 1] += (Nraindrops - NSt.Sum());
            return NSt;
        }
        private List<int[]> GroupStream()
        {
            List<int[]> GroupsCalc = new List<int[]>();
            Dictionary<int, double> distances = new Dictionary<int, double>();
            for (int j = Nsr; j < Npop; j++)
            {
                distances.Add(j, 1);
            }
            int[] group0 = new int[NS[0] + NRivers + 1];
            foreach (int j in distances.Keys.ToArray())
            {
                distances[j] = Distance(Population[0], Population[j]);
            }
            group0[0] = 0;
            for (int j = 1; j <= NS[0]; j++)
            {
                var KeyMinValue = GetKeyByValue(distances, distances.Values.Min());
                group0[j] = KeyMinValue;
                distances.Remove(KeyMinValue);
            }
            for (int j = 1; j < Nsr; j++)
            {
                group0[j + NS[0]] = j;
            }
            GroupsCalc.Add(group0);
            for (int i = 1; i < Nsr; i++)
            {
                int[] group = new int[NS[i] + 1];
                foreach (int j in distances.Keys.ToArray())
                {
                    distances[j] = Distance(Population[i], Population[j]);
                }
                group[0] = i;
                for (int j = 1; j <= NS[i]; j++)
                {
                    var KeyMinValue = GetKeyByValue(distances, distances.Values.Min());
                    group[j] = KeyMinValue;
                    distances.Remove(KeyMinValue);
                }
                GroupsCalc.Add(group);
            }
            GroupsCalc.Reverse();
            return GroupsCalc;
        }
        private static int GetKeyByValue(Dictionary<int, double> myDictionary, double value)
        {
            foreach (var recordOfDictionary in myDictionary)
            {
                if (recordOfDictionary.Value.Equals(value))
                    return recordOfDictionary.Key;
            }
            return -1;
        }
        private double Distance(KnowlegeBasePCRules x, KnowlegeBasePCRules y)
        {
            double sum = 0;
            for (int i = 0; i < x.TermsSet.Count; i++)
            {
                for (int j = 0; j < x.TermsSet[i].Parametrs.Length; j++)
                {
                    sum += Math.Pow(x.TermsSet[i].Parametrs[j] - y.TermsSet[i].Parametrs[j], 2);
                }
            }
            return sum;
        }
        private KnowlegeBasePCRules[] SetNextPosition(List<int[]> groups, KnowlegeBasePCRules[] Population1)
        {
            foreach (var group in groups)
            {
                for (int i = 1; i < group.Length; i++)
                {
                    for (int j = 0; j < Population1[group[i]].TermsSet.Count; j++)
                    {
                        for (int k = 0; k < Population1[group[i]].TermsSet[j].Parametrs.Length; k++)
                        {
                            Population1[group[i]].TermsSet[j].Parametrs[k] = Population1[group[i]].TermsSet[j].Parametrs[k] + rand.NextDouble() * con * (Population1[group[0]].TermsSet[j].Parametrs[k] - Population1[group[i]].TermsSet[j].Parametrs[k]);
                        }
                    }
                }
            }
            return Population1;
        }
        private KnowlegeBasePCRules[] Replacement(List<int[]> groups, KnowlegeBasePCRules[] Population1)
        {
            foreach (var group in groups)
            {
                int MinInd = group[0];
                foreach (int i in group)
                {
                    if (result.ErrorLearnSamples(Population1[MinInd]) > result.ErrorLearnSamples(Population1[i]))
                    {
                        MinInd = i;
                    }
                }
                var tmp = Population1[group[0]];
                Population1[group[0]] = Population1[MinInd];
                Population1[MinInd] = tmp;
            }
            return Population1;
        }
        private void Evaporation(int[] group)
        {
            bool flagtmp = false;
            for (int i = 1; i < group.Length; i++)
            {
                if (Distance(Population[group[0]], Population[group[i]]) < Dmax)
                {
                    flagtmp = true;
                    for (int j = 0; j < Population[group[i]].TermsSet.Count; j++)
                    {
                        for (int k = 0; k < Population[group[i]].TermsSet[j].Parametrs.Length; k++)
                        {
                            double LB = Population[group[i]].TermsSet[j].Parametrs[k] - Dmax;
                            double UB = Population[group[i]].TermsSet[j].Parametrs[k] + Dmax;
                            Population[group[i]].TermsSet[j].Parametrs[k] = LB + rand.NextDouble() * (UB - LB);
                        }
                    }
                }
            }
            if (flagtmp)
            {
                Dmax -= (Dmax / MaxIter);
            }
        }
        public override ILearnAlgorithmConf getConf(int CountFeatures) // Создание класса конфигуратора для вашего метода
        {
            ILearnAlgorithmConf result = new WCAConfig();
            result.Init(CountFeatures);
            return result;
        }
        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.PittsburghClassifier };
            }
        }
        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Water cycle algorithm" + "  {" + Environment.NewLine;
                result += "Итераций= " + MaxIter.ToString() + " ;" + Environment.NewLine;
                result += "Капель= " + Npop.ToString() + " ;" + Environment.NewLine;
                result += "Рек= " + NRivers.ToString() + " ;" + Environment.NewLine;
                result += "Dmax= " + Dmax.ToString() + " ;" + Environment.NewLine;
                result += "const= " + con.ToString() + ";" + Environment.NewLine;
                result += "Испарение - " + flag + "; }" + Environment.NewLine;
                return result;
            }
            return "Water cycle algorithm";
        }
    }
}

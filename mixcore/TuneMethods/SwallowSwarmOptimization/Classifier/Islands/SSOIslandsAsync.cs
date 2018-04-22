using System;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using FuzzyCoreUtils;
using System.Linq;
using System.Threading.Tasks;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm
{
    class SSOIslandsAsync : AbstractNotSafeLearnAlgorithm
    {
        protected PCFuzzySystem result;
        protected Random rand = new Random();
        protected IslandsSSO Config;
        protected int MaxIter, numberOfLocalLeaders, numberOfAimlessParts, numberOfAllParts, numberOfPopulations, changeParts, iter;
        protected double ALocal, BLocal, AGlobal, BGlobal;
        protected List<KnowlegeBasePCRules> SSVector, HeadLeader, VelocityVector, VelocityVectorLL, VelocityVectorHL;
        protected KnowlegeBasePCRules Universal;
        protected List<KnowlegeBasePCRules[]> LocalLeaders, ExplorerParticles, AimlessParticles;
        protected List<List<KnowlegeBasePCRules>> Populations;
        protected List<Dictionary<KnowlegeBasePCRules, KnowlegeBasePCRules>> ParticlesBest;

        public override PCFuzzySystem TuneUpFuzzySystem(PCFuzzySystem Classify, ILearnAlgorithmConf conf)
        {
            preIterate(Classify, conf);
            //Оптимизируем параметры
            iter = 1;
            while (iter <= MaxIter)
            {
                if (iter % changeParts == 0)
                {
                    swapParticles();
                }
                oneIterate();
            }
            //Выводим точность классификации для лучшей частицы из каждой популяции
            for (int j = 0; j < Populations.Count; j++)
            {
                Populations[j] = ListPittsburgClassifierTool.SortRules(Populations[j], result);
                Console.WriteLine("Популяция №" + j + ":");
                Console.WriteLine("Обуч: " + Math.Round(result.ClassifyLearnSamples(Populations[j][0]), 2));
                Console.WriteLine("Тест: " + Math.Round(result.ClassifyTestSamples(Populations[j][0]), 2));
            }
            //Находим самое лучшее решение из всех популяций
            List<KnowlegeBasePCRules> BestPopulation = new List<KnowlegeBasePCRules>();
            for (int i = 0; i < numberOfPopulations; i++)
            {
                BestPopulation.Add(Populations[i][0]);
            }
            BestPopulation = ListPittsburgClassifierTool.SortRules(BestPopulation, result);
            result.RulesDatabaseSet[0] = BestPopulation[0];
            //Возвращаем результат
            return result;
        }
        public virtual void preIterate(PCFuzzySystem Classify, ILearnAlgorithmConf conf)
        {
            result = Classify;
            //Инициализируем параметры
            Init(conf);
            //Инициализируем и зануляем вектора алгоритма
            HeadLeader = new List<KnowlegeBasePCRules>();
            VelocityVector = new List<KnowlegeBasePCRules>();
            VelocityVectorLL = new List<KnowlegeBasePCRules>();
            VelocityVectorHL = new List<KnowlegeBasePCRules>();
            for (int i = 0; i < numberOfPopulations; i++)
            {
                HeadLeader.Add(new KnowlegeBasePCRules(result.RulesDatabaseSet[0]));
                VelocityVector.Add(new KnowlegeBasePCRules(result.RulesDatabaseSet[0]));
                VelocityVectorLL.Add(new KnowlegeBasePCRules(result.RulesDatabaseSet[0]));
                VelocityVectorHL.Add(new KnowlegeBasePCRules(result.RulesDatabaseSet[0]));
            }
            for (int p_i = 0; p_i < numberOfPopulations; p_i++)
            {
                for (int i = 0; i < VelocityVector[p_i].TermsSet.Count; i++)
                {
                    for (int j = 0; j < VelocityVector[p_i].TermsSet[i].Parametrs.Length; j++)
                    {
                        HeadLeader[p_i].TermsSet[i].Parametrs[j] = 0;
                        VelocityVector[p_i].TermsSet[i].Parametrs[j] = 0;
                        VelocityVectorLL[p_i].TermsSet[i].Parametrs[j] = 0;
                        VelocityVectorHL[p_i].TermsSet[i].Parametrs[j] = 0;
                    }
                }
            }
            //Создаем популяции и архив лучших положений каждой частицы
            Populations = new List<List<KnowlegeBasePCRules>>();
            for (int i = 0; i < numberOfPopulations; i++)
            {
                Populations.Add(SetPopulation(new List<KnowlegeBasePCRules>()));
            }
            ParticlesBest = new List<Dictionary<KnowlegeBasePCRules, KnowlegeBasePCRules>>();
            for (int p_i = 0; p_i < Populations.Count; p_i++)
            {
                ParticlesBest.Add(new Dictionary<KnowlegeBasePCRules, KnowlegeBasePCRules>());
                foreach (var Particle in Populations[p_i])
                {
                    ParticlesBest[p_i].Add(Particle, Universal);
                }
            }
            //Инициализируем роли частиц
            LocalLeaders = new List<KnowlegeBasePCRules[]>();
            ExplorerParticles = new List<KnowlegeBasePCRules[]>();
            AimlessParticles = new List<KnowlegeBasePCRules[]>();
            for (int p_i = 0; p_i < Populations.Count; p_i++)
            {
                LocalLeaders.Add(new KnowlegeBasePCRules[numberOfLocalLeaders]);
                ExplorerParticles.Add(new KnowlegeBasePCRules[numberOfAllParts - numberOfAimlessParts - numberOfLocalLeaders - 1]);
                AimlessParticles.Add(new KnowlegeBasePCRules[numberOfAimlessParts]);
            }
        }
        public virtual void swapParticles()
        {
            for (int i = 0; i <= numberOfLocalLeaders; i++)
            {
                KnowlegeBasePCRules tmp_part = new KnowlegeBasePCRules(Populations[0][i]);
                KnowlegeBasePCRules tmp_pos = new KnowlegeBasePCRules(ParticlesBest[0][Populations[0][i]]);
                for (int p_i = 1; p_i < Populations.Count; p_i++)
                {
                    KnowlegeBasePCRules tmp_p = new KnowlegeBasePCRules(ParticlesBest[p_i][Populations[p_i][i]]);
                    ParticlesBest[p_i].Remove(Populations[p_i - 1][i]);
                    Populations[p_i - 1][i] = new KnowlegeBasePCRules(Populations[p_i][i]);
                    ParticlesBest[p_i].Add(Populations[p_i - 1][i], tmp_p);
                }
                ParticlesBest[numberOfPopulations - 1].Remove(Populations[numberOfPopulations - 1][i]);
                Populations[numberOfPopulations - 1][i] = new KnowlegeBasePCRules(tmp_part);
                ParticlesBest[numberOfPopulations - 1].Add(Populations[numberOfPopulations - 1][i], tmp_pos);
            }
        }
        public virtual void oneIterate()
        {
            List<Task> AlgTasks = new List<Task>();
            for (int p_i = 0; p_i < Populations.Count; p_i++)
            {
                Task Algtask = new Task(() =>
                {
                    Populations[p_i] = ListPittsburgClassifierTool.SortRules(Populations[p_i], result);
                    SetRoles(Populations[p_i], p_i);
                    ChangeExplorersPositions(p_i);
                    ChangeAimlessPositions(p_i);
                    Populations[p_i] = DiscardRoles(Populations[p_i], p_i);
                }, TaskCreationOptions.LongRunning);
                AlgTasks.Add(Algtask);
                Algtask.Start();
            }
            iter++;
        }
        private List<KnowlegeBasePCRules> SetPopulation(List<KnowlegeBasePCRules> Population)
        {
            KnowlegeBasePCRules TempRule = new KnowlegeBasePCRules(result.RulesDatabaseSet[0]);
            Population.Add(TempRule);
            Universal = TempRule;
            for (int i = 1; i < numberOfAllParts; i++)
            {
                Population.Add(new KnowlegeBasePCRules(TempRule));
                for (int j = 0; j < Population[i].TermsSet.Count; j++)
                {
                    for (int k = 0; k < Population[i].TermsSet[j].Parametrs.Length; k++)
                    {
                        Population[i].TermsSet[j].Parametrs[k] = GaussRandom.Random_gaussian(rand, Population[i].TermsSet[j].Parametrs[k], 0.1 * Population[i].TermsSet[j].Parametrs[k]);
                    }
                }
                result.UnlaidProtectionFix(Population[i]);
            }
            Universal = new KnowlegeBasePCRules(TempRule);
            for (int i = 0; i < Universal.TermsSet.Count; i++)
            {
                for (int j = 0; j < Universal.TermsSet[i].Parametrs.Length; j++)
                {
                    Universal.TermsSet[i].Parametrs[j] = GaussRandom.Random_gaussian(rand, Universal.TermsSet[i].Parametrs[j], 0.1 * Universal.TermsSet[i].Parametrs[j]);
                }
            }

            return Population;
        }

        private void SetRoles(List<KnowlegeBasePCRules> Population, int p_i)
        {
            HeadLeader[p_i] = Population[0];
            for (int i = 1; i <= numberOfLocalLeaders; i++)
            {
                LocalLeaders[p_i][i - 1] = Population[i];
            }
            for (int i = numberOfAllParts - numberOfAimlessParts; i < numberOfAllParts; i++)
            {
                AimlessParticles[p_i][i - numberOfAllParts + numberOfAimlessParts] = Population[i];
            }
            for (int i = numberOfLocalLeaders + 1; i < numberOfAllParts - numberOfAimlessParts; i++)
            {
                ExplorerParticles[p_i][i - numberOfLocalLeaders - 1] = Population[i];
            }
        }

        private void ChangeExplorersPositions(int p_i)
        {
            int index;
            for (int i = 0; i < ExplorerParticles[p_i].Length; i++)
            {
                index = findNearestLocalLeader(ExplorerParticles[p_i][i], p_i);
                calculateVHL(ExplorerParticles[p_i][i], ParticlesBest[p_i][ExplorerParticles[p_i][i]], p_i);
                calculateVLL(ExplorerParticles[p_i][i], ParticlesBest[p_i][ExplorerParticles[p_i][i]], index, p_i);
                calculateV(p_i);
                ChangeExplorerPositions(i, p_i);
            }
        }

        private int findNearestLocalLeader(KnowlegeBasePCRules Explorer, int p_i)
        {
            int index = 0;
            double minimum = 999999999999;
            for (int k = 0; k < numberOfLocalLeaders; k++)
            {
                double distance = 0;
                for (int i = 0; i < LocalLeaders[p_i][k].TermsSet.Count; i++)
                {
                    for (int j = 0; j < LocalLeaders[p_i][k].TermsSet[i].Parametrs.Length; j++)
                    {
                        distance += Math.Pow(Explorer.TermsSet[i].Parametrs[j] - LocalLeaders[p_i][k].TermsSet[i].Parametrs[j], 2);
                    }
                }
                //for (int i = 0; i < Explorer.RulesDatabase.Count; i++)
                //{
                //  distance += Math.Pow(Explorer.RulesDatabase[i].Cons_DoubleOutput - LocalLeaders[k].RulesDatabase[i].Cons_DoubleOutput, 2);
                //}
                distance = Math.Sqrt(distance);
                if (distance < minimum)
                {
                    minimum = distance;
                    index = k;
                }
            }
            return index;
        }

        private void calculateVHL(KnowlegeBasePCRules Explorer, KnowlegeBasePCRules ExplorerBestPosition, int p_i)
        {
            KnowlegeBasePCRules part1 = new KnowlegeBasePCRules(result.RulesDatabaseSet[0]);
            KnowlegeBasePCRules part2 = new KnowlegeBasePCRules(result.RulesDatabaseSet[0]);

            for (int i = 0; i < part1.TermsSet.Count; i++)
            {
                for (int j = 0; j < part1.TermsSet[i].Parametrs.Length; j++)
                {
                    part1.TermsSet[i].Parametrs[j] = (ExplorerBestPosition.TermsSet[i].Parametrs[j] - Explorer.TermsSet[i].Parametrs[j]) * AGlobal * rand.NextDouble();
                    //
                }
            }

            for (int i = 0; i < part2.TermsSet.Count; i++)
            {
                for (int j = 0; j < part2.TermsSet[i].Parametrs.Length; j++)
                {
                    part2.TermsSet[i].Parametrs[j] = (HeadLeader[p_i].TermsSet[i].Parametrs[j] - Explorer.TermsSet[i].Parametrs[j]) * BGlobal * rand.NextDouble();
                    //
                }
            }

            for (int i = 0; i < VelocityVectorHL[p_i].TermsSet.Count; i++)
            {
                for (int j = 0; j < VelocityVectorHL[p_i].TermsSet[i].Parametrs.Length; j++)
                {
                    VelocityVectorHL[p_i].TermsSet[i].Parametrs[j] = (part1.TermsSet[i].Parametrs[j] + part2.TermsSet[i].Parametrs[j]);
                    //
                }
            }
        }

        private void calculateVLL(KnowlegeBasePCRules Explorer, KnowlegeBasePCRules ExplorerBestPosition, int index, int p_i)
        {
            KnowlegeBasePCRules part1 = new KnowlegeBasePCRules(result.RulesDatabaseSet[0]);
            KnowlegeBasePCRules part2 = new KnowlegeBasePCRules(result.RulesDatabaseSet[0]);

            for (int i = 0; i < part1.TermsSet.Count; i++)
            {
                for (int j = 0; j < part1.TermsSet[i].Parametrs.Length; j++)
                {
                    part1.TermsSet[i].Parametrs[j] = (ExplorerBestPosition.TermsSet[i].Parametrs[j] - Explorer.TermsSet[i].Parametrs[j]) * ALocal * rand.NextDouble();
                }
            }

            for (int i = 0; i < part2.TermsSet.Count; i++)
            {
                for (int j = 0; j < part2.TermsSet[i].Parametrs.Length; j++)
                {
                    part2.TermsSet[i].Parametrs[j] = (LocalLeaders[p_i][index].TermsSet[i].Parametrs[j] - Explorer.TermsSet[i].Parametrs[j]) * BLocal * rand.NextDouble();
                }
            }

            for (int i = 0; i < VelocityVectorLL[p_i].TermsSet.Count; i++)
            {
                for (int j = 0; j < VelocityVectorLL[p_i].TermsSet[i].Parametrs.Length; j++)
                {
                    VelocityVectorLL[p_i].TermsSet[i].Parametrs[j] = (part1.TermsSet[i].Parametrs[j] + part2.TermsSet[i].Parametrs[j]);
                }
            }
        }

        private void calculateV(int p_i)
        {
            for (int i = 0; i < VelocityVector[p_i].TermsSet.Count; i++)
            {
                for (int j = 0; j < VelocityVector[p_i].TermsSet[i].Parametrs.Length; j++)
                {
                    VelocityVector[p_i].TermsSet[i].Parametrs[j] = (VelocityVectorHL[p_i].TermsSet[i].Parametrs[j] + VelocityVectorLL[p_i].TermsSet[i].Parametrs[j]);
                    //
                }
            }
        }

        private void ChangeExplorerPositions(int i, int p_i)
        {
            KnowlegeBasePCRules temp = new KnowlegeBasePCRules();
            temp = ExplorerParticles[p_i][i];
            for (int k = 0; k < temp.TermsSet.Count; k++)
            {
                for (int j = 0; j < temp.TermsSet[k].Parametrs.Length; j++)
                {
                    temp.TermsSet[k].Parametrs[j] += VelocityVector[p_i].TermsSet[k].Parametrs[j];
                }
            }

            if (result.ClassifyLearnSamples(ExplorerParticles[p_i][i]) < result.ClassifyLearnSamples(ParticlesBest[p_i][ExplorerParticles[p_i][i]]))
            {
                ParticlesBest[p_i].Remove(ExplorerParticles[p_i][i]);
                ParticlesBest[p_i].Add(temp, ExplorerParticles[p_i][i]);
            }
            else
            {
                KnowlegeBasePCRules tmp = new KnowlegeBasePCRules();
                tmp = ParticlesBest[p_i][ExplorerParticles[p_i][i]];
                ParticlesBest[p_i].Remove(ExplorerParticles[p_i][i]);
                ParticlesBest[p_i].Add(temp, tmp);
            }
            ExplorerParticles[p_i][i] = temp;

        }

        private void ChangeAimlessPositions(int p_i)
        {
            for (int i = 0; i < AimlessParticles[p_i].Length; i++)
            {
                ChangeAimlessPosition(AimlessParticles[p_i][i], p_i);
            }
        }

        private void ChangeAimlessPosition(KnowlegeBasePCRules Aimless, int p_i)
        {
            KnowlegeBasePCRules GlobalRand = new KnowlegeBasePCRules(result.RulesDatabaseSet[0]);
            SSVector_gen(p_i);
            for (int i = 0; i < Aimless.TermsSet.Count; i++)
            {
                for (int j = 0; j < Aimless.TermsSet[i].Parametrs.Length; j++)
                {
                    Aimless.TermsSet[i].Parametrs[j] = ((rand.NextDouble() * 1.5 + 0.5) * (SSVector[p_i].TermsSet[i].Parametrs[j]));
                }
            }
        }

        public virtual void SSVector_gen(int p_i)
        {
            SSVector[p_i] = new KnowlegeBasePCRules(Populations[0][0]);
            for (int n = 0; n < Populations.Count; n++)
            {
                for (int j = 1; j < numberOfAllParts; j++)
                {
                    for (int k = 0; k < Populations[n][j].TermsSet.Count; k++)
                    {
                        for (int q = 0; q < Populations[n][j].TermsSet[k].CountParams; q++)
                        {
                            SSVector[p_i].TermsSet[k].Parametrs[q] += Populations[n][j].TermsSet[k].Parametrs[q];
                        }
                    }
                }
            }
            for (int k = 0; k < SSVector[p_i].TermsSet.Count; k++)
            {
                for (int q = 0; q < SSVector[p_i].TermsSet[k].CountParams; q++)
                {
                    SSVector[p_i].TermsSet[k].Parametrs[q] /= numberOfAllParts;
                }
            }
        }

        private List<KnowlegeBasePCRules> DiscardRoles(List<KnowlegeBasePCRules> Population, int p_i)
        {
            int k = 1;
            Population[0] = HeadLeader[p_i];
            for (int i = 0; i < LocalLeaders[p_i].Length; i++)
            {
                Population[k] = LocalLeaders[p_i][i];
                k++;
            }
            for (int i = 0; i < ExplorerParticles[p_i].Length; i++)
            {
                Population[k] = ExplorerParticles[p_i][i];
                k++;
            }
            for (int i = 0; i < AimlessParticles[p_i].Length; i++)
            {
                Population[k] = AimlessParticles[p_i][i];
                k++;
            }
            return Population;
        }
        public virtual void Init(ILearnAlgorithmConf Conf)
        {
            Config = Conf as IslandsSSO;
            MaxIter = ((IslandsSSO)Conf).Количество_итераций;
            numberOfLocalLeaders = ((IslandsSSO)Conf).Количество_лок_лидеров;
            numberOfAimlessParts = ((IslandsSSO)Conf).Количество_бесц_част;
            numberOfAllParts = ((IslandsSSO)Conf).Количество_всех_частиц;
            numberOfPopulations = ((IslandsSSO)Conf).Количество_популяций;
            changeParts = ((IslandsSSO)Conf).Обмен;
            ALocal = 1;
            BLocal = 1;
            AGlobal = 1;
            BGlobal = 1;
        }
        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>()
                {
                    FuzzySystemRelisedList.TypeSystem.PittsburghClassifier
                };
            }
        }

        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            IslandsSSO conf = new IslandsSSO();
            conf.Init(CountFeatures);
            return conf;
        }

        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Swallow Swarm Optimization{";
                // result+= param1+Environment.NewLine;
                // result+= param1+Environment.NewLine;
                // result+= param1+Environment.NewLine;
                result += "}";
                return result;
            }
            return "Swallow Swarm Optimization Islands Async";
        }
    }
}
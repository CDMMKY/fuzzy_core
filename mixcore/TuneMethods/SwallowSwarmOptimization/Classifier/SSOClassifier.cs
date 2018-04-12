using System;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using FuzzyCoreUtils;
using System.Linq;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm
{
    public class SSOClassifier : AbstractNotSafeLearnAlgorithm
    {
        protected PCFuzzySystem result;
        Random rand = new Random();
        protected ConfigSSO Config;
        protected int MaxIter, numberOfLocalLeaders, numberOfAimlessParts, numberOfAllParts, numberOfParametrs;
        protected int iter = 0;
        protected double ALocal, BLocal, AGlobal, BGlobal;
        protected KnowlegeBasePCRules[] Population;
        protected KnowlegeBasePCRules SSVector;
        protected KnowlegeBasePCRules HeadLeader, Universal;
        protected KnowlegeBasePCRules[] LocalLeaders, ExplorerParticles, AimlessParticles;
        protected KnowlegeBasePCRules VelocityVector, VelocityVectorLL, VelocityVectorHL;
        protected Dictionary<KnowlegeBasePCRules, KnowlegeBasePCRules> ParticlesBest;

        public override PCFuzzySystem TuneUpFuzzySystem(PCFuzzySystem Classify, ILearnAlgorithmConf conf)
        {
            preIterate(Classify, conf);
            while (iter < MaxIter)
            {
                oneIterate();
            }
            result.RulesDatabaseSet[0] = Population[0];
            return result;
        }

        private void SetPopulation()
        {
            Population = new KnowlegeBasePCRules[numberOfAllParts];
            KnowlegeBasePCRules TempRule = new KnowlegeBasePCRules(result.RulesDatabaseSet[0]);
            Population[0] = TempRule;
            Universal = TempRule;
            for (int i = 1; i < numberOfAllParts; i++)
            {
                Population[i] = new KnowlegeBasePCRules(TempRule);
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
                //Population[i] = new KnowlegeBasePCRules(TempRule);
                for (int j = 0; j < Universal.TermsSet[i].Parametrs.Length; j++)
                {
                    Universal.TermsSet[i].Parametrs[j] = GaussRandom.Random_gaussian(rand, Universal.TermsSet[i].Parametrs[j], 0.1 * Universal.TermsSet[i].Parametrs[j]);
                }
            }
        }

        private void SetRoles()
        {
            HeadLeader = Population[0];
            for (int i = 1; i <= numberOfLocalLeaders; i++)
            {
                LocalLeaders[i - 1] = Population[i];
            }
            for (int i = numberOfAllParts - numberOfAimlessParts; i < numberOfAllParts; i++)
            {
                AimlessParticles[i - numberOfAllParts + numberOfAimlessParts] = Population[i];
            }
            for (int i = numberOfLocalLeaders + 1; i < numberOfAllParts - numberOfAimlessParts; i++)
            {
                ExplorerParticles[i - numberOfLocalLeaders - 1] = Population[i];
            }
        }

        private void ChangeExplorersPositions()
        {
            int index;
            for (int i = 0; i < ExplorerParticles.Length; i++)
            {
                index = findNearestLocalLeader(ExplorerParticles[i]);
                calculateVHL(ExplorerParticles[i], ParticlesBest[ExplorerParticles[i]]);
                calculateVLL(ExplorerParticles[i], ParticlesBest[ExplorerParticles[i]], index);
                calculateV();
                ChangeExplorerPositions(i);
            }
        }

        private int findNearestLocalLeader(KnowlegeBasePCRules Explorer)
        {
            int index = 0;
            double minimum = 999999999999;
            for (int k = 0; k < numberOfLocalLeaders; k++)
            {
                double distance = 0;
                for (int i = 0; i < LocalLeaders[k].TermsSet.Count; i++)
                {
                    for (int j = 0; j < LocalLeaders[k].TermsSet[i].Parametrs.Length; j++)
                    {
                        distance += Math.Pow(Explorer.TermsSet[i].Parametrs[j] - LocalLeaders[k].TermsSet[i].Parametrs[j], 2);
                    }
                }
                distance = Math.Sqrt(distance);
                if (distance < minimum)
                {
                    minimum = distance;
                    index = k;
                }
            }
            return index;
        }

        private void calculateVHL(KnowlegeBasePCRules Explorer, KnowlegeBasePCRules ExplorerBestPosition)
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
                    part2.TermsSet[i].Parametrs[j] = (HeadLeader.TermsSet[i].Parametrs[j] - Explorer.TermsSet[i].Parametrs[j]) * BGlobal * rand.NextDouble();
                    //
                }
            }

            for (int i = 0; i < VelocityVectorHL.TermsSet.Count; i++)
            {
                for (int j = 0; j < VelocityVectorHL.TermsSet[i].Parametrs.Length; j++)
                {
                    VelocityVectorHL.TermsSet[i].Parametrs[j] = (part1.TermsSet[i].Parametrs[j] + part2.TermsSet[i].Parametrs[j]);
                    //
                }
            }
        }

        private void calculateVLL(KnowlegeBasePCRules Explorer, KnowlegeBasePCRules ExplorerBestPosition, int index)
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
                    part2.TermsSet[i].Parametrs[j] = (LocalLeaders[index].TermsSet[i].Parametrs[j] - Explorer.TermsSet[i].Parametrs[j]) * BLocal * rand.NextDouble();
                }
            }

            for (int i = 0; i < VelocityVectorLL.TermsSet.Count; i++)
            {
                for (int j = 0; j < VelocityVectorLL.TermsSet[i].Parametrs.Length; j++)
                {
                    VelocityVectorLL.TermsSet[i].Parametrs[j] = (part1.TermsSet[i].Parametrs[j] + part2.TermsSet[i].Parametrs[j]);
                }
            }
        }

        private void calculateV()
        {
            for (int i = 0; i < VelocityVector.TermsSet.Count; i++)
            {
                for (int j = 0; j < VelocityVector.TermsSet[i].Parametrs.Length; j++)
                {
                    VelocityVector.TermsSet[i].Parametrs[j] = (VelocityVectorHL.TermsSet[i].Parametrs[j] + VelocityVectorLL.TermsSet[i].Parametrs[j]);
                    //
                }
            }
        }

        private void ChangeExplorerPositions(int i)
        {
            KnowlegeBasePCRules temp = new KnowlegeBasePCRules();
            temp = ExplorerParticles[i];
            for (int k = 0; k < temp.TermsSet.Count; k++)
            {
                for (int j = 0; j < temp.TermsSet[k].Parametrs.Length; j++)
                {
                    temp.TermsSet[k].Parametrs[j] += VelocityVector.TermsSet[k].Parametrs[j];
                }
            }

            if (result.ClassifyLearnSamples(ExplorerParticles[i]) < result.ClassifyLearnSamples(ParticlesBest[ExplorerParticles[i]]))
            {
                ParticlesBest.Remove(ExplorerParticles[i]);
                ParticlesBest.Add(temp, ExplorerParticles[i]);
            }
            else
            {
                KnowlegeBasePCRules tmp = new KnowlegeBasePCRules();
                tmp = ParticlesBest[ExplorerParticles[i]];
                ParticlesBest.Remove(ExplorerParticles[i]);
                ParticlesBest.Add(temp, tmp);
            }
            ExplorerParticles[i] = temp;

        }

        private void ChangeAimlessPositions()
        {
            for (int i = 0; i < AimlessParticles.Length; i++)
            {
                ChangeAimlessPosition(AimlessParticles[i]);
            }
        }

        private void ChangeAimlessPosition(KnowlegeBasePCRules Aimless)
        {
            KnowlegeBasePCRules GlobalRand = new KnowlegeBasePCRules(result.RulesDatabaseSet[0]);
            SSVector_gen();
            for (int i = 0; i < Aimless.TermsSet.Count; i++)
            {
                for (int j = 0; j < Aimless.TermsSet[i].Parametrs.Length; j++)
                {
                    Aimless.TermsSet[i].Parametrs[j] = ((rand.NextDouble() * 1.5 + 0.5) * (SSVector.TermsSet[i].Parametrs[j]));
                }
            }
        }

        public virtual void SSVector_gen()
        {
            SSVector = new KnowlegeBasePCRules(Population[0]);
            for (int j = 1; j < numberOfAllParts; j++)
            {
                for (int k = 0; k < Population[j].TermsSet.Count; k++)
                {
                    for (int q = 0; q < Population[j].TermsSet[k].CountParams; q++)
                    {
                        SSVector.TermsSet[k].Parametrs[q] += Population[j].TermsSet[k].Parametrs[q];
                    }
                }
            }
            for (int k = 0; k < SSVector.TermsSet.Count; k++)
            {
                for (int q = 0; q < SSVector.TermsSet[k].CountParams; q++)
                {
                    SSVector.TermsSet[k].Parametrs[q] /= numberOfAllParts;
                }
            }
        }

        private void DiscardRoles()
        {
            int k = 1;
            Population[0] = HeadLeader;
            for (int i = 0; i < LocalLeaders.Length; i++)
            {
                Population[k] = LocalLeaders[i];
                k++;
            }
            for (int i = 0; i < ExplorerParticles.Length; i++)
            {
                Population[k] = ExplorerParticles[i];
                k++;
            }
            for (int i = 0; i < AimlessParticles.Length; i++)
            {
                Population[k] = AimlessParticles[i];
                k++;
            }
        }
        public virtual void preIterate(PCFuzzySystem Classify, ILearnAlgorithmConf conf)
        {
            result = Classify;
            Init(conf);
            HeadLeader = new KnowlegeBasePCRules(result.RulesDatabaseSet[0]);
            VelocityVector = new KnowlegeBasePCRules(result.RulesDatabaseSet[0]);
            VelocityVectorLL = new KnowlegeBasePCRules(result.RulesDatabaseSet[0]);
            VelocityVectorHL = new KnowlegeBasePCRules(result.RulesDatabaseSet[0]);
            for (int i = 0; i < VelocityVector.TermsSet.Count; i++)
            {
                for (int j = 0; j < VelocityVector.TermsSet[i].Parametrs.Length; j++)
                {
                    VelocityVector.TermsSet[i].Parametrs[j] = 0;
                    VelocityVectorLL.TermsSet[i].Parametrs[j] = 0;
                    VelocityVectorHL.TermsSet[i].Parametrs[j] = 0;
                }
            }
            SetPopulation();
            ParticlesBest = new Dictionary<KnowlegeBasePCRules, KnowlegeBasePCRules>();
            foreach (var Particle in Population)
            {
                ParticlesBest.Add(Particle, Universal);
            }
            LocalLeaders = new KnowlegeBasePCRules[numberOfLocalLeaders];
            ExplorerParticles = new KnowlegeBasePCRules[numberOfAllParts - numberOfAimlessParts - numberOfLocalLeaders - 1];
            AimlessParticles = new KnowlegeBasePCRules[numberOfAimlessParts];
            iter = 0;
            Population = ListPittsburgClassifierTool.SortRules(Population, result);
        }
        public virtual void oneIterate()
        {
            SetRoles();
            ChangeExplorersPositions();
            ChangeAimlessPositions();
            DiscardRoles();
            Population = ListPittsburgClassifierTool.SortRules(Population, result);
            iter++;
            if (iter == MaxIter)
            {
                Console.WriteLine("Обуч: " + Math.Round(result.ClassifyLearnSamples(Population[0]), 2));
                Console.WriteLine("Тест: " + Math.Round(result.ClassifyTestSamples(Population[0]), 2));
                //Console.WriteLine();
            }
        }
        public virtual void Init(ILearnAlgorithmConf Conf)
        {
            Config = Conf as ConfigSSO;
            MaxIter = ((ConfigSSO)Conf).Количество_итераций;
            numberOfLocalLeaders = ((ConfigSSO)Conf).Количество_лок_лидеров;
            numberOfAimlessParts = ((ConfigSSO)Conf).Количество_бесц_част;
            numberOfAllParts = ((ConfigSSO)Conf).Количество_всех_частиц;
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
            ConfigSSO conf = new ConfigSSO();
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
            return "Swallow Swarm Optimization";
        }
    }
}

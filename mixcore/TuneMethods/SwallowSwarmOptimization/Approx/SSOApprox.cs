using System;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using FuzzyCoreUtils;

namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm
{
    public class SSOApprox : AbstractNotSafeLearnAlgorithm
    {
        protected SAFuzzySystem result;
        Random rand = new Random();
        protected ConfigSSO Config;
        protected int MaxIter, numberOfLocalLeaders, numberOfAimlessParts, numberOfAllParts, numberOfParametrs;
        protected int iter = 0;
        protected double ALocal, BLocal, AGlobal, BGlobal, unlaidtest;
        protected KnowlegeBaseSARules[] Population;
        protected KnowlegeBaseSARules SSVector;
        protected KnowlegeBaseSARules HeadLeader, Universal;
        protected KnowlegeBaseSARules[] LocalLeaders, ExplorerParticles, AimlessParticles;
        protected KnowlegeBaseSARules VelocityVector, VelocityVectorLL, VelocityVectorHL;
        protected Dictionary<KnowlegeBaseSARules, KnowlegeBaseSARules> ParticlesBest;

        public override SAFuzzySystem TuneUpFuzzySystem(SAFuzzySystem Approx, ILearnAlgorithmConf conf)
        {
            result = Approx;
            Init(conf);
            HeadLeader = new KnowlegeBaseSARules(result.RulesDatabaseSet[0]);
            VelocityVector = new KnowlegeBaseSARules(result.RulesDatabaseSet[0]);
            VelocityVectorLL = new KnowlegeBaseSARules(result.RulesDatabaseSet[0]);
            VelocityVectorHL = new KnowlegeBaseSARules(result.RulesDatabaseSet[0]);
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
            ParticlesBest = new Dictionary<KnowlegeBaseSARules, KnowlegeBaseSARules>();
            foreach(var Particle in Population)
            {
                ParticlesBest.Add(Particle, Universal);
            }
            LocalLeaders = new KnowlegeBaseSARules[numberOfLocalLeaders];
            Console.WriteLine(LocalLeaders.Length);
            ExplorerParticles = new KnowlegeBaseSARules[numberOfAllParts - numberOfAimlessParts - numberOfLocalLeaders - 1];
            Console.WriteLine(ExplorerParticles.Length);
            AimlessParticles = new KnowlegeBaseSARules[numberOfAimlessParts];
            Console.WriteLine(AimlessParticles.Length);
            while (iter < MaxIter)
            {
                Population = ListSingletonApproximateTool.SortRules(Population, result);
                SetRoles();
                ChangeExplorersPositions();
                ChangeAimlessPositions();
                DiscardRoles();
                iter++;
                Console.WriteLine("Iteration: " + iter.ToString());
                Console.WriteLine(result.RMSEtoMSEforLearn(result.approxLearnSamples(Population[0])));
                Console.WriteLine(result.RMSEtoMSEforTest(result.approxTestSamples(Population[0])));
                Console.WriteLine(result.approxLearnSamples(Population[numberOfLocalLeaders + 1]));
            }          
            
            result.RulesDatabaseSet[0] = Population[0];
            return result;
        }

        private void SetPopulation()
        {
            Population = new KnowlegeBaseSARules[numberOfAllParts];
            KnowlegeBaseSARules TempRule = new KnowlegeBaseSARules(result.RulesDatabaseSet[0]);
            Population[0] = TempRule;
            Universal = TempRule;
            for (int i = 1; i < numberOfAllParts; i++)
            {
                Population[i] = new KnowlegeBaseSARules(TempRule);
                for (int j = 0; j < Population[i].TermsSet.Count; j++)
                {
                    for (int k = 0; k < Population[i].TermsSet[j].Parametrs.Length; k++)
                    {
                        Population[i].TermsSet[j].Parametrs[k] = GaussRandom.Random_gaussian(rand, Population[i].TermsSet[j].Parametrs[k], 0.1 * Population[i].TermsSet[j].Parametrs[k]);
                    }
                }
                result.UnlaidProtectionFix(Population[i]);
            }
            Universal = new KnowlegeBaseSARules(TempRule);
            for (int i = 0; i < Universal.TermsSet.Count; i++)
            {
                Population[i] = new KnowlegeBaseSARules(TempRule);
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
            for (int i=numberOfLocalLeaders + 1; i < numberOfAllParts - numberOfAimlessParts; i++)
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
                //findAGlobal(ExplorerParticles[i], ParticlesBest[ExplorerParticles[i]]);
                //findBGlobal(ExplorerParticles[i], ParticlesBest[ExplorerParticles[i]]);
                calculateVHL(ExplorerParticles[i], ParticlesBest[ExplorerParticles[i]]);
                //findALocal(ExplorerParticles[i], ParticlesBest[ExplorerParticles[i]]);
                //findBLocal(ExplorerParticles[i], ParticlesBest[ExplorerParticles[i]], index);
                calculateVLL(ExplorerParticles[i], ParticlesBest[ExplorerParticles[i]], index);
                calculateV();
                ChangeExplorerPositions(i);
            }
        }

        private int findNearestLocalLeader(KnowlegeBaseSARules Explorer)
        {
            int index = 0;
            double minimum = 999999999999;
            for (int k = 0; k < numberOfLocalLeaders; k++)
            {
                double distance = 0;
                for (int i = 0; i < LocalLeaders[k].TermsSet.Count; i++)
                {
                    for (int j = 0; j < numberOfParametrs; j++)
                    {
                        distance += Math.Pow(Explorer.TermsSet[i].Parametrs[j] - LocalLeaders[k].TermsSet[i].Parametrs[j], 2);
                    }
                }
                //for (int i = 0; i < Explorer.RulesDatabase.Count; i++)
                //{
                //    distance += Math.Pow(Explorer.RulesDatabase[i].Cons_DoubleOutput - LocalLeaders[k].RulesDatabase[i].Cons_DoubleOutput, 2);
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

        private void findAGlobal(KnowlegeBaseSARules Explorer, KnowlegeBaseSARules ExplorerBestPosition)
        {
            //AGlobal -= (AGlobal / MaxIter) / 20;
            if (result.approxLearnSamples(Explorer) == 0 || result.approxLearnSamples(ExplorerBestPosition) == 0)
            {
                AGlobal = 1.5;
            }
            else if (result.approxLearnSamples(Explorer) < result.approxLearnSamples(ExplorerBestPosition))
            {
                AGlobal = (2 * rand.NextDouble() * result.approxLearnSamples(ExplorerBestPosition)) * (2 * result.approxLearnSamples(Explorer)) / 30000;
            }
            else
            {
                AGlobal = (result.approxLearnSamples(ExplorerBestPosition)) * (2 * rand.NextDouble()) / 30000;
            }
        }

        private void findBGlobal(KnowlegeBaseSARules Explorer, KnowlegeBaseSARules ExplorerBestPosition)
        {
            //BGlobal -= (BGlobal / MaxIter) / 20;
            if (result.approxLearnSamples(Explorer) == 0 || result.approxLearnSamples(ExplorerBestPosition) == 0)
            {
                BGlobal = 1.5;
            }
            else if (result.approxLearnSamples(Explorer) < result.approxLearnSamples(ExplorerBestPosition))
            {
                BGlobal = (2 * rand.NextDouble() * result.approxLearnSamples(HeadLeader)) * (2 * result.approxLearnSamples(Explorer)) / 30000;
            }
            else
            {
                BGlobal = (result.approxLearnSamples(HeadLeader)) * (2 * rand.NextDouble()) / 30000;
            }
        }

        private void calculateVHL(KnowlegeBaseSARules Explorer, KnowlegeBaseSARules ExplorerBestPosition)
        {
            KnowlegeBaseSARules part1 = new KnowlegeBaseSARules(result.RulesDatabaseSet[0]);
            KnowlegeBaseSARules part2 = new KnowlegeBaseSARules(result.RulesDatabaseSet[0]);

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

        private void findALocal(KnowlegeBaseSARules Explorer, KnowlegeBaseSARules ExplorerBestPosition)
        {
            //ALocal -= (ALocal / MaxIter) / 20;
            if (result.approxLearnSamples(Explorer) == 0 || result.approxLearnSamples(ExplorerBestPosition) == 0)
            {
                ALocal = 2;
            }
            else if (result.approxLearnSamples(Explorer) < result.approxLearnSamples(ExplorerBestPosition))
            {
                ALocal = (2 * rand.NextDouble() * result.approxLearnSamples(ExplorerBestPosition)) * (2 * result.approxLearnSamples(Explorer)) / 30000;
            }
            else
            {
                ALocal = (result.approxLearnSamples(ExplorerBestPosition)) * (2 * rand.NextDouble()) / 30000;
            }
        }

        private void findBLocal(KnowlegeBaseSARules Explorer, KnowlegeBaseSARules ExplorerBestPosition, int index)
        {
            //BLocal -= (BLocal / MaxIter) / 20;
            if (result.approxLearnSamples(Explorer) == 0 || result.approxLearnSamples(ExplorerBestPosition) == 0)
            {
                BLocal = 2;
            }
            else if (result.approxLearnSamples(Explorer) < result.approxLearnSamples(ExplorerBestPosition))
            {
                BLocal = (2 * rand.NextDouble() * result.approxLearnSamples(LocalLeaders[index])) * (2 * result.approxLearnSamples(Explorer)) / 30000;
            }
            else
            {
                BLocal = (result.approxLearnSamples(LocalLeaders[index])) * (2 * rand.NextDouble()) / 30000;
            }
        }

        private void calculateVLL(KnowlegeBaseSARules Explorer, KnowlegeBaseSARules ExplorerBestPosition, int index)
        {
            KnowlegeBaseSARules part1 = new KnowlegeBaseSARules(result.RulesDatabaseSet[0]);
            KnowlegeBaseSARules part2 = new KnowlegeBaseSARules(result.RulesDatabaseSet[0]);

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
            KnowlegeBaseSARules temp = new KnowlegeBaseSARules();
            temp = ExplorerParticles[i];
            for (int k = 0; k < temp.TermsSet.Count; k++)
            {
                for (int j = 0; j < temp.TermsSet[k].Parametrs.Length; j++)
                {
                    temp.TermsSet[k].Parametrs[j] += VelocityVector.TermsSet[k].Parametrs[j];
                }
            }
            
            if (result.approxLearnSamples(ExplorerParticles[i]) < result.approxLearnSamples(ParticlesBest[ExplorerParticles[i]]))
            {
                ParticlesBest.Remove(ExplorerParticles[i]);
                ParticlesBest.Add(temp, ExplorerParticles[i]);
            }
            else
            {
                KnowlegeBaseSARules tmp = new KnowlegeBaseSARules();
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

        private void ChangeAimlessPosition(KnowlegeBaseSARules Aimless)
        {
            KnowlegeBaseSARules GlobalRand = new KnowlegeBaseSARules(result.RulesDatabaseSet[0]);
            SSVector_gen();
            for (int i = 0; i < Aimless.TermsSet.Count; i++)
            {
                for (int j = 0; j < Aimless.TermsSet[i].Parametrs.Length; j++)
                {
                    Aimless.TermsSet[i].Parametrs[j] = ((rand.NextDouble() + 0.5) * (SSVector.TermsSet[i].Parametrs[j]));
                }
            }
        }

        public virtual void SSVector_gen()
        {
            SSVector = new KnowlegeBaseSARules(Population[0]);
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
            for(int i = 0; i < LocalLeaders.Length; i++)
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
                    FuzzySystemRelisedList.TypeSystem.Singletone
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
            if(with_param)
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
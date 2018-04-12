using System;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using FuzzySystem.FuzzyAbstract.conf;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract;


namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm
{
    public  class Term_Config_PSO : AbstractNotSafeLearnAlgorithm
    {
       protected Random rand = new Random();
       protected double c1;
       protected double c2;
       protected int count_particle;
       protected int count_iteration;
       protected KnowlegeBaseSARules[] X;
       protected KnowlegeBaseSARules[] V;
       protected KnowlegeBaseSARules[] Pi;
       protected KnowlegeBaseSARules Pg;
       protected double[] Errors;
       protected double[] OldErrors;
       protected double minError=0;
       protected Random rnd;
       protected double w=1;
       protected SAFuzzySystem theFuzzySystem;

       public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
       {
           get
           {
               return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.Singletone };
           }
       }
       
        public  override  SAFuzzySystem TuneUpFuzzySystem(SAFuzzySystem Approximate, ILearnAlgorithmConf conf)
       {
           theFuzzySystem = Approximate;

           Init(conf);            
            for (int i = 0; i < count_iteration; i++)
            {
              oneIterate(theFuzzySystem);
            }

            Final();
            theFuzzySystem.RulesDatabaseSet[0].TermsSet.Trim();
            return theFuzzySystem;
        }


        protected void preIterate(SAFuzzySystem result)
        {
            for (int i = 0; i < count_particle; i++)
            {
                KnowlegeBaseSARules temp_c_Rule = new KnowlegeBaseSARules(result.RulesDatabaseSet[0]);
                X[i] = temp_c_Rule;
                Errors[i] = result.approxLearnSamples(result.RulesDatabaseSet[ 0]);
                OldErrors[i] = Errors[i];
                Pi[i] = new KnowlegeBaseSARules(X[i]);
                V[i] = new KnowlegeBaseSARules(X[i]);
                //
                for (int j = 0; j < V[i].TermsSet.Count; j++)
                {
                    for (int k = 0; k < Term.CountParamsinSelectedTermType(V[i].TermsSet[j].TermFuncType); k++)
                    {
                        if (i == 0)
                        {

                            V[i].TermsSet[j].Parametrs[k] = 0;

                        }
                        else
                        {
                            V[i].TermsSet[j].Parametrs[k] = rnd.NextDouble() - 0.5;
                        }
                    }
                    double[] bf = new double[V[i].all_conq_of_rules.Length];
                    for (int k = 0; k < V[i].all_conq_of_rules.Length; k++)
                    {
                        if (i == 0)
                        {
                            bf[k] = V[i].all_conq_of_rules[k];
                        }
                        else
                        {
                            bf[k] = GaussRandom.Random_gaussian(rand, V[i].all_conq_of_rules[k], V[i].all_conq_of_rules[k] * 0.01);
                        }
                    }
                    V[i].all_conq_of_rules = bf;

                }
            }
            Pg = new KnowlegeBaseSARules(result.RulesDatabaseSet[0]);
            minError = Errors[0];
          
      
        }


      

        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "роящиеся частицы {";
                result += "Итераций= " + count_iteration.ToString() + " ;" + Environment.NewLine;
                result += "Коэффициент_c1= " + c1.ToString() + " ;" + Environment.NewLine;
                result += "Коэффициент_c2= " + c2.ToString() + " ;" + Environment.NewLine;
                result += "Особей в популяции= " + count_particle.ToString() + " ;" + Environment.NewLine;
                
                result += "}";
                return result;
            }
            return "роящиеся частицы";
     
        }


        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new PSOSearchConf();
            result.Init(CountFeatures);
            return result;
        }



        public virtual void oneIterate(SAFuzzySystem result)
        {
            for (int j = 0; j < count_particle; j++)
            {
                w = 1 / (1 + Math.Exp(-(Errors[j] - OldErrors[j]) / 0.01));
                for (int k = 0; k < X[j].TermsSet.Count; k++)
                {
                    for (int q = 0; q < X[j].TermsSet[k].CountParams; q++)
                    {

                        double bp = Pi[j].TermsSet[k].Parametrs[q];
                        V[j].TermsSet[k].Parametrs[q] = V[j].TermsSet[k].Parametrs[q] * w + c1 * rnd.NextDouble() * (bp - X[j].TermsSet[k].Parametrs[q]) +
                            c2 * rnd.NextDouble() * (Pg.TermsSet[k].Parametrs[q] - X[j].TermsSet[k].Parametrs[q]);
                        X[j].TermsSet[k].Parametrs[q] += V[j].TermsSet[k].Parametrs[q];
                    }
                }
                double[] bf = new double[V[j].all_conq_of_rules.Length];
                double[] bfw = new double[V[j].all_conq_of_rules.Length];
                for (int k = 0; k < V[j].all_conq_of_rules.Length; k++)
                {

                    bfw[k] = V[j].all_conq_of_rules[k] * w + c1 * rnd.NextDouble() * (Pi[j].all_conq_of_rules[k] - X[j].all_conq_of_rules[k]) +
                        c2 * rnd.NextDouble() * (Pg.all_conq_of_rules[k] - X[j].all_conq_of_rules[k]);
                    double sw = X[j].all_conq_of_rules[k] + bfw[k];
                    bf[k] = sw;


                }
                X[j].all_conq_of_rules = bf;
                V[j].all_conq_of_rules = bfw;
                double newError = 0;
                result.RulesDatabaseSet.Add(X[j]);
                int temp_index = result.RulesDatabaseSet.Count - 1;
                bool success = true;
                try
                {
                    newError = result.approxLearnSamples(result.RulesDatabaseSet[ temp_index]);
                }
                catch (Exception)
                {
                    success = false;
                }
                result.RulesDatabaseSet.RemoveAt(temp_index);
                if (success && (newError < Errors[j]))
                {
                    OldErrors[j] = Errors[j];
                    Errors[j] = newError;

                    Pi[j] = new KnowlegeBaseSARules(X[j]);
                }
                if (minError > newError)
                {
                    minError = newError;
                    Pg = new KnowlegeBaseSARules(X[j]);
                }

            }
        }

        public virtual void Init(ILearnAlgorithmConf Config)
        {
            PSOSearchConf conf = Config as PSOSearchConf;
            count_iteration = conf.PSOSCCountIteration;
            c1 = conf.PSOSCC1;
            c2 = conf.PSOSCC2;
            count_particle = conf.PSOSCPopulationSize;
            
            X = new KnowlegeBaseSARules[count_particle];
            V = new KnowlegeBaseSARules[count_particle];
            Pi = new KnowlegeBaseSARules[count_particle];
            Pg = new KnowlegeBaseSARules();
            Errors = new double[count_particle];
            OldErrors = new double[count_particle];
            rnd = new Random();

            preIterate(theFuzzySystem);

        }

        public virtual void Final()
        {
            theFuzzySystem.RulesDatabaseSet[0] = Pg;
        }
    }
}

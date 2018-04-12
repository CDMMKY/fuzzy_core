using System;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;
using System.Collections.Generic;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm
{
    public class Term_Config_PSO : AbstractNotSafeLearnAlgorithm
    {
        public int count_iteration = 0;
        public double c1 = 0;
        public double c2 = 0;
        public double w = 1;
        public int count_particle = 0;

        protected KnowlegeBasePCRules[] X;
        protected KnowlegeBasePCRules[] V;
        protected KnowlegeBasePCRules[] Pi;
        protected KnowlegeBasePCRules Pg;
        protected double[] Errors;
        protected double[] OldErrors;
        protected double minError = 0;
        protected Random rnd;
        protected PCFuzzySystem result;

        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.PittsburghClassifier };
            }
        }

        public override FuzzySystem.PittsburghClassifier.PCFuzzySystem TuneUpFuzzySystem(FuzzySystem.PittsburghClassifier.PCFuzzySystem Classifier, ILearnAlgorithmConf conf)
        {
            result = Classifier;
            Init(conf);

            //OneIteration
            for (int i = 0; i < count_iteration; i++)
            {

                oneIterate(result);

               
            }
            Final();
            return result;
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


        protected void preIterate(PCFuzzySystem Classifier)
        {
            for (int i = 0; i < count_particle; i++)
            {
                KnowlegeBasePCRules temp_c_Rule = new KnowlegeBasePCRules(Classifier.RulesDatabaseSet[0]);
                X[i] = temp_c_Rule;
                Errors[i] = Classifier.ClassifyLearnSamples(Classifier.RulesDatabaseSet[ 0]);
                OldErrors[i] = Errors[i];
                Pi[i] = new KnowlegeBasePCRules(X[i]);
                V[i] = new KnowlegeBasePCRules(X[i]);
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
                }
                double[] bf = new double[V[i].Weigths.Length];
                for (int k = 0; k < V[i].Weigths.Length; k++)
                {
                    if (i == 0)
                    {
                        bf[k] = 1;
                    }
                    else
                    {
                        //System.Windows.Forms.MessageBox.Show(rnd.NextDouble().ToString());
                        bf[k] = rnd.NextDouble() / 200;
                    }
                }
                V[i].Weigths = bf;

            }

            Pg = new KnowlegeBasePCRules(Classifier.RulesDatabaseSet[0]);
            minError = Errors[0];
        }

        public virtual void oneIterate(PCFuzzySystem result)
        {
            for (int j = 0; j < count_particle; j++)
            {
                w = 1 / (1 + Math.Exp(-(Errors[j] - OldErrors[j]) / 0.01));
                for (int k = 0; k < X[j].TermsSet.Count; k++)
                {
                    for (int q = 0; q <X[j].TermsSet[k].CountParams; q++)
                    {

                        double bp = Pi[j].TermsSet[k].Parametrs[q];
                        V[j].TermsSet[k].Parametrs[q] = V[j].TermsSet[k].Parametrs[q] * w + c1 * rnd.NextDouble() * (bp - X[j].TermsSet[k].Parametrs[q]) +
                            c2 * rnd.NextDouble() * (Pg.TermsSet[k].Parametrs[q] - X[j].TermsSet[k].Parametrs[q]);
                        X[j].TermsSet[k].Parametrs[q] += V[j].TermsSet[k].Parametrs[q];
                    }
                }
                double[] bf = new double[V[j].Weigths.Length];
                double[] bfw = new double[V[j].Weigths.Length];
                for (int k = 0; k < V[j].Weigths.Length; k++)
                {

                    bfw[k] = V[j].Weigths[k] * w + c1 * rnd.NextDouble() * (Pi[j].Weigths[k] - X[j].Weigths[k]) +
                        c2 * rnd.NextDouble() * (Pg.Weigths[k] - X[j].Weigths[k]);
                    double sw = X[j].Weigths[k] + bfw[k];
                    if (sw > 0 && sw <= 2)
                    {
                        bf[k] = sw;

                    }
                    else
                    {
                        bf[k] = X[j].Weigths[k];
                        bfw[k] = V[j].Weigths[k];
                    }

                }
                X[j].Weigths = bf;
                V[j].Weigths = bfw;
                double newError = 0;
                bool success = true;
                try
                {
                    newError =    result.ClassifyLearnSamples(X[j]);
                }
                catch (Exception)
                {
                    success = false;
                }
                if (success && (newError > Errors[j]))
                {
                    OldErrors[j] = Errors[j];
                    Errors[j] = newError;

                    Pi[j] = new KnowlegeBasePCRules(X[j]);
                }
                if (minError < newError)
                {
                    minError = newError;
                    Pg = new KnowlegeBasePCRules(X[j]);
                }

            }

        }




        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new PSOSearchConf();
            result.Init(CountFeatures);
            return result;
        }


    
   
        public virtual void Init(ILearnAlgorithmConf Config)
        {
            PSOSearchConf conf = Config as PSOSearchConf;
            count_iteration = conf.PSOSCCountIteration;
            c1 = conf.PSOSCC1;
            c2 = conf.PSOSCC2;
            w = 1;
            count_particle = conf.PSOSCPopulationSize;

            X = new KnowlegeBasePCRules[count_particle];
            V = new KnowlegeBasePCRules[count_particle];
            Pi = new KnowlegeBasePCRules[count_particle];
            Pg = new KnowlegeBasePCRules();
            Errors = new double[count_particle];
            OldErrors = new double[count_particle];

            rnd = new Random();
            preIterate(result);

        }

        public virtual void Final()
        {
            result.RulesDatabaseSet[0] = Pg;

        }
    }
}

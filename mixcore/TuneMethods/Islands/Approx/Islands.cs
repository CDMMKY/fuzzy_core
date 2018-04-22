using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using FuzzySystem.SingletoneApproximate.LearnAlgorithm;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using FuzzySystem.SingletoneApproximate;
using FuzzySystem.FuzzyAbstract.conf;
using System.Threading.Tasks;
using FuzzyCoreUtils;
using FuzzySystem.FuzzyAbstract;
using System.Linq;

namespace Islands
{
    public class Graph
    {
        private List<int>[] childNodes;
        private List<Island> islands;

        public Graph(int size, SAFuzzySystem source)
        {
            childNodes = new List<int>[size];
            islands = new List<Island>();
            for (int i = 0; i < size; i++)
            {
                childNodes[i] = new List<int>();
                islands.Add(new Island(source));
            }
        }

        public Graph(List<int>[] childNodes, SAFuzzySystem source)
        {
            this.childNodes = childNodes;
            islands = new List<Island>();
            for (int i = 0; i < childNodes.Length; i++)
            {
                islands.Add(new Island(source));
            }
        }

        public int Size
        {
            get { return childNodes.Length; }
        }

        public void AddEdge(int u, int v)
        {
            childNodes[u].Add(v);
        }

        public void RemoveEdge(int u, int v)
        {
            childNodes[u].Remove(v);
        }

        public bool HasEdge(int u, int v)
        {
            bool hasEdge = childNodes[u].Contains(v);
            return hasEdge;
        }

        public List<int> GetSuccessors(int v)
        {
            return childNodes[v];
        }

        public Island GetIsland(int v)
        {
            return islands[v];
        }
    }

    public class Island
    {
        protected int graph_ind;
        protected Graph topology;
        public Object lockBuffer = new Object();
        public KnowlegeBaseSARules[] migrants;
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
        protected double minError = 0;
        protected Random rnd;
        protected double w = 1;
        protected SAFuzzySystem theFuzzySystem;
        protected double worst_new, best_new, best_old;

        private void preIterate(SAFuzzySystem result)
        {
            for (int i = 0; i < count_particle; i++)
            {
                KnowlegeBaseSARules temp_c_Rule = new KnowlegeBaseSARules(result.RulesDatabaseSet[0]);
                X[i] = temp_c_Rule;
                Errors[i] = result.approxLearnSamples(result.RulesDatabaseSet[0]);
                OldErrors[i] = Errors[i];
                Pi[i] = new KnowlegeBaseSARules(X[i]);
                V[i] = new KnowlegeBaseSARules(X[i]);

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
            best_new = minError;
            best_old = minError;
            worst_new = minError;
        }

        public void oneIterate()
        {
            best_old = best_new;
            best_new = Errors[0];
            worst_new = Errors[0];

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
                bool success = true;
                try
                {
                    newError = theFuzzySystem.approxLearnSamples(X[j]);
                }
                catch (Exception)
                {
                    success = false;
                }
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
                if (best_new > Errors[j]) best_new = Errors[j];
                if (worst_new < Errors[j]) worst_new = Errors[j];
            }
        }

        public Island(SAFuzzySystem source)
        {
            theFuzzySystem = new SAFuzzySystem(source);
            count_iteration = 100;
            c1 = 1.6;
            c2 = 1.8;
            count_particle = 8;

            X = new KnowlegeBaseSARules[count_particle];
            V = new KnowlegeBaseSARules[count_particle];
            Pi = new KnowlegeBaseSARules[count_particle];
            Pg = new KnowlegeBaseSARules();
            Errors = new double[count_particle];
            OldErrors = new double[count_particle];
            rnd = new Random();
            migrants = new KnowlegeBaseSARules[3];
            preIterate(theFuzzySystem);
        }

        public void Search(int graph_ind, Graph topology)
        {
            this.graph_ind = graph_ind;
            this.topology = topology;
            for (int i = 0; i < count_iteration; i++)
            {
                oneIterate();
                if (i % 50 == 0 && i != 0) Console.WriteLine(graph_ind.ToString() + " thread on "+i.ToString() + "iteration");
                bool t1 = ((worst_new - best_new) < Math.Max(1 / Math.Exp(i), 0.0001));
                bool t2 = best_old - best_new > 0.000001;
                if ((t1 && t2) || (i % 25 == 0))
                {
                    List<int> indexes = topology.GetSuccessors(graph_ind);
                    List<Tuple<int, double>> to_sort = new List<Tuple<int, double>>();
                    for (int j = 0; j < count_particle; j++)
                    {
                        to_sort.Add(new Tuple<int, double>(j, Errors[j]));
                    }
                    to_sort.Sort(new CompareTuple());
                    foreach (int k in indexes)
                    {
                        Island island_to_migrate = topology.GetIsland(k);
                        lock (island_to_migrate.lockBuffer)
                        {
                            Console.WriteLine(graph_ind.ToString() +" thread sending to " + k.ToString());
                            island_to_migrate.migrants[0] = new KnowlegeBaseSARules(X[to_sort[0].Item1]);
                            island_to_migrate.migrants[1] = new KnowlegeBaseSARules(X[to_sort[1].Item1]);
                            island_to_migrate.migrants[2] = new KnowlegeBaseSARules(X[to_sort[2].Item1]);
                        }
                    }
                    lock (this.lockBuffer)
                    {
                        if (migrants[0] != null)
                        {
                            X[X.Count() - 1] = new KnowlegeBaseSARules(migrants[0]);
                            Console.WriteLine(graph_ind.ToString() + " thread consumed buffer item");
                        } 
                        if (migrants[1] != null)
                        {
                            X[X.Count() - 1] = new KnowlegeBaseSARules(migrants[1]);
                            Console.WriteLine(graph_ind.ToString() + " thread consumed buffer item");
                        }
                        if (migrants[2] != null)
                        {
                            X[X.Count() - 1] = new KnowlegeBaseSARules(migrants[2]);
                            Console.WriteLine(graph_ind.ToString() + " thread consumed buffer item");
                        }
                    }
                }
            }
            Console.WriteLine(graph_ind.ToString() + " island finished, result="+minError.ToString());
        }
    }

    public class CompareTuple : Comparer<Tuple<int, double>>
    {
        public override int Compare(Tuple<int, double> x, Tuple<int, double> y)
        {
            if (x.Item2 > y.Item2)
            {
                return 1;
            }
            else if (x.Item2 ==y.Item2)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }

    }

    public class Islands: AbstractNotSafeLearnAlgorithm
    {
        SAFuzzySystem result;

        IslandsConfig Config;

        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.Singletone };
            }
        }

        public override SAFuzzySystem TuneUpFuzzySystem(SAFuzzySystem Approximate, ILearnAlgorithmConf conf)
        {
            result = Approximate;
            Init(conf);

            List<int>[] test_topology = new List<int>[] 
            {
                new List<int>() {1, 2}, // vertice 0
                new List<int>() {2, 0}, // vertice 1
                new List<int>() {1, 0}  // vertice 2
            };

            Graph topology = new Graph(test_topology, Approximate);

            List<Task> AlgTasks = new List<Task>();

            Parallel.For(0, 3, m =>
            {
                Task CurrentTask = new Task(() => { topology.GetIsland(m).Search(m, topology); }, TaskCreationOptions.LongRunning);
                AlgTasks.Add(CurrentTask);
            });
            Parallel.For(0, 3, m =>
            {
                AlgTasks[m].Start();
            });
            Task.WaitAll(AlgTasks.ToArray());

            result.RulesDatabaseSet[0].TermsSet.Trim();
            return result;
        }

        void Init(ILearnAlgorithmConf Conf)
        {
            Config = Conf as IslandsConfig;
        }

        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Острова тест {";
                result += "}";
                return result;
            }

            return "Острова тест";
        }

        public void Final()
        {
            GC.Collect();
        }

        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new IslandsConfig();
            result.Init(CountFeatures);
            return result;
        }
    }
}

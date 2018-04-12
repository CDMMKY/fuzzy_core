using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 using FuzzySystem.PittsburghClassifier;
using FuzzySystem.PittsburghClassifier.UFS;

using FuzzySystem.PittsburghClassifier.LearnAlgorithm;
using System.IO;
using FuzzySystem.FuzzyAbstract.conf;
using System.Threading;
using System.Threading.Tasks;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyFrontEnd;

namespace FuzzySystem.Fuzzy_FrontEnd
{
    class PittsburghClassifier_FrontEnd:IFuzzySystemFroentEnd
    {

        MethodLoadHelperPittsburghClassifier AllMethods;
        string UFS_file_name = "";
        #region IFuzzy_System_FroentEnd implementation

        public PittsburghClassifier_FrontEnd()
        {
            AllMethods = new MethodLoadHelperPittsburghClassifier();
        }

        #region Load samples set
        public bool Load_learn_set(string file_name)
        {


            switch (new FileInfo(file_name).Extension)
            {
                case ".dat":
                    {
                        Classifier_learn_set = new PCSamplesSet(file_name);
                        return false;
                    }
                case ".ufs":
                    {
                        Classifier_learn_set = Classifier_learn_set.LoadLearnFromUFS(file_name);
                        try
                        {
                            Classifier_test_set = Classifier_test_set.LoadTestFromUFS(file_name);
                            UFS_file_name = file_name;
                            is_UFS = true;
                            return true;
                        }
                        catch { return false; }
                    }
                default:
                    {
                        Classifier_learn_set = Classifier_learn_set.LoadLearnFromUFS(file_name);
                        try
                        {
                            Classifier_test_set = Classifier_test_set.LoadTestFromUFS(file_name);
                            UFS_file_name = file_name;
                            is_UFS = true;
                            return true;
                        }
                        catch { return false; }
                    }
            }
        }

        public void Load_test_set(string file_name)
        {

            switch (new FileInfo(file_name).Extension)
            {
                case ".dat":
                    {
                        Classifier_test_set = new PCSamplesSet(file_name);

                        break;

                    }
                case ".ufs":
                    {
                        Classifier_test_set = Classifier_test_set.LoadTestFromUFS(file_name);
                        break;
                    }
                default:
                    {

                        Classifier_test_set = Classifier_test_set.LoadTestFromUFS(file_name);
                        break;
                    }

            }
        }

        #endregion

    
        public void Set_count_add_generator(int count_used_Algorithm)
        {
            Rules_generator_conf = new IGeneratorConf[count_used_Algorithm];
            Rules_generator_type = new int[count_used_Algorithm];
            for (int i = 0; i < count_used_Algorithm; i++)
            {
                Rules_generator_type[i] = -2;
            }
        }

        public void Set_count_learn_algorithm(int count_used_Algorithm)
        {
            Learn_algorithm_conf = new ILearnAlgorithmConf[count_used_Algorithm];
            Learn_algoritm_type = new int [count_used_Algorithm];
            for (int i = 0; i < count_used_Algorithm; i++)
            {
                Learn_algoritm_type[i] = -2;
            }
      
        }

    public IGeneratorConf Set_add_generator_algorothm(int numAlgorithm, int typeAlgorithm)
        {

            if (Rules_generator_type[numAlgorithm] != typeAlgorithm-1)
            {
                Rules_generator_type[numAlgorithm] = typeAlgorithm-1;
                if (typeAlgorithm == 0|| ((typeAlgorithm - 1) >= AllMethods.InstanceOfInit.Count)) { Rules_generator_conf[numAlgorithm] = new NullConfForAll(); }
                else
                {
                    Rules_generator_conf[numAlgorithm] = AllMethods.InstanceOfInit[typeAlgorithm - 1].getConf(Classifier_learn_set.Count_Vars);
                }
            }
            return Rules_generator_conf[numAlgorithm];
        }


        public ILearnAlgorithmConf Set_learn_algorothm(int numAlgorithm, int typeAlgorithm)
        {
            if (Learn_algoritm_type[numAlgorithm] != typeAlgorithm-1)
            {
                Learn_algoritm_type[numAlgorithm] = typeAlgorithm-1;
                if (Learn_algoritm_type[numAlgorithm] < 0) {
                    Learn_algorithm_conf[numAlgorithm] = new NullConfForAll();
                }
                else
                {
                    Learn_algorithm_conf[numAlgorithm] = AllMethods.InstanceOfTune[Learn_algoritm_type[numAlgorithm]].getConf(Classifier_learn_set.Count_Vars);
                }
            }

            return Learn_algorithm_conf[numAlgorithm];
        }

        public void Set_count_repeate_into_cirlucle(int numRepeate) { Repeat_into = numRepeate; }

        public void Set_count_repeate_renew(int numRepeate) { Repeat_renew_global = numRepeate; }

        public string get_log { get { return LOG; } }

        public bool Is_UFS { get { return is_UFS; } }

        public int get_global_completed() { return Repeat_renew_global; }

        public void Calc()
        {

            DateTime start__full_time = DateTime.Now;
            List<int> gen_index = prepare_generate_to_Calc();
            List<int> leant_index = prepare_learn_to_Calc();
            make_Log(Log_line.Start, TimerValue: start__full_time);
            int currentstep = 0;
            int all_step = (Rules_generator.Count() + Learn_algorithms.Count() * Repeat_into) * Repeat_renew_global;
            currentstep = Make_inform_back_process(currentstep, all_step);

         //   Parallel.For(0, Repeat_renew_global, i =>
             for (int i = 0; i < Repeat_renew_global; i++)
            { 
                 Classifier = new PCFuzzySystem(Classifier_learn_set, Classifier_test_set);
                DateTime start__curle_time = DateTime.Now;
                #region Генерация аппроксиматора

                make_Log(Log_line.StartGenerate, TimerValue: start__curle_time);
                if (Rules_generator.Count() == 0 && (is_UFS))
                {
                    Classifier = PCFSUFSLoader.loadUFS(Classifier as PCFuzzySystem, UFS_file_name);
                }
                 for (int ad = 0; ad < Rules_generator.Count(); ad++)
                {
                    make_Log(Log_line.PreGenerate_log, name_Alg: Rules_generator[ad].ToString());

                    Classifier = 
                    Rules_generator[ad].Generate(Classifier, Rules_generator_conf[gen_index[ad]]);
            
                    currentstep = Make_inform_back_process(currentstep, all_step);

                    make_Log(Log_line.PostGenerate_log, Classifier as PCFuzzySystem, name_Alg: Rules_generator[ad].ToString(true));
                    if (is_autosave) { save_FS(Classifier as PCFuzzySystem, Name_alg: Rules_generator[ad].ToString()); }
                    GC.Collect();

                }

                #endregion

                make_Log(Log_line.StartOptimaze, Classifier as PCFuzzySystem);

                for (int j = 0; j < Repeat_into; j++)
                {
                    #region Оптимизация аппроксиматора
                    for (int l = 0; l < Learn_algorithms.Count(); l++)
                    {
                        make_Log(Log_line.PreOptimaze_log, name_Alg: Learn_algorithms[l].ToString());
                        Classifier = 
                            Learn_algorithms[l].TuneUpFuzzySystem(Classifier, Learn_algorithm_conf[leant_index[l]]);
                        
                        currentstep = Make_inform_back_process(currentstep, all_step);

                        make_Log(Log_line.PostOptimaze_log,FS: Classifier as PCFuzzySystem, name_Alg: Learn_algorithms[l].ToString(true));

                        if (is_autosave)
                        {
                            save_FS(Classifier as PCFuzzySystem, Learn_algorithms[l].ToString());
                        }
                        GC.Collect();

                    }
                    #endregion
                }
                make_Log(Log_line.EndCircle, TimerSpan: (DateTime.Now - start__curle_time));

                GC.Collect();

            }
          //  );
            make_Log(Log_line.End, TimerSpan: DateTime.Now - start__full_time);
        }

        public bool is_autosave_FS { set { is_autosave = value; } }

        public string path_to_save { set { path = value; } }

        public int max_sections_of_Calc
        {
            get
            {
                int learn_count = 0;
                foreach (int learn_alg in Learn_algoritm_type)
                {
                    if (learn_alg != -1) { learn_count++; }

                }

                int add_count = 0;
                foreach (int add_alg in Rules_generator_type)
                {
                    if (add_alg != -1) { add_count++; }
                }


                return (add_count + learn_count * Repeat_into) * Repeat_renew_global;
            }
        }

        public System.ComponentModel.BackgroundWorker ProgressSource { set { progressSource = value; } }

        #endregion

        #region algorithms depend
        #region add generations method

 
        public string[] add_generator_for_Singleton()
        {
            List<string> result = new List<string>();
            for (int i = 0; i < AllMethods.InstanceOfInit.Count; i++)
            {
                result.Add(AllMethods.InstanceOfInit[i].ToString());
            }
            addGeneratorAlgorithms = result.ToArray();
          return  result.ToArray();
        }

        public object[] addGeneratorAlgorithm
        {
            get
            {
                add_generator_for_Singleton();
                object[] list_alg = new object[addGeneratorAlgorithms.Count()+1];
                list_alg[0] = "Нет";
                for (int i = 0; i < addGeneratorAlgorithms.Count(); i++)
                {
                    list_alg[i+1] = addGeneratorAlgorithms[i];
                }

                if (is_UFS)
                {
                    List<object> temp_list = new List<object>(list_alg);
                    temp_list.Add("UFS");
                    list_alg = temp_list.ToArray();
                }
                return list_alg;

            }
        }

        private List<int> prepare_generate_to_Calc()
        {
            #region Инициализация алгоритмов генерации

            List<int> rull_gen_conf_to_real_index = new List<int>();
            Rules_generator.Clear();
            for (int Ad = 0; Ad < Rules_generator_type.Count(); Ad++)
            {
                rull_gen_conf_to_real_index.Add(Ad);
                if (Rules_generator_type[Ad]==-1|| (Rules_generator_type[Ad]>=AllMethods.InstanceOfInit.Count))
                {
                    rull_gen_conf_to_real_index.Remove(Ad);
                    continue;
                
                }
                Rules_generator.Add(AllMethods.InstanceOfInit[Rules_generator_type[Ad]]);
                
           
            }

            #endregion
            return rull_gen_conf_to_real_index;
        }

   

        #endregion

        #region learn algorithm
        public string [] learn_algorithm_for_Singletone()
        {
            List<string> result = new List<string>();
            for (int i = 0; i < AllMethods.InstanceOfTune.Count(); i++)
            {
                result.Add(AllMethods.InstanceOfTune[i].ToString()); 
            }
            LearnAlgirithmsNames = result.ToArray();
            return result.ToArray();
        }




        public object[] learn_algorithm
        {
            get
            {
                object[] list_alg = new object[AllMethods.InstanceOfTune.Count()+1];
                list_alg[0] = "Нет";
                for (int i =0; i<AllMethods.InstanceOfTune.Count;i++)
                {list_alg[i+1]= AllMethods.InstanceOfTune[i].ToString();
                }
                return list_alg;

            }
        }
        private List<int> prepare_learn_to_Calc()
        {
            #region Инициализация алгоритмов оптимизации
            List<int> rull_learn_conf_real_index = new List<int>();
            Learn_algorithms.Clear();
            for (int l = 0; l < Learn_algoritm_type.Count(); l++)
            {
                rull_learn_conf_real_index.Add(l);
                if (Learn_algoritm_type[l] == -1) { rull_learn_conf_real_index.Remove(l); continue; }
                Learn_algorithms.Add(AllMethods.InstanceOfTune[Learn_algoritm_type[l]]);
             
            }

            #endregion
            return rull_learn_conf_real_index;
        }

    

        #endregion


        #endregion

        #region InterStruct

        #region Заполнение лога
        private void make_Log(Log_line EventCall, PCFuzzySystem FS = null, string name_Alg = "", DateTime TimerValue = new DateTime(), TimeSpan TimerSpan = new TimeSpan())
        {
            switch (EventCall)
            {
                case Log_line.Start:
                    {
                        LOG += "(" + TimerValue.ToString() + ")" + " Начало построения системы" + Environment.NewLine;
                        break;
                    }
                case Log_line.StartGenerate:
                    {
                        LOG += "(" + TimerValue.ToString() + ")" + " Начата генерация системы" + Environment.NewLine;

                        break;
                    }
                case Log_line.StartOptimaze:
                    {
                        LOG += "(" + DateTime.Now.ToString() + ")" + " Начата оптимизация системы" + Environment.NewLine;
                        break;
                    }


                case Log_line.PreGenerate_log:
                    {
                        LOG += "(" + DateTime.Now.ToString() + ")" + " Генерация алгоритмом " + name_Alg.ToString() + Environment.NewLine;
                        break;
                    }
                case Log_line.PostGenerate_log:
                    {
                        double LearnResult = FS.ClassifyLearnSamples();
                        double TestResult = FS.ClassifyTestSamples();
                     

                        LOG += "(" + DateTime.Now.ToString() + ")" + " Сгенерирована система сложностью " + FS.ValueComplexity().ToString() + Environment.NewLine +
                      "Точностью на обучающей выборке " +LearnResult.ToString() + ", Точность на тестовой выборке " + TestResult.ToString() + Environment.NewLine;
                        LOG += "Использован " + name_Alg.ToString() + Environment.NewLine;
                        break;
                    }
                case Log_line.PreOptimaze_log:
                    {
                        LOG += "(" + DateTime.Now.ToString() + ")" + " Оптимизация алгоритмом " + name_Alg.ToString() + Environment.NewLine;

                        break;
                    }
                case Log_line.PostOptimaze_log:
                    {
                        double LearnResult = FS.ClassifyLearnSamples();
                        double TestResult = FS.ClassifyTestSamples();
                    

                        LOG += "(" + DateTime.Now.ToString() + ")" + " оптимизированная система сложностью " + FS.ValueComplexity().ToString() + Environment.NewLine +
                        "Точностью на обучающей выборке " + LearnResult.ToString() + ", Точность на тестовой выборке " + TestResult.ToString() + Environment.NewLine;
                        LOG += "Использован " + name_Alg.ToString() + Environment.NewLine;

                        break;
                    }


                case Log_line.EndCircle:
                    {
                        LOG += "(" + DateTime.Now.ToString() + ")" + " Время построения системы" + TimerSpan.TotalSeconds.ToString() + Environment.NewLine; break;
                    }
                case Log_line.End:
                    {
                        LOG += "(" + DateTime.Now.ToString() + ")" + " Время построения всех систем" + TimerSpan.TotalSeconds.ToString() + Environment.NewLine; break;
                    }
                default: { LOG += "Не верный вызов" + Environment.NewLine; break; }

            }

        }
        #endregion

        private int Make_inform_back_process(int current_state, int max_state)
        {
            current_state++;
            string result = string.Format("{0}/{1}", current_state, max_state); ;


            progressSource.ReportProgress(current_state, result);
            return current_state;
        }

        private void save_FS(PCFuzzySystem FS, string Name_alg)
        {

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            string file_name = DateTime.Now.ToLongDateString() + " " + DateTime.Now.TimeOfDay.ToString("hh','mm','ss") + " (" + Thread.CurrentThread.ManagedThreadId.ToString() + ")" + "[" + FS.ValueComplexity().ToString() + "]{" + Name_alg + "}.ufs";
            
            PCFSUFSWriter.saveToUFS(FS, path + file_name);

        }

        bool is_UFS = false;
        IFuzzySystem Classifier = null;
        PCSamplesSet Classifier_learn_set = null;
        PCSamplesSet Classifier_test_set = null;
        List<IAbstractGenerator> Rules_generator = new List<IAbstractGenerator>();
        IGeneratorConf[] Rules_generator_conf = null;
        List<IAbstractLearnAlgorithm> Learn_algorithms = new List<IAbstractLearnAlgorithm>();
        ILearnAlgorithmConf[] Learn_algorithm_conf = null;
        int [] Learn_algoritm_type = null;
        int Repeat_into = 0;
        int Repeat_renew_global = 0;
        string LOG = "";
        bool is_autosave = false;
        string path = "";
        System.ComponentModel.BackgroundWorker progressSource = null;
        string[] addGeneratorAlgorithms;
        string[] LearnAlgirithmsNames;
        public int[] Rules_generator_type;

        enum Log_line
        {
            PreGenerate_log = 0,
            PostGenerate_log = 1,
            PreOptimaze_log = 2,
            PostOptimaze_log = 3,
            StartGenerate = 4,
            StartOptimaze = 5,
            Start = 6,
            EndCircle = 7,
            End = 8


        }
        #endregion

    }
}



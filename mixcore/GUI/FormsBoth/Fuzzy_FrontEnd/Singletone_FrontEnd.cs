using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySystem.SingletoneApproximate;
using FuzzySystem.SingletoneApproximate.UFS;

using FuzzySystem.SingletoneApproximate.LearnAlgorithm;
using System.IO;
using FuzzySystem.FuzzyAbstract.conf;
using System.Threading;
using System.Threading.Tasks;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyFrontEnd;
namespace FuzzySystem.Fuzzy_FrontEnd
{
    public class Singletone_FrontEnd : IFuzzySystemFroentEnd
    {

        MethodLoadHelperSingletoneApprox AllMethods;
        string UFS_file_name = "";

        #region IFuzzy_System_FroentEnd implementation

        public Singletone_FrontEnd()
        {
            AllMethods = new MethodLoadHelperSingletoneApprox();
        }

        #region Load samples set
        public virtual bool Load_learn_set(string file_name)
        {


            switch (new FileInfo(file_name).Extension)
            {
                case ".dat":
                    {
                        Approx_learn_set = new SASamplesSet(file_name);
                        return false;
                    }
                case ".ufs":
                    {
                        Approx_learn_set = Approx_learn_set.LoadLearnFromUFS(file_name);
                        try
                        {
                            Approx_test_set = Approx_test_set.LoadtestFromUFS(file_name);
                            UFS_file_name = file_name;
                            is_UFS = true;
                            return true;
                        }
                        catch { return false; }
                    }
                default:
                    {
                        Approx_learn_set = Approx_learn_set.LoadLearnFromUFS(file_name);
                        try
                        {
                            Approx_test_set = Approx_test_set.LoadtestFromUFS(file_name);
                            UFS_file_name = file_name;
                            is_UFS = true;
                            return true;
                        }
                        catch { return false; }
                    }
            }
        }

        public virtual void Load_test_set(string file_name)
        {

            switch (new FileInfo(file_name).Extension)
            {
                case ".dat":
                    {
                        Approx_test_set = new SASamplesSet(file_name);

                        break;

                    }
                case ".ufs":
                    {
                        Approx_test_set = Approx_test_set.LoadtestFromUFS(file_name);
                        UFS_file_name = file_name;
                        break;
                    }
                default:
                    {

                        Approx_test_set = Approx_test_set.LoadtestFromUFS(file_name);
                        break;
                    }

            }
        }

        #endregion

    
        public virtual void Set_count_add_generator(int count_used_Algorithm)
        {
            Rules_generator_conf = new IGeneratorConf[count_used_Algorithm];
            Rules_generator_type = new int[count_used_Algorithm];
            for (int i = 0; i < count_used_Algorithm; i++)
            {
                Rules_generator_type[i] = -2;
            }
        }

        public virtual void Set_count_learn_algorithm(int count_used_Algorithm)
        {
            Learn_algorithm_conf = new ILearnAlgorithmConf[count_used_Algorithm];
            Learn_algoritm_type = new int [count_used_Algorithm];
            for (int i = 0; i < count_used_Algorithm; i++)
            {
                Learn_algoritm_type[i] = -2;
            }
      
        }

        public virtual IGeneratorConf Set_add_generator_algorothm(int numAlgorithm, int typeAlgorithm)
        {

            if (Rules_generator_type[numAlgorithm] != typeAlgorithm-1)
            {
                Rules_generator_type[numAlgorithm] = typeAlgorithm-1;
                if (typeAlgorithm == 0 || ((typeAlgorithm - 1) >= AllMethods.InstanceOfInit.Count)) { Rules_generator_conf[numAlgorithm] = new NullConfForAll(); }
                else
                {
                    Rules_generator_conf[numAlgorithm] = AllMethods.InstanceOfInit[typeAlgorithm - 1].getConf(Approx_learn_set.Count_Vars);
                }
            }
            return Rules_generator_conf[numAlgorithm];
        }


        public virtual ILearnAlgorithmConf Set_learn_algorothm(int numAlgorithm, int typeAlgorithm)
        {
            if (Learn_algoritm_type[numAlgorithm] != typeAlgorithm-1)
            {
                Learn_algoritm_type[numAlgorithm] = typeAlgorithm-1;
                if (Learn_algoritm_type[numAlgorithm] < 0) {
                    Learn_algorithm_conf[numAlgorithm] = new NullConfForAll();
                }
                else
                {
                    Learn_algorithm_conf[numAlgorithm] = AllMethods.InstanceOfTune[Learn_algoritm_type[numAlgorithm]].getConf(Approx_learn_set.Count_Vars);
                }
            }

            return Learn_algorithm_conf[numAlgorithm];
        }

        public virtual void Set_count_repeate_into_cirlucle(int numRepeate) { Repeat_into = numRepeate; }

        public virtual void Set_count_repeate_renew(int numRepeate) { Repeat_renew_global = numRepeate; }

        public virtual string get_log { get { return LOG; } }

        public virtual bool Is_UFS { get { return is_UFS; } }

        public virtual int get_global_completed() { return Repeat_renew_global; }

        public virtual void Calc()
        {

            DateTime start__full_time = DateTime.Now;
            List<int> gen_index = prepare_generate_to_Calc();
            List<int> leant_index = prepare_learn_to_Calc();
            make_Log(Log_line.Start, TimerValue: start__full_time);
            int currentstep = 0;
            int all_step = (Rules_generator.Count() + Learn_algorithms.Count() * Repeat_into) * Repeat_renew_global;
            currentstep = Make_inform_back_process(currentstep, all_step);

         //   Parallel.For(0, Repeat_renew_global, i =>
            LOG = "";
             for (int i = 0; i < Repeat_renew_global; i++)
            { 
                 Approx_Singletone = new SAFuzzySystem(Approx_learn_set, Approx_test_set);
                DateTime start__curle_time = DateTime.Now;
                #region Генерация аппроксиматора

                make_Log(Log_line.StartGenerate, TimerValue: start__curle_time);
                if (Rules_generator.Count() == 0 && (is_UFS))
                {
                    Approx_Singletone = SAFSUFSLoader.loadUFS(Approx_Singletone,UFS_file_name);
                    
                }
                for (int ad = 0; ad < Rules_generator.Count(); ad++)
                {
                    make_Log(Log_line.PreGenerate_log, name_Alg: Rules_generator[ad].ToString());

                    Approx_Singletone = 
                    Rules_generator[ad].Generate(Approx_Singletone as SAFuzzySystem, Rules_generator_conf[gen_index[ad]]) as SAFuzzySystem;
            
                    currentstep = Make_inform_back_process(currentstep, all_step);

                    make_Log(Log_line.PostGenerate_log, Approx_Singletone, name_Alg: Rules_generator[ad].ToString(true));
                    if (is_autosave) { save_FS(Approx_Singletone, Name_alg: Rules_generator[ad].ToString()); }
                    GC.Collect();

                }

                #endregion

                make_Log(Log_line.StartOptimaze, Approx_Singletone);

                for (int j = 0; j < Repeat_into; j++)
                {
                    #region Оптимизация аппроксиматора
                    for (int l = 0; l < Learn_algorithms.Count(); l++)
                    {
                        make_Log(Log_line.PreOptimaze_log, name_Alg: Learn_algorithms[l].ToString());
                        Approx_Singletone = 
                            Learn_algorithms[l].TuneUpFuzzySystem(Approx_Singletone as SAFuzzySystem, Learn_algorithm_conf[leant_index[l]]) as SAFuzzySystem;
                        
                        currentstep = Make_inform_back_process(currentstep, all_step);

                        make_Log(Log_line.PostOptimaze_log,FS: Approx_Singletone, name_Alg: Learn_algorithms[l].ToString(true));

                        if (is_autosave)
                        {
                            save_FS(Approx_Singletone, Learn_algorithms[l].ToString());
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

        public virtual bool is_autosave_FS { set { is_autosave = value; } }

        public virtual string path_to_save { set { path = value; } }

        public virtual int max_sections_of_Calc
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

        public virtual System.ComponentModel.BackgroundWorker ProgressSource { set { progressSource = value; } }

        #endregion

        #region algorithms depend
        #region add generations method


        public virtual string[] add_generator_for_Singleton()
        {
            List<string> result = new List<string>();
            for (int i = 0; i < AllMethods.InstanceOfInit.Count; i++)
            {
                result.Add(AllMethods.InstanceOfInit[i].ToString());
            }
            addGeneratorAlgorithms = result.ToArray();
          return  result.ToArray();
        }

        public virtual object[] addGeneratorAlgorithm
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

        protected virtual List<int> prepare_generate_to_Calc()
        {
            #region Инициализация алгоритмов генерации

            List<int> rull_gen_conf_to_real_index = new List<int>();
            Rules_generator.Clear();
            for (int Ad = 0; Ad < Rules_generator_type.Count(); Ad++)
            {
                rull_gen_conf_to_real_index.Add(Ad);
                if ((Rules_generator_type[Ad] == -1) || (Rules_generator_type[Ad]>=AllMethods.InstanceOfInit.Count))
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
        public virtual string[] learn_algorithm_for_Singletone()
        {
            List<string> result = new List<string>();
            for (int i = 0; i < AllMethods.InstanceOfTune.Count(); i++)
            {
                result.Add(AllMethods.InstanceOfTune[i].ToString()); 
            }
            LearnAlgirithmsNames = result.ToArray();
            return result.ToArray();
        }




        public virtual object[] learn_algorithm
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
        protected virtual List<int> prepare_learn_to_Calc()
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
        protected virtual void make_Log(Log_line EventCall, SAFuzzySystem FS = null, string name_Alg = "", DateTime TimerValue = new DateTime(), TimeSpan TimerSpan = new TimeSpan())
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
                        double LearnResult = FS.approxLearnSamples();
                        double TestResult = FS.approxTestSamples();

                        double LearnResultMSE = FS.RMSEtoMSEforLearn(LearnResult);
                        double TestResultMSE = FS.RMSEtoMSEforTest(TestResult);
                      
                        double LearnResultMSEdiv2 = FS.RMSEtoMSEdiv2forLearn(LearnResult);
                        double TestResultMSEdiv2 = FS.RMSEtoMSEdiv2forTest(TestResult);

                        
                        LOG += "(" + DateTime.Now.ToString() + ")" + " Сгенерирована система сложностью " + FS.ValueComplexity().ToString() + Environment.NewLine +
                      "Точностью на обучающей выборке(RSME) " +LearnResult.ToString() + ", Точность на тестовой выборке(RMSE) " + TestResult.ToString() + Environment.NewLine+
                      "Точностью на обучающей выборке(MSE) " + LearnResultMSE.ToString() + ", Точность на тестовой выборке(MSE) " + TestResultMSE.ToString() + Environment.NewLine+
                      "Точностью на обучающей выборке(MSE/2) " + LearnResultMSEdiv2.ToString() + ", Точность на тестовой выборке(MSE/2) " + TestResultMSEdiv2.ToString() + Environment.NewLine;
                  
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
                        double LearnResult = FS.approxLearnSamples();
                        double TestResult = FS.approxTestSamples();


                        double LearnResultMSE = FS.RMSEtoMSEforLearn(LearnResult);
                        double TestResultMSE = FS.RMSEtoMSEforTest(TestResult);

                        double LearnResultMSEdiv2 = FS.RMSEtoMSEdiv2forLearn(LearnResult);
                        double TestResultMSEdiv2 = FS.RMSEtoMSEdiv2forTest(TestResult);

                        LOG += "(" + DateTime.Now.ToString() + ")" + " оптимизированная система сложностью " + FS.ValueComplexity().ToString() + Environment.NewLine +
                        "Точностью на обучающей выборке(RMSE) " + LearnResult.ToString() + ", Точность на тестовой выборке(RMSE) " + TestResult.ToString() + Environment.NewLine+
                        "Точностью на обучающей выборке(MSE) " + LearnResultMSE.ToString() + ", Точность на тестовой выборке(MSE) " + TestResultMSE.ToString() + Environment.NewLine +
                  "Точностью на обучающей выборке(MSE/2) " + LearnResultMSEdiv2.ToString() + ", Точность на тестовой выборке(MSE/2) " + TestResultMSEdiv2.ToString() + Environment.NewLine;
              
 
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

        protected virtual int Make_inform_back_process(int current_state, int max_state)
        {
            current_state++;
            string result = string.Format("{0}/{1}", current_state, max_state); ;


            progressSource.ReportProgress(current_state, result);
            return current_state;
        }

        protected virtual void save_FS(SAFuzzySystem FS, string Name_alg)
        {

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            string file_name = DateTime.Now.ToLongDateString() + " " + DateTime.Now.TimeOfDay.ToString("hh','mm','ss") + " (" + Thread.CurrentThread.ManagedThreadId.ToString() + ")" + "[" + FS.ValueComplexity().ToString() + "]{" + Name_alg + "}.ufs";
            SAFSUFSWriter.saveToUFS(FS, path + file_name);
          //  SAFSUFSWriter.Knowledge_base_Jsonto(FS.RulesDatabaseSet[0],fileName+".json");
        }

        bool is_UFS = false;
        SAFuzzySystem Approx_Singletone = null;
        SASamplesSet Approx_learn_set = null;
        SASamplesSet Approx_test_set = null;
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

        protected  enum Log_line
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


using System;
using System.Collections.Generic;
using System.Linq;
using Fuzzy_system.Approx_Singletone;
using Fuzzy_system.Approx_Singletone.UFS;
using Fuzzy_system.Approx_Singletone.add_generators;
using Fuzzy_system.Approx_Singletone.add_generators.conf;
using Fuzzy_system.Approx_Singletone.learn_algorithm;
using Fuzzy_system.Approx_Singletone.learn_algorithm.conf;
using System.IO;
using Fuzzy_system.Fuzzy_Abstract.add_generators.conf;
using Fuzzy_system.Fuzzy_Abstract.learn_algorithm.conf;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;

namespace Fuzzy_system.Fuzzy_FrontEnd
{
    public class Singletone_FrontEnd : IFuzzy_System_FroentEnd
    {
        #region IFuzzy_System_FroentEnd implementation
              
        #region Load samples set
        public bool Load_learn_set(string file_name)
        {


            switch (new FileInfo(file_name).Extension)
            {
                case ".dat":
                    {
                        Approx_learn_set = new a_samples_set(file_name);
                        return false;
                    }
                case ".ufs":
                    {
                        Approx_learn_set = Approx_learn_set.Load_learn_from_UFS(file_name);
                        try
                        {
                            Approx_test_set = Approx_test_set.Load_test_from_UFS(file_name);
                            is_UFS = true;
                            return true;
                        }
                        catch { return false; }
                    }
                default:
                    {
                        Approx_learn_set = Approx_learn_set.Load_learn_from_UFS(file_name);
                        try
                        {
                            Approx_test_set = Approx_test_set.Load_test_from_UFS(file_name);
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
                        Approx_test_set = new a_samples_set(file_name);

                        break;

                    }
                case ".ufs":
                    {
                        Approx_test_set = Approx_test_set.Load_test_from_UFS(file_name);
                        break;
                    }
                default:
                    {

                        Approx_test_set = Approx_test_set.Load_test_from_UFS(file_name);
                        break;
                    }

            }
        }

        #endregion

        public Abstract_generator_conf Set_add_generator_algorothm(int numAlgorithm, int typeAlgorithm)
        {
            add_generator_for_Singleton TypeAlgorithm = (add_generator_for_Singleton)typeAlgorithm;
            if (Rules_generator_type[numAlgorithm] != TypeAlgorithm)
            {
                Rules_generator_type[numAlgorithm] = TypeAlgorithm;
                Rules_generator_conf[numAlgorithm] = make_conf_for_gen(TypeAlgorithm);
            }
            return Rules_generator_conf[numAlgorithm];
        }

        public void Set_count_add_generator(int count_used_Algorithm)
        {
            Rules_generator_conf = new Abstract_generator_conf[count_used_Algorithm];
            Rules_generator_type = new add_generator_for_Singleton[count_used_Algorithm];
        }

        public void Set_count_learn_algorithm(int count_used_Algorithm)
        {
            Learn_algorithm_conf = new Abstract_learn_algorithm_conf[count_used_Algorithm];
            Learn_algoritm_type = new learn_algorithm_for_Singletone[count_used_Algorithm];
        }

        public Abstract_learn_algorithm_conf Set_learn_algorothm(int numAlgorithm, int typeAlgorithm)
        {
            learn_algorithm_for_Singletone TypeAlgorithm = (learn_algorithm_for_Singletone)typeAlgorithm;
            if (Learn_algoritm_type[numAlgorithm] != TypeAlgorithm)
            {
                Learn_algoritm_type[numAlgorithm] = TypeAlgorithm;
                Learn_algorithm_conf[numAlgorithm] = make_conf_for_learn(TypeAlgorithm);
             
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

            for (int i = 0; i < Repeat_renew_global; i++)
            {
                Approx_Singletone = new a_Fuzzy_System(Approx_learn_set, Approx_test_set);
                DateTime start__curle_time = DateTime.Now;
                #region Генерация аппроксиматора

                make_Log(Log_line.StartGenerate, TimerValue: start__curle_time);
                for (int ad = 0; ad < Rules_generator.Count(); ad++)
                {
                    make_Log(Log_line.PreGenerate_log, name_Alg: Rules_generator[ad].ToString());
                    Approx_Singletone = Rules_generator[ad].Generate(Approx_Singletone, Rules_generator_conf[gen_index[ad]]);
                    GC.Collect();

                    currentstep = Make_inform_back_process(currentstep, all_step);

                    make_Log(Log_line.PostGenerate_log, Approx_Singletone, name_Alg: Rules_generator[ad].ToString(true));
                    if (is_autosave) { save_FS(Approx_Singletone, Name_alg: Rules_generator[ad].ToString()); }
                }

                #endregion

                make_Log(Log_line.StartOptimaze, Approx_Singletone);

                for (int j = 0; j < Repeat_into; j++)
                {
                    #region Оптимизация аппроксиматора
                    for (int l = 0; l < Learn_algorithms.Count(); l++)
                    {
                        make_Log(Log_line.PreOptimaze_log, name_Alg: Learn_algorithms[l].ToString());
                        Approx_Singletone = Learn_algorithms[l].TuneUpFuzzySystem(Approx_Singletone, Learn_algorithm_conf[leant_index [l]]);
                        GC.Collect();

                        currentstep = Make_inform_back_process(currentstep, all_step);

                        make_Log(Log_line.PostOptimaze_log, Approx_Singletone, name_Alg: Learn_algorithms[l].ToString(true));

                        if (is_autosave)
                        {
                            save_FS(Approx_Singletone, Learn_algorithms[l].ToString());
                        }
                    }
                    #endregion
                }
                make_Log(Log_line.EndCircle, TimerSpan: (DateTime.Now - start__curle_time));

                GC.Collect();

            }
            make_Log(Log_line.End, TimerSpan: DateTime.Now - start__full_time);
        }
        
        public bool is_autosave_FS { set { is_autosave = value; } }
        
        public string path_to_save { set { path = value; } }
        
        public int max_sections_of_Calc
        {
            get
            {
                int learn_count = 0;
                foreach (learn_algorithm_for_Singletone learn_alg in Learn_algoritm_type)
                {
                    if (learn_alg != learn_algorithm_for_Singletone.Нет) { learn_count++; }

                }

                int add_count = 0;
                foreach (add_generator_for_Singleton add_alg in Rules_generator_type)
                {
                    if (add_alg != add_generator_for_Singleton.Нет) { add_count++; }
                }


                return (add_count + learn_count * Repeat_into) * Repeat_renew_global;
            }
        }
      
        public System.ComponentModel.BackgroundWorker ProgressSource { set { progressSource = value; } }
        
        #endregion

        #region algorithms depend block
        #region add generations method
        public enum add_generator_for_Singleton
        {
            Нет = 0,
            Перебором = 1,
            Случайны_генератор = 2,
            Исключение_правил = 3,
            Заданой_структурой = 4,
            C_средних = 5

        }

        public object[] add_generator_algorithm
        {
            get
            {
                object[] list_alg = null;

                list_alg = new object[] { // Full name of add_generator_for_Singleton
            "Нет",
            "Перебором",
            "Случайный генератор",
            "Исключение правил",
            "Перебор с заданой структурой",
            "На основе C-средних"
           };
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
            for (int Ad = 0; Ad < Rules_generator_type.Count(); Ad++)
            {
                rull_gen_conf_to_real_index.Add(Ad);
                switch (Rules_generator_type[Ad])
                {
                    case add_generator_for_Singleton.Нет: { rull_gen_conf_to_real_index.Remove(Ad); break; }
                    case add_generator_for_Singleton.Перебором: { Rules_generator.Add(new Generator_Rules_everyone_with_everyone()); break; }
                    case add_generator_for_Singleton.Случайны_генератор: { Rules_generator.Add(new Generator_Rulees_simple_random()); break; }
                    case add_generator_for_Singleton.Исключение_правил: { Rules_generator.Add(new Generator_Rulles_shrink()); break; }
                    case add_generator_for_Singleton.Заданой_структурой: { Rules_generator.Add(new Generator_Term_shrink_and_rotate()); break; }
                    case add_generator_for_Singleton.C_средних: { Rules_generator.Add(new k_mean_rules_generator()); break; }

                    default: { rull_gen_conf_to_real_index.Remove(Ad); break; }
                }

            }

            #endregion
            return rull_gen_conf_to_real_index;
        }

        private Abstract_generator_conf make_conf_for_gen(add_generator_for_Singleton TypeAlgorithm)
        {
            switch (TypeAlgorithm)
            {
                case add_generator_for_Singleton.Нет: { return null; }
                case add_generator_for_Singleton.Перебором: { return new init_everyone_with_everyone(Approx_learn_set.Count_Vars); }
                case add_generator_for_Singleton.Случайны_генератор: { return new Generator_Rulles_simple_random_conf(); }
                case add_generator_for_Singleton.Исключение_правил: { return new Rulles_shrink_conf(Approx_learn_set.Count_Vars); }
                case add_generator_for_Singleton.Заданой_структурой: { return new Term_shrink_and_rotate_conf(Approx_learn_set.Count_Vars); }
                case add_generator_for_Singleton.C_средних: { return new k_mean_rules_generator_conf(); }

                default: { return null; }

            }

        }


        #endregion

        #region learn algorithm
        public enum learn_algorithm_for_Singletone
        {
            Нет = 0,
            Случайный_поиск = 1,
            Эволюционная_стратегия = 2,
            Исключить_правила = 3,
            Исключить_термы = 4,
            МНК = 5,
            РЧ = 6

        }




        public object[] learn_algorithm
        {
            get
            {
                object[] list_alg = new object[] { // Full name for learn_algorithm_for_Singletone
            "Нет",
            "Случайный поиск",
            "Эволюционная стратегия",
            "Исключить правила",
            "Исключить термы",
             "МНК (для однотипных функций принадлежноти)",
             "Роящиеся частицы"
               };


                Assembly test = Assembly.GetExecutingAssembly();
                Type[] tests = test.GetTypes();
                 List<Type> mb= new List<Type>();
                foreach(Type tsp in tests)
                {
                    if (tsp != null)
                    {
                        if ((tsp.Namespace!=null) && (tsp.Namespace.Equals("Fuzzy_system.Approx_Singletone.add_generators", StringComparison.OrdinalIgnoreCase)))
                        {
                            mb.Add(tsp);
                        }
                    }
                    else { MessageBox.Show("s"); }
                }
                Type to_del = mb.First(x=>x.Name=="Abstract_generator");
                mb.Remove(to_del);
               
                    foreach (Type st in mb)
                {
                        var get_class_name= Activator.CreateInstance(st);
                      MethodInfo mf =  st.GetMethod("ToString",BindingFlags.Instance|BindingFlags.Public|BindingFlags.DeclaredOnly);
                      string tempstg = (string)mf.Invoke(get_class_name,new object[] {false} );
                    MessageBox.Show(tempstg);
                }


                return list_alg;
              
            }
        }
        private List<int> prepare_learn_to_Calc()
        {
            #region Инициализация алгоритмов оптимизации
            List<int> rull_learn_conf_real_index = new List<int>();
            for (int l = 0; l < Learn_algoritm_type.Count(); l++)
            {
                rull_learn_conf_real_index.Add(l);
                switch (Learn_algoritm_type[l])
                {
                    case learn_algorithm_for_Singletone.Нет: { rull_learn_conf_real_index.Remove(l); break; }
                    case learn_algorithm_for_Singletone.Исключить_правила: { Learn_algorithms.Add(new Optimize_Rulles_shrink()); break; }
                    case learn_algorithm_for_Singletone.Исключить_термы: { Learn_algorithms.Add(new Optimize_Term_shrink_and_rotate()); break; }
                    case learn_algorithm_for_Singletone.МНК: { Learn_algorithms.Add(new Adaptive_LSM()); break; }
                    case learn_algorithm_for_Singletone.Случайный_поиск: { Learn_algorithms.Add(new Config_Random_Search()); break; }
                    case learn_algorithm_for_Singletone.Эволюционная_стратегия: { Learn_algorithms.Add(new Es_method()); break; }
                    default: { rull_learn_conf_real_index.Remove(l); break; }
                }
            }

            #endregion
            return rull_learn_conf_real_index;
        }

        private Abstract_learn_algorithm_conf make_conf_for_learn(learn_algorithm_for_Singletone TypeAlgorithm)
        {
            #region Инициализация
            switch (TypeAlgorithm)
            {
                case learn_algorithm_for_Singletone.Нет: { return null; }
                case learn_algorithm_for_Singletone.Случайный_поиск: { return new Term_Config_Random_Search_conf(); }
                case learn_algorithm_for_Singletone.Исключить_правила: { return new Optimize_Rulles_shrink_conf((int)Math.Pow(5, Approx_learn_set.Count_Vars)); }

                case learn_algorithm_for_Singletone.Исключить_термы: { return new Optimize_Term_shrink_and_rotate_conf(Approx_learn_set.Count_Vars); }
                case learn_algorithm_for_Singletone.Эволюционная_стратегия: { return new Es_Config(Approx_learn_set.Count_Vars); }
                case learn_algorithm_for_Singletone.МНК: { return new Null_conf_for_all(); }
                case learn_algorithm_for_Singletone.РЧ: { return new Term_Config_PSO_Search_conf(); }

            }
            #endregion
            return null;
        }
   

        #endregion
        
     
        #endregion

        #region InterStruct

        #region Заполнение лога
        private void make_Log(Log_line EventCall, a_Fuzzy_System FS = null, string name_Alg = "", DateTime TimerValue = new DateTime(), TimeSpan TimerSpan = new TimeSpan())
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
                        LOG += "(" + DateTime.Now.ToString() + ")" + " Сгенерирована система сложностью " + FS.value_complexity().ToString() + Environment.NewLine +
                      "Точностью на обучающей выборке " + FS.approx_Learn_Samples().ToString() + ", Точность на тестовой выборке " + FS.approx_Test_Samples().ToString() + Environment.NewLine;
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
                        LOG += "(" + DateTime.Now.ToString() + ")" + " оптимизированная система сложностью " + FS.value_complexity().ToString() + Environment.NewLine +
                        "Точностью на обучающей выборке " + Approx_Singletone.approx_Learn_Samples().ToString() + ", Точность на тестовой выборке " + FS.approx_Test_Samples().ToString() + Environment.NewLine;
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

        private void save_FS(a_Fuzzy_System FS, string Name_alg)
        {

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            string file_name = DateTime.Now.ToLongDateString() + " " + DateTime.Now.TimeOfDay.ToString("hh','mm','ss") + " (" + Thread.CurrentThread.ManagedThreadId.ToString() + ")" + "[" + Approx_Singletone.value_complexity().ToString() + "]{" + Name_alg + "}.ufs";
            a_FS_UFS_Writer.save_to_UFS(FS, path + file_name);

        }

        bool is_UFS = false;
        a_Fuzzy_System Approx_Singletone = null;
        a_samples_set Approx_learn_set = null;
        a_samples_set Approx_test_set = null;
        List<Abstract_generator> Rules_generator = new List<Abstract_generator>();
        Abstract_generator_conf[] Rules_generator_conf = null;
        add_generator_for_Singleton[] Rules_generator_type = null;
        List<Abstract_learn_algorithm> Learn_algorithms = new List<Abstract_learn_algorithm>();
        Abstract_learn_algorithm_conf[] Learn_algorithm_conf = null;
        learn_algorithm_for_Singletone[] Learn_algoritm_type = null;
        int Repeat_into = 0;
        int Repeat_renew_global = 0;
        string LOG = "";
        bool is_autosave = false;
        string path = "";
        System.ComponentModel.BackgroundWorker progressSource = null;

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


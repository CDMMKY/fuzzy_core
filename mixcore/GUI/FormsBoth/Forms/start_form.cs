using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Mix_core.Properties;
using FuzzySystem;

using System.Collections.Generic;

using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyFrontEnd;

namespace Mix_core.Forms
{
    public partial class Start_F : Form
    {

        public string lastLog = "";
        internal IFuzzySystemFroentEnd Fuzzy_system = null;
        private Result_F temp_result_form;
        private string count_interation_complete;
        List<ComboBox> add_algorithm_CB_list = new List<ComboBox>();
        List<ComboBox> learn_algorithm_CB_list = new List<ComboBox>();
        List<Button> add_algorithm_B_list = new List<Button>();
        List<Button> learn_algorithm_B_list = new List<Button>();

        List<ComboBox> all_algorithms_CB_List = new List<ComboBox>();
        List<Button> all_algorithms_B_List = new List<Button>();
        int current_section = 0;
        int SectNowMf = 0;
        FileMultiSelectForm mf = new FileMultiSelectForm();
        bool isMultiChoosed;

        string path = "";
        string fileName = "";


        public Start_F()
        {
            InitializeComponent();
            add_algorithm_CB_list.Add(generator_rull_choose_CB);
            add_algorithm_CB_list.Add(generator_rull2_CB);
            learn_algorithm_CB_list.Add(learn_algorithm_choose_CB);
            learn_algorithm_CB_list.Add(learn_algorith2_CB);
            learn_algorithm_CB_list.Add(additional_learn_algorithm_choose_CB);
            learn_algorithm_CB_list.Add(learn_algorith4_CB);

            add_algorithm_B_list.Add(generator_rull_conf_B);
            add_algorithm_B_list.Add(generator_rull2_B);

            learn_algorithm_B_list.Add(learn_algorithm_conf_B);
            learn_algorithm_B_list.Add(learn_algorithm2_b);
            learn_algorithm_B_list.Add(additional_learn_algorithm_conf_B);
            learn_algorithm_B_list.Add(learn_algorithm4_b);

            all_algorithms_B_List.AddRange(add_algorithm_B_list);
            all_algorithms_B_List.AddRange(learn_algorithm_B_list);

            all_algorithms_CB_List.AddRange(add_algorithm_CB_list);
            all_algorithms_CB_List.AddRange(learn_algorithm_CB_list);

        }

        private void ready_to_run()
        {

            start_B.Visible = true;
            foreach (Button btn in add_algorithm_B_list)
            {
                btn.Visible = true;
            }
            foreach (Button btn in learn_algorithm_B_list)
            {
                btn.Visible = true;
            }

        }


        private void learn_samples_browse_B_Click(object sender, EventArgs e)
        {
            Open_samples_FD.FileName = "";
            if (Open_samples_FD.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(Open_samples_FD.FileName))
                {
                    if (Fuzzy_system.Load_learn_set(Open_samples_FD.FileName))
                    {
                        #region visual code
                        test_samples_TB.ForeColor = Color.Black;
                        test_samples_TB.Text =Path.GetFileName(  Open_samples_FD.FileName);
                        //       test_samples_B.Visible = false;
                        #endregion

                    }
                    #region visual code


                    learn_samples_TB.ForeColor = Color.Black;
                    learn_samples_TB.Text = Path.GetFileName(Open_samples_FD.FileName);
                    //           learn_samples_browse_B.Visible = false;
                    if (!Fuzzy_system.Is_UFS)
                        test_samples_B.Visible = true;



                    set_enabledisable_CB(all_algorithms_CB_List, true);
                    set_enabledisable_B(all_algorithms_B_List, true);
                    fill_CB(add_algorithm_CB_list, Fuzzy_system.addGeneratorAlgorithm);
                    fill_CB(learn_algorithm_CB_list, Fuzzy_system.learn_algorithm);
                    load_selected_index();
                    if (Fuzzy_system.Is_UFS)
                    {
                        generator_rull_choose_CB.SelectedIndex = generator_rull_choose_CB.Items.Count - 1;
                    }


                    inter_Repeate_UD.Value = Settings.Default.count_repeate_full_cirle;
                    inter_Repeate_UD.Enabled = true;

                    start_count_UD.Enabled = true;
                    start_count_UD.Value = Settings.Default.Global_count_restart;

                    autosave_fuzzy_System_CHB.Enabled = true;
                    autosave_fuzzy_System_CHB.Checked = Settings.Default.save_FS_check;
                    autosave_log_CHB.Checked = Settings.Default.save_log_check;
                    autosave_log_CHB.Enabled = true;
                    ready_to_run();
                    string testFile = Open_samples_FD.FileName.Replace("tra.dat", "tst.dat");
                    if (File.Exists(testFile))
                    {
                        #region visual code

                        test_samples_TB.ForeColor = Color.Black;
                        test_samples_TB.Text = testFile;
                        //        test_samples_B.Visible = false;
                        #endregion
                        Fuzzy_system.Load_test_set(testFile);
                    }


                    #endregion
                }
            }
        }

        private void test_samples_B_Click(object sender, EventArgs e)
        {
            Open_samples_FD.FileName = "";
            if (Open_samples_FD.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(Open_samples_FD.FileName))
                {
                    #region visual code

                    test_samples_TB.ForeColor = Color.Black;
                    test_samples_TB.Text = Path.GetFileName(Open_samples_FD.FileName);
                    //   test_samples_B.Visible = false;
                    #endregion
                    Fuzzy_system.Load_test_set(Open_samples_FD.FileName);
                }
            }
        }

        private void start_B_Click(object sender, EventArgs e)
        {
            Console.Clear();
            #region visual code

            back_ground_process_PB.Visible = true;

            start_B.Visible = false;

            autosave_log_CHB.Enabled = false;
            autosave_fuzzy_System_CHB.Enabled = false;

            int typealg;

            typealg = add_algorithm_CB_list[0].SelectedIndex;
            Fuzzy_system.Set_add_generator_algorothm(0, typealg);
            typealg = add_algorithm_CB_list[1].SelectedIndex;
            Fuzzy_system.Set_add_generator_algorothm(1, typealg);

            typealg = learn_algorithm_CB_list[0].SelectedIndex;
            Fuzzy_system.Set_learn_algorothm(0, typealg);

            typealg = learn_algorithm_CB_list[1].SelectedIndex;
            Fuzzy_system.Set_learn_algorothm(1, typealg);

            typealg = learn_algorithm_CB_list[2].SelectedIndex;
            Fuzzy_system.Set_learn_algorothm(2, typealg);

            typealg = learn_algorithm_CB_list[3].SelectedIndex;
            Fuzzy_system.Set_learn_algorothm(3, typealg);


            Settings.Default.Global_count_restart = (int)start_count_UD.Value;
            Settings.Default.count_repeate_full_cirle = (int)inter_Repeate_UD.Value;

            save_selected_index();

            Fuzzy_system.Set_count_repeate_into_cirlucle(Settings.Default.count_repeate_full_cirle);
            Fuzzy_system.Set_count_repeate_renew(Settings.Default.Global_count_restart);
            progress_T.Enabled = true;

            Fuzzy_system.is_autosave_FS = autosave_fuzzy_System_CHB.Checked;
            Fuzzy_system.path_to_save = (new FileInfo(Application.ExecutablePath)).DirectoryName + "\\FS\\";
            Fuzzy_system.ProgressSource = back_ground_process;
           
            if ((temp_result_form == null) || (temp_result_form.IsDisposed))
          { 
                if (isMultiChoosed)
            temp_result_form = new Result_F(Fuzzy_system.TypeFuzzySystem,isMultiChoosed,mf.TraFiles.Count);
                else temp_result_form = new Result_F(Fuzzy_system.TypeFuzzySystem, isMultiChoosed, 0);



                if (autosave_log_CHB.Checked)
                {
                    path = (new FileInfo(Application.ExecutablePath)).DirectoryName + "\\logs\\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    fileName = DateTime.Now.ToLongDateString() + " " + DateTime.Now.TimeOfDay.ToString("hh','mm','ss") + ".txt";
                }
                temp_result_form.pathtoSave = path + fileName;
            }
          temp_result_form.Show();
          temp_result_form.Activate();


            #endregion
            lastLog = "";

            back_ground_process.RunWorkerAsync();
        }

        private void progress_T_Tick(object sender, EventArgs e)
        {
            if (!lastLog.Equals(Fuzzy_system.get_log, StringComparison.OrdinalIgnoreCase))
            {
                lastLog =  Fuzzy_system.get_log.Clone() as string;
                temp_result_form.Result_RTB.Text = lastLog.Clone() as string;
       
            temp_result_form.Result_RTB.Select(temp_result_form.Result_RTB.Text.Length-1,temp_result_form.Result_RTB.Text.Length-1);
            
            
            }
           
            count_complete_L.Text = count_interation_complete;
        }


        private void autosave_log_CHB_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.save_log_check = autosave_log_CHB.Checked;
            Settings.Default.Save();
        }


        private void autosave_fuzzy_System_CHB_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.save_FS_check = autosave_fuzzy_System_CHB.Checked;
            Settings.Default.Save();
        }



        private void back_ground_process_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            if (isMultiChoosed)
            {
                string[] ps = ((string)e.UserState).Split(new char[] { '\\', '/', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                count_interation_complete = $"{SectNowMf}/{(int.Parse(ps[1])+1)*mf.TraFiles.Count}";
                SectNowMf++;
            }
            else count_interation_complete = (string)e.UserState;

            current_section = e.ProgressPercentage;
            
        }

        private void Start_F_Load(object sender, EventArgs e)
        {

            Type_FuzzySystem_CB.Items.Clear();
            Type_FuzzySystem_CB.Items.AddRange(FuzzySystemRelisedList.AllTypesFuzzySystem);
            Type_FuzzySystem_CB.Text = "Выберите тип нечеткой системы";
        }
       
        private void Type_FuzzySystem_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            learn_samples_TB.Text = "Выберите файл";
            MultiSelectButton.Visible = true;
            learn_samples_TB.Enabled = true;
            learn_samples_browse_B.Visible = true;
            Fuzzy_system = FrontEnd_Construction.init_fuzzy_system(Type_FuzzySystem_CB.SelectedIndex);
            clear_CB(all_algorithms_CB_List);
            set_enabledisable_CB(all_algorithms_CB_List, false);
            set_enabledisable_B(all_algorithms_B_List, false);

            test_samples_TB.Text = "Выберите файл";
            test_samples_TB.Enabled = false;
            test_samples_B.Visible = false;
            start_B.Visible = false;
            inter_Repeate_UD.Enabled = false;
            start_count_UD.Enabled = false;
            autosave_fuzzy_System_CHB.Enabled = false;
            autosave_log_CHB.Enabled = false;

           Fuzzy_system.Set_count_add_generator(2);
           Fuzzy_system.Set_count_learn_algorithm(4);

        }


        private void set_enabledisable_CB(List<ComboBox> List_CB, bool state)
        {
            List_CB.ForEach(x => x.Enabled = state);

        }

        private void clear_CB(List<ComboBox> List_CB)
        {
            List_CB.ForEach(x => x.Items.Clear());
            List_CB.ForEach(x => x.Text = "");

        }

        private void fill_CB(List<ComboBox> List_CB, object[] value)
        {
            List_CB.ForEach(x => x.Items.Clear());

            List_CB.ForEach(x => x.Items.AddRange(value));

        }

        private void set_enabledisable_B(List<Button> List_CB, bool state)
        {
            List_CB.ForEach(x => x.Enabled = state);
        }


        private void load_selected_index()
        {
           generator_rull_choose_CB.SelectedIndex = 0;
           generator_rull2_CB.SelectedIndex = 0;
           learn_algorithm_choose_CB.SelectedIndex = 0;
           learn_algorith2_CB.SelectedIndex = 0;
           learn_algorith4_CB.SelectedIndex = 0;
         
            additional_learn_algorithm_choose_CB.SelectedIndex = 0;

            if ((Settings.Default.Index0_add_algorithm < generator_rull_choose_CB.Items.Count) && (Settings.Default.Index0_add_algorithm > -1))
            { generator_rull_choose_CB.SelectedIndex = Settings.Default.Index0_add_algorithm; }


            if ((Settings.Default.Index1_add_algorithm < generator_rull2_CB.Items.Count) && (Settings.Default.Index1_add_algorithm > -1))
            { generator_rull2_CB.SelectedIndex = Settings.Default.Index1_add_algorithm; }


            if ((Settings.Default.Index0_learn_algorithm < learn_algorithm_choose_CB.Items.Count) && (Settings.Default.Index0_learn_algorithm > -1))
            {
                learn_algorithm_choose_CB.SelectedIndex = Settings.Default.Index0_learn_algorithm;
            }

            if ((Settings.Default.Index1_learn_algorithm < learn_algorith2_CB.Items.Count) && (Settings.Default.Index1_learn_algorithm > -1))
            {
                learn_algorith2_CB.SelectedIndex = Settings.Default.Index1_learn_algorithm;
            }


            if ((Settings.Default.Index2_learn_algorithm < additional_learn_algorithm_choose_CB.Items.Count) && (Settings.Default.Index2_learn_algorithm > -1))
            {
                additional_learn_algorithm_choose_CB.SelectedIndex = Settings.Default.Index2_learn_algorithm;
            }

            if ((Settings.Default.Index4_learn_algorithm < learn_algorith4_CB.Items.Count) && (Settings.Default.Index4_learn_algorithm > -1))
            {
                learn_algorith4_CB.SelectedIndex = Settings.Default.Index4_learn_algorithm;
            }
        }


        private void save_selected_index()
        { 
            Settings.Default.Index0_add_algorithm = generator_rull_choose_CB.SelectedIndex;
            Settings.Default.Index1_add_algorithm = generator_rull2_CB.SelectedIndex;
            Settings.Default.Index0_learn_algorithm = learn_algorithm_choose_CB.SelectedIndex;
            Settings.Default.Index1_learn_algorithm = learn_algorith2_CB.SelectedIndex;
            Settings.Default.Index2_learn_algorithm = additional_learn_algorithm_choose_CB.SelectedIndex;
            Settings.Default.Index4_learn_algorithm = learn_algorith4_CB.SelectedIndex;
            Settings.Default.Save();
        }

        private void Start_F_FormClosing(object sender, FormClosingEventArgs e)
        {
            save_selected_index();
        }

                #region Настройки для генераторов правил
        private void generator_rull_B_Click(object sender, EventArgs e)
        {
            int pushed=0;
            int.TryParse( (sender as Button).Tag.ToString(),out pushed);
            int choosen_alg = add_algorithm_CB_list[pushed].SelectedIndex;
           
            string algName = Fuzzy_system.addGeneratorAlgorithm[choosen_alg].ToString();
            universal_conf_form temp_conf_form =makeConfForm(Fuzzy_system.Set_add_generator_algorothm(pushed, choosen_alg),algName);
     
            if (temp_conf_form.conf_algorithm_params_PG.SelectedObject != null) { temp_conf_form.ShowDialog(); }
        }
        #endregion


        private universal_conf_form makeConfForm(IGeneratorConf SourceObject, string name)
        {
            universal_conf_form temp_conf_form = new universal_conf_form();
                temp_conf_form.Text = "Параметры генерации " + name;
                temp_conf_form.conf_algorithm_params_PG.SelectedObject = SourceObject;
            
            return temp_conf_form;
        }

        private universal_conf_form  makeConfForm(ILearnAlgorithmConf SourceObject, string name)
        {
            universal_conf_form temp_conf_form = new universal_conf_form();
            temp_conf_form.Text = "Параметры идентификации " + name;
            temp_conf_form.conf_algorithm_params_PG.SelectedObject = SourceObject;
            
                return temp_conf_form;
        }

        #region настройки обучающего метода
        private void learn_algorithm_conf_B_Click(object sender, EventArgs e)
        {
       
            int pushed = 0;
            int.TryParse((sender as Button).Tag.ToString(), out pushed);
            int choosen_alg = learn_algorithm_CB_list[pushed].SelectedIndex;
            string AlgName = Fuzzy_system.learn_algorithm[choosen_alg].ToString();
          universal_conf_form temp_conf_form=  makeConfForm(Fuzzy_system.Set_learn_algorothm(pushed, choosen_alg),AlgName);
            if (temp_conf_form.conf_algorithm_params_PG.SelectedObject != null) { temp_conf_form.ShowDialog(); }

        }
        #endregion


        #region Кнопка запуск
        private void back_ground_process_DoWork(object sender, DoWorkEventArgs e)
        {


            temp_result_form.ApproxLearnResult = new List<double>();
            temp_result_form.ApproxTestResult = new List<double>();
            temp_result_form.ApproxLearnResultMSE = new List<double>();
            temp_result_form.ApproxTestResultMSE = new List<double>();
            temp_result_form.ApproxLearnResultMSEdiv2 = new List<double>();
            temp_result_form.ApproxTestResultMSEdiv2 = new List<double>();

            temp_result_form.ClassErLearn = new List<double>();
            temp_result_form.ClassErTest = new List<double>();
            temp_result_form.ClassLearnResult = new List<double>();
            temp_result_form.ClassTestResult = new List<double>();
          

                try
                {
                SectNowMf = 0;

                Fuzzy_system.Calc();
                if (isMultiChoosed)
                { for (int mIndex = 1; mIndex < mf.TraFiles.Count; mIndex++)
                    {
                        Fuzzy_system.Load_learn_set(mf.TraFiles[mIndex]);
                        Fuzzy_system.Load_test_set(mf.TstFiles[mIndex]);
                        Fuzzy_system.Calc();
                    }
                }
            }

            catch (Exception exct)
            {
                var exx = exct;
                while (exx != null) { 
                MessageBox.Show(exx.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(exx.Source, "Место возникновения", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Console.WriteLine(exx.Message);
                Console.WriteLine(exx.Source);
                Console.WriteLine(exx.TargetSite);
                    exx = exx.InnerException;
                }
                //   Application.Exit();
            }
        }


        #region действия по окончанию всех вычислений

        private void back_ground_process_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            #region visual code
            start_B.Visible = true;
            autosave_log_CHB.Enabled = true;
            autosave_fuzzy_System_CHB.Enabled = true;
            progress_T.Enabled = false;
            count_complete_L.Text = "";
            back_ground_process_PB.Value = 0;
            back_ground_process_PB.Visible = false;
            string path2 = (new FileInfo(Application.ExecutablePath)).DirectoryName + "\\";
            string filename = "tada.wav";
            System.Media.SoundPlayer tada_player = new System.Media.SoundPlayer(path2 + filename);
            tada_player.LoadAsync();
            tada_player.PlaySync();
            if (!temp_result_form.Result_RTB.Text.Equals(Fuzzy_system.get_log, StringComparison.OrdinalIgnoreCase))
            {
                temp_result_form.Result_RTB.Text = Fuzzy_system.get_log;
                temp_result_form.Result_RTB.Select(temp_result_form.Result_RTB.Text.Length - 1, temp_result_form.Result_RTB.Text.Length - 1);
            }
            temp_result_form.ApproxLearnResult = Fuzzy_system.ApproxLearnResult;
            temp_result_form.ApproxTestResult = Fuzzy_system.ApproxTestResult;
            temp_result_form.ApproxLearnResultMSE = Fuzzy_system.ApproxLearnResultMSE;
            temp_result_form.ApproxTestResultMSE = Fuzzy_system.ApproxTestResultMSE;
            temp_result_form.ApproxLearnResultMSEdiv2 = Fuzzy_system.ApproxLearnResultMSEdiv2;
            temp_result_form.ApproxTestResultMSEdiv2 = Fuzzy_system.ApproxTestResultMSEdiv2;

            temp_result_form.ClassErLearn = Fuzzy_system.ClassErLearn;
            temp_result_form.ClassErTest = Fuzzy_system.ClassErTest;
            temp_result_form.ClassLearnResult = Fuzzy_system.ClassLearnResult;
            temp_result_form.ClassTestResult = Fuzzy_system.ClassTestResult;

      if (temp_result_form.Visible ==false )
            { temp_result_form.Show(); }
     
            #endregion
            if (autosave_log_CHB.Checked)
            {
                temp_result_form.Result_RTB.SaveFile (path + fileName, RichTextBoxStreamType.UnicodePlainText);
            }


     
        #endregion

        }


        #endregion

        private void learn_algorith4_CB_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (mf.ShowDialog() == DialogResult.OK)
            {if (mf.TraFiles.Count>0)
                {
                    Fuzzy_system.Load_learn_set(mf.TraFiles[0]);
                    Fuzzy_system.Load_test_set(mf.TstFiles[0]);
                    isMultiChoosed = true;
                    MultiChooseStatusLabel.Text = "Используется мультивыбор";
                MultiChooseStatusLabel.ForeColor = Color.Black;
                learn_samples_TB.Text = "Используется мультивыбор";
                learn_samples_TB.ForeColor = Color.Black;
                learn_samples_browse_B.Enabled = false;
                learn_samples_browse_B.Visible = false;
                test_samples_TB.Text = "Используется мультивыбор";
                test_samples_TB.ForeColor = Color.Black;
                test_samples_B.Enabled = false;



                set_enabledisable_CB(all_algorithms_CB_List, true);
                set_enabledisable_B(all_algorithms_B_List, true);
                fill_CB(add_algorithm_CB_list, Fuzzy_system.addGeneratorAlgorithm);
                fill_CB(learn_algorithm_CB_list, Fuzzy_system.learn_algorithm);
                load_selected_index();
                if (Fuzzy_system.Is_UFS)
                {
                    generator_rull_choose_CB.SelectedIndex = generator_rull_choose_CB.Items.Count - 1;
                }


                inter_Repeate_UD.Value = Settings.Default.count_repeate_full_cirle;
                inter_Repeate_UD.Enabled = true;

                start_count_UD.Enabled = true;
                start_count_UD.Value = Settings.Default.Global_count_restart;

                autosave_fuzzy_System_CHB.Enabled = true;
                autosave_fuzzy_System_CHB.Checked = Settings.Default.save_FS_check;
                autosave_log_CHB.Checked = Settings.Default.save_log_check;
                autosave_log_CHB.Enabled = true;
                ready_to_run();

            }
            }

        }
    }
}
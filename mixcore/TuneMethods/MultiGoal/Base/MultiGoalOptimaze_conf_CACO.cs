using System.ComponentModel;
using System.Linq;

namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{
    public partial class MultiGoalOptimaze_conf
    { 
        #region МНАМК


        [Description("Использовать НАМК"), Category("НАМК")]
        public YesNo Использовать_НАМК
        {
            get { return toYesNo(AntsMethods.Properties. SettingsBase.Default.MACO_Used); }
            set
            {
                AntsMethods.Properties.SettingsBase.Default.MACO_Used = toBool(value);
                AntsMethods.Properties.SettingsBase.Default.Save();
            }


        }

        [Description("Использовать раз за такт НАМК"), Category("НАМК")]
        public int Использовать_НАМК_раз_за_такт
        {
            get { return AntsMethods.Properties.SettingsBase.Default.MACO_Used_times; }
            set
            {
                AntsMethods.Properties.SettingsBase.Default.MACO_Used_times = value;
                AntsMethods.Properties.SettingsBase.Default.Save();
            }


        }


            [Description("Количество итераций"), Category("НАМК")]
        public int Количество_итераций_НАМК
        {
            get { return AntsMethods.Properties.SettingsBase.Default.ACO_IterationNum; }
            set
            {
                AntsMethods.Properties.SettingsBase.Default.ACO_IterationNum = value;
                AntsMethods.Properties.SettingsBase.Default.Save();
            }
        }



        [Description("Особей в популяции"), Category("НАМК")]
        public int Муравьев_на_колонию
        {
            get { return AntsMethods.Properties.SettingsBase.Default.ACO_AgentsInPopulation; }
            set
            {
                AntsMethods.Properties.SettingsBase.Default.ACO_AgentsInPopulation = value;
                AntsMethods.Properties.SettingsBase.Default.Save();
            }
        }


        [Description("Размер архива решений"), Category("НАМК")]
        public int Размер_архива_решений
        {
            get { return AntsMethods.Properties.SettingsBase.Default.ACO_decision_archive_size; }
            set
            {
                AntsMethods.Properties.SettingsBase.Default.ACO_decision_archive_size = value;
                AntsMethods.Properties.SettingsBase.Default.Save();
            }
        }

        [Description("Предпочитаемость выбора наилучших"), Category("НАМК")]
        public double Q
        {
            get { return AntsMethods.Properties.SettingsBase.Default.ACO_Q; }
            set
            {
                AntsMethods.Properties.SettingsBase.Default.ACO_Q = value;
                AntsMethods.Properties.SettingsBase.Default.Save();
            }
        }

        [Description("Коэффициент испарения феромона"), Category("НАМК")]
        public double Xi
        {
            get { return AntsMethods.Properties.SettingsBase.Default.ACO_Xi; }
            set
            {
                AntsMethods.Properties.SettingsBase.Default.ACO_Xi = value;
                AntsMethods.Properties.SettingsBase.Default.Save();
            }
        }

        [Description("Количество элитных муравьев"), Category("НАМК")]
        public int Элитных_решений
        {
            get { return AntsMethods.Properties.SettingsBase.Default.ACO_CountEliteAnt; }
            set
            {
                AntsMethods.Properties.SettingsBase.Default.ACO_CountEliteAnt = value;
                AntsMethods.Properties.SettingsBase.Default.Save();
            }
        }

        [Description("Условие замены архива решений на элитные"), Category("НАМК")]
        public int Порог_застревания_архивов_решений_в_экстремуме
        {
            get { return AntsMethods.Properties.SettingsBase.Default.ACO_ExtremumCount; }
            set
            {
                AntsMethods.Properties.SettingsBase.Default.ACO_ExtremumCount = value;
                AntsMethods.Properties.SettingsBase.Default.Save();
            }
        }
      
 #endregion  

        public void loadParams_CACO(string [] param)
        {
            #region MACO
            string stemp = "";
            int itemp = 0;
            double dtemp = 0;
            stemp = (param.Where(x => x.Contains("UseMACO"))).ToArray()[0];
            stemp = stemp.Remove(0, 8);
            switch (stemp)
            {
                case "True": Использовать_НАМК = YesNo.Да; break;
                case "False": Использовать_НАМК = YesNo.Нет; break;
                default: Использовать_НАМК = YesNo.Да; break;
            }

            stemp = (param.Where(x => x.Contains("ACOIter"))).ToArray()[0];
            stemp = stemp.Remove(0, 8);
            int.TryParse(stemp, out itemp);
            Количество_итераций_НАМК = itemp;

            stemp = (param.Where(x => x.Contains("ACOAgents"))).ToArray()[0];
            stemp = stemp.Remove(0, 10);
            int.TryParse(stemp, out itemp);
            Муравьев_на_колонию = itemp;

            stemp = (param.Where(x => x.Contains("ACODecisionArchive"))).ToArray()[0];
            stemp = stemp.Remove(0, 19);
            int.TryParse(stemp, out itemp);
            Размер_архива_решений = itemp;


            stemp = (param.Where(x => x.Contains("ACOQ"))).ToArray()[0];
            stemp = stemp.Remove(0, 5);
            double.TryParse(stemp, out dtemp);
            Q = dtemp;


            stemp = (param.Where(x => x.Contains("ACOXi"))).ToArray()[0];
            stemp = stemp.Remove(0, 6);
            double.TryParse(stemp, out dtemp);
            Xi = dtemp;

            stemp = (param.Where(x => x.Contains("ACOCountElite"))).ToArray()[0];
            stemp = stemp.Remove(0, 14);
            int.TryParse(stemp, out itemp);
            Элитных_решений = itemp;


            stemp = (param.Where(x => x.Contains("ACOExtimeCount"))).ToArray()[0];
            stemp = stemp.Remove(0, 15);
            int.TryParse(stemp, out itemp);
            Порог_застревания_архивов_решений_в_экстремуме = itemp;

            stemp = (param.Where(x => x.Contains("ACOUsedTimes"))).ToArray()[0];
            stemp = stemp.Remove(0, 13);
            int.TryParse(stemp, out itemp);
            Использовать_НАМК_раз_за_такт = itemp;

            #endregion
        }


    }
}

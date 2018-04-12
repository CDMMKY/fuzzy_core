using System.ComponentModel;
using System.Linq;

namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{
    public partial class MultiGoalOptimaze_conf
    { 
       #region АРЧ

        [Description("Использовать АРЧ"), Category("АРЧ")]
        public YesNo Использовать_АРЧ
        {
            get { return toYesNo(PSOMethods.Properties.SettingsBase.Default.PSO_Used); }
            set
            {
                PSOMethods.Properties.SettingsBase.Default.PSO_Used = toBool(value);
                PSOMethods.Properties.SettingsBase.Default.Save();
            }


        }


        [Description("Использовать раз АРЧ за такт"), Category("АРЧ")]
        public int Использовать_за_такт_АРЧ_раз
        {
            get { return PSOMethods.Properties.SettingsBase.Default.PSO_Used_times; }
            set
            {
                PSOMethods.Properties.SettingsBase.Default.PSO_Used_times = value;
                PSOMethods.Properties.SettingsBase.Default.Save();
            }


        }
        
        
        
        [Description("Сколько тактов выполниться алгоритм"), Category("АРЧ")]
        public int Количество_итераций
        {
            get { return PSOMethods.Properties.SettingsBase.Default.PSO_iter; }
            set { PSOMethods.Properties.SettingsBase.Default.PSO_iter = value; PSOMethods.Properties.SettingsBase.Default.Save(); }
        }


        [Description("Особей в популяции"), Category("АРЧ")]
        public int Особей_в_популяции
        {
            get { return PSOMethods.Properties.SettingsBase.Default.PSO_population; }
            set { PSOMethods.Properties.SettingsBase.Default.PSO_population = value; PSOMethods.Properties.SettingsBase.Default.Save(); }
        }
        [Description("Коэффициент c1"), Category("АРЧ")]
        public double Коэффициент_c1
        {
            get { return PSOMethods.Properties.SettingsBase.Default.PSO_c1; }
            set { PSOMethods.Properties.SettingsBase.Default.PSO_c1 = value; PSOMethods.Properties.SettingsBase.Default.Save(); }
        }
        [Description("Коэффициент c2"), Category("АРЧ")]
        public double Коэффициент_c2
        {
            get { return PSOMethods.Properties.SettingsBase.Default.PSO_c2; }
            set { PSOMethods.Properties.SettingsBase.Default.PSO_c2 = value; PSOMethods.Properties.SettingsBase.Default.Save(); }
        }
        #endregion


        public void loadParams_PSO(string [] param)
        {
            string stemp = "";
            int itemp = 0;
            double dtemp = 0;

            #region PSO
            stemp = (param.Where(x => x.Contains("usePSO"))).ToArray()[0];
            stemp = stemp.Remove(0, 14);
            switch (stemp)
            {
                case "True": Использовать_АРЧ = YesNo.Да; break;
                case "False": Использовать_АРЧ = YesNo.Нет; break;
                default: Использовать_АРЧ = YesNo.Да; break;
            }

            stemp = (param.Where(x => x.Contains("Pso_iter"))).ToArray()[0];
            stemp = stemp.Remove(0, 9);
            int.TryParse(stemp, out itemp);
            Количество_итераций = itemp;


            stemp = (param.Where(x => x.Contains("PsoPopulation"))).ToArray()[0];
            stemp = stemp.Remove(0, 14);
            int.TryParse(stemp, out itemp);
            Особей_в_популяции = itemp;

            stemp = (param.Where(x => x.Contains("PSOC1"))).ToArray()[0];
            stemp = stemp.Remove(0, 6);
            double.TryParse(stemp, out dtemp);
            Коэффициент_c1 = dtemp;

            stemp = (param.Where(x => x.Contains("PSOc2"))).ToArray()[0];
            stemp = stemp.Remove(0, 6);
            double.TryParse(stemp, out dtemp);
            Коэффициент_c2 = dtemp;

            stemp = (param.Where(x => x.Contains("PSOUsedTimes"))).ToArray()[0];
            stemp = stemp.Remove(0, 13);
            int.TryParse(stemp, out itemp);
            Использовать_за_такт_АРЧ_раз = itemp;

            #endregion

        }
    }
}

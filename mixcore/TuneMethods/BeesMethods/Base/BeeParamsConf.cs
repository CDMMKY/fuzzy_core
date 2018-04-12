using System.ComponentModel;
using FuzzySystem.FuzzyAbstract.conf;
using Settings = BeesMethods.Properties.SettingsBase;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm.conf
{
    public class BeeParamsConf : ILearnAlgorithmConf
    {

       public enum Type_Selection {
        Рулетка=0,
        Элитный_отбор=1,
        Бинарный_турнир=2,
            Случайный_отбор=3

        }

       [DisplayName("Размер улья")]
        [Description("Размер улья"), Category("Алгоритм пчелиной колонии для оптимизации параметров нечеткой системы")]
        public int ABCWHiveSize
        {
            get { return Settings.Default.ABCP_hiveSize; }
            set { Settings.Default.ABCP_hiveSize = value; Settings.Default.Save(); }
        }

       [DisplayName("Процент разведчиков")]
        [Description("Процент разведчиков"), Category("Алгоритм пчелиной колонии для оптимизации параметров нечеткой системы")]
        public double ABCWPercentScoutInHive
        {
            get { return Settings.Default.ABCP_percentScouts; }
            set { Settings.Default.ABCP_percentScouts = value; Settings.Default.Save(); }
        }


       [DisplayName("Начальная температура")]
        [Description("Начальная температура"), Category("Алгоритм пчелиной колонии для оптимизации параметров нечеткой системы")]
        public double ABCWInitTemperature
        {
            get { return Settings.Default.ABCP_initTemp; }
            set { Settings.Default.ABCP_initTemp = value; Settings.Default.Save(); }
        }

       [DisplayName("Коэффицент охлаждения")]
        [Description("Коэффицент охлаждения"), Category("Алгоритм пчелиной колонии для оптимизации параметров нечеткой системы")]
        public double ABCWColdCoeff
        {
            get { return Settings.Default.ABCP_coeffTemp; }
            set { Settings.Default.ABCP_coeffTemp = value; Settings.Default.Save(); }
 
        }

       [DisplayName("Количество итераций")]
        [Description("Количество итераций"), Category("Алгоритм пчелиной колонии для оптимизации параметров нечеткой системы")]
        public int ABCWCountIteration
        {
            get { return Settings.Default.ABCP_iterration; }
            set { Settings.Default.ABCP_iterration = value; Settings.Default.Save(); }

        }
       [DisplayName("Тип селекции")]
        [Description("Тип селекции"), Category("Алгоритм пчелиной колонии для оптимизации параметров нечеткой системы")]
        public Type_Selection ABCWTypeSelection
        {
            get { return (Type_Selection) Settings.Default.ABCP_selectionType; }
            set { Settings.Default.ABCP_selectionType = (int) value; Settings.Default.Save(); }

        }

       [DisplayName("Первая отсекающая граница")]
        [Description("1 отсекающая граница"), Category("Алгоритм пчелиной колонии для оптимизации параметров нечеткой системы")]
        public double ABCWFirstBorder
        {
            get { return Settings.Default.ABCP_Border1; }
            set { Settings.Default.ABCP_Border1 =value; Settings.Default.Save(); }

        }
       [DisplayName("Первых решений повторить")]
        [Description("1 решений повторить"), Category("Алгоритм пчелиной колонии для оптимизации параметров нечеткой системы")]
        public int ABCWFirstRepeate
        {
            get { return Settings.Default.ABCP_Repeat1; }
            set { Settings.Default.ABCP_Repeat1 = value; Settings.Default.Save(); }

        }


       [DisplayName("Вторая отсекающая граница")]
        [Description("2 отсекающая граница"), Category("Алгоритм пчелиной колонии для оптимизации параметров нечеткой системы")]
        public double ABCWSecondBorder
        {
            get { return Settings.Default.ABCP_Border2; }
            set { Settings.Default.ABCP_Border2 = value; Settings.Default.Save(); }

        }
       [DisplayName("Вторых решений повторить")]
        [Description("2 решений повторить"), Category("Алгоритм пчелиной колонии для оптимизации параметров нечеткой системы")]
        public int ABCWSecondRepeate
        {
            get { return Settings.Default.ABCP_Repeat2; }
            set { Settings.Default.ABCP_Repeat2 = value; Settings.Default.Save(); }

        }

       [DisplayName("Третья отсекающая граница")]
        [Description("3 отсекающая граница"), Category("Алгоритм пчелиной колонии для оптимизации параметров нечеткой системы")]
        public double ABCWFirdBorder
        {
            get { return Settings.Default.ABCP_Border3; }
            set { Settings.Default.ABCP_Border3 = value; Settings.Default.Save(); }

        }
       [DisplayName("Третьих решений повторить")]
        [Description("3 решений повторить"), Category("Алгоритм пчелиной колонии для оптимизации параметров нечеткой системы")]
        public int ABCWFirdRepeate
        {
            get { return Settings.Default.ABCP_Repeat3; }
            set { Settings.Default.ABCP_Repeat3 = value; Settings.Default.Save(); }

        }

        public void loadParams(string param)
        {
            string stemp = "";
            string[] temp = param.Split('}');
            ABCWHiveSize = Extention.getParamValueInt(temp, "ABCWHiveSize");
            ABCWPercentScoutInHive = Extention.getParamValueDouble(temp, "ABCWPercentScoutInHive");
            ABCWInitTemperature = Extention.getParamValueDouble(temp, "ABCWInitTemperature");
            ABCWColdCoeff = Extention.getParamValueDouble(temp, "ABCWColdCoeff");
            ABCWCountIteration = Extention.getParamValueInt(temp, "ABCWCountIteration");
            stemp = Extention.getParamValueString(temp, "ABCWTypeSelection");
            switch(stemp)
            {
                case "Roulete": ABCWTypeSelection = Type_Selection.Рулетка; break;
                case "BinaryTournament": ABCWTypeSelection = Type_Selection.Бинарный_турнир; break;
                case "Random": ABCWTypeSelection = Type_Selection.Случайный_отбор; break;
                case "E;ite": ABCWTypeSelection = Type_Selection.Элитный_отбор; break;
                default: ABCWTypeSelection = Type_Selection.Рулетка; break;
            }

            ABCWFirstBorder = Extention.getParamValueDouble(temp, "ABCWFirstBorder");
            ABCWFirstRepeate = Extention.getParamValueInt(temp, "ABCWFirstRepeate");
            ABCWSecondBorder = Extention.getParamValueDouble(temp, "ABCWSecondBorder");
            ABCWSecondRepeate = Extention.getParamValueInt(temp, "ABCWSecondRepeate");
            ABCWFirdBorder = Extention.getParamValueDouble(temp, "ABCWFirdBorder");
            ABCWFirdRepeate = Extention.getParamValueInt(temp, "ABCWFirdRepeate");


        }

        public void Init(int countVars)
        {
            
       

        }

     
    }
}

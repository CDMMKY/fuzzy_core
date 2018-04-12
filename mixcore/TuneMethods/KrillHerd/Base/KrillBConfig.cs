using FuzzySystem.FuzzyAbstract.conf;
using System.ComponentModel;
using Settings = KrillHerd.Properties.Settings;

namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class KrillBConfig : ILearnAlgorithmConf
    {
        [Description("Количество особей крилей"), Category("Параметры алгоритма")]
        public int Количество_особей
        {
            get { return Settings.Default.population_krill; }
            set { Settings.Default.population_krill = value; Settings.Default.Save(); }
        }
        [Description("Вес инерции вызванного двиежения n"), Category("Параметры алгоритма")]
        public double Вес_инерции_вызванного_движения
        {
            get { return Settings.Default.inertia_weight_n; }
            set { Settings.Default.inertia_weight_n = value; Settings.Default.Save(); }
        }
        [Description("Коэффициент в границе от (0,2)"), Category("Параметры алгоритма")]
        public double Коэффициент_Ct
        {
            get { return Settings.Default.constant_ct; }
            set { Settings.Default.constant_ct = value; Settings.Default.Save(); }
        }
        [Description("Коэффициент cоседей"), Category("Параметры алгоритма")]
        public double Коэффициент_соседей
        {
            get { return Settings.Default.neighbor_coeff; }
            set { Settings.Default.neighbor_coeff = value; Settings.Default.Save(); }
        }
        [Description("Количество_итераций"), Category("Основные параметры")]
        public int Количеств_итераций
        {
            get { return Settings.Default.number_iterations_I; }
            set { Settings.Default.number_iterations_I = value; Settings.Default.Save(); }
        }
        [Description("Количество_популяций"), Category("Основные параметры")]
        public int Количеств_популяций
        {
            get { return Settings.Default.number_of_populations; }
            set { Settings.Default.number_of_populations = value; Settings.Default.Save(); }
        }
        [Description("Вес инерции движения добывающего продовольствие f"), Category("Параметры алгоритма")]
        public double Вес_инерции_движения_добывающего_продовольствие
        {
            get { return Settings.Default.inertia_weight_f; }
            set { Settings.Default.inertia_weight_f = value; Settings.Default.Save(); }
        }
        [Description("Скорость распространений Dmax"), Category("Параметры алгоритма")]
        public double Скорость_распространения_Dmax
        {
            get { return Settings.Default.physical_distribution_Dmax; }
            set { Settings.Default.physical_distribution_Dmax = value; Settings.Default.Save(); }
        }
        [Description("Максимальная вызванная скорость Nmax"), Category("Параметры алгоритма")]
        public double Вызванная_скорость_Nmax
        {
            get { return Settings.Default.physical_distribution_Nmax; }
            set { Settings.Default.physical_distribution_Nmax = value; Settings.Default.Save(); }
        }
        [Description("Небольшое положительное число е"), Category("Параметры алгоритма")]
        public double Положительное_число_е
        {
            get { return Settings.Default.epsilon_e; }
            set { Settings.Default.epsilon_e = value; Settings.Default.Save(); }
        }
        [Description("Добывающая продовльствие скорость"), Category("Параметры алгоритма")]
        public double Добывающая_продовольствие_скорость_Vf
        {
            get { return Settings.Default.foraging_speed_Vf; }
            set { Settings.Default.foraging_speed_Vf = value; Settings.Default.Save(); }
        }

        public void Init(int countVars)
        {

        }
        public void loadParams(string param)
        {
            //doublethrow new NotImplementedException();
        }
    }
}

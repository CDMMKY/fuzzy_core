using FuzzySystem.FuzzyAbstract.conf;
using System.ComponentModel;
using Settings = BrainStorm.Properties.Settings;

namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class BSConfig : ILearnAlgorithmConf
    {
        [DisplayName("Количество итераций"), Category("Итерации")]
        public int iter
        {
            get { return Settings.Default.iter; }
            set { Settings.Default.iter = value; Settings.Default.Save(); }
        }
        [DisplayName("Размер популяции"), Category("Параметры алгоритма")]
        public int N
        {
            get { return Settings.Default.N; }
            set { Settings.Default.N = value; Settings.Default.Save(); }
        }


        [DisplayName("Число кластеров"), Category("Параметры алгоритма")]
        public int m
        {
            get { return Settings.Default.m; }
            set { Settings.Default.m = value; Settings.Default.Save(); }
        }

        [DisplayName("Вероятность выбора оператора"), Category("Параметры алгоритма")]
        public double p_one
        {
            get { return Settings.Default.p_one; }
            set { Settings.Default.p_one = value; Settings.Default.Save(); }
        }
        [DisplayName("Вероятность выбора оператора для 1 кластера"), Category("Параметры алгоритма")]
        public double p_one_center
        {
            get { return Settings.Default.p_one_center; }
            set { Settings.Default.p_one_center = value; Settings.Default.Save(); }
        }

        [DisplayName("Вероятность выбора оператора для 2х кластеров"), Category("Параметры алгоритма")]
        public double p_two_center
        {
            get { return Settings.Default.p_two_center; }
            set { Settings.Default.p_two_center = value; Settings.Default.Save(); }
        }
        [DisplayName("Мутационный фактор"), Category("Параметры алгоритма")]
        public double F
        {
            get { return Settings.Default.F; }
            set { Settings.Default.F = value; Settings.Default.Save(); }
        }

        [DisplayName("Величина для изгиба логсигм"), Category("Параметры алгоритма")]
        public double p
        {
            get { return Settings.Default.p; }
            set { Settings.Default.p = value; Settings.Default.Save(); }
        }
        public void Init(int countVars)
        {
         
        }

        public void loadParams(string param)
        {
           
        }
    }
}

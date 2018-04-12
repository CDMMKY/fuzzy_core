using System.ComponentModel;
using Settings = GSA.Properties.Settings;
using System;
using FuzzySystem.FuzzyAbstract.conf;

namespace Fuzzy_system.Fuzzy_Abstract.learn_algorithm.conf
{
    class gsa_conf : ILearnAlgorithmConf
    {
        [Description("Количество итераций"), Category("Итерации")]
        public int Количество_итераций
        {
            get { return Settings.Default.gsa_iter; }
            set { Settings.Default.gsa_iter = value; Settings.Default.Save(); }
        }


        [Description("Количество частиц"), Category("Параметры алгоритма")]
        public int Количество_частиц
        {
            get { return Settings.Default.gsa_population; }
            set { Settings.Default.gsa_population = value; Settings.Default.Save(); }
        }
        [Description("Гравитационная постоянная"), Category("Параметры алгоритма")]
        public double Гравитационная_постоянная
        {
            get { return Settings.Default.gsa_G0; }
            set { Settings.Default.gsa_G0 = value; Settings.Default.Save(); }
        }
        [Description("Коэффициент уменьшения"), Category("Параметры алгоритма")]
        public double Коэффициент_уменьшения
        {
            get { return Settings.Default.gsa_alpha; }
            set { Settings.Default.gsa_alpha = value; Settings.Default.Save(); }
        }
        [Description("Малая константа"), Category("Параметры алгоритма")]
        public double Малая_константа
        {
            get { return Settings.Default.gsa_epsilon; }
            set { Settings.Default.gsa_epsilon = value; Settings.Default.Save(); }
        }

        public void loadParams(string param)
        {
            throw (new NotImplementedException());
        }
        public void Init(int countVars)
        { }
    }
}

using System.ComponentModel;
using FuzzySystem.FuzzyAbstract.conf;
using System;
using Settings = MonkeyOptimization.Properties.SettingsBase;

namespace FuzzySystem.FuzzyAbstract.conf
{
    public class Param : ILearnAlgorithmConf
    {
        public int Количество_особей
        {
            get { return Settings.Default.population_count; }
            set { Settings.Default.population_count = value; Settings.Default.Save(); }
        }
        public double Шаг
        {
            get { return Settings.Default.step; }
            set { Settings.Default.step = value; Settings.Default.Save(); }
        }
        public double Интервал_локального_прыжка
        {
            get { return Settings.Default.watch_jump_parameter; }
            set { Settings.Default.watch_jump_parameter = value; Settings.Default.Save(); }
        }
        public double Левая_граница_кувырка
        {
            get { return Settings.Default.somersault_interval_left; }
            set { Settings.Default.somersault_interval_left = value; Settings.Default.Save(); }
        }
        public double Правая_граница_кувырка
        {
            get { return Settings.Default.somersault_interval_right; }
            set { Settings.Default.somersault_interval_right = value; Settings.Default.Save(); }
        }
        public int Итераций_движения
        {
            get { return Settings.Default.crawl_iter; }
            set { Settings.Default.crawl_iter = value; Settings.Default.Save(); }
        }
        public int Итераций_прыжка
        {
            get { return Settings.Default.jump_iter; }
            set { Settings.Default.jump_iter = value; Settings.Default.Save(); }
        }
        public int Итераций_кувырка
        {
            get { return Settings.Default.somersault_iter; }
            set { Settings.Default.somersault_iter = value; Settings.Default.Save(); }
        }

        public void loadParams(string param)
        {
            string[] temp = param.Split('}');
            Количество_особей = Extention.getParamValueInt(temp, "Количество_особей");
            Шаг = Extention.getParamValueInt(temp, "Шаг");
            Интервал_локального_прыжка = Extention.getParamValueInt(temp, "Интервал_локального_прыжка");
            Левая_граница_кувырка = Extention.getParamValueInt(temp, "Левая_граница_кувырка");
            Правая_граница_кувырка = Extention.getParamValueInt(temp, "Правая_граница_кувырка");
            Итераций_движения = Extention.getParamValueInt(temp, "Итераций_движения");
            Итераций_прыжка = Extention.getParamValueInt(temp, "Итераций_прыжка");
            Итераций_кувырка = Extention.getParamValueInt(temp, "Итераций_кувырка");
        }
        public void Init(int countVars)
        {   }
    }
}

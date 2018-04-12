using FuzzySystem.FuzzyAbstract.conf;
using Settings = MBA_project.Properties.SettingsMBA;
using System;

namespace FuzzySystem.FuzzyAbstract.conf
{
    class Param : ILearnAlgorithmConf
    {
        public int Количество_итераций
        {
            get { return Settings.Default.count_iteration; }
            set { Settings.Default.count_iteration = value; Settings.Default.Save(); }
        }
        public int параметр_лямбда
        {
            get { return Settings.Default.параметр_лямбда; }
            set { Settings.Default.параметр_лямбда = value; Settings.Default.Save(); }
        }
        public double точность
        {
            get { return Settings.Default.точность; }
            set { Settings.Default.точность = value; Settings.Default.Save(); }
        }
        public int Число_осколков
        {
            get { return Settings.Default.count_population; }
            set { Settings.Default.count_population = value; Settings.Default.Save(); }
        }
        public int Фактор_исследования
        {
            get { return Settings.Default.exploration_factor; }
            set { Settings.Default.exploration_factor = value; Settings.Default.Save(); }
        }
        public int Уменьшающий_коэффициент
        {
            get { return Settings.Default.reduce_koefficient; }
            set { Settings.Default.reduce_koefficient = value; Settings.Default.Save(); }
        }
        public string Усечённые_признаки
        {
            get { return Settings.Default.priznaki_usech; }
            set { Settings.Default.priznaki_usech = value; Settings.Default.Save(); }
        }
        
        public int Итерации_дискр_алг
        {
            get { return Settings.Default.iter_descrete; }
            set { Settings.Default.iter_descrete = value; Settings.Default.Save(); }
        }
        public void loadParams(string param)
        {
            throw (new NotImplementedException());
        }
        public void Init(int countVars)
        { }
    }
}


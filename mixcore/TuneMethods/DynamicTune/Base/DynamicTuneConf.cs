namespace DynamicTune
{
    using System.ComponentModel;

    using FuzzySystem.FuzzyAbstract.conf;

    using Properties;

    public class DynamicTuneConf : ILearnAlgorithmConf
    {
        /// <summary>
        /// Количество итераций алгоритма
        /// </summary>
        [Description("Пороговое значение ошибки системы"), Category("Параметры алгоритма")]
        public double MaxError
        {
            get { return Settings.Default.MaxError; }
            set
            {
                Settings.Default.MaxError = value;
                Settings.Default.Save();
            }
        }
        [Description("Макс. число правил"), Category("Параметры алгоритма")]
        public int RulesCount
        {
            get { return Settings.Default.RulesCount; }
            set
            {
                Settings.Default.RulesCount = value;
                Settings.Default.Save();
            }
        }

        [Description("Макс. число попыток при ухудшении ошибки"), Category("Параметры алгоритма")]
        public int TryCount
        {
            get { return Settings.Default.TryCount; }
            set
            {
                Settings.Default.TryCount = value;
                Settings.Default.Save();
            }
        }

        public void Init(int countVars)
        {
        }

        public void loadParams(string param)
        {
            throw new System.NotImplementedException();
        }
    }
}
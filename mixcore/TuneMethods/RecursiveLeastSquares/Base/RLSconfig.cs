namespace RecursiveLeastSquares.Base
{
    using System.ComponentModel;

    using FuzzySystem.FuzzyAbstract.conf;

    using RecursiveLeastSquares.Properties;

    public class RLSconfig:ILearnAlgorithmConf
    {
        /// <summary>
        /// Количество итераций алгоритма
        /// </summary>
        [Description("Количество итераций"), Category("Параметры алгоритма")]
        public int NumberOfIterantions
        {
            get { return Settings.Default.NumberOfIterations; }
            set 
            { 
                Settings.Default.NumberOfIterations = value;
                Settings.Default.Save();
            }
        }

        /// <summary>
        /// Фактор забывания (чем ближе к 1, тем менее сильно влияние предшествующих данных)
        /// </summary>
        [Description("Фактор забывания"), Category("Параметры алгоритма")]
        public double ForgettingFactor
        {
            get { return Settings.Default.ForgettingFactor; }
            set 
            { 
                Settings.Default.ForgettingFactor = value;
                Settings.Default.Save();
            }
        }

        [Description("Если на входе - авторегрессия, указать её тип"), Category("Параметры алгоритма")]
        public string AutoregressiveInput
        {
            get { return Settings.Default.AutoregressiveInput; }
            set
            {
                Settings.Default.AutoregressiveInput = value;
                Settings.Default.Save();
            }
        }

        [Description("Параметры вышеуказанной авторегрессии"), Category("Параметры алгоритма")]
        public string AutoregressiveParams
        {
            get { return Settings.Default.AutoregressiveParams; }
            set
            {
                Settings.Default.AutoregressiveParams = value;
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuzzySystem.FuzzyAbstract.conf;
using KLI.Properties;

namespace KLI.Base
{

    public class KLI_Flexy_conf : IGeneratorConf
    {
        [Description("Старторый коэффициент"), DisplayName("Старторый коэффициент"), Category("Параметры алгоритма")]
        public Double StartValue
        {
            get { return Settings.Default.StartKoef; }
            set
            {
                Settings.Default.StartKoef = value;
                Settings.Default.Save();
            }
        }

        [Description("Коэффициент поиска"), DisplayName("Коэффициент поиска"), Category("Параметры алгоритма")]
        public Double MagicCoef
        {
            get { return Settings.Default.StepSize; }
            set
            {
                Settings.Default.StepSize = value;
                Settings.Default.Save();
            }
        }
        [Description("Количество итераций"), DisplayName("Количество итераций"), Category("Параметры алгоритма")]
        public int CountTries
        {
            get { return Settings.Default.CountTries; }
            set
            {
                Settings.Default.CountTries = value;
                Settings.Default.Save();
            }
        }


        [Description("Максимальное колличество правил"), DisplayName("Макс правил"), Category("Параметры алгоритма")]
        public int RulesCount
        {
            get { return Settings.Default.MaxRules; }
            set
            {
                Settings.Default.MaxRules = value;
                Settings.Default.Save();
            }
        }


        public void loadParams(string param)
        {
            throw (new NotImplementedException());
        }

        public void Init(int countVars)
        {
        }
    }
}
    
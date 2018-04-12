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

    public class KLI_conf : IGeneratorConf
    {
        [Description("Относительная ошибка от среднего выходного значения"), DisplayName("Относительная ошибка"), Category("Параметры алгоритма")]
        public Double MaxValue
        {
            get { return Settings.Default.Error; }
            set
            {
                Settings.Default.Error = value;
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
    
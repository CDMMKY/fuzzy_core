using System.ComponentModel;
using FuzzySystem.FuzzyAbstract.conf;
using Settings = AntsMethods.Properties.SettingsBase;

namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf

{
    public class ACOSearchConf : ILearnAlgorithmConf
    {

        public enum is_YesNo
        {  Нет=0,
            Да =1 
        }

        [DisplayName("Количество итераций")]
        [Description("Количество итераций"), Category("Итерации")]
        public int ACOCountIteration
        {
            get { return Settings.Default.ACO_IterationNum; }
            set
            {
                Settings.Default.ACO_IterationNum = value;
                Settings.Default.Save();
            }
        }


        [DisplayName("Муравьев в колонии")]
        [Description("Особей в популяции"), Category("Муравьиный алгоритм")]
        public int ACOCountAnt
        {
            get { return Settings.Default.ACO_AgentsInPopulation; }
            set
            {
                Settings.Default.ACO_AgentsInPopulation = value;
                Settings.Default.Save();
            }
        }

        [DisplayName("Размер архива решений")]
        [Description("Количество колоний"), Category("Муравьиный алгоритм")]
        public int ACODescisionArchiveSize
        {
            get { return Settings.Default.ACO_decision_archive_size; }
            set
            {
                Settings.Default.ACO_decision_archive_size = value;
                Settings.Default.Save();
            }
        }

        [DisplayName("Элитарность ACOQ")]
        [Description("Предпочитаемость выбора наилучших"), Category("Муравьиный алгоритм")]
        public double ACOQ
        {
            get { return Settings.Default.ACO_Q; }
            set
            {
                Settings.Default.ACO_Q = value;
                Settings.Default.Save();
            }
        }

        [DisplayName("Коэффициент испарения феромона ACOXi")]
        [Description("Коэффициент испарения феромона"), Category("Муравьиный алгоритм")]
        public double ACOXi
        {
            get { return Settings.Default.ACO_Xi; }
            set
            {
                Settings.Default.ACO_Xi = value;
                Settings.Default.Save();
            }
        }




        public virtual void loadParams(string param)
        {
            string[] temp = param.Split('}');
         
            ACOCountIteration = Extention.getParamValueInt(temp, "ACOCountIteration");

            ACOCountAnt = Extention.getParamValueInt(temp, "ACOCountAnt");

            ACODescisionArchiveSize = Extention.getParamValueInt(temp, "ACODescisionArchiveSize");

            ACOQ = Extention.getParamValueDouble(temp, "ACOQ");

            ACOXi = Extention.getParamValueDouble(temp, "ACOXi");

            
        }

        public void Init(int countVars)
        {
            
         
        }


    }
}

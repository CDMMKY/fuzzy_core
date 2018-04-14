using FuzzySystem.FuzzyAbstract.conf;
using System.ComponentModel;
using Settings = MixBagging.Properties.Settings;

namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class MBConf : ILearnAlgorithmConf
    {
        public enum voteType { Обычное = 0 };
        [Description("Тип голосования"), Category("Основные")]
        public voteType Тип_голосования
        {
            get { return (voteType)Settings.Default.Голосование; }
            set { Settings.Default.Голосование = (int)value; Settings.Default.Save(); }
        }
        [Description("Количество популяций в первом алгоритме"), Category("1")]
        public int Популяций_от_первого_алгоритма
        {
            get { return Settings.Default.popul_in_first_algorithm; }
            set { Settings.Default.popul_in_first_algorithm = (int)value; Settings.Default.Save(); }
        }
        [Description("Количество популяций во втором алгоритме"), Category("2")]
        public int Популяций_от_второго_алгоритма
        {
            get { return Settings.Default.popul_in_second_algorithm; }
            set { Settings.Default.popul_in_second_algorithm = (int)value; Settings.Default.Save(); }
        }
        [Description("Количество популяций в третьем алгоритме"), Category("3")]
        public int Популяций_от_третьего_алгоритма
        {
            get { return Settings.Default.popul_in_third_algorithm; }
            set { Settings.Default.popul_in_third_algorithm = (int)value; Settings.Default.Save(); }
        }
        public void Init(int CountVars)
        {

        }

        public void loadParams(string param)
        {
            string[] temp = param.Split('}');
            Популяций_от_первого_алгоритма = Extention.getParamValueInt(temp, "populInFirstAlg");
            Популяций_от_второго_алгоритма = Extention.getParamValueInt(temp, "populInSecondAlg");
            Популяций_от_третьего_алгоритма = Extention.getParamValueInt(temp, "populInThird");
            string stemp = Extention.getParamValueString(temp, "MBVote");
            switch (stemp)
            {
                case "Common": Тип_голосования = voteType.Обычное; break;
            }
        }
    }
}

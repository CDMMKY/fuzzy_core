using FuzzySystem.FuzzyAbstract.conf;
using System.ComponentModel;
using Settings = MixBagging.Properties.Settings;

namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class MBConf : ILearnAlgorithmConf
    {
        public enum voteType { Обычное = 0};
        [Description("Тип голосования"), Category("Основные")]
        public voteType Тип_голосования
        {
            get { return (voteType)Settings.Default.Голосование; }
            set { Settings.Default.Голосование = (int)value; Settings.Default.Save(); }
        }
        public void Init(int CountVars)
        {

        }

        public void loadParams(string param)
        {
            string[] temp = param.Split('}');
            string stemp = Extention.getParamValueString(temp, "MBVote");
            switch (stemp)
            {
                case "Common": Тип_голосования = voteType.Обычное; break;
            }
        }
    }
}

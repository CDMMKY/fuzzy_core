using FuzzySystem.FuzzyAbstract.conf;
using System.ComponentModel;


namespace FuzzySystem.FuzzyAbstract
{   /// <summary>
/// Класс-конфигуратор для алгоритвом генерации и оптимизации не требующих задания дополнительныз параметров
/// </summary>
    public class NullConfForAll:IGeneratorConf,ILearnAlgorithmConf
    {
        public void Init(int countVars)
        {
            
        }
        public void loadParams(string param)
        { 
        }

        [DisplayName("Нет настраиваемых параметров")]
        [Description("У данного метода нет настраиваемых параметров"), Category("Методы")]
        public string  NoParams { get { return ""; } }
    }
}

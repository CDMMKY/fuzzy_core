using System.ComponentModel;
using FuzzySystem.FuzzyAbstract.conf;
using Settings = BeesMethods.Properties.SettingsBase;

namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf

{
    public class BeeStructureConf : ILearnAlgorithmConf
    {
       [DisplayName("Количество разведчиков")]
        [Description("Сколько разведчиков отправлено"), Category("Алгоритм пчелиной колонии для оптимизации структуры нечеткой системы")]
        public int ABCSCountScout
        {
            get { return Settings.Default.ABCS_CountScout; }
            set { Settings.Default.ABCS_CountScout = value; Settings.Default.Save(); }
        }

       [DisplayName("Количество рабочих пчел")]
        [Description("Сколько рабочих пчел отправлено"), Category("Алгоритм пчелиной колонии для оптимизации структуры нечеткой системы")]
        public int ABCSCountWorkers
        {
            get { return Settings.Default.ABCS_CountWorkers; }
            set { Settings.Default.ABCS_CountWorkers = value; Settings.Default.Save(); }
        }


           [DisplayName("Генерируемых правил")]
   
        [Description("Сколько правил сгенерировать"), Category("Алгоритм пчелиной колонии для оптимизации структуры нечеткой системы")]
        public int ABCSCountRules
        {
            get { return Settings.Default.ABCS_CountRulesAdd; }
            set { Settings.Default.ABCS_CountRulesAdd = value; Settings.Default.Save(); }
        }

       [DisplayName("Функция принадлежности")]
        [Description("Вид функции принадлежности"), Category("Алгоритм пчелиной колонии для оптимизации структуры нечеткой системы")]
        public TypeTermFuncEnum ABCSTypeFunc
        {
            get { return (TypeTermFuncEnum)Settings.Default.ABCS_TypeTerms; }
            set
            {

                Settings.Default.ABCS_TypeTerms = (int)value;
                Settings.Default.Save();

            }
        }


        public virtual void loadParams(string param)
        {

            string stemp = "";
            string[] temp = param.Split('}');
            ABCSCountScout = Extention.getParamValueInt(temp, "ABCSCountScout");
            ABCSCountWorkers = Extention.getParamValueInt(temp, "ABCSCountWorkers");
            ABCSCountRules = Extention.getParamValueInt(temp, "ABCSCountRules");
            stemp = Extention.getParamValueString(temp,"ABCSTypeFunc");
            switch (stemp)
            {
                case "Triangle": ABCSTypeFunc = TypeTermFuncEnum.Треугольник; break;
                case "Gauss": ABCSTypeFunc = TypeTermFuncEnum.Гауссоида; break;
                case "Parabola": ABCSTypeFunc = TypeTermFuncEnum.Парабола; break;
                case "Trapezium": ABCSTypeFunc = TypeTermFuncEnum.Трапеция; break;
                default: ABCSTypeFunc = TypeTermFuncEnum.Треугольник; break;
            }


        }

        public void Init(int countVars)
        {

        }

     
    }
}

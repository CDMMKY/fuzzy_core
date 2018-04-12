using System.ComponentModel;
using Settings = BaseInitMethods.Properties.SettingsBase;

namespace FuzzySystem.FuzzyAbstract.conf
{
    public class GeneratorRullesSimpleRandomConfig : IGeneratorConf
    {
        public void Init(int countVars)
        {
            
        }

        public void loadParams(string param)
        {
            string stemp = "";
            string[] temp = param.Split('}');
           
            stemp = Extention.getParamValueString(temp, "RSRConstant");
            switch (stemp)
            {
                case "Yes": RSRConstant = StableTermType.Указанный; break;
                case "No": RSRConstant = StableTermType.Случайный; break;
                default: RSRConstant = StableTermType.Указанный; break;
            }
              
               stemp = Extention.getParamValueString(temp, "RSRTypeFunc");
          switch (stemp)
          {
              case "Triangle": RSRTypeFunc = TypeTermFuncEnum.Треугольник; break;
              case "Gauss": RSRTypeFunc = TypeTermFuncEnum.Гауссоида; break;
              case "Parabola": RSRTypeFunc = TypeTermFuncEnum.Парабола; break;
              case "Trapezium": RSRTypeFunc = TypeTermFuncEnum.Трапеция; break;
              default: RSRTypeFunc = TypeTermFuncEnum.Треугольник; break;
          }

          RSRCountRules = Extention.getParamValueInt(temp, "RSRCountRules");
         
        }

        public enum StableTermType
        {
            Указанный = 0,
            Случайный = 1
        }

        [DisplayName("Тип Термов")]
        [Description("Использовать термы одинакого типа ?"), Category("Термы")]
        public StableTermType RSRConstant { get { return (StableTermType) Settings.Default.Generator_Rulles_simples_random_stable; }
             set { Settings.Default.Generator_Rulles_simples_random_stable = (int) value; 
                Settings.Default.Save();}  }

        [DisplayName("Вид функции принадлежности")]
        [Description("Вид функции принадлежности"), Category("Термы")]
        public TypeTermFuncEnum RSRTypeFunc { get { return (TypeTermFuncEnum) Settings.Default.Generator_Rulles_simples_random_func; } 
            set { Settings.Default.Generator_Rulles_simples_random_func = (int) value; Settings.Default.Save(); } }

        [DisplayName("Количество правил")]
        [Description("Количество генерируемых правил "), Category("Правила")]
        public int RSRCountRules { get { return Settings.Default.Generator_Rulles_simples_random_сount_rules; } 
            set {Settings.Default.Generator_Rulles_simples_random_сount_rules=value ;
            Settings.Default.Save();
            }
        }
        
    }
}
using System.ComponentModel;
using Settings = BaseInitMethods.Properties.SettingsBase;

namespace FuzzySystem.FuzzyAbstract.conf
{
    public class InitBySamplesConfig:IGeneratorConf
    {
        private TypeTermFuncEnum type_term_func;
        [DisplayName("Вид функции принадлежности")]
         [Description("Вид функции принадлежности"), Category("Правила")]
        public TypeTermFuncEnum IBSTypeFunc { get { return type_term_func; } 
            set { type_term_func = value;
           
                Settings.Default.init_by_samples_conf_Type_func = (int) value;
                Settings.Default.Save();
            }  
        }
        public InitBySamplesConfig()
        {
            type_term_func = (TypeTermFuncEnum)Settings.Default.init_by_samples_conf_Type_func;
        }


        public void Init(int countVars)
        {

        }
        public void loadParams(string param)
        {
             string stemp = "";
            string[] temp = param.Split('}');
          
              
               stemp = Extention.getParamValueString(temp, "IBSTypeFunc");
          switch (stemp)
          {
              case "Triangle": IBSTypeFunc = TypeTermFuncEnum.Треугольник; break;
              case "Gauss": IBSTypeFunc = TypeTermFuncEnum.Гауссоида; break;
              case "Parabola": IBSTypeFunc = TypeTermFuncEnum.Парабола; break;
              case "Trapezium": IBSTypeFunc = TypeTermFuncEnum.Трапеция; break;
              default: IBSTypeFunc = TypeTermFuncEnum.Треугольник; break;
          }
        }




    }
}

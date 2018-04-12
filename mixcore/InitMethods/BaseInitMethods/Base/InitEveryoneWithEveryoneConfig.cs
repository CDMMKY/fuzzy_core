using System.ComponentModel;
using System.Linq;
using Settings = BaseInitMethods.Properties.SettingsBase;

namespace FuzzySystem.FuzzyAbstract.conf
{

    public class InitEveryoneWithEveryone:IGeneratorConf
    {   protected TypeTermFuncEnum type_term_func;
        protected int[] count_terms;


       [DisplayName("Вид функция принадлежности")]
         [Description("Вид функции принадлежности"), Category("Термы")]
         public TypeTermFuncEnum IEWEFuncType { 
             get { return type_term_func; } 
            set { type_term_func = value;
            Settings.Default.Type_func_int = (int)value;
            Settings.Default.Save();
               
           }  
        }
    
        public virtual void Init(int countVars)
        {
            type_term_func = (TypeTermFuncEnum)Settings.Default.Type_func_int;
            count_terms = new int[countVars];
            for (int i = 0; i < countVars; i++)
            {
                count_terms[i] = Settings.Default.AvrCountTerms;
            }
        
        }


        [DisplayName("Количество термов для каждого признака")]
         [Description("Количество термов на каждом для каждого признака"), Category("Термы")]
        public int[] IEWECountSlice
        {
            get { return count_terms.Clone() as int []; }
            set
            {
                Settings.Default.AvrCountTerms = value.Sum() / value.Count();
                Settings.Default.Save();
                count_terms = value.Clone() as int [];
              
            }
    }


         public virtual void loadParams(string param)
         {
             string stemp = "";
             string[] temp = param.Split('}');
             
             stemp = Extention.getParamValueString(temp, "IEWEFuncType");
             switch (stemp)
             {
                 case "Triangle": IEWEFuncType = TypeTermFuncEnum.Треугольник; break;
                 case "Gauss": IEWEFuncType = TypeTermFuncEnum.Гауссоида; break;
                 case "Parabola": IEWEFuncType = TypeTermFuncEnum.Парабола; break;
                 case "Trapezium": IEWEFuncType = TypeTermFuncEnum.Трапеция; break;
                 default: IEWEFuncType = TypeTermFuncEnum.Треугольник; break;
             }

             
             stemp = Extention.getParamValueString(temp, "IEWECountSlice");

             string[] temps = stemp.Split(',');
             count_terms = new int[temps.Count()];
             for (int i = 0; i < temps.Count(); i++)
             {
                 int.TryParse(temps[i], out count_terms[i]);
             }
             IEWECountSlice = count_terms;

        
         }
    }
}

using System.ComponentModel;
using System.Linq;
using Settings = BaseInitMethods.Properties.SettingsBase;
using System;

namespace FuzzySystem.FuzzyAbstract.conf
{

    public class InitEveryoneWithOptimal:IGeneratorConf
    {   protected TypeTermFuncEnum type_term_func;
        public int[] count_terms;


       [DisplayName("Функция принадлежности")]
         [Description("Вид функции принадлежности"), Category("Термы")]
         public TypeTermFuncEnum IEWOTypeFunc { 
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

            string temp = "";
            Array.Sort(count_terms);
            for (int i = 0; i < count_terms.Count() - 1; i++)
            {
                temp += count_terms[i].ToString() + ",";
            } temp += count_terms[count_terms.Count() - 1].ToString();



            IEWOCountTerms = temp;
        }


        [DisplayName("Количество термов на каждом для каждого признака")]
         [Description("Количество термов на каждом для каждого признака"), Category("Термы")]
        public string IEWOCountTerms
        {
            get { string temp = "";
          Array.Sort(  count_terms);
          for (int i = 0; i < count_terms.Count()-1; i++)
          {
              temp += count_terms[i].ToString() + ",";
          } temp += count_terms[count_terms.Count() - 1].ToString();
                
                return temp; }
            set
            {
               string [] temp = value.Split(',');
               count_terms = new int[temp.Count()];
                for (int i = 0; i < temp.Count(); i++)
                {
                    int.TryParse(temp[i], out count_terms[i]); 
               }

                Array.Sort(count_terms);

                Settings.Default.AvrCountTerms = count_terms.Sum() / count_terms.Count<int>();
                Settings.Default.Save();
            }
    }


         public void loadParams(string param)
         {
             string stemp = "";
             string[] temp = param.Split('}');
          
             IEWOCountTerms = Extention.getParamValueString(temp, "StructParams");
            
             stemp = Extention.getParamValueString(temp, "TypeFunc");
             switch (stemp)
             {
                 case "Triangle": IEWOTypeFunc = TypeTermFuncEnum.Треугольник; break;
                 case "Gauss": IEWOTypeFunc = TypeTermFuncEnum.Гауссоида; break;
                 case "Parabola": IEWOTypeFunc = TypeTermFuncEnum.Парабола; break;
                 case "Trapezium": IEWOTypeFunc = TypeTermFuncEnum.Трапеция; break;
                 default: IEWOTypeFunc = TypeTermFuncEnum.Треугольник; break;
             }
             
         }
    }
}

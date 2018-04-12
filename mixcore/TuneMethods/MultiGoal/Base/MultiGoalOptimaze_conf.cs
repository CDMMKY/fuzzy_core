using System;
using System.ComponentModel;
using FuzzySystem.FuzzyAbstract.conf;
using MultiGoal.Properties;
using System.Linq;

namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{

    public enum YesNo
    {
        Нет = 0,
        Да = 1
    }


    public enum TypeComplexity
    { Правила_И_Термы=0,
        Правила=1
    }

    public enum TypeInterpreting
    { Нормированный_индекс=0,
        Вещественный_индекс =1
    }


 public   partial class  MultiGoalOptimaze_conf : ILearnAlgorithmConf
    {
        public YesNo toYesNo(bool value)
        {
            if (value)
            {
                return YesNo.Да;
            }
            return YesNo.Нет;
        }


         internal string path {get; set;}
         internal string dataSetName {get;set;}
      


        public bool toBool(YesNo value)
        {
            if (value == YesNo.Да)
            {
                return true;
            }
            return false;
        }

  
        #region Собственные
        
        [Description("Количество итераций алгоритма "), Category("Трехцелевая оптимизация")]
        public int Итераций_алгоритма
        {
            get { return Settings.Default.ThreeMultiGoal_Iterrate; }
            set
            {
                Settings.Default.ThreeMultiGoal_Iterrate = value;
                Settings.Default.Save();
            }


        }
    
        [Description("Отличие на сколько процентов точности классификации будет браться за одну клетку "), Category("Трехцелевая оптимизация")]
            public double Размер_шага_по_точности
        {
            get { return Settings.Default.ThreeMultiGoal_stepPercent; }
            set
            {
                Settings.Default.ThreeMultiGoal_stepPercent = value;
                Settings.Default.Save();
            }


        }


        [Description("Отличие на сколько единиц сложности классификатора будет браться за одну клетку "), Category("Трехцелевая оптимизация")]
        public double Размер_шага_по_сложности
        {
            get { return Settings.Default.ThreeMultiGoal_stepCoplexity; }
            set
            {
                Settings.Default.ThreeMultiGoal_stepCoplexity = value;
                Settings.Default.Save();
            }


        }


        [Description("Отличие на сколько долей по индексу интерпретируемости классификатора будет браться за одну клетку "), Category("Трехцелевая оптимизация")]
        public double Размер_шага_по_интерпретируемости
        {
            get { return Settings.Default.ThreeMultiGoal_stepIterability; }
            set
            {
                Settings.Default.ThreeMultiGoal_stepIterability = value;
                Settings.Default.Save();
            }


        }

        [Description("Во сколько раз после достижения количества попыток будет умешаться шаги по точности, сложности, интерпретируемости "), Category("Трехцелевая оптимизация")]
        public double Уменьшать_шаги_в
        {
            get { return Settings.Default.ThreeMultiGoal_stepDivider; }
            set
            {
                Settings.Default.ThreeMultiGoal_stepDivider = value;
                Settings.Default.Save();
            }


        }

        [Description("Неудачных попыток оптимизации перед уменьшеним шага "), Category("Трехцелевая оптимизация")]
        public int Уменьшать_шаг_после
        {
            get { return Settings.Default.ThreeMultiGoal_stepTries; }
            set
            {
                Settings.Default.ThreeMultiGoal_stepTries = value;
                Settings.Default.Save();
            }


        }

        [Description("Допустимое количество схожих систем "), Category("Трехцелевая оптимизация")]
        public int Разрешено_похожих_систем
        {
            get { return Settings.Default.ThreeMultiGoal_countSysteminCell; }
            set
            {
                Settings.Default.ThreeMultiGoal_countSysteminCell = value;
                Settings.Default.Save();
            }


        }

        [Description("Критерий сложности"), Category("Трехцелевая оптимизация")]
        public TypeComplexity Критерий_сложности
        {
            get { return (TypeComplexity)Settings.Default.ThreeMultiGoal_typeComplexity; }
            set
            {
                Settings.Default.ThreeMultiGoal_typeComplexity = (int)  value;
                Settings.Default.Save();
            }


        }


        [Description("Критерий интерпретируемости"), Category("Трехцелевая оптимизация")]
        public TypeInterpreting Критерий_интерпретируемости
        {
            get { return (TypeInterpreting) Settings.Default.ThreeMultiGoal_typeInterpreting; }
            set
            {
                Settings.Default.ThreeMultiGoal_typeInterpreting = (int)value;
                Settings.Default.Save();
            }


        }


        #endregion

        public void Init2( string Path, string DataSetName)
        {
            path = Path;
            dataSetName = DataSetName;

           
        
        }

        public void Init(int CountFeatures)
        {
            max_count_shrink_vars = CountFeatures;
            count_vars = CountFeatures;

            size_of_individ = CountFeatures * 3 + (int)Math.Pow(3, CountFeatures); //Sqare


            EsMethods.Properties.SettingsBase.Default.ES_method_Count_Multipoint = size_of_individ;
            EsMethods.Properties.SettingsBase.Default.Save();

      
        }
        public void loadParams(string param)
        {
            string stemp = "";
            int itemp = 0;
        
            string[] temp = param.Split('}');

            loadParams_Struct(temp);
            loadParams_PSO(temp);

        

     


            stemp = (temp.Where(x => x.Contains("UseABCS"))).ToArray()[0];
            stemp = stemp.Remove(0, 8);
            int.TryParse(stemp, out itemp);
            Использовать_НАМК_раз_за_такт = itemp;
            




            ////!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            stemp = (temp.Where(x => x.Contains("countGenRules"))).ToArray()[0];
            stemp = stemp.Remove(0, 14);
            int.TryParse(stemp, out itemp);
            Генерировать_правил = itemp;


            stemp = (temp.Where(x => x.Contains("TypeFunc"))).ToArray()[0];
            stemp = stemp.Remove(0, 9);
            switch (stemp)
            {
                case "Triangle": Функция_принадлежности = TypeTermFuncEnum.Треугольник; break;
                case "Gauss": Функция_принадлежности = TypeTermFuncEnum.Гауссоида; break;
                case "Parabola": Функция_принадлежности = TypeTermFuncEnum.Парабола; break;
                case "Trapezium": Функция_принадлежности = TypeTermFuncEnum.Трапеция; break;
                default: Функция_принадлежности = TypeTermFuncEnum.Треугольник; break;
            }




        }

    }
}

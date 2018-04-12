using System.ComponentModel;

namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{
    public partial class MultiGoalOptimaze_conf
    {  
        #region ABCS

        [Description("Использовать САПК"), Category("Алгоритм пчелиной колонии для оптимизации структуры нечеткой системы")]
        public YesNo Использовать_САПК
        {
            get { return toYesNo(BeesMethods.Properties. SettingsBase.Default.ABCS_Used); }
            set
            {
                BeesMethods.Properties.SettingsBase.Default.ABCS_Used = toBool(value);
                BeesMethods.Properties.SettingsBase.Default.Save();
            }


        }

        [Description("Использовать раз за такт САПК"), Category("Алгоритм пчелиной колонии для оптимизации структуры нечеткой системы")]
        public int Использовать_САПК_раз_за_такт
        {
            get { return BeesMethods.Properties.SettingsBase.Default.ABCS_UsedTimes; }
            set
            {
                BeesMethods.Properties.SettingsBase.Default.ABCS_UsedTimes = value;
                BeesMethods.Properties.SettingsBase.Default.Save();
            }


        }



        [Description("Сколько разведчиков отправлено"), Category("Алгоритм пчелиной колонии для оптимизации структуры нечеткой системы")]
        public int Количество_разведчиков
        {
            get { return BeesMethods.Properties.SettingsBase.Default.ABCS_CountScout; }
            set { BeesMethods.Properties.SettingsBase.Default.ABCS_CountScout = value; BeesMethods.Properties.SettingsBase.Default.Save(); }
        }


        [Description("Сколько рабочих пчел отправлено"), Category("Алгоритм пчелиной колонии для оптимизации структуры нечеткой системы")]
        public int Количество_рабочих_пчел
        {
            get { return BeesMethods.Properties.SettingsBase.Default.ABCS_CountWorkers; }
            set { BeesMethods.Properties.SettingsBase.Default.ABCS_CountWorkers = value; BeesMethods.Properties.SettingsBase.Default.Save(); }
        }



        [Description("Сколько правил сгенерировать"), Category("Алгоритм пчелиной колонии для оптимизации структуры нечеткой системы")]
        public int Генерировать_правил
        {
            get { return BeesMethods.Properties.SettingsBase.Default.ABCS_CountRulesAdd; }
            set { BeesMethods.Properties.SettingsBase.Default.ABCS_CountRulesAdd = value; BeesMethods.Properties.SettingsBase.Default.Save(); }
        }


        [Description("Вид функции принадлежности"), Category("Алгоритм пчелиной колонии для оптимизации структуры нечеткой системы")]
        public TypeTermFuncEnum Функция_принадлежности
        {
            get { return (TypeTermFuncEnum)BeesMethods.Properties.SettingsBase.Default.ABCS_TypeTerms; }
            set
            {

                BeesMethods.Properties.SettingsBase.Default.ABCS_TypeTerms = (int)value;
                BeesMethods.Properties.SettingsBase.Default.Save();

            }
        }
     


        #endregion

    }
}

using MultiGoal.Properties;
using System.ComponentModel;
using System.Linq;

namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{
    public partial class MultiGoalOptimaze_conf
    {
        #region Оптимизация правил
        [Description("Оптимизировать состав правил"), Category("Оптимизация состава правил")]
        public YesNo Удалять_правила
        {
            get { return toYesNo(ShrinkMethods.Properties.SettingsBase.Default.Pareto_simpler_Rules); }
            set
            {
                ShrinkMethods.Properties.SettingsBase.Default.Pareto_simpler_Rules = toBool(value);
                ShrinkMethods.Properties.SettingsBase.Default.Save();
            }


        }



        [Description("Удалить правил "), Category("Оптимизация состава правил")]
        public int Удалить_правил
        {
            get { return ShrinkMethods.Properties.SettingsBase.Default.Pareto_simpler_count_shrink_rules; }
            set
            {
                ShrinkMethods.Properties.SettingsBase.Default.Pareto_simpler_count_shrink_rules = value;
                ShrinkMethods.Properties.SettingsBase.Default.Save();
            }


        }


        #endregion


        #region Оптимизация состава термов

        int max_count_shrink_vars;
        readonly int min_count_shrink_vars = 1;

        [Description("Оптимизировать состав термов"), Category("Оптимизация состава термов")]
        public YesNo Удалять_термы
        {
            get { return toYesNo(ShrinkMethods.Properties.SettingsBase.Default.Pareto_simpler_Terms); }
            set
            {
                ShrinkMethods.Properties.SettingsBase.Default.Pareto_simpler_Terms = toBool(value);
                ShrinkMethods.Properties.SettingsBase.Default.Save();
            }


        }



        [Description("По скольки входным параметрам будем уменьшено количество термов "), Category("Оптимизация состава термов")]
        public int Число_параметров_для_уменьшения_термов
        {
            get { return ShrinkMethods.Properties.SettingsBase.Default.Term_shrink_and_rotate_conf_count_shrink; }
            set
            {
                ShrinkMethods.Properties.SettingsBase.Default.Term_shrink_and_rotate_conf_count_shrink = value < max_count_shrink_vars ? value : max_count_shrink_vars;
                ShrinkMethods.Properties.SettingsBase.Default.Term_shrink_and_rotate_conf_count_shrink = ShrinkMethods.Properties.SettingsBase.Default.Term_shrink_and_rotate_conf_count_shrink < min_count_shrink_vars ? min_count_shrink_vars : ShrinkMethods.Properties.SettingsBase.Default.Term_shrink_and_rotate_conf_count_shrink;
                ShrinkMethods.Properties.SettingsBase.Default.Save();
            }


        }

        [Description("Максимальная количество параметров которое вы можете уменьшить "), Category("Оптимизация состава термов")]

        public int Максимально_параметров_для_уменьшения_термов
        {
            get { return max_count_shrink_vars; }



        }

        [Description("Насколько будет уменьшено количество термов"), Category("Оптимизация состава термов")]

        public int Значение_уменьшения_термов
        {
            get { return ShrinkMethods.Properties.SettingsBase.Default.Term_shrink_and_rotate_conf_size_of_shrink; }
            set
            {
                ShrinkMethods.Properties.SettingsBase.Default.Term_shrink_and_rotate_conf_size_of_shrink = value;
                ShrinkMethods.Properties.SettingsBase.Default.Save();
            }


        }


        #endregion


        #region Разделение далеких термов
        [Description("Разьединять термы имеющие далекое лингвистическо значение "), Category("Оптимизация пересечений термов")]
        public YesNo Исключать_пересечение_лигвистически_далеких_термов
        {
            get { return toYesNo( UnionAndUnCrossTermsMethods.Properties.Settings.Default.Pareto_simpler_BreakTheCross); }
            set
            {
                UnionAndUnCrossTermsMethods.Properties.Settings.Default.Pareto_simpler_BreakTheCross = toBool(value);
                UnionAndUnCrossTermsMethods.Properties.Settings.Default.Save();
            }


        }



        #endregion


        #region Объединение термов


        [Description("Объединять термы"), Category("Объединение термов")]
        public YesNo Объединять_термы
        {
            get { return toYesNo(UnionAndUnCrossTermsMethods.Properties.Settings.Default.Pareto_simpler_UnionTerms); }
            set
            {
                UnionAndUnCrossTermsMethods.Properties.Settings.Default.Pareto_simpler_UnionTerms = toBool(value);
                UnionAndUnCrossTermsMethods.Properties.Settings.Default.Save();
            }


        }





        [Description("Процент допустимого перекрытия по площади "), Category("Объединение термов")]
        public double Допустимый_процент_перекрытия_по_площади_термов
        {
            get { return UnionAndUnCrossTermsMethods.Properties.Settings.Default.Pareto_simpler_UnionTerms_bySqarePercent; }
            set
            {
                UnionAndUnCrossTermsMethods.Properties.Settings.Default.Pareto_simpler_UnionTerms_bySqarePercent = value;
                UnionAndUnCrossTermsMethods.Properties.Settings.Default.Save();
            }


        }

        [Description("Процент допустимого перекрытия граници к расстоянию между пиками термов "), Category("Объединение термов")]
        public double Допустимый_процент_перекрытия_по_границам
        {
            get { return UnionAndUnCrossTermsMethods.Properties.Settings.Default.Pareto_simpler_UnionTerms_byBorderPercent; }
            set
            {
                UnionAndUnCrossTermsMethods.Properties.Settings.Default.Pareto_simpler_UnionTerms_byBorderPercent = value;
                UnionAndUnCrossTermsMethods.Properties.Settings.Default.Save();
            }


        }




        #endregion


        public void loadParams_Struct(string[] param)
        {
               string stemp = "";
            int itemp = 0;
            double dtemp = 0;
           

            #region Shrinkers And Unions
            stemp = (param.Where(x => x.Contains("useShrinkRules"))).ToArray()[0];
            stemp = stemp.Remove(0, 15);
            switch (stemp)
            {
                case "True": Удалять_правила = YesNo.Да; break;
                case "False": Удалять_правила = YesNo.Нет; break;
                default: Удалять_правила = YesNo.Да; break;
            }

            stemp = (param.Where(x => x.Contains("ShrinkerRulesCount"))).ToArray()[0];
            stemp = stemp.Remove(0, 19);
            int.TryParse(stemp, out itemp);
            Удалить_правил = itemp;


            stemp = (param.Where(x => x.Contains("UseShrinkerTerms"))).ToArray()[0];
            stemp = stemp.Remove(0, 17);
            switch (stemp)
            {
                case "True": Удалять_термы = YesNo.Да; break;
                case "False": Удалять_термы = YesNo.Нет; break;
                default: Удалять_термы = YesNo.Да; break;
            }


            stemp = (param.Where(x => x.Contains("ShrinkInputFeatures"))).ToArray()[0];
            stemp = stemp.Remove(0, 20);
            int.TryParse(stemp, out itemp);
            Число_параметров_для_уменьшения_термов = itemp;


            stemp = (param.Where(x => x.Contains("ShrinkCountByFeature"))).ToArray()[0];
            stemp = stemp.Remove(0, 21);
            int.TryParse(stemp, out itemp);
            Значение_уменьшения_термов = itemp;

            stemp = (param.Where(x => x.Contains("useBreakCross"))).ToArray()[0];
            stemp = stemp.Remove(0, 14);
            switch (stemp)
            {
                case "True": Исключать_пересечение_лигвистически_далеких_термов = YesNo.Да; break;
                case "False": Исключать_пересечение_лигвистически_далеких_термов = YesNo.Нет; break;
                default: Исключать_пересечение_лигвистически_далеких_термов = YesNo.Да; break;
            }

            stemp = (param.Where(x => x.Contains("useUnionTerms"))).ToArray()[0];
            stemp = stemp.Remove(0, 14);
            switch (stemp)
            {
                case "True": Объединять_термы = YesNo.Да; break;
                case "False": Объединять_термы = YesNo.Нет; break;
                default: Объединять_термы = YesNo.Да; break;
            }

            stemp = (param.Where(x => x.Contains("UnionPercentbySqare"))).ToArray()[0];
            stemp = stemp.Remove(0, 20);
            double.TryParse(stemp, out dtemp);
            Допустимый_процент_перекрытия_по_площади_термов = dtemp;

            stemp = (param.Where(x => x.Contains("UniTermByBorderPercent"))).ToArray()[0];
            stemp = stemp.Remove(0, 23);
            double.TryParse(stemp, out dtemp);
            Допустимый_процент_перекрытия_по_площади_термов = dtemp;
            #endregion

        }

    }
}

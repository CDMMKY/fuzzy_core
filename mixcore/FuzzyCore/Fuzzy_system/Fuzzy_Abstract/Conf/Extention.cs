using System;
using System.Linq;

namespace FuzzySystem.FuzzyAbstract.conf
{ /// <summary>
/// Статический класс для помощи в написании реализации функции loadParams, для любого из конфигурационных файлов
/// </summary>
    public static class Extention
    {/// <summary>
     /// Метод для получения целочисленого значения параметра из строки параметров с указанным именем
     /// </summary>
     /// <param name="Source">Строки со всеми параметрами алгоритма</param>
     /// <param name="NameParam">Имя параметра, чьё значение необходимо извлечь из строки</param>
     /// <returns></returns>
        public static int getParamValueInt(String[] Source, String NameParam)
        {
            string stemp = (Source.Where(x => x.Contains(NameParam))).ToArray()[0];
            stemp = stemp.Remove(0, NameParam.Length + 1);
            int itemp = 0;
            int.TryParse(stemp, out itemp);
            return itemp;
        }

        /// <summary>
        /// Метод для получения логического значения параметра из строки параметров с указанным именем
        /// </summary>
        /// <param name="Source">Строки со всеми параметрами алгоритма</param>
        /// <param name="NameParam">Имя параметра, чьё значение необходимо извлечь из строки</param>
        /// <returns></returns>
        public static bool getParamValueBool(String[] Source, String NameParam)
        {
            string stemp = (Source.Where(x => x.Contains(NameParam))).ToArray()[0];
            stemp = stemp.Remove(0, NameParam.Length + 1);
            bool btemp = false;
            bool.TryParse(stemp, out btemp);
            return btemp;
        }
        /// <summary>
        /// Метод для получения вещественного значения параметра из строки параметров с указанным именем
        /// </summary>
        /// <param name="Source">Строки со всеми параметрами алгоритма</param>
        /// <param name="NameParam">Имя параметра, чьё значение необходимо извлечь из строки</param>
        /// <returns></returns>
        public static double getParamValueDouble(String[] Source, String NameParam)
        {
            string stemp = (Source.Where(x => x.Contains(NameParam))).ToArray()[0];
            stemp = stemp.Remove(0, NameParam.Length + 1);
            double dtemp = 0;
            double.TryParse(stemp, out dtemp);
            return dtemp;
        }

        /// <summary>
        /// Метод для получения строкового значения параметра из строки параметров с указанным именем
        /// </summary>
        /// <param name="Source">Строки со всеми параметрами алгоритма</param>
        /// <param name="NameParam">Имя параметра, чьё значение необходимо извлечь из строки</param>
        /// <returns></returns>
        public static string getParamValueString(String[] Source, String NameParam)
        {
            string stemp = (Source.Where(x => x.Contains(NameParam))).ToArray()[0];
            stemp = stemp.Remove(0, NameParam.Length + 1);
            return stemp;
        }

    }
}

using PropertyGridUtils;
using System.ComponentModel;

namespace FuzzySystem.FuzzyAbstract.conf
{
    /// <summary>
    /// Основа для всех классов-конфигураторов
    /// </summary>
    [TypeConverter(typeof(PropertySorter))]
    public interface IBaseConf 
    { /// <summary>
    /// Метод инициализации, вызывается до показа формы с содержимым класса, служит для корректного расчета максимального и минимального значения некоторых параметров
    /// </summary>
    /// <param name="countFeatures">Количество входных параметров в обучающей выборке данных</param>
        void Init(int countFeatures);
        /// <summary>
        /// Метод загрузки параметров алгоритма из строки, как правило при запуске оптимизации из консоли.
        /// </summary>
        /// <param name="param">Строрка параметров поддерживаемая алгоритмов вида ИмяПараметра=Значение параметра</param>
        void loadParams(string param);
    }
}

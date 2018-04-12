
using System.Collections.Generic;

namespace FuzzySystem.FuzzyAbstract
{
   public interface IAlgorithm
   {
       string ToString(bool with_param = false);
       List<FuzzySystemRelisedList.TypeSystem> SupportedFS { get; }
    }
}

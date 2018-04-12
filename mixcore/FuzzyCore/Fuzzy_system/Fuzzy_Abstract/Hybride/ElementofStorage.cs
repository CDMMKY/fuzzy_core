using System;

namespace FuzzySystem.FuzzyAbstract.Hybride
{
    public class ElementofStorage
    {
       public DateTime TimeStamp{ get; protected set; }
       public double LearnError { get; set; }
       public double TestError { get;  set; }
       public string AlgName { get; protected set; }

    
     public  ElementofStorage( string algSourceName)
       {
           TimeStamp = DateTime.UtcNow;
           LearnError = double.MaxValue;
           TestError = double.MaxValue;
           this.AlgName = algSourceName;
       }

    }
}

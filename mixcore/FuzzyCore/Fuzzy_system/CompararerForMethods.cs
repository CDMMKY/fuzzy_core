using System.Collections.Generic;


namespace FuzzySystem
{
    class CompararerForMethods : IComparer<FuzzySystem.FuzzyAbstract.IAbstractGenerator>,
                                 IComparer<FuzzySystem.FuzzyAbstract.IAbstractLearnAlgorithm>
                                                        
    {


        // Compares by Height, Length, and Width.
        public int Compare(FuzzySystem.FuzzyAbstract.IAbstractGenerator x, FuzzySystem.FuzzyAbstract.IAbstractGenerator y)
        {
            return x.ToString().CompareTo(y.ToString()) ;
            
          
        }

        public int Compare(FuzzySystem.FuzzyAbstract.IAbstractLearnAlgorithm x, FuzzySystem.FuzzyAbstract.IAbstractLearnAlgorithm y)
        {
            return x.ToString().CompareTo(y.ToString());


        }

       
   


    }

    
}

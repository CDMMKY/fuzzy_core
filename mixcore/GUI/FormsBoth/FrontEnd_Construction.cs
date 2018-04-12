using FuzzySystem.FuzzyFrontEnd;



namespace FuzzySystem
{



    public class FrontEnd_Construction
    {




        #region Public methods



        public static IFuzzySystemFroentEnd init_fuzzy_system(int Type)
        {
            IFuzzySystemFroentEnd result = null;

         FuzzySystem.FuzzyAbstract.FuzzySystemRelisedList.TypeSystem   type_fuzzy_system = (FuzzySystem.FuzzyAbstract.FuzzySystemRelisedList.TypeSystem)Type;

         result = new BaseFrontEnd(type_fuzzy_system);
            return result;
        }


   

        #endregion



        #region private_interStuctor

  
      
      

        #endregion

    }
}

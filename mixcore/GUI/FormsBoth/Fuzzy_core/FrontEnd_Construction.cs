using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fuzzy_system.Fuzzy_FrontEnd;
using System.IO;



namespace Fuzzy_system
{



    public class FrontEnd_Construction
    {


        #region Fuzzy system Types
        public enum Type_System
        {
            Singletone = 0,
            PittsburghClassifier = 1
        }



        #region Public methods
        public static object[] AllTypesFuzzySystem
        {
            get
            {
                return new object[] { "Аппроксиматор Синглтон" , //Singletone = 0
        "Классификатор Питтсбургский" //PittsburgClassifier =1
        };

            }


        }

#endregion


          


        public static IFuzzy_System_FroentEnd init_fuzzy_system(int Type)
        {
            IFuzzy_System_FroentEnd result = null;

         Type_System   type_fuzzy_system = (Type_System)Type;
            switch (type_fuzzy_system)
            {
                case Type_System.PittsburghClassifier: { break; }
                case Type_System.Singletone: { result = new Singletone_FrontEnd(); break; }
            }
            return result;
        }


   

        #endregion



        #region private_interStuctor

  
      
      

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace Fuzzy_system.Fuzzy_Abstract
{
   public abstract class Fuzzy_System
    {
        
        #region Visible public methods

       abstract public int value_complexity(int index = 0);


      

  
        public sample_set Learn_Samples_set {get {return learn_samples_set; }
        }

        public sample_set Test_Samples_set
        {
            get { return test_samples_set; }
        }

        abstract public int Count_Rules(int index = 0);
     
        public int Count_Samples
        {
            get { return learn_samples_set.Count_Samples; }
        }

        public int Count_Vars
        {
            get { return learn_samples_set.Count_Vars; }
        }

           

        


        #endregion

        #region constructor

        public Fuzzy_System(sample_set learn_set, sample_set test_set)
        {
            learn_samples_set = learn_set;
            if (test_set != null)
            {
                test_samples_set = test_set;
                for (int i = 0; i < Count_Vars; i++)
                {

                    if (
                        !learn_samples_set.Input_Attribute(i).Name.Equals(test_samples_set.Input_Attribute(i).Name,
                                                                           StringComparison.OrdinalIgnoreCase))
                    {
                        throw (new InvalidEnumArgumentException("Атрибуты обучающей таблицы и тестовой не совпадают"));
                    }

                }
            }

        }

        #endregion

       

        protected int  [] dec_count(int [] counter, int [] slice_count)
        {
            int[] result = counter;
            int j = Count_Vars - 1;
            result[j]--;
            while ((result[j]<0) &&(j>0) )
            {
                result[j] = slice_count[j] - 1;
                j--;
                result[j]--;
            }
            return result;
        }



       


  
        #region  private interstruct




        protected  List<Knowlege_base_Rules> rulles_database_set = new List<Knowlege_base_Rules>();
        protected sample_set learn_samples_set;
        protected  sample_set test_samples_set;
        #endregion





    }
}

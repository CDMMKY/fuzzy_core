using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;


namespace FuzzySystem.FuzzyAbstract
{
   public abstract class IFuzzySystem
    {
        
        #region Visible public methods

       abstract public int ValueComplexity(int index = 0);

       public abstract double ErrorTestSamples(KnowlegeBaseRules Source);
       public abstract double ErrorLearnSamples(KnowlegeBaseRules Source);


       public abstract double ErrorTestSamples();
       public abstract double ErrorLearnSamples();


       public SampleSet LearnSamplesSet
       {
           get { return learnSamplesSet; }
        }

       public SampleSet TestSamplesSet
        {
            get { return testSamplesSet; }
        }

    
        public int CountVars
        {
            get { return learnSamplesSet.CountVars; }
        }

        public bool [] AcceptedFeatures { get {return acceptedFeatures; } set { acceptedFeatures = value; } }
      
       protected bool [] acceptedFeatures;
      
           

        


        #endregion

        #region constructor

        public  IFuzzySystem(SampleSet learn_set, SampleSet test_set)
        {
            learnSamplesSet = learn_set;
            acceptedFeatures = new bool[CountVars];
         
            if (test_set != null)
            {
                testSamplesSet = test_set;
                for (int i = 0; i < CountVars; i++)
                {
                    acceptedFeatures[i] = true;

                    if (
                        !learnSamplesSet.InputAttribute(i).Name.Equals(testSamplesSet.InputAttribute(i).Name,
                                                                           StringComparison.OrdinalIgnoreCase))
                    {
                        throw (new InvalidEnumArgumentException("Атрибуты обучающей таблицы и тестовой не совпадают"));
                    }

                }
            }

        }

        public IFuzzySystem(IFuzzySystem Source)
        {
            learnSamplesSet = Source.learnSamplesSet;
            acceptedFeatures = Source.acceptedFeatures.Clone() as bool [];

            testSamplesSet = Source.testSamplesSet;
        
            }

        


        #endregion
        public virtual void UnlaidProtectionFix(KnowlegeBaseRules Source)
        {

            lock (Source)
            {
                UnlaidProtectionFixMaxMinBorder(Source);
                UnlaidProtectionInMiddle(Source);
                Source.TermsSet.Trim();
            }
        }


        public virtual void UnlaidProtectionFixMaxMinBorder(KnowlegeBaseRules Source)
        {
            for (int i = 0; i < CountVars; i++)
            {
                if (AcceptedFeatures[i] == false)
                {
                    continue;
                }
                List<Term> all_terms_for_var =
                      Source.TermsSet.FindAll(x => x.NumberOfInputVar == i);
                if (all_terms_for_var.Find(x => x.TermFuncType == TypeTermFuncEnum.Гауссоида) != null)
                {
                    continue;
                }
                else
                {
                    double min = all_terms_for_var.Min(x => x.Min);
                    int min_index = all_terms_for_var.FindIndex(x => (x.Min == min));
                    all_terms_for_var[min_index].Min =
                        learnSamplesSet.InputAttributes[i].Min - 0.001 * learnSamplesSet.InputAttributes[i].Scatter;
                    //double max = double.NegativeInfinity;
                    double max = all_terms_for_var.Max(x => x.Max);
                    int max_index = all_terms_for_var.FindIndex(x => (x.Max == max));
                    all_terms_for_var[max_index].Max = learnSamplesSet.InputAttributes[i].Max +
                         0.001 * learnSamplesSet.InputAttributes[i].Scatter;
                }
            }

        }


        public virtual void UnlaidProtectionInMiddle(KnowlegeBaseRules current_database)
        {

            for (int i = 0; i < CountVars; i++)
            {
                if (AcceptedFeatures[i] == false)
                {
                    continue;
                }
                List<Term> current_terms = current_database.TermsSet.FindAll(x => x.NumberOfInputVar == i);
                if (current_terms.Exists(x => x.TermFuncType == TypeTermFuncEnum.Гауссоида)) { continue; }
                for (int j = 0; j < current_terms.Count - 1; j++)
                {

                    if ((current_terms[j].Max < current_terms[j + 1].Min))
                    {
                        double temp = current_terms[j].Max;
                        current_terms[j].Max = current_terms[j + 1].Min;
                        current_terms[j + 1].Min = temp;
                    }
                    if (current_terms[j].Max == current_terms[j + 1].Min)
                    {
                        current_terms[j].Max += learnSamplesSet.InputAttributes[i].Scatter * 0.001;
                        current_terms[j + 1].Min -= learnSamplesSet.InputAttributes[i].Scatter * 0.001;
                    }
                }
            }
        }



         

        

       


  
        #region  private interstruct

        string nameObj = null;
        public override string ToString()
        {
            if (nameObj == null)
            {
                Random rand = new Random(DateTime.Now.Millisecond);
                nameObj = base.ToString() + (rand.Next()+ rand.Next()).ToString();
            }
        return  nameObj;
        }

        protected  List<KnowlegeBaseRules> rullesDatabaseSet = new List<KnowlegeBaseRules>();
        protected SampleSet learnSamplesSet;
        protected SampleSet testSamplesSet;
        #endregion





    }
}

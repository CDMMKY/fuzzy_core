namespace ReCalcUFSForm
{
    public  class RecombineUFSExample
    
   {
       protected abstract_RecombineUFS Child;

          public string SourceI
       {
           get { return Child.Source; } 
       }

    
      public double GIBNormal
      {
          get { return Child.GIBNormalI; }

      }
  
      public double GIBSumStraigth
      {
          get { return Child.GIBSumStraigthI; }
      }

      public double GIBSumReverce
      {
          get { return Child.GIBSumReverceI; }
      }

      public double GISNormal
      {
          get { return Child.GISNormalI; }
      }
   
      public double GISSumStraigth
      {
          get { return Child.GISSumStraigthI; }
      }
    
      public double GISSumReverce
      {
          get { return Child.GISSumReverceI; }

      }
  
      public double GICNormal
      {
          get { return Child.GICNormalI; }

      }
     
      public double GICSumStraigh
      {
          get { return Child.GICSumStraighI; }
      }
    
      public double GICSumReverce
      {
          get { return Child.GICSumReverceI; }
      }
  
      public double LindisNormal
      {
          get { return Child.LindisNormalI; }

      }

      public double LindisSumStraigh
      {
          get { return Child.LindisSumStraighI; }

      }
    
      public double LindisSumReverce
      {
          get { return Child.LindisSumReverceI; }

      }
      public double NormalIndex
      {
          get { return Child.NormalIndexI; }

      }

      public double SumsStraigthIndex
      {
          get { return Child.SumsStraigthIndexI; }
      }
   
      public double SumReverseIndex
      {
          get { return Child.SumReverseIndexI; }
      }


       public RecombineUFSExample(string UFSPAth)
        {
            try
            {
                Child = new RecombineUFSApproximate(UFSPAth);
            }
            catch
            {
                Child = new RecombineUFSClassifier(UFSPAth); 
            }

       }

       public  void Work()
       {
           Child.Work();

       }
    }
}

using FuzzySystem.PittsburghClassifier;
using FuzzySystem.PittsburghClassifier.Mesure;
using FuzzySystem.PittsburghClassifier.UFS;
using FuzzySystem.FuzzyAbstract;
using FuzzyCore.FuzzySystem.FuzzyAbstract;


namespace ReCalcUFSForm
{
    class RecombineUFSClassifier : abstract_RecombineUFS
    {
          protected  PCFuzzySystem fuzzy_system;
           public RecombineUFSClassifier(string UFSPAth)
               : base(UFSPAth)
           {

               SampleSet LearnTable = BaseUFSLoader.LoadLearnFromUFS(Source);
               SampleSet TestTable = BaseUFSLoader.LoadTestFromUFS(Source);

            fuzzy_system = new PCFuzzySystem(LearnTable, TestTable);
            fuzzy_system =fuzzy_system.loadUFS(Source);
                                  
        }

        public override void Work()
        {
            GIBNormal = fuzzy_system.getGIBNormal();
            GIBSumStraigth = fuzzy_system.getGIBSumStrait();
            GIBSumReverce = fuzzy_system.getGIBSumReverse();

            GICNormal = fuzzy_system.getGICNormal();
            GICSumReverce = fuzzy_system.getGICSumReverce();
            GICSumStraigh = fuzzy_system.getGICSumStraigth();

            GISNormal = fuzzy_system.getGISNormal();
            GISSumReverce = fuzzy_system.getGISSumReverce();
            GISSumStraigth = fuzzy_system.getGISSumStraigt();

            LindisNormal = fuzzy_system.getLindisNormal();
            LindisSumStraigh = fuzzy_system.getLindisSumStraight();
            LindisSumReverce = fuzzy_system.getLindisSumReverse();

            NormalIndex = fuzzy_system.getNormalIndex();
            SumReverseIndex = fuzzy_system.getIndexSumReverse();
            SumsStraigthIndex = fuzzy_system.getIndexSumStraigt();
            PCFSUFSWriter.saveToUFS(fuzzy_system, Source);

        }


    }
}

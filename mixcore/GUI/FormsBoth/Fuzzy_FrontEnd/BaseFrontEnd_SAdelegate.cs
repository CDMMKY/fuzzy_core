
using FuzzyCore.FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.SingletoneApproximate;
using FuzzySystem.SingletoneApproximate.UFS;
using System;

namespace FuzzySystem.FuzzyFrontEnd
{
    partial class BaseFrontEnd
    {
        private void loadSALearnFromUFS(string file_name)
        {
            learnSet = BaseUFSLoader.LoadLearnFromUFS(file_name);
        }

        private void loadSALearnFromKeel(string file_name)
        {
            learnSet = new SampleSet(file_name);
        }

        private void loadSATestFromKeel(string file_name)
        {
            testSet = new SampleSet(file_name);
        }

        private void loadSATestFromUFS(string fileName)
        {
            testSet = BaseUFSLoader.LoadTestFromUFS(fileName);
        }

        private void createSAFSFromSampleSet()
        {
            FuzzySystem = new SAFuzzySystem(learnSet, testSet);
        }

        private void loadSAFSFromUFS()
        {
            FuzzySystem = SAFSUFSLoader.loadUFS(FuzzySystem as SAFuzzySystem, UFS_file_name);
        }

        private void writeSAtoUFS(IFuzzySystem FS, string fileName)
        {
            SAFSUFSWriter.saveToUFS(FS as SAFuzzySystem, fileName);
        }


        private string ErrorInfoSA(IFuzzySystem FS)
        {
            SAFuzzySystem IFS = FS as SAFuzzySystem;
            if (IFS.RulesDatabaseSet.Count < 1) { return "Точность нечеткой системы недоступна"; }


            approxLearnResult.Add(IFS.approxLearnSamples(IFS.RulesDatabaseSet[0]));
           approxTestResult.Add(IFS.approxTestSamples(IFS.RulesDatabaseSet[0]));

           approxLearnResultMSE.Add(IFS.RMSEtoMSEforLearn(approxLearnResult[approxLearnResult.Count - 1]));
           approxTestResultMSE.Add(IFS.RMSEtoMSEforTest(approxTestResult[approxTestResult.Count - 1]));

           approxLearnResultMSEdiv2.Add(IFS.RMSEtoMSEdiv2forLearn(approxLearnResult[approxLearnResult.Count - 1]));
           approxTestResultMSEdiv2.Add(IFS.RMSEtoMSEdiv2forTest(approxTestResult[approxTestResult.Count - 1]));

    
            return "Точностью на обучающей выборке(RSME)  " + approxLearnResult [approxLearnResult.Count-1].ToString() + " , Точность на тестовой выборке(RMSE)  " + approxTestResult[approxTestResult.Count-1].ToString() + " " + Environment.NewLine +
          "Точностью на обучающей выборке(MSE)  " + approxLearnResultMSE[approxLearnResultMSE.Count-1].ToString() + " , Точность на тестовой выборке(MSE)  " + approxTestResultMSE[approxTestResultMSE.Count-1].ToString() + " " + Environment.NewLine +
          "Точностью на обучающей выборке(MSE/2)  " + approxLearnResultMSEdiv2[approxLearnResultMSEdiv2.Count-1].ToString() + " , Точность на тестовой выборке(MSE/2)  " + approxTestResultMSEdiv2[approxTestResultMSEdiv2.Count-1].ToString()+ " " + Environment.NewLine;
      

        }


    }
}

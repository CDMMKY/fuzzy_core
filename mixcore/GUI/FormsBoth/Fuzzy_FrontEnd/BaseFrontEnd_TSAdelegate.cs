
using FuzzyCore.FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.SingletoneApproximate;
using FuzzySystem.TakagiSugenoApproximate;
using FuzzySystem.TakagiSugenoApproximate.UFS;
using System;

namespace FuzzySystem.FuzzyFrontEnd
{
    partial class BaseFrontEnd
    {
        private void loadTSALearnFromUFS(string file_name)
        {
            learnSet = BaseUFSLoader.LoadLearnFromUFS(file_name);
        }

        private void loadTSALearnFromKeel(string file_name)
        {
            learnSet = new SampleSet(file_name);
        }

        private void loadTSATestFromKeel(string file_name)
        {
            testSet = new SampleSet(file_name);

        }

        private void loadTSATestFromUFS(string fileName)
        {
            testSet = BaseUFSLoader.LoadLearnFromUFS(fileName);
        }

        private void createTSAFSFromSampleSet()
        {
            FuzzySystem = new TSAFuzzySystem(learnSet, testSet);
        }

        private void loadTSAFSFromUFS()
        {
            FuzzySystem = TSAFSUFSLoader.loadUFS(FuzzySystem as TSAFuzzySystem, UFS_file_name);
        }

        private void writeTSAtoUFS(IFuzzySystem FS, string fileName)
        {
            TSAFSUFSWriter.saveToUFS(FS as TSAFuzzySystem, fileName);
        }

        private string ErrorInfoTSA(IFuzzySystem FS)
        {
            TSAFuzzySystem IFS = FS as TSAFuzzySystem;
            if (IFS.RulesDatabaseSet.Count < 1) { return "Точность нечеткой системы недоступна"; }

            approxLearnResult.Add(IFS.approxLearnSamples(IFS.RulesDatabaseSet[0]));
            approxTestResult.Add(IFS.approxTestSamples(IFS.RulesDatabaseSet[0]));
            approxLearnResultMSE.Add(IFS.RMSEtoMSEforLearn(approxLearnResult[approxLearnResult.Count-1]));
            approxTestResultMSE.Add(IFS.RMSEtoMSEforTest(approxTestResult[approxTestResult.Count-1]));
            approxLearnResultMSEdiv2.Add(IFS.RMSEtoMSEdiv2forLearn(approxLearnResult[approxLearnResult.Count - 1]));
            approxTestResultMSEdiv2.Add(IFS.RMSEtoMSEdiv2forTest(approxTestResult[approxTestResult.Count - 1]));

        //    Console.WriteLine($"Time\t{IFS.sw.ElapsedMilliseconds} {Environment.NewLine }Ticks\t{IFS.sw.ElapsedTicks}");

            return "Точностью на обучающей выборке(RSME)  " + approxLearnResult[approxLearnResult.Count-1].ToString() + " , Точность на тестовой выборке(RMSE)  " + approxTestResult[approxTestResult.Count-1].ToString()+ " " + Environment.NewLine +
          "Точностью на обучающей выборке(MSE)  " + approxLearnResultMSE[approxLearnResultMSE.Count-1].ToString() + " , Точность на тестовой выборке(MSE)  " + approxTestResultMSE[approxTestResultMSE.Count-1].ToString()+ " " + Environment.NewLine +
          "Точностью на обучающей выборке(MSE/2)  " + approxLearnResultMSEdiv2[approxLearnResultMSEdiv2.Count-1].ToString() + " , Точность на тестовой выборке(MSE/2)  " + approxTestResultMSEdiv2[approxTestResultMSEdiv2.Count-1].ToString()+ " " + Environment.NewLine;
        }

    }
}

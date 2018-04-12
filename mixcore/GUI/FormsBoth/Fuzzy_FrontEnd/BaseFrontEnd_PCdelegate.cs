
using FuzzyCore.FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.PittsburghClassifier;
using FuzzySystem.PittsburghClassifier.UFS;
using System;

namespace FuzzySystem.FuzzyFrontEnd
{
    partial class BaseFrontEnd
    {
        private void loadPCLearnFromUFS(string file_name)
        {
            learnSet = BaseUFSLoader.LoadLearnFromUFS(file_name);
        }

        private void loadPCLearnFromKeel(string file_name)
        {
            learnSet = new SampleSet(file_name);
        }

        private void loadPCTestFromKeel(string file_name)
        {
            testSet = new SampleSet(file_name);
        }


        private void loadPCTestFromUFS(string fileName)
        {
            testSet = BaseUFSLoader.LoadTestFromUFS(fileName);
        }

        private void createPCFSFromSampleSet()
        {
            FuzzySystem = new PCFuzzySystem(learnSet, testSet);
        }

        private void loadPCFSFromUFS()
        {
            FuzzySystem = PCFSUFSLoader.loadUFS(FuzzySystem as PCFuzzySystem, UFS_file_name);
        }

        private void writePCtoUFS(IFuzzySystem FS, string fileName)
        {
            PCFSUFSWriter.saveToUFS(FS as PCFuzzySystem, fileName);
        }

        private string ErrorInfoPC(IFuzzySystem FS)
        {
            PCFuzzySystem IFS = FS as PCFuzzySystem;
            if (IFS.RulesDatabaseSet.Count < 1) { return "Точность нечеткой системы недоступна"; } 
            classLearnResult.Add (IFS.ClassifyLearnSamples(IFS.RulesDatabaseSet[0]));
            classTestResult.Add( IFS.ClassifyTestSamples(IFS.RulesDatabaseSet[0]));
            classErLearn.Add(IFS.ErrorLearnSamples(IFS.RulesDatabaseSet[0]));
            classErTest.Add( IFS.ErrorTestSamples(IFS.RulesDatabaseSet[0]));
     
            return "Точностью на обучающей выборке  " + classLearnResult[classLearnResult.Count-1].ToString() + " , Точность на тестовой выборке  " + classTestResult[classTestResult.Count-1].ToString() + " " + Environment.NewLine +
                "Ошибкой на обучающей выборке  " + classErLearn[classErLearn.Count-1].ToString() + " , Ошибкой на тестовой выборке  " + classErTest[classErTest.Count-1].ToString() + " " + Environment.NewLine;
            

        }


    }
}

using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;

namespace FuzzySystem.FuzzyFrontEnd
{
    public interface IFuzzySystemFroentEnd
    {
        bool Load_learn_set(string file_name);
        void Load_test_set(string file_name);


        object[] addGeneratorAlgorithm { get; }
        object[] learn_algorithm { get; }

        void Set_count_add_generator(int count_used_Algorithm);
        void Set_count_learn_algorithm(int count_used_Algorithm);

        IGeneratorConf Set_add_generator_algorothm(int numAlgorithm, int typeAlgorithm);

        ILearnAlgorithmConf Set_learn_algorothm(int numAlgorithm, int typeAlgorithm);

        void Set_count_repeate_into_cirlucle(int numRepeate);
        void Set_count_repeate_renew(int numRepeate);
        void Calc();
        string get_log { get; }
        int get_global_completed();
        bool Is_UFS { get; }
        bool is_autosave_FS { set; }
        string path_to_save { set; }
        int max_sections_of_Calc { get; }
        System.ComponentModel.BackgroundWorker ProgressSource { set; }

        List<double> ApproxLearnResult { get; }
        List<double> ApproxTestResult { get; }
        List<double> ApproxLearnResultMSE { get; }
        List<double> ApproxTestResultMSE { get; }
        List<double> ApproxLearnResultMSEdiv2 { get; }
        List<double> ApproxTestResultMSEdiv2 { get; }

        List<double> ClassLearnResult { get; }
        List<double> ClassTestResult { get; }
        List<double> ClassErLearn { get; }
        List<double> ClassErTest { get; }

        FuzzySystemRelisedList.TypeSystem TypeFuzzySystem { get; }

    }
}

using FuzzySystem.FuzzyAbstract;
using System;
using System.Collections.Generic;

namespace FuzzySystem.SingletoneApproximate
{
    public static class KNNConsequent
    {
        public static double NearestApprox(SAFuzzySystem Approximate,TermSetGlobal<Term> termSet)
        {
            return NearestApprox(Approximate,termSet.ToList());
        }

        public static double NearestApprox(SAFuzzySystem Approximate, TermSetInRule<Term> termSet)
        {
            return NearestApprox(Approximate, termSet.ToList());
        }

        public static double NearestApprox(SAFuzzySystem Approximate,List<Term> termSet)
        {
            double min_diff = double.PositiveInfinity;
            int min_diff_index = 0;
            for (int c = 0; c <Approximate.LearnSamplesSet.CountSamples; c++)
            {
                double current_diff = 0;
                for (int i = 0; i < termSet.Count; i++)
                {
                    
                        if (Approximate.AcceptedFeatures[termSet[i].NumVar] == false) { continue; }

                    if (termSet[i] != null)
                    {
                        switch (termSet[i].TermFuncType)
                        {
                            case TypeTermFuncEnum.Треугольник:
                                current_diff +=
                                    Math.Abs(Approximate.LearnSamplesSet.DataRows[c].InputAttributeValue[termSet[i].NumVar] -
                                             termSet[i].Parametrs[1]);
                                break;
                            case TypeTermFuncEnum.Гауссоида: current_diff +=
                               Math.Abs(Approximate.LearnSamplesSet.DataRows[c].InputAttributeValue[termSet[i].NumVar] -
                                        termSet[i].Parametrs[0]);
                                break;
                            case TypeTermFuncEnum.Парабола:
                                double argv = (termSet[i].Parametrs[0] + termSet[i].Parametrs[1]) / 2;
                                current_diff +=
                               Math.Abs(Approximate.LearnSamplesSet.DataRows[c].InputAttributeValue[termSet[i].NumVar] -
                                        argv);
                                break;
                            case TypeTermFuncEnum.Трапеция: double argvTR = (termSet[i].Parametrs[1] + termSet[i].Parametrs[2]) / 2;
                                current_diff +=
                               Math.Abs(Approximate.LearnSamplesSet.DataRows[c].InputAttributeValue[termSet[i].NumVar] -
                                        argvTR);
                                break;
                        }
                    }
                }
                if (current_diff < min_diff)
                {
                    min_diff = current_diff;
                    min_diff_index = c;
                }
            }
            return Approximate.LearnSamplesSet.DataRows[min_diff_index].DoubleOutput;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.Mesure;

namespace FuzzySystem.TakagiSugenoApproximate.Mesure
{
    public static class InterpretingGi3
    {
        public static double getNormalIndex(this TSAFuzzySystem source, double goodsForBorder = 0, double goodsForAreas = 0, int indexDataBase = 0)
        {
            double result = 0.0;
            if (goodsForBorder == 0)
            {
                goodsForBorder = FuzzyCore.Properties.Settings.Default.Pareto_simpler_UnionTerms_byBorderPercent * 0.01;
            }
            if (goodsForAreas == 0)
            {
                goodsForAreas = FuzzyCore.Properties.Settings.Default.Pareto_simpler_UnionTerms_bySqarePercent * 0.01;
            }
            if (source != null)
            {
                double temp = 0.0;
                for (int i = 0; i < source.CountFeatures; i++)
                {
                    temp = 0.0;
                    List<Term> termList = source.RulesDatabaseSet[indexDataBase].TermsSet.Where(x => x.NumVar == i).ToList();
                    if (termList.Count() <= 1)
                    {
                        temp = 1.0;
                    }
                    else
                    {
                        for (int j = 0; j < termList.Count; j++)
                        {
                            for (int k = j + 1; k < termList.Count; k++)
                            {
                                temp += TermOnterpreting.getG3(termList[j], termList[k], termList.Count(), source.LearnSamplesSet.InputAttributes[i].Scatter, goodsForBorder, goodsForAreas) * TermOnterpreting.getIndexByLinds(termList[j], termList[k], termList);
                            }
                        }
                        temp = temp / (double) ((termList.Count() * (termList.Count() - 1)) * 0.5);
                    }
                    result += temp;
                }
                result = result / (double) source.CountFeatures;
            }
            return result;
        }


        public static double getGIBNormal(this TSAFuzzySystem source, double goodsForBorder = 0, int indexDataBase = 0)
        {
            double result = 0.0;
            if (goodsForBorder == 0)
            {
                goodsForBorder = FuzzyCore.Properties.Settings.Default.Pareto_simpler_UnionTerms_byBorderPercent * 0.01;
            }
            if (source != null)
            {
                double temp = 0.0;
                for (int i = 0; i < source.CountFeatures; i++)
                {
                    temp = 0.0;
                    List<Term> termList = source.RulesDatabaseSet[indexDataBase].TermsSet.Where(x => x.NumVar == i).ToList();
                    if (termList.Count() <= 1)
                    {
                        temp = 1.0;
                    }
                    else
                    {
                        for (int j = 0; j < termList.Count; j++)
                        {
                            for (int k = j + 1; k < termList.Count; k++)
                            {
                                temp += TermOnterpreting.getIndexByBordersClose(termList[j], termList[k], goodsForBorder);
                            }
                        }
                        temp = temp / (double) ((termList.Count() * (termList.Count() - 1)) * 0.5);
                    }
                    result += temp;
                }
                result = result / (double) source.CountFeatures;
            }
            return result;
        }

        public static double getGIBSumStrait(this TSAFuzzySystem source, double goodsForBorder = 0, int indexDataBase = 0)
        {
            double result = 0.0;
            if (goodsForBorder == 0)
            {
                goodsForBorder = FuzzyCore.Properties.Settings.Default.Pareto_simpler_UnionTerms_byBorderPercent * 0.01;
            }

            if (source != null)
            {
                double temp = 0.0;
                for (int i = 0; i < source.CountFeatures; i++)
                {
                    temp = 0.0;
                    List<Term> termList = source.RulesDatabaseSet[indexDataBase].TermsSet.Where(x => x.NumVar == i).ToList();
                    if (termList.Count() <= 1)
                    {
                        temp = 1.0;
                    }
                    else
                    {

                        for (int j = 0; j < termList.Count; j++)
                        {
                            for (int k = j + 1; k < termList.Count; k++)
                            {
                                temp += TermOnterpreting.getIndexByBordersClose(termList[j], termList[k], goodsForBorder);
                            }
                        }

                    }
                    result += temp;
                }
                result = result / (double)source.CountFeatures;
            }
            return result;
        }

        public static double getGIBSumReverse(this TSAFuzzySystem source, double goodsForBorder = 0, int indexDataBase = 0)
        {
            double result = 0.0;
            if (goodsForBorder == 0)
            {
                goodsForBorder = FuzzyCore.Properties.Settings.Default.Pareto_simpler_UnionTerms_byBorderPercent * 0.01;
            }
            if (source != null)
            {
                double temp = 0.0;
                for (int i = 0; i < source.CountFeatures; i++)
                {
                    temp = 0.0;
                    List<Term> termList = source.RulesDatabaseSet[indexDataBase].TermsSet.Where(x => x.NumVar == i).ToList();
                    if (termList.Count() <= 1)
                    {
                        temp = 0.0;
                    }
                    else
                    {
                        for (int j = 0; j < termList.Count; j++)
                        {
                            for (int k = j + 1; k < termList.Count; k++)
                            {
                                temp += 1 - TermOnterpreting.getIndexByBordersClose(termList[j], termList[k], goodsForBorder);
                            }
                        }
                    }
                    result += temp;
                }
                result = result / (double) source.CountFeatures;
            }
            return result;
        }

        public static double getGISNormal(this TSAFuzzySystem source, double goodsForArea = 0, int indexDataBase = 0)
        {
            double result = 0.0;
            if (goodsForArea == 0)
            {
                goodsForArea = FuzzyCore.Properties.Settings.Default.Pareto_simpler_UnionTerms_bySqarePercent * 0.01;
            }
            if (source != null)
            {
                double temp = 0.0;
                for (int i = 0; i < source.CountFeatures; i++)
                {
                    temp = 0.0;
                    List<Term> termList = source.RulesDatabaseSet[indexDataBase].TermsSet.Where(x => x.NumVar == i).ToList();
                    if (termList.Count() <= 1)
                    {
                        temp = 1.0;
                    }
                    else
                    {
                        for (int j = 0; j < termList.Count; j++)
                        {
                            for (int k = j + 1; k < termList.Count; k++)
                            {
                                temp += TermOnterpreting.getIndexByAreaTerms(termList[j], termList[k], goodsForArea);
                            }
                        }
                        temp = temp / (double) ((termList.Count() * (termList.Count() - 1)) * 0.5);
                    }
                    result += temp;
                }
                result = result / (double) source.CountFeatures;
            }
            return result;
        }

        public static double getGISSumStraigt(this TSAFuzzySystem source, double goodsForArea = 0, int indexDataBase = 0)
        {
            double result = 0.0;
            if (goodsForArea == 0)
            {
                goodsForArea = FuzzyCore.Properties.Settings.Default.Pareto_simpler_UnionTerms_bySqarePercent * 0.01;
            }

            if (source != null)
            {
                double temp = 0.0;
                for (int i = 0; i < source.CountFeatures; i++)
                {
                    temp = 0.0;
                    List<Term> termList = source.RulesDatabaseSet[indexDataBase].TermsSet.Where(x => x.NumVar == i).ToList();
                    if (termList.Count() <= 1)
                    {
                        temp = 1.0;
                    }
                    else
                    {
                        for (int j = 0; j < termList.Count; j++)
                        {
                            for (int k = j + 1; k < termList.Count; k++)
                            {
                                temp += TermOnterpreting.getIndexByAreaTerms(termList[j], termList[k], goodsForArea);
                            }
                        }
                    }
                    result += temp;
                }
                result = result / (double) source.CountFeatures;
            }
            return result;
        }


        public static double getGISSumReverce(this TSAFuzzySystem source, double goodsForArea = 0, int indexDataBase = 0)
        {
            double result = 0.0;
            if (goodsForArea == 0)
            {
                goodsForArea = FuzzyCore.Properties.Settings.Default.Pareto_simpler_UnionTerms_bySqarePercent * 0.01;
            }
            if (source != null)
            {
                double temp = 0.0;
                for (int i = 0; i < source.CountFeatures; i++)
                {
                    temp = 0.0;
                    List<Term> termList = source.RulesDatabaseSet[indexDataBase].TermsSet.Where(x => x.NumVar == i).ToList();
                    if (termList.Count() <= 1)
                    {
                        temp = 0.0;
                    }
                    else
                    {
                        for (int j = 0; j < termList.Count; j++)
                        {
                            for (int k = j + 1; k < termList.Count; k++)
                            {
                                temp += 1 - TermOnterpreting.getIndexByAreaTerms(termList[j], termList[k], goodsForArea);
                            }
                        }
                    }
                    result += temp;
                }
                result = result / (double) source.CountFeatures;
            }
            return result;
        }


        public static double getGICNormal(this TSAFuzzySystem source, int indexDataBase = 0)
        {
            double result = 0.0;
            if (source != null)
            {
                double temp = 0.0;
                for (int i = 0; i < source.CountFeatures; i++)
                {
                    temp = 0.0;
                    List<Term> termList = source.RulesDatabaseSet[indexDataBase].TermsSet.Where(x => x.NumVar == i).ToList();
                    if (termList.Count() <= 1)
                    {
                        temp = 1.0;
                    }
                    else
                    {
                        for (int j = 0; j < termList.Count; j++)
                        {
                            for (int k = j + 1; k < termList.Count; k++)
                            {
                                temp += TermOnterpreting.getIndexByCentersClose(termList[j], termList[k], termList.Count(), source.LearnSamplesSet.InputAttributes[i].Scatter);
                            }
                        }
                        temp = temp / (double) ((termList.Count() * (termList.Count() - 1)) * 0.5);
                    }

                    result += temp;
                }
                result = result / (double) source.CountFeatures;

            }
            return result;
        }

        public static double getGICSumStraigth(this TSAFuzzySystem source, int indexDataBase = 0)
        {
            double result = 0.0;


            if (source != null)
            {
                double temp = 0.0;
                for (int i = 0; i < source.CountFeatures; i++)
                {
                    temp = 0.0;
                    List<Term> termList = source.RulesDatabaseSet[indexDataBase].TermsSet.Where(x => x.NumVar == i).ToList();
                    if (termList.Count() <= 1)
                    {
                        temp = 1.0;
                    }
                    else
                    {

                        for (int j = 0; j < termList.Count; j++)
                        {
                            for (int k = j + 1; k < termList.Count; k++)
                            {
                                temp += TermOnterpreting.getIndexByCentersClose(termList[j], termList[k], termList.Count(), source.LearnSamplesSet.InputAttributes[i].Scatter);

                            }
                        }

                    }

                    result += temp;
                }
                result = result / (double)source.CountFeatures;

            }


            return result;
        }


        public static double getGICSumReverce(this TSAFuzzySystem source, int indexDataBase = 0)
        {
            double result = 0.0;


            if (source != null)
            {
                double temp = 0;
                for (int i = 0; i < source.CountFeatures; i++)
                {
                    temp = 0.0;
                    List<Term> termList = source.RulesDatabaseSet[indexDataBase].TermsSet.Where(x => x.NumVar == i).ToList();
                    if (termList.Count() <= 1)
                    {
                        temp = 0.0;
                    }
                    else
                    {
                        for (int j = 0; j < termList.Count; j++)
                        {
                            for (int k = j + 1; k < termList.Count; k++)
                            {
                                temp += 1.0 - TermOnterpreting.getIndexByCentersClose(termList[j], termList[k], termList.Count(), source.LearnSamplesSet.InputAttributes[i].Scatter);
                            }
                        }
                    }
                    result += temp;
                }
                result = result / (double) source.CountFeatures;
            }
            return result;
        }

        public static double getLindisNormal(this TSAFuzzySystem source, int indexDataBase = 0)
        {
            double result = 0.0;
            if (source != null)
            {
                double temp = 0.0;
                for (int i = 0; i < source.CountFeatures; i++)
                {
                    temp = 0;
                    List<Term> termList = source.RulesDatabaseSet[indexDataBase].TermsSet.Where(x => x.NumVar == i).ToList();
                    if (termList.Count() <= 1)
                    {
                        temp = 1.0;
                    }
                    else
                    {

                        for (int j = 0; j < termList.Count; j++)
                        {
                            for (int k = j + 1; k < termList.Count; k++)
                            {
                                temp += TermOnterpreting.getIndexByLinds(termList[j], termList[k], termList);
                            }
                        }
                        temp = temp / (double) ((termList.Count() * (termList.Count() - 1)) * 0.5);

                    }

                    result += temp;
                }
                result = result / (double) source.CountFeatures;
            }
            return result;
        }

        public static double getLindisSumStraight(this TSAFuzzySystem source, int indexDataBase = 0)
        {
            double result = 0.0;
            if (source != null)
            {
                double temp = 0.0;
                for (int i = 0; i < source.CountFeatures; i++)
                {
                    temp = 0.0;
                    List<Term> termList = source.RulesDatabaseSet[indexDataBase].TermsSet.Where(x => x.NumVar == i).ToList();
                    if (termList.Count() <= 1)
                    {
                        temp = 1.0;
                    }
                    else
                    {

                        for (int j = 0; j < termList.Count; j++)
                        {
                            for (int k = j + 1; k < termList.Count; k++)
                            {
                                temp += TermOnterpreting.getIndexByLinds(termList[j], termList[k], termList);

                            }
                        }
                    }
                    result += temp;
                }
                result = result / (double)source.CountFeatures;
            }

            return result;
        }

        public static double getLindisSumReverse(this TSAFuzzySystem source, int indexDataBase = 0)
        {
            double result = 0.0;
            if (source != null)
            {
                double temp = 0.0;
                for (int i = 0; i < source.CountFeatures; i++)
                {
                    temp = 0.0;
                    List<Term> termList = source.RulesDatabaseSet[indexDataBase].TermsSet.Where(x => x.NumVar == i).ToList();
                    if (termList.Count() <= 1)
                    {
                        temp = 0.0;
                    }
                    else
                    {

                        for (int j = 0; j < termList.Count; j++)
                        {
                            for (int k = j + 1; k < termList.Count; k++)
                            {
                                temp += 1 - TermOnterpreting.getIndexByLinds(termList[j], termList[k], termList);
                            }
                        }
                    }
                    result += temp;
                }
                result = result / (double)source.CountFeatures;
            }
            return result;
        }

        public static double getIndexSumStraigt(this TSAFuzzySystem source, double goodsForBorder = 0, double goodsForAreas = 0, int indexDataBase = 0)
        {
            double result = 0.0;
            if (goodsForBorder == 0)
            {
                goodsForBorder = FuzzyCore.Properties.Settings.Default.Pareto_simpler_UnionTerms_byBorderPercent * 0.01;
            }
            if (goodsForAreas == 0)
            {
                goodsForAreas = FuzzyCore.Properties.Settings.Default.Pareto_simpler_UnionTerms_bySqarePercent * 0.01;
            }
            if (source != null)
            {
                double temp = 0.0;
                for (int i = 0; i < source.CountFeatures; i++)
                {
                    temp = 0.0;
                    List<Term> termList = source.RulesDatabaseSet[indexDataBase].TermsSet.Where(x => x.NumVar == i).ToList();
                    if (termList.Count() <= 1)
                    {
                        temp = 1.0;
                    }
                    else
                    {
                        for (int j = 0; j < termList.Count; j++)
                        {
                            for (int k = j + 1; k < termList.Count; k++)
                            {
                                temp += TermOnterpreting.getG3(termList[j], termList[k], termList.Count(), source.LearnSamplesSet.InputAttributes[i].Scatter, goodsForBorder, goodsForAreas) * TermOnterpreting.getIndexByLinds(termList[j], termList[k], termList);
                            }
                        }
                    }
                    result += temp;
                }
                result = result /(double) source.CountFeatures;
            }
            return result;
        }

        public static double getIndexSumReverse(this TSAFuzzySystem source, double goodsForBorder = 0, double goodsForAreas = 0, int indexDataBase = 0)
        {
            double result = 0.0;
            if (goodsForBorder == 0)
            {
                goodsForBorder = FuzzyCore.Properties.Settings.Default.Pareto_simpler_UnionTerms_byBorderPercent * 0.01;
            }
            if (goodsForAreas == 0)
            {
                goodsForAreas = FuzzyCore.Properties.Settings.Default.Pareto_simpler_UnionTerms_bySqarePercent * 0.01;
            }
            if (source != null)
            {
                double temp = 0.0;
                for (int i = 0; i < source.CountFeatures; i++)
                {
                    temp = 0.0;
                    List<Term> termList = source.RulesDatabaseSet[indexDataBase].TermsSet.Where(x => x.NumVar == i).ToList();
                    if (termList.Count() <= 1)
                    {
                        temp = 1.0;
                    }
                    else
                    {
                        for (int j = 0; j < termList.Count; j++)
                        {
                            for (int k = j + 1; k < termList.Count; k++)
                            {
                                double[] tempParts =  {(1 - TermOnterpreting.getG3(termList[j], termList[k], termList.Count(), source.LearnSamplesSet.InputAttributes[i].Scatter, goodsForBorder, goodsForAreas))
                                                           ,
                                                       (1-TermOnterpreting.getIndexByLinds(termList[j], termList[k], termList))};
                                temp += tempParts.Max();
                            }
                        }
                    }
                    result += temp;
                }
                result = result / (double) source.CountFeatures;
            }
            return result;
        }

        public static double getIndexReal(this TSAFuzzySystem source, double goodsForBorder = 0, double goodsForAreas = 0, int indexDataBase = 0)
        {
            double result = 0.0;
            if (goodsForBorder == 0)
            {
                goodsForBorder = FuzzyCore.Properties.Settings.Default.Pareto_simpler_UnionTerms_byBorderPercent * 0.01;
            }
            if (goodsForAreas == 0)
            {
                goodsForAreas = FuzzyCore.Properties.Settings.Default.Pareto_simpler_UnionTerms_bySqarePercent * 0.01;
            }
            if (source != null)
            {
                double temp = 0.0;
                for (int i = 0; i < source.CountFeatures; i++)
                {
                    temp = 0.0;
                    List<Term> termList = source.RulesDatabaseSet[indexDataBase].TermsSet.Where(x => x.NumVar == i).ToList();
                    if (termList.Count() <= 1)
                    {
                        temp = 1.0;
                    }
                    else
                    {
                        for (int j = 0; j < termList.Count; j++)
                        {
                            for (int k = j + 1; k < termList.Count; k++)
                            {
                                temp += TermOnterpreting.getG3(termList[j], termList[k], termList.Count(), source.LearnSamplesSet.InputAttributes[i].Scatter, goodsForBorder, goodsForAreas) * TermOnterpreting.getIndexByLinds(termList[j], termList[k], termList);
                            }
                        }
                        temp = temp / (double) ((termList.Count() * (termList.Count() - 1)) * 0.5);
                        temp = AlphaIndex(termList.Count(), temp);
                    }
                    result += temp;
                }
                result = result / (double) source.CountFeatures;
            }
            return result;
        }

        private static double AlphaIndex(int CountTerms, double G3Index)
        {
            double result = G3Index;
            double c = 5.0;
            double a = 4.0;
            double b = 2.0;
            if (CountTerms > c)
            {
                result *= 1.0 / (1.0 + Math.Pow(((CountTerms - c) / a), b));
            }
            return result;
        }
    }
}

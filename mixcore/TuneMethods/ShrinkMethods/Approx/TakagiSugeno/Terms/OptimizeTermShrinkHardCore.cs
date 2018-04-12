using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;

namespace FuzzySystem.TakagiSugenoApproximate.LearnAlgorithm
{
    public class OptimizeTermShrinkHardCode : AbstractNotSafeLearnAlgorithm
    {
        List<List<int>> Pull_of_systems = new List<List<int>>();
        List<KnowlegeBaseTSARules> Systems_ready_to_test = new List<KnowlegeBaseTSARules>();
        List<double> errors_of_systems = new List<double>();

        int count_shrink = 0;

        TSAFuzzySystem theFuzzySystem;

        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate };
            }
        }

        private void Generate_all_variant_in_pool(List<int> Bool_struct)
        {
            int pos = 0;
            do
            {
                Pull_of_systems.Add(new List<int>(Bool_struct));

                pos = Bool_struct.Count - 2;
                for (int i = Bool_struct.Count - 1; pos >= 0 && Bool_struct[pos] >= Bool_struct[i]; i--) pos--;
                int j = Bool_struct.Count - 1;
                while (pos >= 0 && Bool_struct[pos] >= Bool_struct[j]) j--;
                //j++;
                if (pos >= 0)
                {
                    int temp = Bool_struct[pos];
                    Bool_struct[pos] = Bool_struct[j];

                    Bool_struct[j] = temp;

                }
                int l = pos + 1, r = Bool_struct.Count - 1;
                while (l < r)
                {
                    int temp = Bool_struct[l];

                    Bool_struct[l] = Bool_struct[r];
                    Bool_struct[r] = temp;
                    l++;
                    r--;
                }
            } while (pos >= 0);




        }


        public override TSAFuzzySystem TuneUpFuzzySystem(TSAFuzzySystem Approximate, ILearnAlgorithmConf config)
        {
           theFuzzySystem = Approximate;
            if (theFuzzySystem.RulesDatabaseSet.Count == 0)
            {
                throw new System.FormatException("Что то не то с входными данными");
            }
            OptimizeTermShrinkHardCoreConf Config = config as OptimizeTermShrinkHardCoreConf;
            count_shrink = Config.OTSHCCountShrinkTerm;


            for (int i = 0; i < Approximate.CountFeatures; i++)
            {
                int count_terms_for_var = Approximate.RulesDatabaseSet[0].TermsSet.FindAll(x => x.NumVar == i).Count;

                if (count_terms_for_var >= count_shrink)
                {
                    int shrinkcounter = count_shrink;
                    List<int> Varians_of_cut = new List<int>();
                    for (int j = 0; j < count_terms_for_var; j++)
                    {
                        if (shrinkcounter > 0) Varians_of_cut.Add(0);
                        else Varians_of_cut.Add(1);
                        shrinkcounter--;
                    }
                    Generate_all_variant_in_pool(Varians_of_cut);


                    for (int j = 0; j < Pull_of_systems.Count; j++)
                    {
                        KnowlegeBaseTSARules current = MakeCut(Approximate.RulesDatabaseSet[0], Pull_of_systems[j], i);
                        Systems_ready_to_test.Add(current);
                        errors_of_systems.Add(theFuzzySystem.approxLearnSamples(current));
                    }
                    Pull_of_systems.Clear();
                }


            }

            int best_index = errors_of_systems.IndexOf(errors_of_systems.Min());
            theFuzzySystem.RulesDatabaseSet[0] = Systems_ready_to_test[best_index];

            return theFuzzySystem;

        }

        private KnowlegeBaseTSARules MakeCut(KnowlegeBaseTSARules Source, List<int> VectorCut, int NumVars)
        {

            KnowlegeBaseTSARules result = new KnowlegeBaseTSARules(Source);
            List<Term> cuttedTerms = result.TermsSet.FindAll(x => x.NumVar == NumVars);
            List<TSARule> toChangeArules;
            for (int i = 0; i < VectorCut.Count; i++)
            {
                if (VectorCut[i] == 0)
                {
                     toChangeArules = result.RulesDatabase.Where(x => x.ListTermsInRule.Contains(cuttedTerms[i])).ToList();
                    for (int j = 0; j < toChangeArules.Count(); j++)
                    {
                        toChangeArules[j].ListTermsInRule.Remove(cuttedTerms[i]);
                    }
                    result.TermsSet.Remove(cuttedTerms[i]);
                    
                 
                }
            }


         RemoveDuplicate(theFuzzySystem, result.RulesDatabase);
            return result;
        }


        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "отсечение термов с удалением из правил {";
                result += "На сколько уменьшать термов для одного параметра =" + this.count_shrink.ToString() + " ; " + Environment.NewLine;
                result += "}";
                return result;
            }
            return "отсечение термов с удалением из правил";
        }

        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new OptimizeTermShrinkHardCoreConf();
            result.Init(CountFeatures);
            return result;
        }


        private List<TSARule> RemoveDuplicate(TSAFuzzySystem Approximate ,List<TSARule> Source)
        {
            List<TSARule> result = Source;
            for (int i = Source.Count - 1; i > 0; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (Equals(Source[i], Source[j]))
                    {
                        Source.RemoveAt(i);
                        double[] coeffs = null;
                        double coeff = LSMWeghtReqursiveSimple.EvaluteConsiquent(Approximate, Source[j].ListTermsInRule, out coeffs);
                     //   Source[j].IndependentConstantConsequent = KNNConsequent.NearestApprox( Approximate,Source[j].Term_of_Rule_Set);
                        Source[j].RegressionConstantConsequent = coeffs;
                        break;
                    }
                }

            }
                return result;
        }

      

            public bool Equals(TSARule x, TSARule y)
            {

                if (x.ListTermsInRule.Count == y.ListTermsInRule.Count)
                {

                    bool result = true;
                    for (int i = 0; i < x.ListTermsInRule.Count;i++ )
                    {
                        if (x.ListTermsInRule[i] != y.ListTermsInRule[i])
                        {
                            return false; 
                        }
                    }
                    return result;
                }
                return false;
            }

        



    }
}

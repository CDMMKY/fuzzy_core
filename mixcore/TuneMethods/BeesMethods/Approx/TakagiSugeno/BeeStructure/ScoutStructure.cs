#define TSApprox
#if PClass
using KnowlegeBaseRules = FuzzySystem.PittsburghClassifier.KnowlegeBasePCRules;
using Rule = FuzzySystem.PittsburghClassifier.PCRule;
using FS = FuzzySystem.PittsburghClassifier.PCFuzzySystem;
#elif SApprox
using KnowlegeBaseRules = FuzzySystem.SingletoneApproximate.KnowlegeBaseSARules;
using Rule = FuzzySystem.SingletoneApproximate.SARule;
using FS = FuzzySystem.SingletoneApproximate.SAFuzzySystem;
#elif TSApprox
using KnowlegeBaseRules =  FuzzySystem.TakagiSugenoApproximate.KnowlegeBaseTSARules;
using Rule = FuzzySystem.TakagiSugenoApproximate.TSARule;
using FS = FuzzySystem.TakagiSugenoApproximate.TSAFuzzySystem;
#else
using KnowlegeBaseRules = FuzzySystem.SingletoneApproximate.KnowlegeBaseSARules;
using Rule = FuzzySystem.SingletoneApproximate.SARule;
using FS = FuzzySystem.SingletoneApproximate.SAFuzzySystem;
#endif



using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySystem.FuzzyAbstract;


namespace FuzzySystem.TakagiSugenoApproximate.LearnAlgorithm.Bee
{
    public class Scout : Bee
    {
       
        int[] numOfTerms;
        int numoFRules;
        Rule theRule;
        public int NumOFRule { get { return numoFRules; } }
        List<Term> forClass = new List<Term>();

        public Scout(KnowlegeBaseRules theSource, FS parrent)
             : base(theSource, parrent)
        {
        }


        public void generateNewRule(TypeTermFuncEnum termType, Random rand)
        {
            int CountVaribles = Parrent.CountFeatures;
            forClass.Clear();
            numOfTerms = new int[CountVaribles];
            for (int i = 0; i < CountVaribles; i++)
            {
                double min = Parrent.LearnSamplesSet.InputAttributes[i].Min;
            double max = Parrent.LearnSamplesSet.InputAttributes[i].Max;
            double Scatter = Parrent.LearnSamplesSet.InputAttributes[i].Scatter;
            double[] Params = null;
            Term newTerm = null;
           

                switch (termType)
                {
                    case TypeTermFuncEnum.Гауссоида: { Params = generateGauss(min, max, Scatter, rand); } break;
                    case TypeTermFuncEnum.Парабола: { Params = generateParabolic(min, max, Scatter, rand); } break;
                    case TypeTermFuncEnum.Трапеция:
                        {
                            Params = generateTrapec(min, max, Scatter, rand);
                            Params = inValidateTrapec(Params, min, max, Scatter, rand);
                        }
                        break;
                    case TypeTermFuncEnum.Треугольник:
                        {
                            Params = generateTrianlge(min, max, Scatter, rand);
                            Params = inValidateTriangle(Params, min, max, Scatter, rand);
                        }
                        break;
                }

                newTerm = new Term(Params, termType, i);
                forClass.Add(newTerm);
                thePositionOfBee.TermsSet.Add(newTerm);
                numOfTerms[i] = thePositionOfBee.TermsSet.Count() - 1;
            }

            makeKons(Parrent, forClass);
            thePositionOfBee.RulesDatabase.Add(theRule);
            numoFRules = thePositionOfBee.RulesDatabase.Count - 1;

        }

        void makeKons( FS CurrentFS, List<Term> forClass)
        {

#if PClass
string Kons = FuzzySystem.PittsburghClassifier.KNNClassName.NearestClass(CurrentFS, forClass);
theRule = new Rule(thePositionOfBee.TermsSet, numOfTerms, Kons);
            
#elif SApprox
double Kons = FuzzySystem.SingletoneApproximate.KNNConsequent.NearestApprox(CurrentFS, forClass);
theRule = new Rule(thePositionOfBee.TermsSet, numOfTerms, Kons);
#elif TSApprox
 double[] koefs;
 double val =  FuzzySystem.TakagiSugenoApproximate.LSMWeghtReqursiveSimple.EvaluteConsiquent (CurrentFS, forClass, out koefs);
theRule = new Rule(thePositionOfBee.TermsSet, numOfTerms, val,koefs);

#else
            double Kons = FuzzySystem.SingletoneApproximate.KNNConsequent.NearestApprox(CurrentFS, forClass);
            theRule = new Rule(thePositionOfBee.TermsSet, numOfTerms, Kons);
#endif
          
            }

       


  

    }
}

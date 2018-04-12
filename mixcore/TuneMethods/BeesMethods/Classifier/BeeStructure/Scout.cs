/*
using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySystem.FuzzyAbstract;
namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm.Bee
{
    public class Scout : Bee
    {
        int[] numOfTerms;
        int numoFRules;
        PCRule theRule;
        public int NumOFRule { get { return numoFRules; } }
        List<Term> forClass = new List<Term>();

       public Scout(KnowlegeBasePCRules theSource, BeeStructureAlgorithm parrent)
            : base(theSource, parrent)
        { 
        }
        

        public void generateNewRule(TypeTermFuncEnum termType, Random rand)
        {
            int CountVaribles = Parrent.getCurrentNs().CountVars;
            forClass.Clear();
            numOfTerms = new int[CountVaribles];
            for (int i = 0; i < CountVaribles; i++)
            {
                double min = Parrent.getCurrentNs().LearnSamplesSet.InputAttributes[i].Min;
                double max = Parrent.getCurrentNs().LearnSamplesSet.InputAttributes[i].Max;
                double Scatter = Parrent.getCurrentNs().LearnSamplesSet.InputAttributes[i].Scatter;
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

            string Kons = KNNClassName.NearestClass(Parrent.getCurrentNs(), forClass);
            theRule = new PCRule(thePositionOfBee.TermsSet, numOfTerms, Kons);
            thePositionOfBee.RulesDatabase.Add(theRule);
            numoFRules = thePositionOfBee.RulesDatabase.Count - 1;

        }

    }
}


*/
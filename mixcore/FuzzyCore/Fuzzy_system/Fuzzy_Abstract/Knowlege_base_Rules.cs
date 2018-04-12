#define CONTRACTS_FULL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FuzzySystem.FuzzyAbstract
{
   abstract public class  KnowlegeBaseRules:Object
    {
        /*     public KnowlegeBaseRules(KnowlegeBaseRules source, List<bool> used_rules = null)
             { 
             }
          */



        /* public KnowlegeBaseRules()
              {

              }
              */

        public TermSetGlobal<Term> TermsSet { get; set; } = new TermSetGlobal<Term>();

    public abstract void TrimTerms();

        public void ShrinkNotAcceptedFeatures(bool [] AcceptedFeatures)
        { for (Int64 i =AcceptedFeatures.LongLength-1; i>=0; i--)
                { if (AcceptedFeatures[i] == false)
                {
                  var delList= TermsSet.AsParallel() .Where(x => x.NumVar == i).ToList();
                    delList.AsParallel().ForAll(y =>TermsSet.Remove(y));
                }
            }
        }

    }
}

using FuzzySystem.FuzzyAbstract;
using FuzzySystem.TakagiSugenoApproximate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLI
{
    public class FlexyKLI : KLI2
    {

        ScoreListElem Best;

        public override FuzzySystem.TakagiSugenoApproximate.TSAFuzzySystem Generate(FuzzySystem.TakagiSugenoApproximate.TSAFuzzySystem Approximate, FuzzySystem.FuzzyAbstract.conf.IGeneratorConf config)
        {
            TSAFuzzySystem result = Approximate;
            type_func = TypeTermFuncEnum.Гауссоида;

            ResultSystem = Approximate;

            var kliConf = config as KLI_conf;
            if (kliConf != null)
            {
                double meanValue = result.LearnSamplesSet.DataRows.Select(x => x.DoubleOutput).Average();
                var mayError = kliConf.MaxValue * meanValue;


                double centerValue = mayError;
         
                ScoreListElem[] ResultsKLI = new ScoreListElem[3];
                ScoreListElem[] ResultsKLI2 = new ScoreListElem[3];
                List<ScoreListElem> ResultsKLIFull = new List<ScoreListElem>();
                List<ScoreListElem> ResultsKLI2Full = new List<ScoreListElem>();
                double magic = 0.25;
                KLI.kliGenerate(result, type_func, centerValue);
                ScoreListElem tempRes = new ScoreListElem();
                tempRes.MaxError = centerValue;
                tempRes.Alg = "Кли";
                tempRes.Res = result.RulesDatabaseSet[0];
                tempRes.Error = result.ErrorLearnSamples(tempRes.Res);
                ResultsKLI[1] = new ScoreListElem(tempRes);
                if (!double.IsNaN(tempRes.Error))
                    ResultsKLIFull.Add( new ScoreListElem(tempRes));
           
                for (int i = 0; i < 100; i++)
                {
                    double leftValue = centerValue * (1- magic);
                    double rigthValue = centerValue * (1 + magic);

                    tempRes = new ScoreListElem();
                    KLI.kliGenerate(result, type_func, leftValue);
                    tempRes.Alg = "Кли";
                    tempRes.MaxError = leftValue;
                    tempRes.Res = result.RulesDatabaseSet[0];
                    tempRes.Error = result.ErrorLearnSamples(tempRes.Res);
                    ResultsKLI[0] = new ScoreListElem(tempRes);
                    if (!double.IsNaN(tempRes.Error))
                        ResultsKLIFull.Add(new ScoreListElem(tempRes));
                
                    result.RulesDatabaseSet.Clear();

                    tempRes = new ScoreListElem();
                    KLI.kliGenerate(result, type_func, rigthValue);
                    tempRes.Alg = "Кли";
                    tempRes.MaxError = rigthValue;
                    tempRes.Res = result.RulesDatabaseSet[0];
                    tempRes.Error = result.ErrorLearnSamples(tempRes.Res);
                    ResultsKLI[2] = tempRes;
                    if (!double.IsNaN(tempRes.Error))
                        ResultsKLIFull.Add(new ScoreListElem(tempRes));
                
                    result.RulesDatabaseSet.Clear();

                    if (((ResultsKLI[0].Error >= ResultsKLI[1].Error)||double.IsNaN(ResultsKLI[0].Error)) && ((ResultsKLI[1].Error <= ResultsKLI[2].Error) ||(double.IsNaN(ResultsKLI[2].Error))))
                    {
                        magic *= 0.7;
                    }
                    double err= ResultsKLIFull.Min(y => y.Error);
                    ResultsKLI[1].Error = err;
                            centerValue = ResultsKLIFull.First(x=>x.Error==err) .MaxError;
                
                }

                centerValue = mayError;
                tempRes = new ScoreListElem();
                kliGenerate(result, type_func, centerValue);
                tempRes.Alg = "Кли2";
                tempRes.MaxError = centerValue;
                tempRes.Res = result.RulesDatabaseSet[0];
                tempRes.Error = result.ErrorLearnSamples(tempRes.Res);
                ResultsKLI2[1] = new ScoreListElem(tempRes);
                if (!double.IsNaN(tempRes.Error))
                    ResultsKLI2Full.Add(new ScoreListElem(tempRes));
                result.RulesDatabaseSet.Clear();

                magic = 0.25;
                for (int i = 0; i < 100; i++)
                {
                    double leftValue = centerValue * (1-magic);
                    double rigthValue = centerValue * (1+magic);
    

                    tempRes = new ScoreListElem();
                    kliGenerate(result, type_func, leftValue);
                    tempRes.Alg = "Кли2";
                    tempRes.MaxError = leftValue;
                    tempRes.Res = result.RulesDatabaseSet[0];
                    tempRes.Error = result.ErrorLearnSamples(tempRes.Res);
                    ResultsKLI2[0] = new ScoreListElem(tempRes);
                    if (!double.IsNaN(tempRes.Error))
                        ResultsKLI2Full.Add(new ScoreListElem(tempRes));
               
                    result.RulesDatabaseSet.Clear();

                    tempRes = new ScoreListElem();
                    kliGenerate(result, type_func, rigthValue);
                    tempRes.Alg = "Кли2";
                    tempRes.MaxError = rigthValue;
                    tempRes.Res = result.RulesDatabaseSet[0];
                    tempRes.Error = result.ErrorLearnSamples(tempRes.Res);
                    ResultsKLI2[2] =new ScoreListElem (tempRes);
                    if (!double.IsNaN(tempRes.Error))
                        ResultsKLI2Full.Add(new ScoreListElem(tempRes));
               
                    result.RulesDatabaseSet.Clear();

                      if (((ResultsKLI2[0].Error >= ResultsKLI2[1].Error)||double.IsNaN(ResultsKLI2[0].Error)) && ((ResultsKLI2[1].Error <= ResultsKLI2[2].Error) ||(double.IsNaN(ResultsKLI2[2].Error))))
                  {
                        magic *= 0.7;
                    }
                      double err = ResultsKLI2Full.Min(y => y.Error);
                      ResultsKLI2[1].Error = err;
                      centerValue = ResultsKLI2Full.First(x => x.Error == err).MaxError;
 
             
                   
                }


                ScoreListElem BestKli = ResultsKLIFull.Where(x => x.Error == ResultsKLIFull.Min(y => y.Error)).First();
                ScoreListElem BestKli2 = ResultsKLI2Full.Where(x => x.Error == ResultsKLI2Full.Min(y => y.Error)).First();

                if (BestKli.Error > BestKli2.Error)
                {
                    Best = BestKli2;
                }
                else { Best = BestKli; }


                result.RulesDatabaseSet.Add( Best.Res);

            }
            return result;
        }

        public override string ToString(bool with_param = false)
        {
            return with_param ? @"КЛИ подбор параметров: Тип алгоритма = " + Best.Alg + Environment.NewLine + "Допустимое значение ошибки = " + Best.Error + Environment.NewLine : "КЛИ подбор параметров";
 
        }


        public class ScoreListElem
        {
            public string Alg { get; set; }
            public KnowlegeBaseTSARules Res { get; set; }
            public double MaxError { get; set; }
            public double Error { get; set; }
            public ScoreListElem()
            { }
          public  ScoreListElem (ScoreListElem Source)
            { 
           
              Alg = Source.Alg.Clone() as string;
              Res = new KnowlegeBaseTSARules(Source.Res);
              MaxError = Source.MaxError;
              Error = Source.Error;
       }
        }

    }
}

using System;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm.Term_config_Aco
{
    public class Modified_ACO:Base_ACO
    {
        protected double MACOCurrentError;
        protected int MACOCountRepeatError;
        protected int MACOCountBorderRepeat;
        protected int MACOCountEliteDecision;
       

   
        protected override void init(PCFuzzySystem Classifier, ACOSearchConf config)
        {
            base.init(Classifier, config);
            MACOSearchConf configNew = config as MACOSearchConf;
            MACOCountBorderRepeat = configNew.MACOCountExtremum;
            MACOCountRepeatError = 0;
            MACOCurrentError = getPrecission();
            MACOCountEliteDecision = configNew.MACOCountElite;
        }

        public override void oneIterate(PCFuzzySystem result)
        {
            base.oneIterate(result);
            if (isInExtremum())
            {
                foreach (Colony colony in colonyList)    //Шаг **. Обновляем архивы, заполняем их случайными решениями и элитными 
                {
                    colony.refillDesicionArchive(MACOCountEliteDecision - 1, rand, this);
                }
            }
        }

        protected virtual bool isInExtremum()
        { double currentError = getPrecission();
        if (MACOCurrentError == currentError)
        {
            MACOCountRepeatError++;
            if (MACOCountRepeatError > MACOCountBorderRepeat)
            {
                MACOCountRepeatError = 0;
                return true;
            }
            return false;
        }
        MACOCurrentError = currentError;
        return false;
        }


        public override string ToString(bool with_param = false)
        {

            if (with_param)
            {
                string result = " Модифицированный алгоритм муравьиной колонии {";
                result += "Итераций= " + ACO_iterationCount.ToString() + " ;" + Environment.NewLine;
                result += "Количество муравьев= " + ACO_antCount.ToString() + " ;" + Environment.NewLine;
                result += "Размер архива решений= " + ACO_decisionArchiveCount.ToString() + " ;" + Environment.NewLine;
                result += "Коэффицент q= " + ACO_q.ToString() + " ;" + Environment.NewLine;
                result += "Коэффицент xi= " + ACO_xi.ToString() + " ;" + Environment.NewLine;
                result += "Количество колоний= " + colonyCount.ToString() + " ;" + Environment.NewLine;
                result += "}";
                return result;
            }
            return "Модифицированный алгоритм муравьиной колонии";
        }
    }
    }


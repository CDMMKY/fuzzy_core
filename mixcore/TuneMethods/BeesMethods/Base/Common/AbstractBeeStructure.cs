using System;
using System.Collections.Generic;

namespace BeesMethods.Base.Common
{
    abstract public class AbstractBeeStructure
    {
        virtual public double Goods { get; set; }

        protected double[] inValidateTriangle(double[] Params, double min, double max, double Scatter, Random rand)
        {
            double[] result = Params.Clone() as double[];
            if (Params[1] < Params[0])
            {
                result[1] = Params[0];
                result[0] = Params[1];

            }

            if (Params[1] > Params[2])
            {
                result[2] = Params[1];
                result[1] = Params[2];
            }


            if (result[0] < min)
            {
                result[0] = min - Scatter * 0.05;
            }
            if (result[2] > max)
            {
                result[2] = max + Scatter * 0.05;

            }

            if ((result[1] > result[2]) || (result[1] < result[0]))
            {
                result[1] = result[0] + (result[2] - result[0]) * rand.NextDouble();
            }

            return result;

        }



        protected double[] inValidateTrapec(double[] Params, double min, double max, double Scatter, Random rand)
        {
            double[] result = Params.Clone() as double[];
            if (Params[1] < Params[0])
            {
                result[1] = Params[0];
                result[0] = Params[1];

            }

            if (Params[1] > Params[2])
            {
                result[2] = Params[1];
                result[1] = Params[2];
            }

            if (Params[2] > Params[3])
            {
                result[3] = Params[2];
                result[2] = Params[3];
            }



            if (result[0] < min)
            {
                result[0] = min - Scatter * 0.05;
            }
            if (result[3] > max)
            {
                result[3] = max + Scatter * 0.05;

            }

            if ((result[1] > result[2]) || (result[1] < result[0]))
            {
                result[1] = result[0] + (result[2] - result[0]) * rand.NextDouble();
            }


            if ((result[2] > result[3]) || (result[2] < result[0]))
            {
                result[1] = result[1] + (result[3] - result[1]) * rand.NextDouble();
            }


            return result;

        }


        protected double[] generateTrianlge(double min, double max, double Scatter, Random rand)
        {
            double[] Params = new double[3];
            int Choose = rand.Next(4);
            Params[1] = min + (Scatter) * rand.NextDouble();
            switch (Choose)
            {
                case 0:
                    {
                        Params[0] = min - 1 / 10 * (Scatter) - (Params[1] - min) * rand.NextDouble();
                        Params[2] = max + 1 / 10 * (Scatter) - (max - Params[1]) * rand.NextDouble();
                    }
                    break;
                case 1:
                    {
                        Params[0] = min - 1 / 10 * (Scatter) - (Params[1] - min) * rand.NextDouble();
                        Params[2] = max + 1 / 10 * (Scatter) + (max - Params[1]) * rand.NextDouble();
                    }
                    break;
                case 2:
                    {
                        Params[0] = min - 1 / 10 * (Scatter) + (Params[1] - min) * rand.NextDouble();
                        Params[2] = max + 1 / 10 * (Scatter) - (max - Params[1]) * rand.NextDouble();
                    }
                    break;
                case 3:
                    {
                        Params[0] = min - 1 / 10 * (Scatter) + (Params[1] - min) * rand.NextDouble();
                        Params[2] = max + 1 / 10 * (Scatter) + (max - Params[1]) * rand.NextDouble();
                    }
                    break;
                default: { Console.Write("{0}", Choose); } break;
            }
            return Params;
        }


        protected double[] generateGauss(double min, double max, double Scatter, Random rand)
        {
            double[] Params = new double[2];
            Params[0] = min + (Scatter) * rand.NextDouble();
            Params[1] = (Scatter) * rand.NextDouble() / 2;

            return Params;

        }


        protected double[] generateParabolic(double min, double max, double Scatter, Random rand)
        {
            double[] Params = new double[2];
            Params[0] = min + (Scatter) * rand.NextDouble();
            Params[1] = max - (Scatter) * rand.NextDouble();
            if (Params[0] > Params[1])
            {

                double Temp = Params[1];
                Params[1] = Params[0];
                Params[0] = Temp;

            }
            return Params;
        }

        protected double[] generateTrapec(double min, double max, double Scatter, Random rand)
        {
            double[] Params = new double[4];
            int Choose = rand.Next(4);
            Params[1] = min + (Scatter) * rand.NextDouble();
            Params[2] = min + (Scatter) * rand.NextDouble();
            if (Params[1] > Params[2])
            {
                double Temp = Params[2];
                Params[2] = Params[1];
                Params[1] = Temp;
            }
            switch (Choose)
            {
                case 0:
                    {
                        Params[0] = min - 1 / 10 * (Scatter) - (Params[1] - min) * rand.NextDouble();
                        Params[3] = max + 1 / 10 * (Scatter) - (max - Params[2]) * rand.NextDouble();
                    }
                    break;
                case 1:
                    {
                        Params[0] = min - 1 / 10 * (Scatter) - (Params[1] - min) * rand.NextDouble();
                        Params[3] = max + 1 / 10 * (Scatter) + (max - Params[2]) * rand.NextDouble();
                    }
                    break;
                case 2:
                    {
                        Params[0] = min - 1 / 10 * (Scatter) + (Params[1] - min) * rand.NextDouble();
                        Params[3] = max + 1 / 10 * (Scatter) - (max - Params[2]) * rand.NextDouble();
                    }
                    break;
                case 3:
                    {
                        Params[0] = min - 1 / 10 * (Scatter) + (Params[1] - min) * rand.NextDouble();
                        Params[3] = max + 1 / 10 * (Scatter) + (max - Params[2]) * rand.NextDouble();
                    }
                    break;
                default: { Console.Write("{0}", Choose); } break;
            }
            return Params;
        }

    }


    public class BeeComparer : IComparer<AbstractBeeStructure>
    {
        public int Compare(AbstractBeeStructure s1, AbstractBeeStructure s2)
        {
            if (s1.Goods > s2.Goods) { return 1; }
            if (s1.Goods < s2.Goods) { return -1; }

            return 0;
        }
    }
}

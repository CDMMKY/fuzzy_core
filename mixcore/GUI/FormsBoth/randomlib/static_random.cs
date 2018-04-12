using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class GaussRandom
    {

        public static double Random_gaussian(Random rand,double mean=0, double stdev=1 )
        {
            double u1;
            double u2;
            double w = 0;

            do
            {
                u1 = 2 * rand.NextDouble() - 1;
                u2 = 2 * rand.NextDouble() - 1;
                w = u1 * u1 + u2 * u2;
            } while (w >= 1);
            w = Math.Sqrt((-2 * Math.Log(w)) / w);

            return mean + (u2 * w) * stdev;

        }




    }
}

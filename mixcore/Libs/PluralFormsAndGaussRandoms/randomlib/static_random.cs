using System.Threading;

namespace System
{
    public static class GaussRandom

    {
        public static double Random_gaussian(Random rand, double mu = 0, double sigma = 1)
        {
            double dSumm = 0, dRandValue = 0;

            for (int i = 0; i < 12; i++)
            {
                double R = rand.NextDouble();
                dSumm = dSumm + R;
            }
            dRandValue = Math.Round((mu + sigma * (dSumm - 6)), 3);
            return dRandValue;

        }
    }

    public static class StaticRandom

    {
    

       
            static int seed = Environment.TickCount;

            static readonly ThreadLocal<Random> random =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

            public static int Next(int x)
            {
                return random.Value.Next(x);
            }
            public static double NextDouble(double a, double b)
            {
                double x = Math.Abs(a - b);
                double y = random.Value.NextDouble();
                return (random.Value.NextDouble() * Math.Abs(a - b) + a);
            }


        public static double Random_gaussian( double mean = 0, double stdev = 1)
        {
           

            return GaussRandom.Random_gaussian(random.Value, mean,stdev);

        }







    }
}

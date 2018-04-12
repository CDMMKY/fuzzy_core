namespace RecursiveLeastSquares.Base
{
    using System;

    public class HyperVector
    {
        public Vector[] Elements;

        public HyperVector(int length, int elementLength)
        {
            Elements = new Vector[length];
            for (int i = 0; i < Elements.Length; i++)
            {
                Elements[i] = new Vector(elementLength);
            }
        }

        public HyperVector(double[][] a)
        {
            Elements = new Vector[a.Length];
            for (int i = 0; i < a.Length; i++)
                Elements[i] = new Vector(a[i]);
        }

        public HyperVector(HyperVector a)
        {
            a.Elements.CopyTo(Elements, 0);
        }

        /// <summary>
        /// Finds vectors' sum
        /// </summary>
        /// <param name="a">1st vector</param>
        /// <param name="b">2nd vector</param>
        /// <returns>Vectors' sum</returns>
        public static HyperVector operator +(HyperVector a, HyperVector b)
        {
            if (a.Elements.Length != b.Elements.Length)
                throw new ArgumentException("HyperVectors must have the same length!");

            HyperVector c = new HyperVector(a.Elements.Length, a.Elements[0].Elements.Length);

            for (int i = 0; i < a.Elements.Length; i++)
                c.Elements[i] = a.Elements[i] + b.Elements[i];

            return c;
        }

        /// <summary>
        /// Multiply vector on constant
        /// </summary>
        /// <param name="a">The vector</param>
        /// <param name="b">The constant</param>
        /// <returns>vector a * b</returns>
        public static HyperVector operator *(HyperVector a, double b)
        {
            HyperVector c = new HyperVector(a.Elements.Length, a.Elements[0].Elements.Length);

            // Multiplication
            for (int i = 0; i < a.Elements.Length; i++)
                c.Elements[i] = a.Elements[i] * b;

            return c;
        }

        public static HyperVector operator *(double a, HyperVector b)
        {
            return b * a;
        }

        /// <summary>
        /// Vector multiply
        /// </summary>
        /// <param name="a">1st Vector</param>
        /// <param name="b">2nd Vector</param>
        /// <returns>Vector a * b</returns>
        public static double operator *(HyperVector a, HyperVector b)
        {
            // Dimentionality check
            if (a.Elements.Length != b.Elements.Length)
                throw new ArgumentException("Dimension mismatch for HyperVector multiplication!");

            // Multiplication
            double answer = 0;
            for (int i = 0; i < a.Elements.Length; i++)
            {
                answer += a.Elements[i] * b.Elements[i];
            }

            return answer;
        }

        public static HyperVector operator *(HyperVector a, Matrix b)
        {
            // Dimentionality check
            if (a.Elements.Length != b.Elements.Length)
                throw new ArgumentException("Dimension mismatch for HyperVactor*Matrix multiplication!");

            // Multiplication
            int m = a.Elements.Length;
            int n = a.Elements[0].Elements.Length;
            HyperVector answer = new HyperVector(m, n);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                    answer.Elements[i].Elements[j] = 0;
                for (int j = 0; j < m; j++)
                {
                    answer.Elements[i] += a.Elements[j] * b.Elements[j][i];
                }
            }

            return answer;
        }

        public static HyperVector operator *(Matrix a, HyperVector b)
        {
            // Dimentionality check
            if (a.Elements[0].Length != b.Elements.Length)
                throw new ArgumentException("Dimension mismatch for Matrix*HyperVector multiplication!");

            // Multiplication
            int m = b.Elements.Length;
            int n = b.Elements[0].Elements.Length;
            HyperVector answer = new HyperVector(m, n);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                    answer.Elements[i].Elements[j] = 0;
                for (int j = 0; j < m; j++)
                {
                    answer.Elements[i] += a.Elements[i][j] * b.Elements[j];
                }
            }

            return answer;
        }

        public static Matrix operator ^(HyperVector a, HyperVector b)
        {
            if (a.Elements.Length != b.Elements.Length)
                throw new ArgumentException("HyperVectors must have the same length!");

            double[][] answer = new double[a.Elements.Length][];

            for (int i = 0; i < a.Elements.Length; i++)
            {
                answer[i] = new double[a.Elements.Length];
                for (int j = 0; j < a.Elements.Length; j++)
                    answer[i][j] = a.Elements[i] * b.Elements[j];
            }

            return new Matrix(answer);
        }
    }
}

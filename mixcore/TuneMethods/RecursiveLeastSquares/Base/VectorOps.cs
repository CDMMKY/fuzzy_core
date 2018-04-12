namespace RecursiveLeastSquares.Base
{
    using System;

    public class Vector
    {
        public double[] Elements;

        public Vector(int length)
        {
            Elements = new double[length];
        }

        public Vector(double[] a)
        {
            Elements = new double[a.Length];
            a.CopyTo(Elements, 0);
        }

        public Vector(Vector a)
        {
            Elements = a.Elements;
        }

        /// <summary>
        /// Finds vectors' sum
        /// </summary>
        /// <param name="a">1st vector</param>
        /// <param name="b">2nd vector</param>
        /// <returns>Vectors' sum</returns>
        public static Vector operator +(Vector a, Vector b)
        {
            if (a.Elements.Length != b.Elements.Length)
                throw new ArgumentException("Vectors must have the same length!");

            Vector c = new Vector(a.Elements.Length);

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
        public static Vector operator *(Vector a, double b)
        {
            Vector c = new Vector(a.Elements.Length);

            // Multiplication
            for (int i = 0; i < a.Elements.Length; i++)
                c.Elements[i] = a.Elements[i] * b;

            return c;
        }

        public static Vector operator *(double a, Vector b)
        {
            return b * a;
        }

        /// <summary>
        /// Vector multiply
        /// </summary>
        /// <param name="a">1st Vector</param>
        /// <param name="b">2nd Vector</param>
        /// <returns>Vector a * b</returns>
        public static double operator *(Vector a, Vector b)
        {
            // Dimentionality check
            if (a.Elements.Length != b.Elements.Length)
                throw new ArgumentException("Dimension mismatch for vector multiplication!");

            // Multiplication
            double answer = 0;
            for (int i = 0; i < a.Elements.Length; i++)
            {
                answer += a.Elements[i] * b.Elements[i];
            }

            return answer;
        }

        public static Vector operator *(Vector a, Matrix b)
        {
            // Dimentionality check
            if (a.Elements.Length != b.Elements.Length)
                throw new ArgumentException("Dimension mismatch for matrix multiplication!");

            // Multiplication
            double[] answer = new double[a.Elements.Length];
            for (int i = 0; i < a.Elements.Length; i++)
            {
                answer[i] = 0;
                for (int j = 0; j < b.Elements[0].Length; j++)
                {
                    answer[i] += a.Elements[i] * b.Elements[j][i];
                }
            }

            return new Vector(answer);
        }

        public static Vector operator *(Matrix a, Vector b)
        {
            // Dimentionality check
            if (a.Elements[0].Length != b.Elements.Length)
                throw new ArgumentException("Dimension mismatch for matrix multiplication!");

            // Multiplication
            double[] answer = new double[b.Elements.Length];
            for (int i = 0; i < b.Elements.Length; i++)
            {
                answer[i] = 0;
                for (int j = 0; j < a.Elements[0].Length; j++)
                {
                    answer[i] += a.Elements[i][j] * b.Elements[j];
                }
            }

            return new Vector(answer);
        }

        /// <summary>
        /// Evaluates vector's length
        /// </summary>
        /// <param name="a">The vector</param>
        /// <returns>Length of the vector</returns>
        public static double VectorLength(Vector a)
        {
            return Math.Sqrt(a * a);
        }

        /// <summary>
        /// Evaluates vector's distance
        /// </summary>
        /// <param name="a">The vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>Length of the vector</returns>
        public static double VectorDistance(Vector a, Vector b)
        {
            double r = 0;
            for (int i = 0; i < a.Elements.Length; i++)
            {
                r += (a.Elements[i] - b.Elements[i]) * (a.Elements[i] - b.Elements[i]);
            }
            return Math.Sqrt(r);
        }
    }
}
namespace RecursiveLeastSquares.Base
{
    using System;

    public class Matrix
    {
        public double[][] Elements;

        public Matrix(double[][] a)
        {
            if (!IsMatrix(a))
                throw new ArgumentException("Argument must be a proper matrix!");
            
            Elements = new double[a.Length][];
            for (int i = 0; i < a.Length; i++)
            {
                Elements[i] = new double[a[0].Length];
            }

            a.CopyTo(Elements, 0);
        }

        public Matrix(double[] a, bool shouldBeHorizontal)
        {
            if (shouldBeHorizontal)
            {
                Elements = new double[1][];
                Elements[0] = new double[a.Length];
                a.CopyTo(Elements[0], 0);
            }
            else
            {
                Elements = new double[a.Length][];
                for (int i = 0; i < a.Length; i++)
                {
                    Elements[i] = new double[1];
                    Elements[i][0] = a[i];
                }
            }
        }

        public Matrix(Vector a, bool shouldBeHorizontal)
        {
            if (shouldBeHorizontal)
            {
                Elements = new double[1][];
                Elements[0] = new double[a.Elements.Length];
                a.Elements.CopyTo(Elements[0], 0);
            }
            else
            {
                Elements = new double[a.Elements.Length][];
                for (int i = 0; i < a.Elements.Length; i++)
                {
                    Elements[i] = new double[1];
                    Elements[i][0] = a.Elements[i];
                }
            }
        }

        public int RowsCount
        {
            get { return Elements.Length; }
        }

        public int ColumsCount
        {
            get { return Elements[0].Length; }
        }

        /// <summary>
        /// Check if the array is matrix
        /// </summary>
        /// <param name="a">Array to check</param>
        /// <returns>True if matrix and false otherwise</returns>
        public static bool IsMatrix(double[][] a)
        {
            for (int i = 1; i < a.Length; i++)
            {
                if (a[i].Length != a[0].Length)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Matrix addition
        /// </summary>
        /// <param name="a">1st matrix</param>
        /// <param name="b">2nd matrix b</param>
        /// <returns>matrix a + b</returns>
        public static Matrix operator +(Matrix a, Matrix b)
        {
            // Dimentionality check
            if (a.RowsCount != b.RowsCount || a.ColumsCount != b.ColumsCount)
                throw new ArgumentException("Dimension mismatch: matrices should have the same number of dimensions!");
            
            // Addition
            double[][] answer = new double[a.RowsCount][];
            for (int i = 0; i < a.RowsCount; i++)
            {
                answer[i] = new double[a.ColumsCount];
                for (int j = 0; j < a.ColumsCount; j++)
                {
                    answer[i][j] = a.Elements[i][j] + b.Elements[i][j];
                }
            }

            return new Matrix(answer);
        }

        /// <summary>
        /// Matrix multiply
        /// </summary>
        /// <param name="a">1st matrix</param>
        /// <param name="b">2nd matrix</param>
        /// <returns>matrix a * b</returns>
        public static Matrix operator *(Matrix a, Matrix b)
        {
            // Dimentionality check
            if (a.ColumsCount != b.RowsCount)
                throw new ArgumentException("Dimension mismatch for matrix multiplication!");

            // Multiplication
            double[][] answer = new double[a.RowsCount][];
            for (int i = 0; i < a.RowsCount; i++)
            {
                answer[i] = new double[b.ColumsCount];
                for (int j = 0; j < b.ColumsCount; j++)
                {
                    answer[i][j] = 0;
                    for (int k = 0; k < a.ColumsCount; k++)
                        answer[i][j] += a.Elements[i][k] * b.Elements[k][j];
                }
            }

            return new Matrix(answer);
        }

        /// <summary>
        /// Multiply matrix on constant
        /// </summary>
        /// <param name="a">The matrix</param>
        /// <param name="b">The constant</param>
        /// <returns>matrix a * b</returns>
        public static Matrix operator *(Matrix a, double b)
        {
            double[][] answer = new double[a.RowsCount][];

            // Multiplication
            for (int i = 0; i < a.RowsCount; i++)
            {
                answer[i] = new double[a.ColumsCount];
                for (int j = 0; j < a.ColumsCount; j++)
                    answer[i][j] = a.Elements[i][j] * b;
            }
                
            return new Matrix(answer);
        }

        public static Matrix operator *(double a, Matrix b)
        {
            return b * a;
        }

        public static Matrix operator /(Matrix a, double b)
        {
            return a * (1d / b);
        }

        /// <summary>
        /// Finds determinant of a matrix
        /// </summary>
        /// <param name="a">Square matrix</param>
        /// <returns>The matrix's determinant</returns>
        public static double Determinant(Matrix a)
        {
            if (a.Elements.Length != a.Elements[0].Length)
                throw new ArgumentException("Argument must be a square matrix!");

            if (a.Elements.Length == 1)
                return a.Elements[0][0];

            throw new NotImplementedException();
        }
    }
}

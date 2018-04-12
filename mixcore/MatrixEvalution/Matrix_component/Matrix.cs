#region Copyright ©2000 The MathWorks and NIST, ©2004 Joannes Vermorel

// This software is provided 'as-is', without any express or implied warranty. 
// In no event will the authors be held liable for any damages arising from the 
// use of this software.
//
// Permission is granted to anyone to use this software for any purpose, including 
// commercial applications, and to alter it and redistribute it freely, subject to the 
// following restrictions:
//
// 1. The origin of this software must not be misrepresented; you must not claim that 
// you wrote the original software. If you use this software in a product, an acknowledgment 
// (see the following) in the product documentation is required.
//
// Portions Copyright 
// © 2000 The MathWorks and NIST 
// © 2004 Joannes Vermorel
//
// 2. Altered source versions must be plainly marked as such, and must not be 
// misrepresented as being the original software.
//
// 3. This notice may not be removed or altered from any source distribution.

#endregion

using System;

namespace Matrix_component.MatrixN
{
	#region Internal Maths utility
	internal class Maths 
	{
		/// <summary>sqrt(a^2 + b^2) without under/overflow.</summary>
		public static double Hypot(double a, double b) 
		{
			double r;
			if (Math.Abs(a) > Math.Abs(b)) 
			{
				r = b/a;
				r = Math.Abs(a) * Math.Sqrt(1 + r * r);
			} 
			else if (b != 0) 
			{
				r = a/b;
				r = Math.Abs(b) * Math.Sqrt(1 + r * r);
			} 
			else 
			{
				r = 0.0;
			}
			return r;
		}
	}
	#endregion   // Internal Maths utility

	/// <summary>.NET Matrix class.</summary>
	/// <remarks>
	/// The .NET Matrix Class provides the fundamental operations of numerical
	/// linear algebra.  Various constructors create Matrices from two dimensional
	/// arrays of double precision floating point numbers.  Various "gets" and
	/// "sets" provide access to submatrices and matrix elements.  Several methods 
	/// implement basic matrix arithmetic, including matrix addition and
	/// multiplication, matrix norms, and element-by-element array operations.
	/// Methods for reading and printing matrices are also included.  All the
	/// operations in this version of the Matrix Class involve real matrices.
	/// Complex matrices may be handled in a future version.<br/>
	/// 
	/// Five fundamental matrix decompositions, which consist of pairs or triples
	/// of matrices, permutation vectors, and the like, produce results in five
	/// decomposition classes.  These decompositions are accessed by the Matrix
	/// class to compute solutions of simultaneous linear equations, determinants,
	/// inverses and other matrix functions.  The five decompositions are:<br/>
	/// <UL>
	/// <LI>Cholesky Decomposition of symmetric, positive definite matrices.</LI>
	/// <LI>LU Decomposition of rectangular matrices.</LI>
	/// <LI>QR Decomposition of rectangular matrices.</LI>
	/// <LI>Singular Value Decomposition of rectangular matrices.</LI>
	/// <LI>Eigenvalue Decomposition of both symmetric and nonsymmetric square matrices.</LI>
	/// </UL>
	/// <DL>
	/// <DT><B>Example of use:</B></DT>
	/// <br/> Solve a linear system A x = b and compute the residual norm, ||b - A x||. <br/>
	/// <DD><PRE>
	/// double[,] vals = {{1.,2.,3},{4.,5.,6.},{7.,8.,10.}};
	/// Matrix A = new Matrix(vals);
	/// Matrix b = Matrix.Random(3,1);
	/// Matrix x = A.Solve(b);
	/// Matrix r = A.Multiply(x).Subtract(b);
	/// double rnorm = r.NormInf();
	/// </PRE></DD>
	/// </DL>
	/// </remarks>
	/// <author>  
	/// The MathWorks, Inc. and the National Institute of Standards and Technology.
	/// </author>
	/// <version>5 August 1998</version>
	[Serializable]
	public class Matrix : System.ICloneable
	{
		/// <summary>Array for internal storage of elements.</summary>
		private double[,] A;

		/// <seealso cref="RowDimension"/>
		private int m
		{
			get { return A.GetLength(0); }
		}

		/// <seealso cref="ColumnDimension"/>
		private int n
		{
			get { return A.GetLength(1); }
		}
		
		#region Constructors
		
		/// <summary>Construct an m-by-n matrix of zeros. </summary>
		/// <param name="m">Number of rows.</param>
		/// <param name="n">Number of colums.</param>
		public Matrix(int m, int n)
		{
			A = new double[m, n];
		}
		
		/// <summary>Construct an m-by-n constant matrix.</summary>
		/// <param name="m">Number of rows.</param>
		/// <param name="n">Number of colums.</param>
		/// <param name="s">Fill the matrix with this scalar value.</param>
		public Matrix(int m, int n, double s)
		{
			A = new double[m, n];
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					A[i, j] = s;
				}
			}
		}
		
		/// <summary>Construct a matrix from a 2-D array.</summary>
		/// <param name="A">Two-dimensional array of doubles.</param>
		/// <exception cref="System.ArgumentException">All rows must have the same length.</exception>
		/// <seealso cref="Create"/>
		public Matrix(double[,] A)
		{
			this.A = A;
		}
		
		/// <summary>Construct a matrix from a one-dimensional packed array</summary>
		/// <param name="vals">One-dimensional array of doubles, packed by columns (ala Fortran).</param>
		/// <param name="m">Number of rows.</param>
		/// <exception cref="System.ArgumentException">Array length must be a multiple of m.</exception>
		public Matrix(double[] vals, int m)
		{
			int n = (m != 0?vals.Length / m:0);
			if (m * n != vals.Length)
			{
				throw new System.ArgumentException("Array length must be a multiple of m.");
			}

			A = new double[m, n];
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					A[i, j] = vals[i + j * m];
				}
			}
		}
		#endregion //  Constructors
		
		#region Public Properties
		/// <summary>Access the internal two-dimensional array.</summary>
		/// <returns>Pointer to the two-dimensional array of matrix elements.</returns>
		virtual public double[,] Array
		{			
			get
			{
				return A;
			}
		}
		/// <summary>Copy the internal two-dimensional array.</summary>
		/// <returns>Two-dimensional array copy of matrix elements.</returns>
		virtual public double[,] ArrayCopy
		{
			get
			{
				double[,] C = new double[m, n];
				for (int i = 0; i < m; i++)
				{
					for (int j = 0; j < n; j++)
					{
						C[i, j] = A[i, j];
					}
				}
				return C;
			}
			
		}
		/// <summary>Make a one-dimensional column packed copy of the internal array.</summary>
		/// <returns>Matrix elements packed in a one-dimensional array by columns.</returns>
		virtual public double[] ColumnPackedCopy
		{
			get
			{
				double[] vals = new double[m * n];
				for (int i = 0; i < m; i++)
				{
					for (int j = 0; j < n; j++)
					{
						vals[i + j * m] = A[i, j];
					}
				}
				return vals;
			}
			
		}

		/// <summary>Makes a one-dimensional row packed copy of the internal array.</summary>
		/// <returns>Matrix elements packed in a one-dimensional array by rows.</returns>
		virtual public double[] RowPackedCopy
		{
			get
			{
				double[] vals = new double[m * n];
				for (int i = 0; i < m; i++)
				{
					for (int j = 0; j < n; j++)
					{
						vals[i * n + j] = A[i, j];
					}
				}
				return vals;
			}
		}

		/// <summary>Gets the number of rows.</summary>
		virtual public int RowDimension
		{
			get
			{
				return A.GetLength(0);
			}
		}

		/// <summary>Gets the number of columns.</summary>
		virtual public int ColumnDimension
		{
			get
			{
				return A.GetLength(1);
			}
		}
		#endregion   // Public Properties
		
		#region	 Public Methods
		
		/// <summary>Construct a matrix from a copy of a 2-D array.</summary>
		/// <param name="A">Two-dimensional array of doubles.</param>
		/// <exception cref="System.ArgumentException">All rows must have the same length.</exception>
		public static Matrix Create(double[,] A)
		{
			return (Matrix) (new Matrix(A)).Clone();
		}
		
		/// <summary>Makes a deep copy of a matrix.</summary>
		public virtual Matrix Copy()
		{
			Matrix X = new Matrix(m, n);
			double[,] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i, j] = A[i, j];
				}
			}
			return X;
		}
		
		/// <summary>Gets a single element.</summary>
		/// <param name="i">Row index.</param>
		/// <param name="j">Column index.</param>
		/// <returns>A(i,j)</returns>
		/// <exception cref="System.IndexOutOfRangeException"/>  
		public virtual double GetElement(int i, int j)
		{
			return A[i, j];
		}
		
		/// <summary>Gets a submatrix.</summary>
		/// <param name="i0">Initial row index.</param>
		/// <param name="i1">Final row index.</param>
		/// <param name="j0">Initial column index.</param>
		/// <param name="j1">Final column index.</param>
		/// <returns>A(i0:i1,j0:j1)</returns>
		/// <exception cref="System.IndexOutOfRangeException">Submatrix indices</exception>
		public virtual Matrix GetMatrix(int i0, int i1, int j0, int j1)
		{
			Matrix X = new Matrix(i1 - i0 + 1, j1 - j0 + 1);
			double[,] B = X.Array;
			try
			{
				for (int i = i0; i <= i1; i++)
				{
					for (int j = j0; j <= j1; j++)
					{
						B[i - i0, j - j0] = A[i, j];
					}
				}
			}
			catch (System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException("Submatrix indices", e);
			}
			return X;
		}
		
		/// <summary>Gets a submatrix.</summary>
		/// <param name="r">Array of row indices.</param>
		/// <param name="c">Array of column indices.</param>
		/// <returns>A(r(:),c(:))</returns>
		/// <exception cref="System.IndexOutOfRangeException">Submatrix indices.</exception>
		public virtual Matrix GetMatrix(int[] r, int[] c)
		{
			Matrix X = new Matrix(r.Length, c.Length);
			double[,] B = X.Array;
			try
			{
				for (int i = 0; i < r.Length; i++)
				{
					for (int j = 0; j < c.Length; j++)
					{
						B[i, j] = A[r[i], c[j]];
					}
				}
			}
			catch (System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException("Submatrix indices", e);
			}
			return X;
		}
		
		/// <summary>Get a submatrix.</summary>
		/// <param name="i0">Initial row index.</param>
		/// <param name="i1">Final row index.</param>
		/// <param name="c">Array of column indices.</param>
		/// <returns>A(i0:i1,c(:))</returns>
		/// <exception cref="System.IndexOutOfRangeException">Submatrix indices.</exception>
		public virtual Matrix GetMatrix(int i0, int i1, int[] c)
		{
			Matrix X = new Matrix(i1 - i0 + 1, c.Length);
			double[,] B = X.Array;
			try
			{
				for (int i = i0; i <= i1; i++)
				{
					for (int j = 0; j < c.Length; j++)
					{
						B[i - i0, j] = A[i, c[j]];
					}
				}
			}
			catch (System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException("Submatrix indices", e);
			}
			return X;
		}
		
		/// <summary>Get a submatrix.</summary>
		/// <param name="r">Array of row indices.</param>
		/// <param name="j0">Initial column index.</param>
		/// <param name="j1">Final column index.</param>
		/// <returns>A(r(:),j0:j1)</returns>
		/// <exception cref="System.IndexOutOfRangeException">Submatrix indices.</exception>
		public virtual Matrix GetMatrix(int[] r, int j0, int j1)
		{
			Matrix X = new Matrix(r.Length, j1 - j0 + 1);
			double[,] B = X.Array;
			try
			{
				for (int i = 0; i < r.Length; i++)
				{
					for (int j = j0; j <= j1; j++)
					{
						B[i, j - j0] = A[r[i], j];
					}
				}
			}
			catch (System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException("Submatrix indices", e);
			}
			return X;
		}
		
		/// <summary>Sets a single element.</summary>
		/// <param name="i">Row index.</param>
		/// <param name="j">Column index.</param>
		/// <param name="s">A(i,j).</param>
		/// <exception cref="System.IndexOutOfRangeException"/> 
		public virtual void  SetElement(int i, int j, double s)
		{
			A[i, j] = s;
		}
		
		/// <summary>Set a submatrix.</summary>
		/// <param name="i0">Initial row index.</param>
		/// <param name="i1">Final row index.</param>
		/// <param name="j0">Initial column index.</param>
		/// <param name="j1">Final column index.</param>
		/// <param name="X">A(i0:i1,j0:j1)</param>
		/// <exception cref="System.IndexOutOfRangeException">Submatrix indices.</exception>
		public virtual void  SetMatrix(int i0, int i1, int j0, int j1, Matrix X)
		{
			try
			{
				for (int i = i0; i <= i1; i++)
				{
					for (int j = j0; j <= j1; j++)
					{
						A[i, j] = X.GetElement(i - i0, j - j0);
					}
				}
			}
			catch (System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException("Submatrix indices", e);
			}
		}
		
		/// <summary>Sets a submatrix.</summary>
		/// <param name="r">Array of row indices.</param>
		/// <param name="c">Array of column indices.</param>
		/// <param name="X">A(r(:),c(:))</param>
		/// <exception cref="System.IndexOutOfRangeException">Submatrix indices</exception>
		public virtual void  SetMatrix(int[] r, int[] c, Matrix X)
		{
			try
			{
				for (int i = 0; i < r.Length; i++)
				{
					for (int j = 0; j < c.Length; j++)
					{
						A[r[i], c[j]] = X.GetElement(i, j);
					}
				}
			}
			catch (System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException("Submatrix indices", e);
			}
		}
		
		/// <summary>Sets a submatrix.</summary>
		/// <param name="r">Array of row indices.</param>
		/// <param name="j0">Initial column index.</param>
		/// <param name="j1">Final column index.</param>
		/// <param name="X">A(r(:),j0:j1)</param>
		/// <exception cref="System.IndexOutOfRangeException">Submatrix indices</exception>
		public virtual void  SetMatrix(int[] r, int j0, int j1, Matrix X)
		{
			try
			{
				for (int i = 0; i < r.Length; i++)
				{
					for (int j = j0; j <= j1; j++)
					{
						A[r[i], j] = X.GetElement(i, j - j0);
					}
				}
			}
			catch (System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException("Submatrix indices", e);
			}
		}
		
		/// <summary>Set a submatrix.</summary>
		/// <param name="i0">Initial row index.</param>
		/// <param name="i1">Final row index.</param>
		/// <param name="c">Array of column indices.</param>
		/// <param name="X">A(i0:i1,c(:))</param>
		/// <exception cref="System.IndexOutOfRangeException">Submatrix indices.</exception>
		public virtual void  SetMatrix(int i0, int i1, int[] c, Matrix X)
		{
			try
			{
				for (int i = i0; i <= i1; i++)
				{
					for (int j = 0; j < c.Length; j++)
					{
						A[i, c[j]] = X.GetElement(i - i0, j);
					}
				}
			}
			catch (System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException("Submatrix indices", e);
			}
		}
		
		/// <summary>Matrix transpose.</summary>
		public virtual Matrix Transpose()
		{
			Matrix X = new Matrix(n, m);
			double[,] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[j, i] = A[i, j];
				}
			}
			return X;
		}
		
		/// <summary>One norm</summary>
		/// <returns>Maximum column sum.</returns>
		public virtual double Norm1()
		{
			double f = 0;
			for (int j = 0; j < n; j++)
			{
				double s = 0;
				for (int i = 0; i < m; i++)
				{
					s += System.Math.Abs(A[i, j]);
				}
				f = System.Math.Max(f, s);
			}
			return f;
		}
		
		/// <summary>Two norm</summary>
		/// <returns>Maximum singular value.</returns>
		public virtual double Norm2()
		{
			return (new SingularValueDecomposition(this).Norm2());
		}
		
		/// <summary>Infinity norm</summary>
		/// <returns>Maximum row sum.</returns>
		public virtual double NormInf()
		{
			double f = 0;
			for (int i = 0; i < m; i++)
			{
				double s = 0;
				for (int j = 0; j < n; j++)
				{
					s += System.Math.Abs(A[i, j]);
				}
				f = System.Math.Max(f, s);
			}
			return f;
		}
		
		/// <summary>Frobenius norm</summary>
		/// <returns>Sqrt of sum of squares of all elements.</returns>
		public virtual double NormF()
		{
			double f = 0;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					f = Maths.Hypot(f, A[i, j]);
				}
			}
			return f;
		}
		
		/// <summary>Unary minus</summary>
		public virtual Matrix UnaryMinus()
		{
			Matrix X = new Matrix(m, n);
			double[,] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i, j] = -A[i, j];
				}
			}
			return X;
		}
		
		/// <summary>C = A + B</summary>
		/// <param name="B">another matrix</param>
		/// <returns>A + B</returns>
		public virtual Matrix Add(Matrix B)
		{
			CheckMatrixDimensions(B);
			Matrix X = new Matrix(m, n);
			double[,] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i, j] = A[i, j] + B.A[i, j];
				}
			}
			return X;
		}
		
		/// <summary>A = A + B</summary>
		/// <param name="B">another matrix</param>
		/// <returns>A + B</returns>
		public virtual Matrix AddEquals(Matrix B)
		{
			CheckMatrixDimensions(B);
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					A[i, j] = A[i, j] + B.A[i, j];
				}
			}
			return this;
		}
		
		/// <summary>C = A - B</summary>
		/// <param name="B">another matrix</param>
		/// <returns>A - B</returns>
		public virtual Matrix Subtract(Matrix B)
		{
			CheckMatrixDimensions(B);
			Matrix X = new Matrix(m, n);
			double[,] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i, j] = A[i, j] - B.A[i, j];
				}
			}
			return X;
		}
		
		/// <summary>A = A - B</summary>
		/// <param name="B">another matrix</param>
		/// <returns>A - B</returns>
		public virtual Matrix SubtractEquals(Matrix B)
		{
			CheckMatrixDimensions(B);
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					A[i, j] = A[i, j] - B.A[i, j];
				}
			}
			return this;
		}
		
		/// <summary>Element-by-element multiplication, C = A.*B</summary>
		/// <param name="B">another matrix</param>
		/// <returns>A.*B</returns>
		public virtual Matrix ArrayMultiply(Matrix B)
		{
			CheckMatrixDimensions(B);
			Matrix X = new Matrix(m, n);
			double[,] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i, j] = A[i, j] * B.A[i, j];
				}
			}
			return X;
		}
		
		/// <summary>Element-by-element multiplication in place, A = A.*B</summary>
		/// <param name="B">another matrix</param>
		/// <returns>A.*B</returns>
		public virtual Matrix ArrayMultiplyEquals(Matrix B)
		{
			CheckMatrixDimensions(B);
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					A[i, j] = A[i, j] * B.A[i, j];
				}
			}
			return this;
		}
		
		/// <summary>Element-by-element right division, C = A./B</summary>
		/// <param name="B">another matrix</param>
		/// <returns>A./B</returns>
		public virtual Matrix ArrayRightDivide(Matrix B)
		{
			CheckMatrixDimensions(B);
			Matrix X = new Matrix(m, n);
			double[,] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i, j] = A[i, j] / B.A[i, j];
				}
			}
			return X;
		}
		
		/// <summary>Element-by-element right division in place, A = A./B</summary>
		/// <param name="B">another matrix</param>
		/// <returns>A./B</returns>
		public virtual Matrix ArrayRightDivideEquals(Matrix B)
		{
			CheckMatrixDimensions(B);
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					A[i, j] = A[i, j] / B.A[i, j];
				}
			}
			return this;
		}
		
		/// <summary>Element-by-element left division, C = A.\B</summary>
		/// <param name="B">another matrix</param>
		/// <returns>A.\B</returns>
		public virtual Matrix ArrayLeftDivide(Matrix B)
		{
			CheckMatrixDimensions(B);
			Matrix X = new Matrix(m, n);
			double[,] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i, j] = B.A[i, j] / A[i, j];
				}
			}
			return X;
		}
		
		/// <summary>Element-by-element left division in place, A = A.\B</summary>
		/// <param name="B">another matrix</param>
		/// <returns>A.\B</returns>
		public virtual Matrix ArrayLeftDivideEquals(Matrix B)
		{
			CheckMatrixDimensions(B);
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					A[i, j] = B.A[i, j] / A[i, j];
				}
			}
			return this;
		}
		
		/// <summary>Multiply a matrix by a scalar, C = s*A</summary>
		/// <param name="s">scalar</param>
		/// <returns>s*A</returns>
		public virtual Matrix Multiply(double s)
		{
			Matrix X = new Matrix(m, n);
			double[,] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i, j] = s * A[i, j];
				}
			}
			return X;
		}
		
		/// <summary>Multiply a matrix by a scalar in place, A = s*A</summary>
		/// <param name="s">scalar</param>
		public virtual Matrix MultiplyEquals(double s)
		{
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					A[i, j] = s * A[i, j];
				}
			}
			return this;
		}
		
		/// <summary>Linear algebraic matrix multiplication, A * B</summary>
		/// <param name="B">another matrix</param>
		/// <returns>Matrix product, A * B</returns>
		/// <exception cref="System.ArgumentException">Matrix inner dimensions must agree.</exception>
		public virtual Matrix Multiply(Matrix B)
		{
			if (B.m != n)
			{
				throw new System.ArgumentException("Matrix inner dimensions must agree.");
			}
			Matrix X = new Matrix(m, B.n);
			double[,] C = X.Array;
			double[] Bcolj = new double[n];
			for (int j = 0; j < B.n; j++)
			{
				for (int k = 0; k < n; k++)
				{
					Bcolj[k] = B.A[k, j];
				}
				for (int i = 0; i < m; i++)
				{
					//double[] Arowi = A[i];
					double s = 0;
					for (int k = 0; k < n; k++)
					{
						s += A[i,k] * Bcolj[k];
					}
					C[i, j] = s;
				}
			}
			return X;
		}

		/// <summary>LU Decomposition</summary>
		/// <seealso cref="LUDecomposition"/>
		public virtual LUDecomposition LUD()
		{
			return new LUDecomposition(this);
		}
		
		/// <summary>QR Decomposition</summary>
		/// <returns>QRDecomposition</returns>
		/// <seealso cref="QRDecomposition"/>
		public virtual QRDecomposition QRD()
		{
			return new QRDecomposition(this);
		}
		
		/// <summary>Cholesky Decomposition</summary>
		/// <seealso cref="CholeskyDecomposition"/>
		public virtual CholeskyDecomposition chol()
		{
			return new CholeskyDecomposition(this);
		}
		
		/// <summary>Singular Value Decomposition</summary>
		/// <seealso cref="SingularValueDecomposition"/>
		public virtual SingularValueDecomposition SVD()
		{
			return new SingularValueDecomposition(this);
		}
		
		/// <summary>Eigenvalue Decomposition</summary>
		/// <seealso cref="EigenvalueDecomposition"/>
		public virtual EigenvalueDecomposition Eigen()
		{
			return new EigenvalueDecomposition(this);
		}
		
		/// <summary>Solve A*X = B</summary>
		/// <param name="B">right hand side</param>
		/// <returns>solution if A is square, least squares solution otherwise.</returns>
		public virtual Matrix Solve(Matrix B)
		{
			return (m == n ? (new LUDecomposition(this)).Solve(B):(new QRDecomposition(this)).Solve(B));
		}
		
		/// <summary>Solve X*A = B, which is also A'*X' = B'</summary>
		/// <param name="B">right hand side</param>
		/// <returns>solution if A is square, least squares solution otherwise.</returns>
		public virtual Matrix SolveTranspose(Matrix B)
		{
			return Transpose().Solve(B.Transpose());
		}
		
		/// <summary>Matrix inverse or pseudoinverse</summary>
		/// <returns> inverse(A) if A is square, pseudoinverse otherwise.</returns>
		public virtual Matrix Inverse()
		{
			return Solve(Identity(m, m));
		}
		
		/// <summary>Matrix determinant</summary>
		public virtual double Determinant()
		{
			return new LUDecomposition(this).Determinant();
		}
		
		/// <summary>Matrix rank</summary>
		/// <returns>effective numerical rank, obtained from SVD.</returns>
		public virtual int Rank()
		{
			return new SingularValueDecomposition(this).Rank();
		}
		
		/// <summary>Matrix condition (2 norm)</summary>
		/// <returns>ratio of largest to smallest singular value.</returns>
		public virtual double Condition()
		{
			return new SingularValueDecomposition(this).Condition();
		}
		
		/// <summary>Matrix trace.</summary>
		/// <returns>sum of the diagonal elements.</returns>
		public virtual double Trace()
		{
			double t = 0;
			for (int i = 0; i < System.Math.Min(m, n); i++)
			{
				t += A[i, i];
			}
			return t;
		}
		
		/// <summary>Generates matrix with random elements</summary>
		/// <param name="m">Number of rows.</param>
		/// <param name="n">Number of colums.</param>
		/// <returns>An m-by-n matrix with uniformly distributed random elements.</returns>
		public static Matrix Random(int m, int n)
		{
			System.Random random = new System.Random();

			Matrix A = new Matrix(m, n);
			double[,] X = A.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					X[i, j] = random.NextDouble();
				}
			}
			return A;
		}
		
		/// <summary>Generates identity matrix</summary>
		/// <param name="m">Number of rows.</param>
		/// <param name="n">Number of colums.</param>
		/// <returns>An m-by-n matrix with ones on the diagonal and zeros elsewhere.</returns>
		public static Matrix Identity(int m, int n)
		{
			Matrix A = new Matrix(m, n);
			double[,] X = A.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					X[i, j] = (i == j ? 1.0 : 0.0);
				}
			}
			return A;
		}		
		
		#endregion //  Public Methods

		#region Operator Overloading

		/// <summary>Addition of matrices</summary>
		public static Matrix operator +(Matrix m1, Matrix m2) 
		{ 
			return m1.Add(m2); 
		} 

		/// <summary>Subtraction of matrices</summary>
		public static Matrix operator -(Matrix m1, Matrix m2) 
		{ 
			return m1.Subtract(m2); 
		} 

		/// <summary>Multiplication of matrices</summary>
		public static Matrix operator *(Matrix m1, Matrix m2) 
		{ 
			return m1.Multiply(m2); 
		}

		/// <summary>Implicit convertion to a <c>double[,]</c> array.</summary>
		public static implicit operator double[,] (Matrix m)
		{
			return m.A;
		}

		#endregion   //Operator Overloading

		#region	 Private Methods
		
		/// <summary>Check if size(A) == size(B) *</summary>
		private void  CheckMatrixDimensions(Matrix B)
		{
			if (B.m != m || B.n != n)
			{
				throw new System.ArgumentException("Matrix dimensions must agree.");
			}
		}
		#endregion //  Private Methods

		/// <summary>Returns a deep copy of this instance.</summary>
		public object Clone()
		{
			return this.Copy();
		}
	}
}
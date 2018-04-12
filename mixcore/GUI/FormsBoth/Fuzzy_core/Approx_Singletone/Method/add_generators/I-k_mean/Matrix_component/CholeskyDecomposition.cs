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
	
	/// <summary>Cholesky Decomposition.</summary>
	/// <remarks>
	/// For a symmetric, positive definite matrix A, the Cholesky decomposition
	/// is an lower triangular matrix L so that A = L*L'.
	/// If the matrix is not symmetric or positive definite, the constructor
	/// returns a partial decomposition and sets an internal flag that may
	/// be queried by the isSPD() method.
	/// </remarks>
	[Serializable]
	public class CholeskyDecomposition
	{
		#region Class variables
		
		/// <summary>Array for internal storage of decomposition.</summary>
		private double[,] L;
		
		/// <summary>Row and column dimension (square matrix).</summary>
		private int n
		{
			get { return L.GetLength(0); }
		}
		
		/// <summary>Symmetric and positive definite flag.</summary>
		private bool isspd;
		
		#endregion //  Class variables

		#region Constructor
		
		/// <summary>Cholesky algorithm for symmetric and positive definite matrix.</summary>
		/// <param name="Arg">Square, symmetric matrix.</param>
		/// <returns>Structure to access L and isspd flag.</returns>
		public CholeskyDecomposition(Matrix Arg)
		{
			// Initialize.
			double[,] A = Arg.Array;
			L = new double[Arg.RowDimension, Arg.RowDimension];

			isspd = (Arg.ColumnDimension == n);
			// Main loop.
			for (int j = 0; j < n; j++)
			{
				//double[] Lrowj = L[j];
				double d = 0.0;
				for (int k = 0; k < j; k++)
				{
					//double[] Lrowk = L[k];
					double s = 0.0;
					for (int i = 0; i < k; i++)
					{
						s += L[k,i] * L[j,i];
					}
					L[j,k] = s = (A[j, k] - s) / L[k, k];
					d = d + s * s;
					isspd = isspd & (A[k, j] == A[j, k]);
				}
				d = A[j, j] - d;
				isspd = isspd & (d > 0.0);
				L[j, j] = System.Math.Sqrt(System.Math.Max(d, 0.0));
				for (int k = j + 1; k < n; k++)
				{
					L[j, k] = 0.0;
				}
			}
		}
		
		#endregion //  Constructor

		#region Public Properties
		/// <summary>Is the matrix symmetric and positive definite?</summary>
		/// <returns><c>true</c> if A is symmetric and positive definite.</returns>
		virtual public bool SPD
		{
			get
			{
				return isspd;
			}
		}
		#endregion   // Public Properties
		
		#region Public Methods
		
		/// <summary>Return triangular factor.</summary>
		/// <returns>L</returns>
		public virtual Matrix GetL()
		{
			return new Matrix(L);
		}
		
		/// <summary>Solve A*X = B</summary>
		/// <param name="B">  A Matrix with as many rows as A and any number of columns.</param>
		/// <returns>X so that L*L'*X = B</returns>
		/// <exception cref="System.ArgumentException">Matrix row dimensions must agree.</exception>
		/// <exception cref="System.SystemException">Matrix is not symmetric positive definite.</exception>
		public virtual Matrix Solve(Matrix B)
		{
			if (B.RowDimension != n)
			{
				throw new System.ArgumentException("Matrix row dimensions must agree.");
			}
			if (!isspd)
			{
				throw new System.SystemException("Matrix is not symmetric positive definite.");
			}
			
			// Copy right hand side.
			double[,] X = B.ArrayCopy;
			int nx = B.ColumnDimension;
			
			// Solve L*Y = B;
			for (int k = 0; k < n; k++)
			{
				for (int i = k + 1; i < n; i++)
				{
					for (int j = 0; j < nx; j++)
					{
						X[i, j] -= X[k, j] * L[i, k];
					}
				}
				for (int j = 0; j < nx; j++)
				{
					X[k, j] /= L[k, k];
				}
			}
			
			// Solve L'*X = Y;
			for (int k = n - 1; k >= 0; k--)
			{
				for (int j = 0; j < nx; j++)
				{
					X[k, j] /= L[k, k];
				}
				for (int i = 0; i < k; i++)
				{
					for (int j = 0; j < nx; j++)
					{
						X[i, j] -= X[k, j] * L[k, i];
					}
				}
			}
			return new Matrix(X);
		}
		#endregion //  Public Methods
	}
}
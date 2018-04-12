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
	/// <summary>QR Decomposition.</summary>
	/// <remarks>
	/// For an m-by-n matrix A with m >= n, the QR decomposition is an m-by-n
	/// orthogonal matrix Q and an n-by-n upper triangular matrix R so that
	/// A = Q*R.<br/>
	/// 
	/// The QR decompostion always exists, even if the matrix does not have
	/// full rank, so the constructor will never fail.  The primary use of the
	/// QR decomposition is in the least squares solution of nonsquare systems
	/// of simultaneous linear equations.  This will fail if <c>IsFullRank()</c>
	/// returns false.
	/// </remarks>
	[Serializable]
	public class QRDecomposition
	{
		#region Class variables
		
		/// <summary>Array for internal storage of decomposition.</summary>
		private double[,] QR;
		
		/// <summary>Row dimensions.</summary>
		private int m
		{
			get { return QR.GetLength(0); }
		}
		
		/// <summary>Column dimensions.</summary>
		private int n
		{
			get { return QR.GetLength(1); }
		}
		
		/// <summary>Array for internal storage of diagonal of R.</summary>
		private double[] Rdiag;

		#endregion //  Class variables
		
		#region Constructor
		
		/// <summary>QR Decomposition, computed by Householder reflections.</summary>
		/// <remarks>Provides access to R, the Householder vectors and computes Q.</remarks>
		/// <param name="A">Rectangular matrix</param>
		public QRDecomposition(Matrix A)
		{
			// Initialize.
			QR = A.ArrayCopy;
			Rdiag = new double[n];
			
			// Main loop.
			for (int k = 0; k < n; k++)
			{
				// Compute 2-norm of k-th column without under/overflow.
				double nrm = 0;
				for (int i = k; i < m; i++)
				{
					nrm = Maths.Hypot(nrm, QR[i, k]);
				}
				
				if (nrm != 0.0)
				{
					// Form k-th Householder vector.
					if (QR[k, k] < 0)
					{
						nrm = - nrm;
					}
					for (int i = k; i < m; i++)
					{
						QR[i, k] /= nrm;
					}
					QR[k, k] += 1.0;
					
					// Apply transformation to remaining columns.
					for (int j = k + 1; j < n; j++)
					{
						double s = 0.0;
						for (int i = k; i < m; i++)
						{
							s += QR[i, k] * QR[i, j];
						}
						s = (- s) / QR[k, k];
						for (int i = k; i < m; i++)
						{
							QR[i, j] += s * QR[i, k];
						}
					}
				}
				Rdiag[k] = - nrm;
			}
		}

		#endregion //  Constructor
		
		#region Public Properties

		/// <summary>Indicates whether the matrix is full rank.</summary>
		/// <returns><c>true</c> if R, and hence A, has full rank.</returns>
		virtual public bool FullRank
		{
			get
			{
				for (int j = 0; j < n; j++)
				{
					if (Rdiag[j] == 0)
						return false;
				}
				return true;
			}
		}

		/// <summary>Returns the Householder vectors.</summary>
		/// <returns>Lower trapezoidal matrix whose columns define the reflections.</returns>
		virtual public Matrix H
		{
			get
			{
				Matrix X = new Matrix(m, n);
				double[,] H = X.Array;
				for (int i = 0; i < m; i++)
				{
					for (int j = 0; j < n; j++)
					{
						if (i >= j)
						{
							H[i, j] = QR[i, j];
						}
						else
						{
							H[i, j] = 0.0;
						}
					}
				}
				return X;
			}
		}

		/// <summary>Returns the upper triangular factor</summary>
		virtual public Matrix R
		{
			get
			{
				Matrix X = new Matrix(n, n);
				double[,] R = X.Array;
				for (int i = 0; i < n; i++)
				{
					for (int j = 0; j < n; j++)
					{
						if (i < j)
						{
							R[i, j] = QR[i, j];
						}
						else if (i == j)
						{
							R[i, j] = Rdiag[i];
						}
						else
						{
							R[i, j] = 0.0;
						}
					}
				}
				return X;
			}
		}

		/// <summary>Generate and return the (economy-sized) orthogonal factor.</summary>
		virtual public Matrix Q
		{
			get
			{
				Matrix X = new Matrix(m, n);
				double[,] Q = X.Array;
				for (int k = n - 1; k >= 0; k--)
				{
					for (int i = 0; i < m; i++)
					{
						Q[i, k] = 0.0;
					}
					Q[k, k] = 1.0;
					for (int j = k; j < n; j++)
					{
						if (QR[k, k] != 0)
						{
							double s = 0.0;
							for (int i = k; i < m; i++)
							{
								s += QR[i, k] * Q[i, j];
							}
							s = (- s) / QR[k, k];
							for (int i = k; i < m; i++)
							{
								Q[i, j] += s * QR[i, k];
							}
						}
					}
				}
				return X;
			}
		}
		#endregion //  Public Properties
		
		#region Public Methods
		
		/// <summary>Least squares solution of A*X = B</summary>
		/// <param name="B">A Matrix with as many rows as A and any number of columns.</param>
		/// <returns>X that minimizes the two norm of Q*R*X-B.</returns>
		/// <exception cref="System.ArgumentException">Matrix row dimensions must agree.</exception>
		/// <exception cref="System.SystemException"> Matrix is rank deficient.</exception>
		public virtual Matrix Solve(Matrix B)
		{
			if (B.RowDimension != m)
			{
				throw new System.ArgumentException("Matrix row dimensions must agree.");
			}
			if (!this.FullRank)
			{
				throw new System.SystemException("Matrix is rank deficient.");
			}
			
			// Copy right hand side
			int nx = B.ColumnDimension;
			double[,] X = B.ArrayCopy;
			
			// Compute Y = transpose(Q)*B
			for (int k = 0; k < n; k++)
			{
				for (int j = 0; j < nx; j++)
				{
					double s = 0.0;
					for (int i = k; i < m; i++)
					{
						s += QR[i, k] * X[i, j];
					}
					s = (- s) / QR[k, k];
					for (int i = k; i < m; i++)
					{
						X[i, j] += s * QR[i, k];
					}
				}
			}
			// Solve R*X = Y;
			for (int k = n - 1; k >= 0; k--)
			{
				for (int j = 0; j < nx; j++)
				{
					X[k, j] /= Rdiag[k];
				}
				for (int i = 0; i < k; i++)
				{
					for (int j = 0; j < nx; j++)
					{
						X[i, j] -= X[k, j] * QR[i, k];
					}
				}
			}

			return (new Matrix(X).GetMatrix(0, n - 1, 0, nx - 1));
		}

		#endregion //  Public Methods
	}
}
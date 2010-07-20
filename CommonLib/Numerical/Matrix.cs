using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;


//adapted from: http://www.trentfguidry.net/post/2009/06/30/Creating-a-Matrix-Class-in-C-Part-3-LU-Decomposition-with-Partial-Pivoting-and-Inversion.aspx
//This guys use of Hungrian Notation sucks. In fact, Hungarian Notation sucks.
namespace CommonLib.Numerical
{
	public enum LUDecompositionResultStatus { OK = 0, LinearlyDependent = 1 }

	public class Matrix
	{
		private int m_nNumRows = 3;
		private int m_nNumColumns = 3;
		private double[,] m_dValues;

		public Matrix(){
			m_dValues = new double[m_nNumRows, m_nNumColumns];
		}

		public Matrix(int nNumRows, int nNumColumns){
			m_nNumRows = nNumRows;
			m_nNumColumns = nNumColumns;

			m_dValues = new double[m_nNumRows, m_nNumColumns];
		}

 
		public double this[int nRow, int nColumn]{
			get { return m_dValues[nRow, nColumn]; }
			set { m_dValues[nRow, nColumn] = value; }
		}

		public int NumRows { get { return m_nNumRows; } }
		public int NumColumns { get { return m_nNumColumns; } }

		public Matrix Clone(){
			Matrix mRet = new Matrix(m_nNumRows, m_nNumColumns);
			for (int i = 0; i < m_nNumRows; i++){
				for (int j = 0; j < m_nNumColumns; j++){
					mRet[i, j] = this[i, j];
				}
			}

			return mRet;
		}

		public static Matrix Identity(int nSize){
			Matrix mRet = new Matrix(nSize, nSize);

			for (int i = 0; i < nSize; i++){
				for (int j = 0; j < nSize; j++){
					mRet[i, j] = (i == j) ? 1.0 : 0.0;
				}
			}

			return mRet;
		}

		public static double[] Regress(double[,] dZ, double[] dY) {
			//y=a0 z1 + a1 z1 +a2 z2 + a3 z3 +...
			//Z is the functional values.
			//Z index 0 is a row, the variables go across index 1.
			//Y is the summed value.
			//returns the coefficients.

			System.Diagnostics.Debug.Assert(dZ != null && dY != null);
			System.Diagnostics.Debug.Assert(dZ.GetLength(0) == dY.GetLength(0));

			Matrix mZ = dZ;
			Matrix mZTran = mZ.Transpose();
			Matrix mLHS = mZTran * mZ;
			Matrix mRHS = mZTran * dY;
			Matrix mCoefs = mLHS.SolveFor(mRHS);

			return mCoefs;
		}

		public static double[] RegressionCoefficients(int polyOrder, double[] Y, double[] X) {
			double[,] Z = new double[Y.Length, polyOrder + 1];
			for (int i = 0; i < Y.Length; i++) 
				for (int j = 0; j <= polyOrder; j++)
					Z[i, j] = Math.Pow(X[i], j);

			double[] coefs = Matrix.Regress(Z, Y);

			return coefs;
		}

		public static double[] RegressionLine(int polyOrder, double[] Y, double[] X) {
			double[] coefs = Matrix.RegressionCoefficients(polyOrder, Y, X);

			double[] newY = new double[Y.Length];

			for (int i = 0; i < Y.Length; i++){
				double y = 0.0;
				for (int j = 0; j <= polyOrder; j++ )
					y += coefs[j] * Math.Pow(X[i], j);
				newY[i] = y;
			}

			return newY;
		}

		public Matrix Transpose() {
			Matrix mRet = new Matrix(m_nNumColumns, m_nNumRows);
			for (int i = 0; i < m_nNumRows; i++){
				for (int j = 0; j < m_nNumColumns; j++){
					mRet[j, i] = this[i, j];
				}
			}

			return mRet;
		}

		public static Matrix FromArray(double[] dLeft){
			int nLength = dLeft.Length;
			Matrix mRet = new Matrix(nLength, 1);

			for (int i = 0; i < nLength; i++)
				mRet[i, 0] = dLeft[i];

			return mRet;
		}

		public static implicit operator Matrix(double[] dLeft){
			return FromArray(dLeft);
		}

		public static double[] ToArray(Matrix mLeft){
			Debug.Assert((mLeft.NumColumns == 1 && mLeft.NumRows >= 1) || (mLeft.NumRows == 1 && mLeft.NumColumns >= 1));

			double[] dRet = null;

			if (mLeft.NumColumns > 1){
				int nNumElements = mLeft.NumColumns;
				dRet = new double[nNumElements];

				for (int i = 0; i < nNumElements; i++)
					dRet[i] = mLeft[0, i];
			} else {
				int nNumElements = mLeft.NumRows;
				dRet = new double[nNumElements];

				for (int i = 0; i < nNumElements; i++)
					dRet[i] = mLeft[i, 0];
			}

			return dRet;
		}

		public static implicit operator double[](Matrix mLeft){
			return ToArray(mLeft);
		}

		public static Matrix FromDoubleArray(double[,] dLeft){
			int nLength0 = dLeft.GetLength(0);
			int nLength1 = dLeft.GetLength(1);

			Matrix mRet = new Matrix(nLength0, nLength1);

			for (int i = 0; i < nLength0; i++)
				for (int j = 0; j < nLength1; j++)
					mRet[i, j] = dLeft[i, j];

			return mRet;
		}

		public static implicit operator Matrix(double[,] dLeft){
			return FromDoubleArray(dLeft);
		}

		public static double[,] ToDoubleArray(Matrix mLeft){
			double[,] dRet = new double[mLeft.NumRows, mLeft.NumColumns];
			for (int i = 0; i < mLeft.NumRows; i++)
				for (int j = 0; j < mLeft.NumColumns; j++)
					dRet[i, j] = mLeft[i, j];

			return dRet;
		}

		public static implicit operator double[,](Matrix mLeft){
			return ToDoubleArray(mLeft);
		}

		public static Matrix Add(Matrix mLeft, Matrix mRight){
			Debug.Assert(mLeft.NumColumns == mRight.NumColumns);
			Debug.Assert(mLeft.NumRows == mRight.NumRows);

			Matrix mRet = new Matrix(mLeft.NumRows, mRight.NumColumns);

			for (int i = 0; i < mLeft.NumRows; i++)
				for (int j = 0; j < mLeft.NumColumns; j++)
					mRet[i, j] = mLeft[i, j] + mRight[i, j];

			return mRet;
		}

		public static Matrix operator +(Matrix mLeft, Matrix mRight){
			return Matrix.Add(mLeft, mRight);
		}

		public static Matrix Subtract(Matrix mLeft, Matrix mRight){
			Debug.Assert(mLeft.NumColumns == mRight.NumColumns);
			Debug.Assert(mLeft.NumRows == mRight.NumRows);

			Matrix mRet = new Matrix(mLeft.NumRows, mRight.NumColumns);

			for (int i = 0; i < mLeft.NumRows; i++)
				for (int j = 0; j < mLeft.NumColumns; j++)
					mRet[i, j] = mLeft[i, j] - mRight[i, j];

			return mRet;
		}

		public static Matrix operator -(Matrix mLeft, Matrix mRight){
			return Matrix.Subtract(mLeft, mRight);
		}

		public static Matrix Multiply(Matrix mLeft, Matrix mRight){
			Debug.Assert(mLeft.NumColumns == mRight.NumRows);
			Matrix mRet = new Matrix(mLeft.NumRows, mRight.NumColumns);

			for (int i = 0; i < mRight.NumColumns; i++){
				for (int j = 0; j < mLeft.NumRows; j++){
					double dValue = 0.0;

					for (int k = 0; k < mRight.NumRows; k++)
						dValue += mLeft[j, k] * mRight[k, i];

					mRet[j, i] = dValue;
				}
			}

			return mRet;
		}

		public static Matrix operator *(Matrix mLeft, Matrix mRight){
			return Matrix.Multiply(mLeft, mRight);
		}

		public static Matrix Multiply(double dLeft, Matrix mRight){
			Matrix mRet = new Matrix(mRight.NumRows, mRight.NumColumns);
			for (int i = 0; i < mRight.NumRows; i++)
				for (int j = 0; j < mRight.NumColumns; j++)
					mRet[i, j] = dLeft * mRight[i, j];

			return mRet;
		}

		public static Matrix operator *(double dLeft, Matrix mRight){
			return Matrix.Multiply(dLeft, mRight);
		}

		public static Matrix Multiply(Matrix mLeft, double dRight){
			Matrix mRet = new Matrix(mLeft.NumRows, mLeft.NumColumns);

			for (int i = 0; i < mLeft.NumRows; i++){
				for (int j = 0; j < mLeft.NumColumns; j++){
					mRet[i, j] = mLeft[i, j] * dRight;
				}
			}

			return mRet;
		}

		public static Matrix operator *(Matrix mLeft, double dRight){
			return Matrix.Multiply(mLeft, dRight);
		}

		public static Matrix Divide(Matrix mLeft, double dRight){

			Matrix mRet = new Matrix(mLeft.NumRows, mLeft.NumColumns);

			for (int i = 0; i < mLeft.NumRows; i++)
				for (int j = 0; j < mLeft.NumColumns; j++)
					mRet[i, j] = mLeft[i, j] / dRight;

			return mRet;
		}

		public static Matrix operator /(Matrix mLeft, double dRight){
			return Matrix.Divide(mLeft, dRight);
		}

		public Matrix SolveFor(Matrix mRight){
			Debug.Assert(mRight.NumRows == m_nNumColumns);
			Debug.Assert(m_nNumColumns == m_nNumRows);

			Matrix mRet = new Matrix(m_nNumColumns, mRight.NumColumns);
			LUDecompositionResults resDecomp = LUDecompose();

			int[] nP = resDecomp.PivotArray;
			Matrix mL = resDecomp.L;
			Matrix mU = resDecomp.U;

			double dSum = 0.0;

			for (int k = 0; k < mRight.NumColumns; k++){
				Matrix D = new Matrix(m_nNumRows, 1);
				D[0, 0] = mRight[nP[0], k] / mL[0, 0];
				for (int i = 1; i < m_nNumRows; i++){
					dSum = 0.0;

					for (int j = 0; j < i; j++)
						dSum += mL[i, j] * D[j, 0];

					D[i, 0] = (mRight[nP[i], k] - dSum) / mL[i, i];
				}

 				mRet[m_nNumRows - 1, k] = D[m_nNumRows - 1, 0];
				for (int i = m_nNumRows - 2; i >= 0; i--){
					dSum = 0.0;
					for (int j = i + 1; j < m_nNumRows; j++)
						dSum += mU[i, j] * mRet[j, k];

					mRet[i, k] = D[i, 0] - dSum;
				}
			}

			return mRet;
		}

		private LUDecompositionResults LUDecompose(){
			Debug.Assert(m_nNumColumns == m_nNumRows);

			LUDecompositionResults resRet = new LUDecompositionResults();
			int[] nP = new int[m_nNumRows]; //Pivot matrix.
			Matrix mU = new Matrix(m_nNumRows, m_nNumColumns);
			Matrix mL = new Matrix(m_nNumRows, m_nNumColumns);
			Matrix mUWorking = Clone();
			Matrix mLWorking = new Matrix(m_nNumRows, m_nNumColumns);

			for (int i = 0; i < m_nNumRows; i++)
				nP[i] = i;

			for (int i = 0; i < m_nNumRows; i++){
				double dMaxRowRatio = double.NegativeInfinity;
				int nMaxRow = -1;
				int nMaxPosition = -1;

				for (int j = i; j < m_nNumRows; j++){
					double dRowSum = 0.0;

					for (int k = i; k < m_nNumColumns; k++)
						dRowSum += Math.Abs(mUWorking[nP[j], k]);

					if (Math.Abs(mUWorking[nP[j], i] / dRowSum) > dMaxRowRatio){
						dMaxRowRatio = Math.Abs(mUWorking[nP[j], i] / dRowSum);
						nMaxRow = nP[j];
						nMaxPosition = j;
					}
				}

				if (nMaxRow != nP[i]){
					int nHold = nP[i];
					nP[i] = nMaxRow;
					nP[nMaxPosition] = nHold;
				}

 				double dRowFirstElementValue = mUWorking[nP[i], i];

				for (int j = 0; j < m_nNumRows; j++){
					if (j < i)
						mUWorking[nP[i], j] = 0.0;
					else if (j == i){
						mLWorking[nP[i], j] = dRowFirstElementValue;
						mUWorking[nP[i], j] = 1.0;
					} else {
						mUWorking[nP[i], j] /= dRowFirstElementValue;
						mLWorking[nP[i], j] = 0.0;
					}

				}

 
				for (int k = i + 1; k < m_nNumRows; k++){
					dRowFirstElementValue = mUWorking[nP[k], i];

					for (int j = 0; j < m_nNumRows; j++){
						if (j < i){
							mUWorking[nP[k], j] = 0.0;
						} else if (j == i) {
							mLWorking[nP[k], j] = dRowFirstElementValue;
							mUWorking[nP[k], j] = 0.0;
						} else {
							mUWorking[nP[k], j] = mUWorking[nP[k], j] - dRowFirstElementValue * mUWorking[nP[i], j];
						}
					}
				}
			}

			for (int i = 0; i < m_nNumRows; i++){
				for (int j = 0; j < m_nNumRows; j++){
					mU[i, j] = mUWorking[nP[i], j];
					mL[i, j] = mLWorking[nP[i], j];
				}
			}

			resRet.U = mU;
			resRet.L = mL;

			resRet.PivotArray = nP;

			return resRet;

		}

		public Matrix Invert(){
			Debug.Assert(m_nNumRows == m_nNumColumns);
			return SolveFor(Identity(m_nNumRows));
		}
	}

	public class LUDecompositionResults{
		private Matrix m_matL;
		private Matrix m_matU;
		private int[] m_nPivotArray;

		private LUDecompositionResultStatus m_enuStatus = LUDecompositionResultStatus.OK;

		public LUDecompositionResults() { }

		public LUDecompositionResults(Matrix matL, Matrix matU, int[] nPivotArray, LUDecompositionResultStatus enuStatus){
			m_matL = matL;
			m_matU = matU;
			m_nPivotArray = nPivotArray;
			m_enuStatus = enuStatus;
		}

		public Matrix L{
			get { return m_matL; }
			set { m_matL = value; }
		}

		public Matrix U{
			get { return m_matU; }
			set { m_matU = value; }
		}

		public int[] PivotArray{
			get { return m_nPivotArray; }
			set { m_nPivotArray = value; }
		}

		public LUDecompositionResultStatus Status{
			get { return m_enuStatus; }
			set { m_enuStatus = value; }
		}

	}

}

using System;
using System.Collections.Generic;
using static System.MathF;

namespace RayTracer
{
    public class Matrix : System.IEquatable<Matrix>
    {
        public Matrix Inverse()
        {
            float invDet = 1.0f / Determinant();

            Matrix invMat = new Matrix(Size);

            for (int row = 0; row < Size; ++row)
            {
                for (int col = 0; col < Size; ++col)
                {
                    invMat[col, row] = Cofactor(row, col) * invDet;
                }
            }

            return invMat;
        }

        public bool IsInvertible()
        {
            return Abs( Determinant() ) > Constants.floatEps;
        }

        public float Cofactor(int row, int col)
        {
            return Minor(row, col) * (((row + col) % 2) == 0 ? 1 : -1);
        }

        public float Minor(int row, int col)
        {
            return SubMatrix(row, col).Determinant();
        }

        public Matrix SubMatrix(int row, int col)
        {
            Matrix sub = new Matrix(Size - 1);

            int destRow = 0;
            for (int srcRow = 0; srcRow < Size; ++srcRow)
            {
                if ( srcRow != row )
                {
                    int destCol = 0;
                    for (int srcCol = 0; srcCol < Size; ++srcCol)
                    {
                        if (srcCol != col)
                        {
                            sub[destRow, destCol] = this[srcRow, srcCol];
                            ++destCol;
                        }
                    }
                    ++destRow;
                }
            }

            return sub;
        }

        public float Determinant()
        {
            if (Size == 2)
            {
                return M[0] * M[5] - M[1] * M[4];
            }

            float determinant = 0;

            for (int col = 0; col < Size; ++col)
            {
                determinant += this[0, col] * Cofactor(0, col);
            }

            return determinant;
        }

        public Matrix Transpose()
        {
            return new Matrix(
                M[0], M[4], M[8], M[12],
                M[1], M[5], M[9], M[13],
                M[2], M[6], M[10], M[14],
                M[3], M[7], M[11], M[15]);
        }

        public static Matrix Identity()
        {
            return new Matrix(
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is Matrix m)
            {
                return Equals(m);
            }

            return false;
        }

        public bool Equals(Matrix other)
        {
            return Abs(M[0] - other.M[0]) < Constants.floatEps && Abs(M[1] - other.M[1]) < Constants.floatEps && Abs(M[2] - other.M[2]) < Constants.floatEps && Abs(M[3] - other.M[3]) < Constants.floatEps &&
                Abs(M[4] - other.M[4]) < Constants.floatEps && Abs(M[5] - other.M[5]) < Constants.floatEps && Abs(M[6] - other.M[6]) < Constants.floatEps && Abs(M[7] - other.M[7]) < Constants.floatEps &&
                Abs(M[8] - other.M[8]) < Constants.floatEps && Abs(M[9] - other.M[9]) < Constants.floatEps && Abs(M[10] - other.M[10]) < Constants.floatEps && Abs(M[11] - other.M[11]) < Constants.floatEps &&
                Abs(M[12] - other.M[12]) < Constants.floatEps && Abs(M[13] - other.M[13]) < Constants.floatEps && Abs(M[14] - other.M[14]) < Constants.floatEps && Abs(M[15] - other.M[15]) < Constants.floatEps;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(M[0]); hash.Add(M[1]); hash.Add(M[2]); hash.Add(M[3]);
            hash.Add(M[4]); hash.Add(M[5]); hash.Add(M[6]); hash.Add(M[7]);
            hash.Add(M[8]); hash.Add(M[9]); hash.Add(M[10]); hash.Add(M[11]);
            hash.Add(M[12]); hash.Add(M[13]); hash.Add(M[14]); hash.Add(M[15]);
            return hash.ToHashCode();
        }

        public static Matrix operator *(Matrix lhs, Matrix rhs)
        {
            Matrix m = new Matrix(4);

            for (int row = 0; row < 4; ++row)
            {
                for (int col = 0; col < 4; ++col)
                {
                    m[row, col] = lhs[row, 0] * rhs[0, col] +
                                lhs[row, 1] * rhs[1, col] +
                                lhs[row, 2] * rhs[2, col] +
                                lhs[row, 3] * rhs[3, col];
                }
            }

            return m;
        }

        public static Tuple operator *(Matrix lhs, Tuple rhs)
        {
            Tuple t = new Tuple(0, 0, 0, 0);

            t.X = lhs.M[0] * rhs.X + lhs.M[1] * rhs.Y + lhs.M[2] * rhs.Z + lhs.M[3] * rhs.W;
            t.Y = lhs.M[4] * rhs.X + lhs.M[5] * rhs.Y + lhs.M[6] * rhs.Z + lhs.M[7] * rhs.W;
            t.Z = lhs.M[8] * rhs.X + lhs.M[9] * rhs.Y + lhs.M[10] * rhs.Z + lhs.M[11] * rhs.W;
            t.W = lhs.M[12] * rhs.X + lhs.M[13] * rhs.Y + lhs.M[14] * rhs.Z + lhs.M[15] * rhs.W;

            return t;
        }

        public float this[int row, int col]
        {
            get
            {
                return M[row * 4 + col];
            }
            set
            {
                M[row * 4 + col] = value;
            }
        }

        public Matrix(float m00, float m01, float m02, float m03,
            float m10, float m11, float m12, float m13,
            float m20, float m21, float m22, float m23,
            float m30, float m31, float m32, float m33)
        {
            Size = 4;
            M = new float[16];

            M[0] = m00; M[1] = m01; M[2] = m02; M[3] = m03;
            M[4] = m10; M[5] = m11; M[6] = m12; M[7] = m13;
            M[8] = m20; M[9] = m21; M[10] = m22; M[11] = m23;
            M[12] = m30; M[13] = m31; M[14] = m32; M[15] = m33;
        }

        public Matrix(float m00, float m01, float m02,
            float m10, float m11, float m12,
            float m20, float m21, float m22)
        {
            Size = 3;
            M = new float[16];

            M[0] = m00; M[1] = m01; M[2] = m02;
            M[4] = m10; M[5] = m11; M[6] = m12;
            M[8] = m20; M[9] = m21; M[10] = m22;
        }

        public Matrix(float m00, float m01,
            float m10, float m11)
        {
            Size = 2;
            M = new float[16];

            M[0] = m00; M[1] = m01;
            M[4] = m10; M[5] = m11;
        }

        public Matrix(int size)
        {
            Size = size;
            M = new float[16];
        }

        public float[] M { get; private set; }
        public int Size { get; private set; }
    }
}

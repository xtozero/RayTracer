using System;
using System.Collections.Generic;

namespace RayTracer
{
    public class Matrix : System.IEquatable<Matrix>
    {
        public Matrix Inverse()
        {
            float det = Determinant();

            Matrix invMat = new Matrix(Size);

            for (int row = 0; row < Size; ++row)
            {
                for (int col = 0; col < Size; ++col)
                {
                    invMat[col, row] = Cofactor(row, col) / det;
                }
            }

            return invMat;
        }

        public bool IsInvertible()
        {
            return Determinant() != 0;
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

            IEnumerator<float> it = SubMatrixSequence(row, col).GetEnumerator();
            it.MoveNext();
            for (int j = 0; j < sub.Size; ++j)
            {
                for (int i = 0; i < sub.Size; ++i)
                {
                    sub[j, i] = it.Current;
                    it.MoveNext();
                }
            }

            return sub;
        }

        private IEnumerable<float> SubMatrixSequence(int row, int col)
        {
            for (int j = 0; j < Size; ++j)
            {
                for (int i = 0; i < Size; ++i)
                {
                    if (j != row && i != col)
                    {
                        yield return M[j, i];
                    }
                }
            }
        }

        public float Determinant()
        {
            if (Size == 2)
            {
                return M[0, 0] * M[1, 1] - M[0, 1] * M[1, 0];
            }

            float determinant = 0;

            for (int col = 0; col < Size; ++col)
            {
                determinant += M[0, col] * Cofactor(0, col);
            }

            return determinant;
        }

        public Matrix Transpose()
        {
            return new Matrix(
                M[0, 0], M[1, 0], M[2, 0], M[3, 0],
                M[0, 1], M[1, 1], M[2, 1], M[3, 1],
                M[0, 2], M[1, 2], M[2, 2], M[3, 2],
                M[0, 3], M[1, 3], M[2, 3], M[3, 3]);
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

        public bool Equals(Matrix m)
        {
            return System.MathF.Abs(M[0, 0] - m[0, 0]) <= Constants.floatEps && System.MathF.Abs(M[0, 1] - m[0, 1]) <= Constants.floatEps && System.MathF.Abs(M[0, 2] - m[0, 2]) <= Constants.floatEps && System.MathF.Abs(M[0, 3] - m[0, 3]) <= Constants.floatEps &&
                System.MathF.Abs(M[1, 0] - m[1, 0]) <= Constants.floatEps && System.MathF.Abs(M[1, 1] - m[1, 1]) <= Constants.floatEps && System.MathF.Abs(M[1, 2] - m[1, 2]) <= Constants.floatEps && System.MathF.Abs(M[1, 3] - m[1, 3]) <= Constants.floatEps &&
                System.MathF.Abs(M[2, 0] - m[2, 0]) <= Constants.floatEps && System.MathF.Abs(M[2, 1] - m[2, 1]) <= Constants.floatEps && System.MathF.Abs(M[2, 2] - m[2, 2]) <= Constants.floatEps && System.MathF.Abs(M[2, 3] - m[2, 3]) <= Constants.floatEps &&
                System.MathF.Abs(M[3, 0] - m[3, 0]) <= Constants.floatEps && System.MathF.Abs(M[3, 1] - m[3, 1]) <= Constants.floatEps && System.MathF.Abs(M[3, 2] - m[3, 2]) <= Constants.floatEps && System.MathF.Abs(M[3, 3] - m[3, 3]) <= Constants.floatEps;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(M[0, 0]); hash.Add(M[0, 1]); hash.Add(M[0, 2]); hash.Add(M[0, 3]);
            hash.Add(M[1, 0]); hash.Add(M[1, 1]); hash.Add(M[1, 2]); hash.Add(M[1, 3]);
            hash.Add(M[2, 0]); hash.Add(M[2, 1]); hash.Add(M[2, 2]); hash.Add(M[2, 3]);
            hash.Add(M[3, 0]); hash.Add(M[3, 1]); hash.Add(M[3, 2]); hash.Add(M[3, 3]);
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

            t.X = lhs[0, 0] * rhs.X + lhs[0, 1] * rhs.Y + lhs[0, 2] * rhs.Z + lhs[0, 3] * rhs.W;
            t.Y = lhs[1, 0] * rhs.X + lhs[1, 1] * rhs.Y + lhs[1, 2] * rhs.Z + lhs[1, 3] * rhs.W;
            t.Z = lhs[2, 0] * rhs.X + lhs[2, 1] * rhs.Y + lhs[2, 2] * rhs.Z + lhs[2, 3] * rhs.W;
            t.W = lhs[3, 0] * rhs.X + lhs[3, 1] * rhs.Y + lhs[3, 2] * rhs.Z + lhs[3, 3] * rhs.W;

            return t;
        }

        public float this[int row, int col]
        {
            get
            {
                return M[row, col];
            }
            set
            {
                M[row, col] = value;
            }
        }

        public Matrix(float m00, float m01, float m02, float m03,
            float m10, float m11, float m12, float m13,
            float m20, float m21, float m22, float m23,
            float m30, float m31, float m32, float m33)
        {
            Size = 4;
            M = new float[4, 4];

            M[0, 0] = m00; M[0, 1] = m01; M[0, 2] = m02; M[0, 3] = m03;
            M[1, 0] = m10; M[1, 1] = m11; M[1, 2] = m12; M[1, 3] = m13;
            M[2, 0] = m20; M[2, 1] = m21; M[2, 2] = m22; M[2, 3] = m23;
            M[3, 0] = m30; M[3, 1] = m31; M[3, 2] = m32; M[3, 3] = m33;
        }

        public Matrix(float m00, float m01, float m02,
            float m10, float m11, float m12,
            float m20, float m21, float m22)
        {
            Size = 3;
            M = new float[4, 4];

            M[0, 0] = m00; M[0, 1] = m01; M[0, 2] = m02;
            M[1, 0] = m10; M[1, 1] = m11; M[1, 2] = m12;
            M[2, 0] = m20; M[2, 1] = m21; M[2, 2] = m22;
        }

        public Matrix(float m00, float m01,
            float m10, float m11)
        {
            Size = 2;
            M = new float[4, 4];

            M[0, 0] = m00; M[0, 1] = m01;
            M[1, 0] = m10; M[1, 1] = m11;
        }

        public Matrix(int size)
        {
            Size = size;
            M = new float[4, 4];
        }

        public float[,] M { get; set; }
        public int Size { get; private set; }
    }
}

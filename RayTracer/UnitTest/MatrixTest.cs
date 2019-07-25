using RayTracer;
using Xunit;

namespace UnitTest
{
    public class MatrixTest
    {
        [Fact]
        public void TestCase01()
        {
            Matrix m = new Matrix(
                1, 2, 3, 4,
                5.5f, 6.5f, 7.5f, 8.5f,
                9, 10, 11, 12,
                13.5f, 14.5f, 15.5f, 16.5f);

            Assert.Equal(1, m[0, 0]);
            Assert.Equal(4, m[0, 3]);
            Assert.Equal(5.5f, m[1, 0]);
            Assert.Equal(7.5f, m[1, 2]);
            Assert.Equal(11, m[2, 2]);
            Assert.Equal(13.5f, m[3, 0]);
            Assert.Equal(15.5f, m[3, 2]);
        }

        [Fact]
        public void TestCase02()
        {
            Matrix m = new Matrix(
                -3, 5,
                1, -2);

            Assert.Equal(-3, m[0, 0]);
            Assert.Equal(5, m[0, 1]);
            Assert.Equal(1, m[1, 0]);
            Assert.Equal(-2, m[1, 1]);

            m = new Matrix(
                -3, 5, 0,
                1, -2, -7,
                0, 1, 1);

            Assert.Equal(-3, m[0, 0]);
            Assert.Equal(-2, m[1, 1]);
            Assert.Equal(1, m[2, 2]);
        }

        [Fact]
        public void TestCase03()
        {
            Matrix a = new Matrix(
                1, 2, 3, 4,
                5, 6, 7, 8,
                9, 8, 7, 6,
                5, 4, 3, 2);

            Matrix b = new Matrix(
                1, 2, 3, 4,
                5, 6, 7, 8,
                9, 8, 7, 6,
                5, 4, 3, 2);

            Assert.Equal(a, b);

            a = new Matrix(
                1, 2, 3, 4,
                5, 6, 7, 8,
                9, 8, 7, 6,
                5, 4, 3, 2);

            b = new Matrix(
                2, 3, 4, 5,
                6, 7, 8, 9,
                8, 7, 6, 5,
                4, 3, 2, 1);

            Assert.NotEqual(a, b);
        }

        [Fact]
        public void TestCase04()
        {
            Matrix a = new Matrix(
                1, 2, 3, 4,
                5, 6, 7, 8,
                9, 8, 7, 6,
                5, 4, 3, 2);

            Matrix b = new Matrix(
                -2, 1, 2, 3,
                3, 2, 1, -1,
                4, 3, 6, 5,
                1, 2, 7, 8);

            Matrix axb = new Matrix(
                20, 22, 50, 48,
                44, 54, 114, 108,
                40, 58, 110, 102,
                16, 26, 46, 42);

            Assert.Equal(axb, a * b);
        }

        [Fact]
        public void TestCase05()
        {
            Matrix a = new Matrix(
                1, 2, 3, 4,
                2, 4, 4, 2,
                8, 6, 4, 1,
                0, 0, 0, 1);

            Tuple b = new Tuple(1, 2, 3, 1);

            Assert.Equal(new Tuple(18, 24, 33, 1), a * b);
        }

        [Fact]
        public void TestCase06()
        {
            Matrix a = new Matrix(
                0, 1, 2, 4,
                1, 2, 4, 8,
                2, 4, 8, 16,
                4, 8, 16, 32);

            Assert.Equal(a, a * Matrix.Identity);
        }

        [Fact]
        public void TestCase07()
        {
            Matrix a = new Matrix(
                0, 9, 3, 0,
                9, 8, 0, 8,
                1, 8, 5, 3,
                0, 0, 5, 8);

            Matrix transposeA = new Matrix(
                0, 9, 1, 0,
                9, 8, 8, 0,
                3, 0, 5, 5,
                0, 8, 3, 8);

            Assert.Equal(transposeA, a.Transpose());
        }

        [Fact]
        public void TestCase08()
        {
            Matrix a = new Matrix(
                1, 5,
                -3, 2);

            Assert.Equal(17, a.Determinant());
        }

        [Fact]
        public void TestCase09()
        {
            Matrix a = new Matrix(
                1, 5, 0,
                -3, 2, 7,
                0, 6, -3);

            Matrix subA = new Matrix(
                -3, 2,
                0, 6);

            Assert.Equal(subA, a.SubMatrix(0, 2));

            a = new Matrix(
                -6, 1, 1, 6,
                -8, 5, 8, 6,
                -1, 0, 8, 2,
                -7, 1, -1, 1);

            subA = new Matrix(
                -6, 1, 6,
                -8, 8, 6,
                -7, -1, 1);

            Assert.Equal(subA, a.SubMatrix(2, 1));
        }

        [Fact]
        public void TestCase10()
        {
            Matrix a = new Matrix(
                3, 5, 0,
                2, -1, -7,
                6, -1, 5);

            Matrix b = a.SubMatrix(1, 0);

            Assert.Equal(25, b.Determinant());
            Assert.Equal(25, a.Minor(1, 0));
        }

        [Fact]
        public void TestCase11()
        {
            Matrix a = new Matrix(
                3, 5, 0,
                2, -1, -7,
                6, -1, 5);

            Assert.Equal(-12, a.Minor(0, 0));
            Assert.Equal(-12, a.Cofactor(0, 0));
            Assert.Equal(25, a.Minor(1, 0));
            Assert.Equal(-25, a.Cofactor(1, 0));
        }

        [Fact]
        public void TestCase12()
        {
            Matrix a = new Matrix(
                1, 2, 6,
                -5, 8, -4,
                2, 6, 4);

            Assert.Equal(56, a.Cofactor(0, 0));
            Assert.Equal(12, a.Cofactor(0, 1));
            Assert.Equal(-46, a.Cofactor(0, 2));
            Assert.Equal(-196, a.Determinant());

            a = new Matrix(
                -2, -8, 3, 5,
                -3, 1, 7, 3,
                1, 2, -9, 6,
                -6, 7, 7, -9);

            Assert.Equal(690, a.Cofactor(0, 0));
            Assert.Equal(447, a.Cofactor(0, 1));
            Assert.Equal(210, a.Cofactor(0, 2));
            Assert.Equal(51, a.Cofactor(0, 3));
            Assert.Equal(-4071, a.Determinant());
        }

        [Fact]
        public void TestCase13()
        {
            Matrix a = new Matrix(
                6, 4, 4, 4,
                5, 5, 7, 6,
                4, -9, 3, -7,
                9, 1, 7, -6);

            Assert.True(a.IsInvertible());

            a = new Matrix(
                -4, 2, -2, -3,
                9, 6, 2, 6,
                0, -5, 1, -5,
                0, 0, 0, 0);

            Assert.False(a.IsInvertible());
        }

        [Fact]
        public void TestCase14()
        {
            Matrix a = new Matrix(
                -5, 2, 6, -8,
                1, -5, 1, 8,
                7, 7, -6, -7,
                1, -3, 7, 4);

            Matrix b = a.Inverse();

            Assert.Equal(532, a.Determinant());
            Assert.Equal(-160, a.Cofactor(2, 3));
            Assert.Equal(-160.0f / 532, b[3, 2]);
            Assert.Equal(105, a.Cofactor(3, 2));
            Assert.Equal(105.0f / 532, b[2, 3]);

            Matrix inverseA = new Matrix(
                0.21805f, 0.45113f, 0.24060f, -0.04511f,
                -0.80827f, -1.45677f, -0.44361f, 0.52068f,
                -0.07895f, -0.22368f, -0.05263f, 0.19737f,
                -0.52256f, -0.81391f, -0.30075f, 0.30639f);

            Assert.Equal(inverseA, b);
        }

        [Fact]
        public void TestCase15()
        {
            Matrix a = new Matrix(
                8, -5, 9, 2,
                7, 5, 6, 1,
                -6, 0, 9, 6,
                -3, 0, -9, -4);

            Matrix inverseA = new Matrix(
                -0.15385f, -0.15385f, -0.28205f, -0.53846f,
                -0.07692f, 0.12308f, 0.02564f, 0.03077f,
                0.35897f, 0.35897f, 0.43590f, 0.92308f,
                -0.69231f, -0.69231f, -0.76923f, -1.92308f);

            Assert.Equal(inverseA, a.Inverse());

            a = new Matrix(
                9, 3, 0, 9,
                -5, -2, -6, -3,
                -4, 9, 6, 4,
                -7, 6, 6, 2);

            inverseA = new Matrix(
                -0.04074f, -0.07778f, 0.14444f, -0.22222f,
                -0.07778f, 0.03333f, 0.36667f, -0.33333f,
                -0.02901f, -0.14630f, -0.10926f, 0.12963f,
                0.17778f, 0.06667f, -0.26667f, 0.33333f);

            Assert.Equal(inverseA, a.Inverse());
        }

        [Fact]
        public void TestCase16()
        {
            Matrix a = new Matrix(
                3, -9, 7, 3,
                3, -8, 2, -9,
                -4, 4, 4, 1,
                -6, 5, -1, 1);

            Matrix b = new Matrix(
                8, 2, 2, 2,
                3, -1, 7, 0,
                7, 0, 5, 4,
                6, -2, 0, 5);

            Matrix c = a * b;

            Assert.Equal(a, c * b.Inverse());
        }
    }
}

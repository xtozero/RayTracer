using Xunit;
using RayTracer;

namespace UnitTest
{
    public static class ViewTransformTest
    {
        [Fact]
        private static void TestCase01()
        {
            Tuple from = Tuple.Point(0, 0, 0);
            Tuple to = Tuple.Point(0, 0, -1);
            Tuple up = Tuple.Vector(0, 1, 0);

            Matrix t = Transformation.LookAt(from, to, up);

            Assert.Equal(Matrix.Identity(), t);
        }

        [Fact]
        private static void TestCase02()
        {
            Tuple from = Tuple.Point(0, 0, 0);
            Tuple to = Tuple.Point(0, 0, 1);
            Tuple up = Tuple.Vector(0, 1, 0);

            Matrix t = Transformation.LookAt(from, to, up);

            Assert.Equal(Transformation.Scaling(-1, 1, -1), t);
        }

        [Fact]
        private static void TestCase03()
        {
            Tuple from = Tuple.Point(0, 0, 8);
            Tuple to = Tuple.Point(0, 0, 0);
            Tuple up = Tuple.Vector(0, 1, 0);

            Matrix t = Transformation.LookAt(from, to, up);

            Assert.Equal(Transformation.Translation(0, 0, -8), t);
        }

        [Fact]
        private static void TestCase04()
        {
            Tuple from = Tuple.Point(1, 3, 2);
            Tuple to = Tuple.Point(4, -2, 8);
            Tuple up = Tuple.Vector(1, 1, 0);

            Matrix t = Transformation.LookAt(from, to, up);

            Matrix expected = new Matrix(
                                        -0.50709f,  0.50709f,   0.67612f,   -2.36643f,
                                        0.76772f,   0.60609f,   0.12122f,   -2.82843f,
                                        -0.35857f,  0.59761f,   -0.71714f,  0.00000f,
                                        0.00000f,   0.00000f,   0.00000f,   1.00000f);

            Assert.Equal(expected, t);
        }
    }
}

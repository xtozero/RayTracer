using RayTracer;
using Xunit;

namespace UnitTest
{
    public static class TransformationTest
    {
        [Fact]
        private static void TestCase01()
        {
            Matrix transform = Transformation.Translation(5, -3, 2);
            Tuple p = Tuple.Point(-3, 4, 5);

            Assert.Equal(Tuple.Point(2, 1, 7), transform * p);

            Matrix inv = transform.Inverse();

            Assert.Equal(Tuple.Point(-8, 7, 3), inv * p);

            Tuple v = Tuple.Vector(-3, 4, 5);

            Assert.Equal(v, transform * v);
        }

        [Fact]
        private static void TestCase02()
        {
            Matrix transform = Transformation.Scaling(2, 3, 4);
            Tuple p = Tuple.Point(-4, 6, 8);

            Assert.Equal(Tuple.Point(-8, 18, 32), transform * p);

            Tuple v = Tuple.Vector(-4, 6, 8);

            Assert.Equal(Tuple.Vector(-8, 18, 32), transform * v);

            Matrix inv = transform.Inverse();

            Assert.Equal(Tuple.Vector(-2, 2, 2), inv * v);

            // Reflection is scaling by a negative value
            transform = Transformation.Scaling(-1, 1, 1);
            p = Tuple.Point(2, 3, 4);

            Assert.Equal(Tuple.Point(-2, 3, 4), transform * p);
        }

        [Fact]
        private static void TestCase03()
        {
            Tuple p = Tuple.Point(0, 1, 0);
            Matrix halfQuarter = Transformation.RotationX(System.MathF.PI / 4);
            Matrix fullQuarter = Transformation.RotationX(System.MathF.PI / 2);

            Assert.Equal(Tuple.Point(0, System.MathF.Sqrt(2) / 2, System.MathF.Sqrt(2) / 2), halfQuarter * p);
            Assert.Equal(Tuple.Point(0, 0, 1), fullQuarter * p);

            Matrix inv = halfQuarter.Inverse();
            Assert.Equal(Tuple.Point(0, System.MathF.Sqrt(2) / 2, -System.MathF.Sqrt(2) / 2), inv * p);
        }

        [Fact]
        private static void TestCase04()
        {
            Tuple p = Tuple.Point(0, 0, 1);
            Matrix halfQuarter = Transformation.RotationY(System.MathF.PI / 4);
            Matrix fullQuarter = Transformation.RotationY(System.MathF.PI / 2);

            Assert.Equal(Tuple.Point(System.MathF.Sqrt(2) / 2, 0, System.MathF.Sqrt(2) / 2), halfQuarter * p);
            Assert.Equal(Tuple.Point(1, 0, 0), fullQuarter * p);
        }

        [Fact]
        private static void TestCase05()
        {
            Tuple p = Tuple.Point(0, 1, 0);
            Matrix halfQuarter = Transformation.RotationZ(System.MathF.PI / 4);
            Matrix fullQuarter = Transformation.RotationZ(System.MathF.PI / 2);

            Assert.Equal(Tuple.Point(-System.MathF.Sqrt(2) / 2, System.MathF.Sqrt(2) / 2, 0), halfQuarter * p);
            Assert.Equal(Tuple.Point(-1, 0, 0), fullQuarter * p);
        }

        [Fact]
        private static void TestCase06()
        {
            Matrix transform = Transformation.Shearing(1, 0, 0, 0, 0, 0);
            Tuple p = Tuple.Point(2, 3, 4);

            Assert.Equal(Tuple.Point(5, 3, 4), transform * p);

            transform = Transformation.Shearing(0, 1, 0, 0, 0, 0);

            Assert.Equal(Tuple.Point(6, 3, 4), transform * p);

            transform = Transformation.Shearing(0, 0, 1, 0, 0, 0);

            Assert.Equal(Tuple.Point(2, 5, 4), transform * p);

            transform = Transformation.Shearing(0, 0, 0, 1, 0, 0);

            Assert.Equal(Tuple.Point(2, 7, 4), transform * p);

            transform = Transformation.Shearing(0, 0, 0, 0, 1, 0);

            Assert.Equal(Tuple.Point(2, 3, 6), transform * p);

            transform = Transformation.Shearing(0, 0, 0, 0, 0, 1);

            Assert.Equal(Tuple.Point(2, 3, 7), transform * p);
        }

        [Fact]
        private static void TestCase07()
        {
            Tuple p = Tuple.Point(1, 0, 1);
            Matrix a = Transformation.RotationX(System.MathF.PI / 2);
            Matrix b = Transformation.Scaling(5, 5, 5);
            Matrix c = Transformation.Translation(10, 5, 7);

            Tuple p2 = a * p;

            Assert.Equal(Tuple.Point(1, -1, 0), p2);

            Tuple p3 = b * p2;

            Assert.Equal(Tuple.Point(5, -5, 0), p3);

            Tuple p4 = c * p3;

            Assert.Equal(Tuple.Point(15, 0, 7), p4);

            Matrix t = c * b * a;
            Assert.Equal(p4, t * p);
        }
    }
}

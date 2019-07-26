using RayTracer;
using Xunit;

namespace UnitTest
{
    public class TransformationTest
    {
        [Fact]
        public void TestCase01()
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
        public void TestCase02()
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
        public void TestCase03()
        {
            Tuple p = Tuple.Point(0, 1, 0);
            Matrix halfQuarter = Transformation.RotationX(System.MathF.PI / 4);
            Matrix fullQuarter = Transformation.RotationX(System.MathF.PI / 2);

            Assert.Equal(Tuple.Point(0, System.MathF.Sqrt(2) / 2, System.MathF.Sqrt(2) / 2), halfQuarter * p);
            Assert.Equal(Tuple.Point(0, 0, 1), fullQuarter * p);

            Matrix inv = halfQuarter.Inverse();
            Assert.Equal(Tuple.Point(0, System.MathF.Sqrt(2) / 2, -System.MathF.Sqrt(2) / 2), inv * p);
        }
    }
}

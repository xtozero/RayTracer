using RayTracer;
using Xunit;

namespace UnitTest
{
    public static class TupleTest
    {
        [Fact]
        private static void TestCase01()
        {
            Tuple a = new Tuple(4.3f, -4.2f, 3.1f, 1);
            Assert.Equal(4.3f, a.X);
            Assert.Equal(-4.2f, a.Y);
            Assert.Equal(3.1f, a.Z);
            Assert.Equal(1.0f, a.W);
            Assert.True(a.IsPoint());
            Assert.False(a.IsVector());

            a = new Tuple(4.3f, -4.2f, 3.1f, 0);
            Assert.Equal(4.3f, a.X);
            Assert.Equal(-4.2f, a.Y);
            Assert.Equal(3.1f, a.Z);
            Assert.Equal(0, a.W);
            Assert.False(a.IsPoint());
            Assert.True(a.IsVector());
        }

        [Fact]
        private static void TestCase02()
        {
            Tuple p = Tuple.Point(4, -4, 3);
            Assert.True(p.Equals(new Tuple(4, -4, 3, 1)));

            Tuple v = Tuple.Vector(4, -4, 3);
            Assert.True(v.Equals(new Tuple(4, -4, 3, 0)));
        }

        [Fact]
        private static void TestCase03()
        {
            Tuple a1 = new Tuple(3, -2, 5, 1);
            Tuple a2 = new Tuple(-2, 3, 1, 0);
            Assert.Equal(new Tuple(1, 1, 6, 1), a1 + a2);
        }

        [Fact]
        private static void TestCase04()
        {
            Tuple p1 = Tuple.Point(3, 2, 1);
            Tuple p2 = Tuple.Point(5, 6, 7);
            Assert.Equal(Tuple.Vector(-2, -4, -6), p1 - p2);
        }

        [Fact]
        private static void TestCase05()
        {
            Tuple p = Tuple.Point(3, 2, 1);
            Tuple v = Tuple.Vector(5, 6, 7);
            Assert.Equal(Tuple.Point(-2, -4, -6), p - v);
        }

        [Fact]
        private static void TestCase06()
        {
            Tuple v1 = Tuple.Vector(3, 2, 1);
            Tuple v2 = Tuple.Vector(5, 6, 7);
            Assert.Equal(Tuple.Vector(-2, -4, -6), v1 - v2);
        }

        [Fact]
        private static void TestCase07()
        {
            Tuple zero = Tuple.Vector(0, 0, 0);
            Tuple v = Tuple.Vector(1, -2, 3);
            Assert.Equal(Tuple.Vector(-1, 2, -3), zero - v);
        }

        [Fact]
        private static void TestCase08()
        {
            Tuple a = new Tuple(1, -2, 3, -4);
            Assert.Equal(new Tuple(-1, 2, -3, 4), -a);
        }

        [Fact]
        private static void TestCase09()
        {
            Tuple a = new Tuple(1, -2, 3, -4);
            Assert.Equal(new Tuple(3.5f, -7, 10.5f, -14), a * 3.5f);

            a = new Tuple(1, -2, 3, -4);
            Assert.Equal(new Tuple(0.5f, -1, 1.5f, -2), a * 0.5f);
        }

        [Fact]
        private static void TestCase10()
        {
            Tuple a = new Tuple(1, -2, 3, -4);
            Assert.Equal(new Tuple(0.5f, -1, 1.5f, -2), a / 2);
        }

        [Fact]
        private static void TestCase11()
        {
            Tuple v = Tuple.Vector(1, 0, 0);
            Assert.Equal(1, v.Magnitude());

            v = Tuple.Vector(0, 1, 0);
            Assert.Equal(1, v.Magnitude());

            v = Tuple.Vector(0, 0, 1);
            Assert.Equal(1, v.Magnitude());

            v = Tuple.Vector(1, 2, 3);
            Assert.Equal(System.MathF.Sqrt(14), v.Magnitude());

            v = Tuple.Vector(-1, -2, -3);
            Assert.Equal(System.MathF.Sqrt(14), v.Magnitude());
        }

        [Fact]
        private static void TestCase12()
        {
            Tuple v = Tuple.Vector(4, 0, 0);
            Assert.Equal(Tuple.Vector(1, 0, 0), v.Normalize());

            v = Tuple.Vector(1, 2, 3);
            float sqrt14 = System.MathF.Sqrt(14);
            Assert.Equal(Tuple.Vector(1 / sqrt14, 2 / sqrt14, 3 / sqrt14), v.Normalize());

            v = Tuple.Vector(1, 2, 3);
            Tuple norm = v.Normalize();
            Assert.Equal(1, norm.Magnitude(), 5);
        }

        [Fact]
        private static void TestCase13()
        {
            Tuple a = Tuple.Vector(1, 2, 3);
            Tuple b = Tuple.Vector(2, 3, 4);
            Assert.Equal(20, a.Dot(b));
        }

        [Fact]
        private static void TestCase14()
        {
            Tuple a = Tuple.Vector(1, 2, 3);
            Tuple b = Tuple.Vector(2, 3, 4);
            Assert.Equal(Tuple.Vector(-1, 2, -1), a.Cross(b));
            Assert.Equal(Tuple.Vector(1, -2, 1), b.Cross(a));
        }

        [Fact]
        private static void TestCase15()
        {
            Tuple c = Tuple.Color(-0.5f, 0.4f, 1.7f);
            Assert.Equal(-0.5f, c.R);
            Assert.Equal(0.4f, c.G);
            Assert.Equal(1.7f, c.B);
        }

        [Fact]
        private static void TestCase16()
        {
            Tuple c1 = Tuple.Color(0.9f, 0.6f, 0.75f);
            Tuple c2 = Tuple.Color(0.7f, 0.1f, 0.25f);
            Assert.Equal(Tuple.Color(1.6f, 0.7f, 1), c1 + c2);

            c1 = Tuple.Color(0.9f, 0.6f, 0.75f);
            c2 = Tuple.Color(0.7f, 0.1f, 0.25f);
            Assert.Equal(Tuple.Color(0.2f, 0.5f, 0.5f), c1 - c2);

            Tuple c = Tuple.Color(0.2f, 0.3f, 0.4f);
            Assert.Equal(Tuple.Color(0.4f, 0.6f, 0.8f), c * 2);
        }

        [Fact]
        private static void TestCase17()
        {
            Tuple c1 = Tuple.Color(1, 0.2f, 0.4f);
            Tuple c2 = Tuple.Color(0.9f, 1, 0.1f);
            Assert.Equal(Tuple.Color(0.9f, 0.2f, 0.04f), c1 * c2);
        }
    }
}

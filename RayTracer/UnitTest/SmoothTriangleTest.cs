using RayTracer;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace UnitTest
{
    public static class SmoothTriangleTest
    {
        private static SmoothTriangle ConstructSmoothTriangle()
        {
            return new SmoothTriangle(
                        Tuple.Point(0, 1, 0),
                        Tuple.Point(-1, 0, 0),
                        Tuple.Point(1, 0, 0),
                        Tuple.Vector(0, 1, 0),
                        Tuple.Vector(-1, 0, 0),
                        Tuple.Vector(1, 0, 0));
        }

        [Fact]
        private static void TestCase01()
        {
            // Constructing a smooth triangle
            SmoothTriangle tri = ConstructSmoothTriangle();

            Assert.Equal(Tuple.Point(0, 1, 0), tri.P1);
            Assert.Equal(Tuple.Point(-1, 0, 0), tri.P2);
            Assert.Equal(Tuple.Point(1, 0, 0), tri.P3);
            Assert.Equal(Tuple.Vector(0, 1, 0), tri.N1);
            Assert.Equal(Tuple.Vector(-1, 0, 0), tri.N2);
            Assert.Equal(Tuple.Vector(1, 0, 0), tri.N3);
        }

        [Fact]
        private static void TestCase02()
        {
            // Accessing 'u', 'v' is not supported in Intersection class
            Shape s = new Triangle(Tuple.Point(0, 1, 0), Tuple.Point(-1, 0, 0), Tuple.Point(1, 0, 0));
            Intersection i = new Intersection(3.5f, s);

            Assert.Throws<System.NotSupportedException>(() => i.U);
            Assert.Throws<System.NotSupportedException>(() => i.V);
        }

        [Fact]
        private static void TestCase03()
        {
            // An intersection can encapsulate 'u' and 'v'
            Shape s = new Triangle(Tuple.Point(0, 1, 0), Tuple.Point(-1, 0, 0), Tuple.Point(1, 0, 0));
            Intersection i = new IntersectionWithUV(3.5f, s, 0.2f, 0.4f);

            Assert.Equal(0.2f, i.U);
            Assert.Equal(0.4f, i.V);
        }

        [Fact]
        private static void TestCase04()
        {
            // An intersection with a smooth triangle stores u/v
            Shape tri = ConstructSmoothTriangle();
            Ray r = new Ray(Tuple.Point(-0.2f, 0.3f, -2), Tuple.Vector(0, 0, 1));

            MethodInfo mi = tri.GetType().GetMethod("LocalIntersect", BindingFlags.NonPublic | BindingFlags.Instance);

            List<Intersection> xs = (List<Intersection>)mi.Invoke(tri, new[] { r });

            Assert.Equal(0.45f, xs[0].U);
            Assert.Equal(0.25f, xs[0].V);
        }

        [Fact]
        private static void TestCase05()
        {
            // A smooth triangle uses u/v to interpolate the normal
            Shape tri = ConstructSmoothTriangle();
            Intersection i = new IntersectionWithUV(1, tri, 0.45f, 0.25f);

            Tuple n = tri.NormalAt(Tuple.Point(0, 0, 0), i);

            Assert.Equal(Tuple.Vector(-0.5547f, 0.83205f, 0), n);
        }

        [Fact]
        private static void TestCase06()
        {
            // Preparing the normal on smooth triangle
            Shape tri = ConstructSmoothTriangle();
            Intersection i = new IntersectionWithUV(1, tri, 0.45f, 0.25f);
            Ray r = new Ray(Tuple.Point(-0.2f, 0.3f, -2), Tuple.Vector(0, 0, 1));
            List<Intersection> xs = tri.Intersect(r);
            Computation comps = new Computation(i, r, xs);

            Assert.Equal(comps.Normalv, Tuple.Vector(-0.5547f, 0.83205f, 0));
        }
    }
}

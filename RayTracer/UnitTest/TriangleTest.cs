using RayTracer;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace UnitTest
{
    public static class TriangleTest
    {
        [Fact]
        private static void TestCase01()
        {
            // Constructing a triangle
            Tuple p1 = Tuple.Point(0, 1, 0);
            Tuple p2 = Tuple.Point(-1, 0, 0);
            Tuple p3 = Tuple.Point(1, 0, 0);

            Triangle t = new Triangle(p1, p2, p3);

            Assert.Equal(p1, t.P1);
            Assert.Equal(p2, t.P2);
            Assert.Equal(p3, t.P3);
            Assert.Equal(Tuple.Vector(-1, -1, 0), t.E1);
            Assert.Equal(Tuple.Vector(1, -1, 0), t.E2);
            Assert.Equal(Tuple.Vector(0, 0, -1), t.Normal);
        }

        [Fact]
        private static void TestCase02()
        {
            // Finding the normal on a triangle
            Triangle t = new Triangle(Tuple.Point(0, 1, 0),
                                    Tuple.Point(-1, 0, 0),
                                    Tuple.Point(1, 0, 0));
            MethodInfo mi = t.GetType().GetMethod("LocalNormalAt", BindingFlags.NonPublic | BindingFlags.Instance);
            Tuple n1 = (Tuple)mi.Invoke(t, new[] { Tuple.Point(0, 0.5f, 0), null });
            Tuple n2 = (Tuple)mi.Invoke(t, new[] { Tuple.Point(-0.5f, 0.75f, 0), null });
            Tuple n3 = (Tuple)mi.Invoke(t, new[] { Tuple.Point(0.5f, 0.25f, 0), null });

            Assert.Equal(n1, t.Normal);
            Assert.Equal(n2, t.Normal);
            Assert.Equal(n3, t.Normal);
        }

        [Fact]
        private static void TestCase03()
        {
            // Intersecting a ray parallel to the triangle
            Triangle t = new Triangle(Tuple.Point(0, 1, 0),
                                    Tuple.Point(-1, 0, 0),
                                    Tuple.Point(1, 0, 0));
            Ray r = new Ray(Tuple.Point(0, -1, -2), Tuple.Vector(0, 1, 0));
            MethodInfo mi = t.GetType().GetMethod("LocalIntersect", BindingFlags.NonPublic | BindingFlags.Instance);
            List<Intersection> xs = (List<Intersection>)mi.Invoke(t, new[] { r });

            Assert.Empty(xs);
        }

        [Fact]
        private static void TestCase04()
        {
            // A ray misses the p1-p3 edge
            Triangle t = new Triangle(Tuple.Point(0, 1, 0),
                                    Tuple.Point(-1, 0, 0),
                                    Tuple.Point(1, 0, 0));
            Ray r = new Ray(Tuple.Point(1, 1, -2), Tuple.Vector(0, 0, 1));
            MethodInfo mi = t.GetType().GetMethod("LocalIntersect", BindingFlags.NonPublic | BindingFlags.Instance);
            List<Intersection> xs = (List<Intersection>)mi.Invoke(t, new[] { r });

            Assert.Empty(xs);
        }

        [Fact]
        private static void TestCase05()
        {
            // A ray misses the p1-p2 edge
            Triangle t = new Triangle(Tuple.Point(0, 1, 0),
                                    Tuple.Point(-1, 0, 0),
                                    Tuple.Point(1, 0, 0));
            Ray r = new Ray(Tuple.Point(-1, 1, -2), Tuple.Vector(0, 0, 1));
            MethodInfo mi = t.GetType().GetMethod("LocalIntersect", BindingFlags.NonPublic | BindingFlags.Instance);
            List<Intersection> xs = (List<Intersection>)mi.Invoke(t, new[] { r });

            Assert.Empty(xs);

            // A ray misses the p2-p3 edge
            r = new Ray(Tuple.Point(0, -1, -2), Tuple.Vector(0, 0, 1));
            xs = (List<Intersection>)mi.Invoke(t, new[] { r });

            Assert.Empty(xs);
        }

        [Fact]
        private static void TestCase06()
        {
            // A ray strikes a triangle
            Triangle t = new Triangle(Tuple.Point(0, 1, 0),
                                    Tuple.Point(-1, 0, 0),
                                    Tuple.Point(1, 0, 0));
            Ray r = new Ray(Tuple.Point(0, 0.5f, -2), Tuple.Vector(0, 0, 1));
            MethodInfo mi = t.GetType().GetMethod("LocalIntersect", BindingFlags.NonPublic | BindingFlags.Instance);
            List<Intersection> xs = (List<Intersection>)mi.Invoke(t, new[] { r });

            Assert.Single(xs);
            Assert.Equal(2, xs[0].T);
        }
    }
}

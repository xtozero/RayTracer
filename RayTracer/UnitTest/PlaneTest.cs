using RayTracer;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace UnitTest
{
    public static class PlaneTest
    {
        [Fact]
        private static void TestCase01()
        {
            Plane p = new Plane();

            MethodInfo mi = p.GetType().GetMethod("LocalNormalAt", BindingFlags.NonPublic | BindingFlags.Instance);
            Tuple n1 = (Tuple)mi.Invoke(p, new[] { Tuple.Point(0, 0, 0), null });
            Tuple n2 = (Tuple)mi.Invoke(p, new[] { Tuple.Point(10, 0, -10), null });
            Tuple n3 = (Tuple)mi.Invoke(p, new[] { Tuple.Point(-5, 0, 150), null });

            Assert.Equal(Tuple.Vector(0, 1, 0), n1);
            Assert.Equal(Tuple.Vector(0, 1, 0), n2);
            Assert.Equal(Tuple.Vector(0, 1, 0), n3);
        }

        [Fact]
        private static void TestCase02()
        {
            Plane p = new Plane();
            Ray r = new Ray(Tuple.Point(0, 10, 0), Tuple.Vector(0, 0, 1));

            MethodInfo mi = p.GetType().GetMethod("LocalIntersect", BindingFlags.NonPublic | BindingFlags.Instance);
            List<Intersection> xs = (List<Intersection>)mi.Invoke(p, new[] { r });

            Assert.Empty(xs);

            r = new Ray(Tuple.Point(0, 0, 0), Tuple.Vector(0, 0, 1));
            xs = (List<Intersection>)mi.Invoke(p, new[] { r });

            Assert.Empty(xs);
        }

        [Fact]
        private static void TestCase03()
        {
            Plane p = new Plane();
            Ray r = new Ray(Tuple.Point(0, 1, 0), Tuple.Vector(0, -1, 0));

            MethodInfo mi = p.GetType().GetMethod("LocalIntersect", BindingFlags.NonPublic | BindingFlags.Instance);
            List<Intersection> xs = (List<Intersection>)mi.Invoke(p, new[] { r });

            Assert.Equal(1, xs[0].T);
            Assert.Same(p, xs[0].Object);

            r = new Ray(Tuple.Point(0, -1, 0), Tuple.Vector(0, 1, 0));
            xs = (List<Intersection>)mi.Invoke(p, new[] { r });

            Assert.Equal(1, xs[0].T);
            Assert.Same(p, xs[0].Object);
        }
    }
}

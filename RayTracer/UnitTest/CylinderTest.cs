using RayTracer;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace UnitTest
{
    public static class CylinderTest
    {
        [Fact]
        private static void TestCase01()
        {
            // A ray misses a cylinder
            Shape cyl = new Cylinder();
            var origin = new[] { Tuple.Point(1, 0, 0),
                                    Tuple.Point(0, 0, 0),
                                    Tuple.Point(0, 0, -5)};
            var direction = new[] { Tuple.Vector(0, 1, 0),
                                        Tuple.Vector(0, 1, 0),
                                        Tuple.Vector(1, 1, 1) };
            MethodInfo mi = cyl.GetType().GetMethod("LocalIntersect", BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < 3; ++i)
            {
                Ray r = new Ray(origin[i], direction[i].Normalize());
                List<Intersection> xs = (List<Intersection>)mi.Invoke(cyl, new[] { r });
                Assert.Empty(xs);
            }
        }

        [Fact]
        private static void TestCase02()
        {
            // A ray strikes a cylinder
            Shape cyl = new Cylinder();
            var origin = new[] { Tuple.Point(1, 0, -5),
                                    Tuple.Point(0, 0, -5),
                                    Tuple.Point(0.5f, 0, -5) };
            var direction = new[] { Tuple.Vector(0, 0, 1),
                                        Tuple.Vector(0, 0, 1),
                                        Tuple.Vector(0.1f, 1, 1) };
            var t1 = new[] { 5, 4, 6.80798f };
            var t2 = new[] { 5, 6, 7.08872f };
            MethodInfo mi = cyl.GetType().GetMethod("LocalIntersect", BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < 3; ++i)
            {
                Ray r = new Ray(origin[i], direction[i].Normalize());
                List<Intersection> xs = (List<Intersection>)mi.Invoke(cyl, new[] { r });
                Assert.Equal(2, xs.Count);
                Assert.Equal(t1[i], xs[0].T, 4);
                Assert.Equal(t2[i], xs[1].T, 4);
            }
        }

        [Fact]
        private static void TestCase03()
        {
            // Normal vector on a cylinder
            Shape cyl = new Cylinder();
            var point = new[] { Tuple.Point(1, 0, 0),
                                Tuple.Point(0, 5, -1),
                                Tuple.Point(0, -2, 1),
                                Tuple.Point(-1, 1, 0) };
            var normal = new[] { Tuple.Vector(1, 0, 0),
                                Tuple.Vector(0, 0, -1),
                                Tuple.Vector(0, 0, 1),
                                Tuple.Vector(-1, 0, 0) };
            MethodInfo mi = cyl.GetType().GetMethod("LocalNormalAt", BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < 4; ++i)
            {
                Tuple n = (Tuple)mi.Invoke(cyl, new[] { point[i], null });
                Assert.Equal(normal[i], n);
            }
        }

        [Fact]
        private static void TestCase04()
        {
            // The default minimum and maximum for a cylinder
            Cylinder cyl = new Cylinder();
            Assert.Equal(float.NegativeInfinity, cyl.Minimum, 5);
            Assert.Equal(float.PositiveInfinity, cyl.Maximum, 5);
        }

        [Fact]
        private static void TestCase05()
        {
            // Intersecting a constrained cylinder
            Shape cyl = new Cylinder
            {
                Minimum = 1,
                Maximum = 2
            };
            var origin = new[] { Tuple.Point(0, 1.5f, 0),
                                Tuple.Point(0, 3, -5),
                                Tuple.Point(0, 0, -5),
                                Tuple.Point(0, 2, -5),
                                Tuple.Point(0, 1, -5),
                                Tuple.Point(0, 1.5f, -2) };
            var direction = new[] { Tuple.Vector(0.1f, 1, 0),
                                Tuple.Vector(0, 0, 1),
                                Tuple.Vector(0, 0, 1),
                                Tuple.Vector(0, 0, 1),
                                Tuple.Vector(0, 0, 1),
                                Tuple.Vector(0, 0 , 1) };
            var count = new[] { 0, 0, 0, 0, 0, 2 };
            MethodInfo mi = cyl.GetType().GetMethod("LocalIntersect", BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < 6; ++i)
            {
                Ray r = new Ray(origin[i], direction[i].Normalize());
                List<Intersection> xs = (List<Intersection>)mi.Invoke(cyl, new[] { r });
                Assert.Equal(count[i], xs.Count);
            }
        }

        [Fact]
        private static void TestCase06()
        {
            // The default closed value for a cylinder
            Cylinder cyl = new Cylinder();
            Assert.False(cyl.Closed);
        }

        [Fact]
        private static void TestCase07()
        {
            // Intersecting the caps of a closed cylinder
            Shape cyl = new Cylinder
            {
                Minimum = 1,
                Maximum = 2,
                Closed = true
            };
            var origin = new[] { Tuple.Point(0, 3, 0),
                                Tuple.Point(0, 3, -2),
                                Tuple.Point(0, 4, -2),
                                Tuple.Point(0, 0, -2),
                                Tuple.Point(0, -1, -2) };
            var direction = new[] { Tuple.Vector(0, -1, 0),
                                Tuple.Vector(0, -1, 2),
                                Tuple.Vector(0, -1, 1),
                                Tuple.Vector(0, 1, 2),
                                Tuple.Vector(0, 1, 1) };
            var count = new[] { 2, 2, 2, 2, 2 };
            MethodInfo mi = cyl.GetType().GetMethod("LocalIntersect", BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < 5; ++i)
            {
                Ray r = new Ray(origin[i], direction[i].Normalize());
                List<Intersection> xs = (List<Intersection>)mi.Invoke(cyl, new[] { r });
                Assert.Equal(count[i], xs.Count);
            }
        }

        [Fact]
        private static void TestCase08()
        {
            // The normal vector on a cylinder's end caps
            Shape cyl = new Cylinder
            {
                Minimum = 1,
                Maximum = 2,
                Closed = true
            };
            var point = new[] { Tuple.Point(0, 1, 0),
                                Tuple.Point(0.5f, 1, 0),
                                Tuple.Point(0, 1, 0.5f),
                                Tuple.Point(0, 2, 0),
                                Tuple.Point(0.5f, 2, 0),
                                Tuple.Point(0, 2, 0.5f) };
            var normal = new[] { Tuple.Vector(0, -1, 0),
                                Tuple.Vector(0, -1, 0),
                                Tuple.Vector(0, -1, 0),
                                Tuple.Vector(0, 1, 0),
                                Tuple.Vector(0, 1, 0),
                                Tuple.Vector(0, 1, 0) };
            MethodInfo mi = cyl.GetType().GetMethod("LocalNormalAt", BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < 6; ++i)
            {
                Tuple n = (Tuple)mi.Invoke(cyl, new[] { point[i], null });
                Assert.Equal(normal[i], n);
            }
        }
    }
}

using RayTracer;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace UnitTest
{
    public static class CubeTest
    {
        [Fact]
        private static void TestCase01()
        {
            // A ray intersects a cube
            Shape c = new Cube();
            var origins = new[] { Tuple.Point(5, 0.5f, 0),
                                Tuple.Point(-5, 0.5f, 0),
                                Tuple.Point(0.5f, 5, 0),
                                Tuple.Point(0.5f, -5, 0),
                                Tuple.Point(0.5f, 0, 5),
                                Tuple.Point(0.5f, 0, -5),
                                Tuple.Point(0, 0.5f, 0)};
            var directions = new[] { Tuple.Vector(-1, 0, 0),
                                    Tuple.Vector(1, 0, 0),
                                    Tuple.Vector(0, -1, 0),
                                    Tuple.Vector(0, 1, 0),
                                    Tuple.Vector(0, 0, -1),
                                    Tuple.Vector(0, 0, 1),
                                    Tuple.Vector(0, 0, 1) };
            var expectedT1 = new[] { 4, 4, 4, 4, 4, 4, -1 };
            var expectedT2 = new[] { 6, 6, 6, 6, 6, 6, 1 };
            MethodInfo mi = c.GetType().GetMethod("LocalIntersect", BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < 7; ++i)
            {
                Ray r = new Ray(origins[i], directions[i]);
                List<Intersection> xs = (List<Intersection>)mi.Invoke(c, new[] { r });

                Assert.Equal(2, xs.Count);
                Assert.Equal(expectedT1[i], xs[0].T);
                Assert.Equal(expectedT2[i], xs[1].T);
            }
        }

        [Fact]
        private static void TestCase02()
        {
            // A ray misses a cube
            Shape c = new Cube();
            var origins = new[] { Tuple.Point(-2, 0, 0),
                                Tuple.Point(0, -2, 0),
                                Tuple.Point(0, 0, -2),
                                Tuple.Point(2, 0, 2),
                                Tuple.Point(0, 2, 2),
                                Tuple.Point(2, 2, 0) };
            var directions = new[] { Tuple.Vector(0.2673f, 0.5345f, 0.8018f),
                                    Tuple.Vector(0.8018f, 0.2673f, 0.5345f),
                                    Tuple.Vector(0.5345f, 0.8018f, 0.2673f),
                                    Tuple.Vector(0, 0, -1),
                                    Tuple.Vector(0, -1, 0),
                                    Tuple.Vector(-1, 0, 0) };
            MethodInfo mi = c.GetType().GetMethod("LocalIntersect", BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < 6; ++i)
            {
                Ray r = new Ray(origins[i], directions[i]);
                List<Intersection> xs = (List<Intersection>)mi.Invoke(c, new[] { r });

                Assert.Empty(xs);
            }
        }

        [Fact]
        private static void TestCase03()
        {
            // The normal on the surface of a cube
            Shape c = new Cube();
            var points = new[] { Tuple.Point(1, 0.5f, -0.8f),
                                Tuple.Point(-1, -0.2f, 0.9f),
                                Tuple.Point(-0.4f, 1, -0.1f),
                                Tuple.Point(-0.3f, -1, -0.7f),
                                Tuple.Point(-0.6f, 0.3f, 1),
                                Tuple.Point(0.4f, 0.4f, -1),
                                Tuple.Point(1, 1, 1),
                                Tuple.Point(-1, -1, -1) };
            var normals = new[] { Tuple.Vector(1, 0, 0),
                                Tuple.Vector(-1, 0, 0),
                                Tuple.Vector(0, 1, 0),
                                Tuple.Vector(0, -1, 0),
                                Tuple.Vector(0, 0, 1),
                                Tuple.Vector(0, 0, -1),
                                Tuple.Vector(1, 0, 0),
                                Tuple.Vector(-1, 0, 0) };
            MethodInfo mi = c.GetType().GetMethod("LocalNormalAt", BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < 8; ++i)
            {
                Tuple normal = (Tuple)mi.Invoke(c, new[] { points[i] });

                Assert.Equal(normals[i], normal);
            }
        }
    }
}

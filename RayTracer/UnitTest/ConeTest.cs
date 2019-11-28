using RayTracer;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using static System.MathF;

namespace UnitTest
{
    public static class ConeTest
    {
        [Fact]
        private static void TestCase01()
        {
            // Intersecting a cone with a ray
            Shape shape = new Cone();
            var origin = new[] { Tuple.Point(0, 0, -5),
                                Tuple.Point(0, 0, -5),
                                Tuple.Point(1, 1, -5) };
            var direction = new[] { Tuple.Vector(0, 0, 1),
                                    Tuple.Vector(1, 1, 1),
                                    Tuple.Vector(-0.5f, -1, 1) };
            var t0 = new[] { 5, 8.66025, 4.55006 };
            var t1 = new[] { 5, 8.66025, 49.44994 };

            MethodInfo mi = shape.GetType().GetMethod("LocalIntersect", BindingFlags.NonPublic | BindingFlags.Instance);
            for ( int i = 0; i < 3; ++i )
            {
                Ray r = new Ray(origin[i], direction[i].Normalize());
                List<Intersection> xs = (List<Intersection>)mi.Invoke(shape, new[] { r } );
                Assert.Equal(2, xs.Count);
                Assert.Equal(t0[i], xs[0].T, 3);
                Assert.Equal(t1[i], xs[1].T, 3);
            }
        }

        [Fact]
        private static void TestCase02()
        {
            // Intersecting a cone with a ray parallel to one of it's halves
            Shape shape = new Cone();

            MethodInfo mi = shape.GetType().GetMethod("LocalIntersect", BindingFlags.NonPublic | BindingFlags.Instance);
            Ray r = new Ray(Tuple.Point(0, 0, -1), Tuple.Vector(0, 1, 1).Normalize());
            List<Intersection> xs = (List<Intersection>)mi.Invoke(shape, new[] { r });
            Assert.Single(xs);
            Assert.Equal(0.35355, xs[0].T, 5);
        }

        [Fact]
        private static void TestCase03()
        {
            // Intersecting a cone's end caps
            Shape shape = new Cone
            {
                Minimum = -0.5f,
                Maximum = 0.5f,
                Closed = true
            };
            var origin = new[] { Tuple.Point(0, 0, -5),
                                Tuple.Point(0, 0, -0.25f),
                                Tuple.Point(0, 0, -0.25f) };
            var direction = new[] { Tuple.Vector(0, 1, 0),
                                    Tuple.Vector(0, 1, 1),
                                    Tuple.Vector(0, 1, 0) };
            var count = new[] { 0, 2, 4 };

            MethodInfo mi = shape.GetType().GetMethod("LocalIntersect", BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < 3; ++i)
            {
                Ray r = new Ray(origin[i], direction[i].Normalize());
                List<Intersection> xs = (List<Intersection>)mi.Invoke(shape, new[] { r });
                Assert.Equal(count[i], xs.Count);
            }
        }

        [Fact]
        private static void TestCase04()
        {
            // Computing the normal vector on a cone
            Shape shape = new Cone();
            var point = new[] { Tuple.Point(0, 0, 0),
                                Tuple.Point(1, 1, 1),
                                Tuple.Point(-1, -1, 0) };
            var normal = new[] { Tuple.Vector(0, 0, 0),
                                Tuple.Vector(1, -Sqrt(2), 1),
                                Tuple.Vector(-1, 1, 0) };

            MethodInfo mi = shape.GetType().GetMethod("LocalNormalAt", BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < 3; ++i)
            {
                Tuple n = (Tuple)mi.Invoke(shape, new[] { point[i], null });
                Assert.Equal(normal[i], n);
            }
        }
    }
}

using RayTracer;
using System.Collections.Generic;
using Xunit;

namespace UnitTest
{
    public class IntersectingWithSphereTest
    {
        [Fact]
        private static void TestCase01()
        {
            Tuple origin = Tuple.Point(1, 2, 3);
            Tuple direction = Tuple.Vector(4, 5, 6);

            Ray r = new Ray(origin, direction);

            Assert.Equal(origin, r.Origin);
            Assert.Equal(direction, r.Direction);
        }

        [Fact]
        private static void TestCase02()
        {
            Ray r = new Ray(Tuple.Point(2, 3, 4), Tuple.Vector(1, 0, 0));

            Assert.Equal(Tuple.Point(2, 3, 4), r.Position(0));
            Assert.Equal(Tuple.Point(3, 3, 4), r.Position(1));
            Assert.Equal(Tuple.Point(1, 3, 4), r.Position(-1));
            Assert.Equal(Tuple.Point(4.5f, 3, 4), r.Position(2.5f));
        }

        [Fact]
        private static void TestCase03()
        {
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            Sphere s = new Sphere();
            var xs = s.Intersect(r);

            Assert.Equal(2, xs.Count);
            Assert.Equal(4, xs[0].T);
            Assert.Equal(6, xs[1].T);
        }

        [Fact]
        private static void TestCase04()
        {
            Ray r = new Ray(Tuple.Point(0, 1, -5), Tuple.Vector(0, 0, 1));
            Sphere s = new Sphere();
            var xs = s.Intersect(r);

            Assert.Equal(2, xs.Count);
            Assert.Equal(5, xs[0].T);
            Assert.Equal(5, xs[1].T);
        }

        [Fact]
        private static void TestCase05()
        {
            Ray r = new Ray(Tuple.Point(0, 2, -5), Tuple.Vector(0, 0, 1));
            Sphere s = new Sphere();
            var xs = s.Intersect(r);

            Assert.Empty(xs);
        }

        [Fact]
        private static void TestCase06()
        {
            Ray r = new Ray(Tuple.Point(0, 0, 0), Tuple.Vector(0, 0, 1));
            Sphere s = new Sphere();
            var xs = s.Intersect(r);

            Assert.Equal(2, xs.Count);
            Assert.Equal(-1, xs[0].T);
            Assert.Equal(1, xs[1].T);
        }

        [Fact]
        private static void TestCase07()
        {
            Ray r = new Ray(Tuple.Point(0, 0, 5), Tuple.Vector(0, 0, 1));
            Sphere s = new Sphere();
            var xs = s.Intersect(r);

            Assert.Equal(2, xs.Count);
            Assert.Equal(-6, xs[0].T);
            Assert.Equal(-4, xs[1].T);
        }

        [Fact]
        private static void TestCase08()
        {
            Sphere s = new Sphere();
            Intersection i = new Intersection(3.5f, s);

            Assert.Equal(3.5f, i.T);
            Assert.Equal(s, i.Object);
        }

        [Fact]
        private static void TestCase09()
        {
            Sphere s = new Sphere();
            Intersection i1 = new Intersection(1, s);
            Intersection i2 = new Intersection(2, s);
            List<Intersection> xs = Intersection.Aggregate(i1, i2);

            Assert.Equal(2, xs.Count);
            Assert.Equal(1, xs[0].T);
            Assert.Equal(2, xs[1].T);
        }

        [Fact]
        private static void TestCase10()
        {
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            Sphere s = new Sphere();
            var xs = s.Intersect(r);
        }

        [Fact]
        private static void TestCase11()
        {
            Sphere s = new Sphere();
            Intersection i1 = new Intersection(1, s);
            Intersection i2 = new Intersection(2, s);
            List<Intersection> xs = Intersection.Aggregate(i2, i1);
            Intersection i = Intersection.Hit(xs);

            Assert.Equal(i1, i);

            i1 = new Intersection(-1, s);
            i2 = new Intersection(1, s);
            xs = Intersection.Aggregate(i2, i1);
            i = Intersection.Hit(xs);

            Assert.Equal(i2, i);

            i1 = new Intersection(-2, s);
            i2 = new Intersection(-1, s);
            xs = Intersection.Aggregate(i2, i1);
            i = Intersection.Hit(xs);

            Assert.Null(i);

            i1 = new Intersection(5, s);
            i2 = new Intersection(7, s);
            Intersection i3 = new Intersection(-3, s);
            Intersection i4 = new Intersection(2, s);
            xs = Intersection.Aggregate(i1, i2, i3, i4);
            i = Intersection.Hit(xs);

            Assert.Equal(i4, i);
        }

        [Fact]
        private static void TestCase12()
        {
            Ray r = new Ray(Tuple.Point(1, 2, 3), Tuple.Vector(0, 1, 0));
            Matrix m = Transformation.Translation(3, 4, 5);

            Ray r2 = r.Transform(m);

            Assert.Equal(Tuple.Point(4, 6, 8), r2.Origin);
            Assert.Equal(Tuple.Vector(0, 1, 0), r2.Direction);

            m = Transformation.Scaling(2, 3, 4);

            r2 = r.Transform(m);

            Assert.Equal(Tuple.Point(2, 6, 12), r2.Origin);
            Assert.Equal(Tuple.Vector(0, 3, 0), r2.Direction);
        }

        [Fact]
        private static void TestCase13()
        {
            Sphere s = new Sphere();
            Assert.Equal(Matrix.Identity(), s.Transform);

            Matrix t = Transformation.Translation(2, 3, 4);
            s.Transform = t;

            Assert.Equal(t, s.Transform);
        }

        [Fact]
        private static void TestCase14()
        {
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            Sphere s = new Sphere();
            s.Transform = Transformation.Scaling(2, 2, 2);

            List<Intersection> xs = s.Intersect(r);
            Assert.Equal(2, xs.Count);
            Assert.Equal(3, xs[0].T);
            Assert.Equal(7, xs[1].T);

            s.Transform = Transformation.Translation(5, 0, 0);
            xs = s.Intersect(r);

            Assert.Empty(xs);
        }
    }
}

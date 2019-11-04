using RayTracer;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using static System.MathF;

namespace UnitTest
{
    public static class GroupTest
    {
        [Fact]
        private static void TestCase01()
        {
            // Creating a new group
            Group g = new Group();

            Assert.Equal(Matrix.Identity(), g.Transform);
            Assert.Empty(g.Shapes);
        }

        [Fact]
        private static void TestCase02()
        {
            // A Shape has a parent attribute
            Shape s = new Sphere();
            Assert.Null(s.Parent);
        }

        [Fact]
        private static void TestCase03()
        {
            // Adding a child to a group
            Group g = new Group();
            Shape s = new Sphere();

            g.AddChild(s);

            Assert.NotEmpty(g.Shapes);
            Assert.Contains(s, g.Shapes);
            Assert.Same(g, s.Parent);
        }

        [Fact]
        private static void TestCase04()
        {
            // Intersecting a ray with an empty group
            Group g = new Group();
            Ray r = new Ray(Tuple.Point(0, 0, 0), Tuple.Vector(0, 0, 1));

            MethodInfo mi = g.GetType().GetMethod("LocalIntersect", BindingFlags.NonPublic | BindingFlags.Instance);
            List<Intersection> xs = (List<Intersection>)mi.Invoke(g, new[] { r });

            Assert.Empty(xs);
        }

        [Fact]
        private static void TestCase05()
        {
            // Intersecting a ray with an nonempty group
            Group g = new Group();
            Shape s1 = new Sphere();
            Shape s2 = new Sphere
            {
                Transform = Transformation.Translation(0, 0, -3)
            };
            Shape s3 = new Sphere
            {
                Transform = Transformation.Translation(5, 0, 0)
            };
            g.AddChild(s1);
            g.AddChild(s2);
            g.AddChild(s3);
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));

            MethodInfo mi = g.GetType().GetMethod("LocalIntersect", BindingFlags.NonPublic | BindingFlags.Instance);
            List<Intersection> xs = (List<Intersection>)mi.Invoke(g, new[] { r });

            Assert.Equal(4, xs.Count);
            Assert.Same(s2, xs[0].Object);
            Assert.Same(s2, xs[1].Object);
            Assert.Same(s1, xs[2].Object);
            Assert.Same(s1, xs[3].Object);
        }

        [Fact]
        private static void TestCase06()
        {
            // Intersecting a transformed group
            Group g = new Group
            {
                Transform = Transformation.Scaling(2, 2, 2)
            };
            Shape s = new Sphere
            {
                Transform = Transformation.Translation(5, 0, 0)
            };
            g.AddChild(s);
            Ray r = new Ray(Tuple.Point(10, 0, -10), Tuple.Vector(0, 0, 1));

            List<Intersection> xs = g.Intersect(r);

            Assert.Equal(2, xs.Count);
        }

        [Fact]
        private static void TestCase07()
        {
            // Converting a point from world to object space
            Group g1 = new Group
            {
                Transform = Transformation.RotationY(PI / 2)
            };
            Group g2 = new Group
            {
                Transform = Transformation.Scaling(2, 2, 2)
            };
            g1.AddChild(g2);
            Shape s = new Sphere
            {
                Transform = Transformation.Translation(5, 0, 0)
            };
            g2.AddChild(s);

            MethodInfo mi = s.GetType().BaseType.GetMethod("WorldToObject", BindingFlags.NonPublic | BindingFlags.Instance);
            Tuple p = (Tuple)mi.Invoke(s, new[] { Tuple.Point(-2, 0, -10) });

            Assert.Equal(Tuple.Point(0, 0, -1), p);
        }

        [Fact]
        private static void TestCase08()
        {
            // Converting a normal from object to world space
            Group g1 = new Group
            {
                Transform = Transformation.RotationY(PI / 2)
            };
            Group g2 = new Group
            {
                Transform = Transformation.Scaling(1, 2, 3)
            };
            g1.AddChild(g2);
            Shape s = new Sphere
            {
                Transform = Transformation.Translation(5, 0, 0)
            };
            g2.AddChild(s);

            MethodInfo mi = s.GetType().BaseType.GetMethod("NormalToWorld", BindingFlags.NonPublic | BindingFlags.Instance);
            Tuple p = (Tuple)mi.Invoke(s, new[] { Tuple.Point(Sqrt(3) / 3, Sqrt(3) / 3, Sqrt(3) / 3) });

            Assert.Equal(Tuple.Vector(0.2857f, 0.4286f, -0.8571f), p);
        }

        [Fact]
        private static void TestCase09()
        {
            // Finding the normal on a child object
            Group g1 = new Group
            {
                Transform = Transformation.RotationY(PI / 2)
            };
            Group g2 = new Group
            {
                Transform = Transformation.Scaling(1, 2, 3)
            };
            g1.AddChild(g2);
            Shape s = new Sphere
            {
                Transform = Transformation.Translation(5, 0, 0)
            };
            g2.AddChild(s);

            Tuple n = s.NormalAt(Tuple.Point(1.7321f, 1.1547f, -5.5774f));

            Assert.Equal(Tuple.Vector(0.2857f, 0.4286f, -0.8571f), n);
        }
    }
}

using RayTracer;
using System.Collections.Generic;
using Xunit;
using static System.MathF;

namespace UnitTest
{
    class PositionPattern : Pattern
    {
        public override Tuple ColorAt(Tuple point)
        {
            return Tuple.Color(point.X, point.Y, point.Z);
        }
    }

    public static class RefractionTest
    {
        [Fact]
        private static void TestCase01()
        {
            // Transparency and Refractive Index for the default material
            Material m = new Material();

            Assert.Equal(0, m.Transparency);
            Assert.Equal(1, m.RefractiveIndex);
        }

        [Fact]
        private static void TestCase02()
        {
            // A helper for producing a sphere with a glassy material
            Shape s = new GlassSphere();

            Assert.Equal(s.Transform, Matrix.Identity());
            Assert.Equal(1, s.Material.Transparency);
            Assert.Equal(1.5f, s.Material.RefractiveIndex);
        }

        [Fact]
        private static void TestCase03()
        {
            // Finding n1 and n2 at various intersections
            Shape A = new GlassSphere();
            A.Transform = Transformation.Scaling(2, 2, 2);
            A.Material.RefractiveIndex = 1.5f;
            Shape B = new GlassSphere();
            B.Transform = Transformation.Translation(0, 0, -0.25f);
            B.Material.RefractiveIndex = 2;
            Shape C = new GlassSphere();
            C.Transform = Transformation.Translation(0, 0, 0.25f);
            C.Material.RefractiveIndex = 2.5f;
            Ray r = new Ray(Tuple.Point(0, 0, -4), Tuple.Vector(0, 0, 1));
            List<Intersection> xs = new List<Intersection> { new Intersection(2, A),
                                                            new Intersection(2.75f, B),
                                                            new Intersection(3.25f, C),
                                                            new Intersection(4.75f, B),
                                                            new Intersection(5.25f, C),
                                                            new Intersection(6, A)};

            var expectedN1 = new[] { 1, 1.5f, 2, 2.5f, 2.5f, 1.5f };
            var expectedN2 = new[] { 1.5f, 2, 2.5f, 2.5f, 1.5f, 1 };

            for( int i = 0; i < 6; ++i )
            {
                Computation comps = new Computation(xs[i], r, xs);

                Assert.Equal(expectedN1[i], comps.N1);
                Assert.Equal(expectedN2[i], comps.N2);
            }
        }

        [Fact]
        private static void TestCase04()
        {
            // The under point is offset below the surface
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            Shape shape = new GlassSphere();
            shape.Transform = Transformation.Translation(0, 0, 1);
            Intersection i = new Intersection(5, shape);
            List<Intersection> xs = new List<Intersection> { i };
            Computation comps = new Computation(i, r, xs);

            Assert.True(comps.UnderPoint.Z > Constants.floatEps / 2);
            Assert.True(comps.Point.Z < comps.UnderPoint.Z);
        }

        [Fact]
        private static void TestCase05()
        {
            // The refracted color with an opaque surface
            World w = new DefaultWorld();
            Shape shape = w.Shapes[0];
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            List<Intersection> xs = new List<Intersection> { new Intersection(4, shape), new Intersection(6, shape) };
            Computation comps = new Computation(xs[0], r, xs);
            LightingModel l = new PhongReflection();
            Tuple c = l.RefractedColor(w, comps, 5);

            Assert.Equal(Tuple.Color(0, 0, 0), c);
        }

        [Fact]
        private static void TestCase06()
        {
            // The refracted color at the maximum recursive depth
            World w = new DefaultWorld();
            Shape shape = w.Shapes[0];
            shape.Material.Transparency = 1;
            shape.Material.RefractiveIndex = 1.5f;
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            List<Intersection> xs = new List<Intersection> { new Intersection(4, shape), new Intersection(6, shape) };
            Computation comps = new Computation(xs[0], r, xs);
            LightingModel l = new PhongReflection();
            Tuple c = l.RefractedColor(w, comps, 0);

            Assert.Equal(Tuple.Color(0, 0, 0), c);
        }

        [Fact]
        private static void TestCase07()
        {
            // The refracted color under total internal reflection
            World w = new DefaultWorld();
            Shape shape = w.Shapes[0];
            shape.Material.Transparency = 1;
            shape.Material.RefractiveIndex = 1.5f;
            Ray r = new Ray(Tuple.Point(0, 0, Sqrt(2) / 2), Tuple.Vector(0, 1, 0));
            List<Intersection> xs = new List<Intersection> { new Intersection(-Sqrt(2) / 2, shape), new Intersection(Sqrt(2) / 2, shape) };
            Computation comps = new Computation(xs[1], r, xs);
            LightingModel l = new PhongReflection();
            Tuple c = l.RefractedColor(w, comps, 5);

            Assert.Equal(Tuple.Color(0, 0, 0), c);
        }

        [Fact]
        private static void TestCase08()
        {
            // The refracted color with a refracted ray
            World w = new DefaultWorld();
            Shape A = w.Shapes[0];
            A.Material.Ambient = 1;
            A.Material.Pattern = new PositionPattern();
            Shape B = w.Shapes[1];
            B.Material.Transparency = 1;
            B.Material.RefractiveIndex = 1.5f;
            Ray r = new Ray(Tuple.Point(0, 0, 0.1f), Tuple.Vector(0, 1, 0));
            List<Intersection> xs = new List<Intersection> { new Intersection(-0.9899f, A),
                                                            new Intersection(-0.4899f, B),
                                                            new Intersection(0.4899f, B),
                                                            new Intersection(0.9899f, A)};
            Computation comps = new Computation(xs[2], r, xs);
            LightingModel l = new PhongReflection();
            Tuple c = l.RefractedColor(w, comps, 5);

            Assert.Equal(Tuple.Color(0, 0.99888f, 0.04725f), c);
        }

        [Fact]
        private static void TestCase09()
        {
            // ShadeHit() with a transparent material
            World w = new DefaultWorld();
            Shape floor = new Plane();
            floor.Transform = Transformation.Translation(0, -1, 0);
            floor.Material.Transparency = 0.5f;
            floor.Material.RefractiveIndex = 1.5f;
            w.Shapes.Add(floor);
            Shape Ball = new Sphere();
            Ball.Material.Color = Tuple.Color(1, 0, 0);
            Ball.Material.Ambient = 0.5f;
            Ball.Transform = Transformation.Translation(0, -3.5f, -0.5f);
            w.Shapes.Add(Ball);
            Ray r = new Ray(Tuple.Point(0, 0, -3), Tuple.Vector(0, -Sqrt(2) / 2, Sqrt(2) / 2));
            List<Intersection> xs = new List<Intersection> { new Intersection(Sqrt(2), floor) };
            Computation comps = new Computation(xs[0], r, xs);
            LightingModel l = new PhongReflection();
            Tuple c = l.ShadeHit(w, comps, 5);

            Assert.Equal(Tuple.Color(0.93642f, 0.68642f, 0.68642f), c);
        }
    }
}

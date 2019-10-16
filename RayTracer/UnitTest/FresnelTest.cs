using RayTracer;
using System.Collections.Generic;
using Xunit;
using static System.MathF;

namespace UnitTest
{
    public static class FresnelTest
    {
        [Fact]
        private static void TestCase01()
        {
            // The Schlick approximation under total internal reflection
            Shape shape = new GlassSphere();
            Ray r = new Ray(Tuple.Point(0, 0, Sqrt(2) / 2), Tuple.Vector(0, 1, 0));
            List<Intersection> xs = Intersection.Aggregate(new Intersection(-Sqrt(2) / 2, shape), new Intersection(Sqrt(2) / 2, shape));
            Computation comps = new Computation(xs[1], r, xs);
            float reflectance = Fresnel.Schlick(comps);

            Assert.Equal(1, reflectance);
        }

        [Fact]
        private static void TestCase02()
        {
            // The Schlick approximation with a perpendicular viewing angle
            Shape shape = new GlassSphere();
            Ray r = new Ray(Tuple.Point(0, 0, 0), Tuple.Vector(0, 1, 0));
            List<Intersection> xs = Intersection.Aggregate(new Intersection(-1, shape), new Intersection(1, shape));
            Computation comps = new Computation(xs[1], r, xs);
            float reflectance = Fresnel.Schlick(comps);

            Assert.Equal(0.04f, reflectance, 5);
        }

        [Fact]
        private static void TestCase03()
        {
            // The Schlick approximation with small angle and n2 > n1
            Shape shape = new GlassSphere();
            Ray r = new Ray(Tuple.Point(0, 0.99f, -2), Tuple.Vector(0, 0, 1));
            List<Intersection> xs = Intersection.Aggregate(new Intersection(1.8589f, shape));
            Computation comps = new Computation(xs[0], r, xs);
            float reflectance = Fresnel.Schlick(comps);

            Assert.Equal(0.48873f, reflectance, 5);
        }

        [Fact]
        private static void TestCase04()
        {
            // ShadeHit() with a reflective, transparent material
            World w = new DefaultWorld();
            Ray r = new Ray(Tuple.Point(0, 0, -3), Tuple.Vector(0, -Sqrt(2) / 2, Sqrt(2) / 2));
            Shape floor = new Plane();
            floor.Transform = Transformation.Translation(0, -1, 0);
            floor.Material.Reflective = 0.5f;
            floor.Material.Transparency = 0.5f;
            floor.Material.RefractiveIndex = 1.5f;
            w.Shapes.Add(floor);
            Shape ball = new Sphere();
            ball.Material.Color = Tuple.Color(1, 0, 0);
            ball.Material.Ambient = 0.5f;
            ball.Transform = Transformation.Translation(0, -3.5f, -0.5f);
            w.Shapes.Add(ball);
            List<Intersection> xs = Intersection.Aggregate(new Intersection(Sqrt(2), floor));
            Computation comps = new Computation(xs[0], r, xs);
            LightingModel l = new PhongReflection();
            Tuple color = l.ShadeHit(w, comps, 5);

            Assert.Equal(Tuple.Color(0.93391f, 0.69643f, 0.69243f), color);
        }
    }
}

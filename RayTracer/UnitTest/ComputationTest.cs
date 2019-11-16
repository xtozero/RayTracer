using RayTracer;
using System.Collections.Generic;
using Xunit;

namespace UnitTest
{
    public static class ComputationTest
    {
        [Fact]
        private static void TestCase01()
        {
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            Shape shape = new Sphere();
            Intersection i = new Intersection(4, shape);

            Computation comps = new Computation(i, r);

            Assert.Equal(i.T, comps.T);
            Assert.Equal(i.Object, comps.Object);
            Assert.Equal(Tuple.Point(0, 0, -1), comps.Point);
            Assert.Equal(Tuple.Vector(0, 0, -1), comps.Eyev);
            Assert.Equal(Tuple.Vector(0, 0, -1), comps.Normalv);
        }

        [Fact]
        private static void TestCase02()
        {
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            Shape shape = new Sphere();
            Intersection i = new Intersection(4, shape);

            Computation comps = new Computation(i, r);

            Assert.False(comps.Inside);
        }

        [Fact]
        private static void TestCase03()
        {
            Ray r = new Ray(Tuple.Point(0, 0, 0), Tuple.Vector(0, 0, 1));
            Shape shape = new Sphere();
            Intersection i = new Intersection(1, shape);

            Computation comps = new Computation(i, r);

            Assert.Equal(Tuple.Point(0, 0, 1), comps.Point);
            Assert.Equal(Tuple.Vector(0, 0, -1), comps.Eyev);
            Assert.True(comps.Inside);
            Assert.Equal(Tuple.Vector(0, 0, -1), comps.Normalv);
        }

        [Fact]
        private static void TestCase04()
        {
            World w = new DefaultWorld();
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            Shape shape = w.Shapes[0];
            Intersection i = new Intersection(4, shape);
            Computation comps = new Computation(i, r);
            LightingModel phong = new PhongReflection();
            Tuple c = phong.ShadeHit(w, comps);

            Assert.Equal(Tuple.Color(0.38066f, 0.47583f, 0.2855f), c);
        }

        [Fact]
        private static void TestCase05()
        {
            World w = new DefaultWorld();
            w.Lights[0] = new PointLight(Tuple.Point(0, 0.25f, 0), Tuple.Color(1, 1, 1));
            Ray r = new Ray(Tuple.Point(0, 0, 0), Tuple.Vector(0, 0, 1));
            Shape shape = w.Shapes[1];
            Intersection i = new Intersection(0.5f, shape);
            Computation comps = new Computation(i, r);
            LightingModel phong = new PhongReflection();
            Tuple c = phong.Lighting(comps.Object.Material,
                                    comps.Object,
                                    w.Lights[0],
                                    comps.Point,
                                    comps.Eyev,
                                    comps.Normalv,
                                    false);

            Assert.Equal(Tuple.Color(0.90498f, 0.90498f, 0.90498f), c);
        }

        [Fact]
        private static void TestCase06()
        {
            World w = new DefaultWorld();
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 1, 0));
            LightingModel phong = new PhongReflection();
            Tuple c = phong.ColorAt(w, r);

            Assert.Equal(Tuple.Color(0, 0, 0), c);
        }

        [Fact]
        private static void TestCase07()
        {
            World w = new DefaultWorld();
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            LightingModel phong = new PhongReflection();
            Tuple c = phong.ColorAt(w, r);

            Assert.Equal(Tuple.Color(0.38066f, 0.47583f, 0.2855f), c);
        }

        [Fact]
        private static void TestCase08()
        {
            World w = new DefaultWorld();
            Shape outer = w.Shapes[0];
            outer.Material.Ambient = 1;
            Shape inner = w.Shapes[1];
            inner.Material.Ambient = 1;
            Ray r = new Ray(Tuple.Point(0, 0, 0.75f), Tuple.Vector(0, 0, -1));
            LightingModel phong = new PhongReflection();
            Tuple c = phong.ColorAt(w, r);

            Assert.Equal(c, inner.Material.Color);
        }
    }
}

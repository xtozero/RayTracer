using Xunit;
using RayTracer;

namespace UnitTest
{
    public static class ShadowTest
    {
        [Fact]
        private static void TestCase01()
        {
            Material m = new Material();
            Tuple position = Tuple.Point(0, 0, 0);

            Tuple eyev = Tuple.Vector(0, 0, -1);
            Tuple normalv = Tuple.Vector(0, 0, -1);
            Light light = new PointLight(Tuple.Point(0, 0, -10), Tuple.Color(1, 1, 1));
            bool inShadow = true;

            PhongReflection phong = new PhongReflection();
            Tuple result = phong.Lighting(m, new Sphere(), light, position, eyev, normalv, inShadow);

            Assert.Equal(Tuple.Color(0.1f, 0.1f, 0.1f), result);
        }

        [Fact]
        private static void TestCase02()
        {
            World w = new DefaultWorld();
            Tuple p = Tuple.Point(0, 10, 0);

            Assert.False(w.IsShadowed(w.Lights[0], p));
        }

        [Fact]
        private static void TestCase03()
        {
            World w = new DefaultWorld();
            Tuple p = Tuple.Point(10, -10, 10);

            Assert.True(w.IsShadowed(w.Lights[0], p));
        }

        [Fact]
        private static void TestCase04()
        {
            World w = new DefaultWorld();
            Tuple p = Tuple.Point(-20, 20, -20);

            Assert.False(w.IsShadowed(w.Lights[0], p));
        }

        [Fact]
        private static void TestCase05()
        {
            World w = new DefaultWorld();
            Tuple p = Tuple.Point(-2, 2, -2);

            Assert.False(w.IsShadowed(w.Lights[0], p));
        }

        [Fact]
        private static void TestCase06()
        {
            World w = new World();
            w.Lights.Add(new PointLight(Tuple.Point(0, 0, -10), Tuple.Color(1, 1, 1)));

            Shape s1 = new Sphere();
            w.Shapes.Add(s1);

            Shape s2 = new Sphere();
            s2.Transform = Transformation.Translation(0, 0, 10);
            w.Shapes.Add(s2);

            Ray r = new Ray(Tuple.Point(0, 0, 5), Tuple.Vector(0, 0, 1));
            Intersection i = new Intersection(4, s2);

            Computation comps = new Computation(i, r);
            PhongReflection phong = new PhongReflection();
            Tuple c = phong.ShadeHit(w, comps);

            Assert.Equal(Tuple.Color(0.1f, 0.1f, 0.1f), c);
        }

        [Fact]
        private static void TestCase07()
        {
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            Shape shape = new Sphere();
            shape.Transform = Transformation.Translation(0, 0, 1);

            Intersection i = new Intersection(5, shape);

            Computation comps = new Computation(i, r);
            Assert.True(comps.OverPoint.Z < -Constants.overPointEps / 2);
            Assert.True(comps.Point.Z > comps.OverPoint.Z);
        }
    }
}

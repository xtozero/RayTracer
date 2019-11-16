using RayTracer;
using System.Collections.Generic;
using Xunit;

namespace UnitTest
{
    public static class WorldTest
    {
        [Fact]
        private static void TestCase01()
        {
            World w = new World();

            Assert.Empty(w.Lights);
            Assert.Empty(w.Shapes);
        }

        [Fact]
        private static void TestCase02()
        {
            World w = new DefaultWorld();

            Light light = new PointLight(Tuple.Point(-10, 10, -10), Tuple.Color(1, 1, 1));

            Shape s1 = new Sphere();
            s1.Material.Color = Tuple.Color(0.8f, 1, 0.6f);
            s1.Material.Diffuse = 0.7f;
            s1.Material.Specular = 0.2f;

            Shape s2 = new Sphere();
            s2.Transform = Transformation.Scaling(0.5f, 0.5f, 0.5f);

            Assert.Equal(light, w.Lights[0]);

            Assert.Contains(s1, w.Shapes);
            Assert.Contains(s2, w.Shapes);
        }

        [Fact]
        private static void TestCase03()
        {
            World w = new DefaultWorld();
            Ray r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            List<Intersection> xs = w.Intersection(r);

            Assert.True(xs.Count == 4);
            Assert.Equal(4, xs[0].T);
            Assert.Equal(4.5, xs[1].T);
            Assert.Equal(5.5, xs[2].T);
            Assert.Equal(6, xs[3].T);
        }
    }
}

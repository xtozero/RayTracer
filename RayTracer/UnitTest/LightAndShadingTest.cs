using static System.MathF;
using RayTracer;
using Xunit;

namespace UnitTest
{
    public static class LightAndShadingTest
    {
        [Fact]
        private static void TestCase01()
        {
            Sphere s = new Sphere();
            Tuple n = s.NormalAt(Tuple.Point(1, 0, 0));

            Assert.Equal(Tuple.Vector(1, 0, 0), n);

            n = s.NormalAt(Tuple.Point(0, 1, 0));

            Assert.Equal(Tuple.Vector(0, 1, 0), n);

            n = s.NormalAt(Tuple.Point(0, 0, 1));

            Assert.Equal(Tuple.Vector(0, 0, 1), n);

            n = s.NormalAt(Tuple.Point(Sqrt(3) / 3, Sqrt(3) / 3, Sqrt(3) / 3));

            Assert.Equal(Tuple.Vector(Sqrt(3) / 3, Sqrt(3) / 3, Sqrt(3) / 3), n);
        }

        [Fact]
        private static void TestCase02()
        {
            Sphere s = new Sphere();
            Tuple n = s.NormalAt(Tuple.Point(Sqrt(3) / 3, Sqrt(3) / 3, Sqrt(3) / 3));

            Assert.Equal(n, n.Normalize());
        }

        [Fact]
        private static void TestCase03()
        {
            Sphere s = new Sphere();
            s.Transform = Transformation.Translation(0, 1, 0);
            Tuple n = s.NormalAt(Tuple.Point(0, 1.70711f, -0.70711f));

            Assert.Equal(Tuple.Vector(0, 0.70711f, -0.70711f), n);

            s.Transform = Transformation.Scaling(1, 0.5f, 1) * Transformation.RotationZ(PI/5);
            n = s.NormalAt(Tuple.Point(0, Sqrt(2) / 2, -Sqrt(2) / 2));

            Assert.Equal(Tuple.Vector(0, 0.97014f, -0.24254f), n);
        }

        [Fact]
        private static void TestCase04()
        {
            Tuple v = Tuple.Vector(1, -1, 0);
            Tuple n = Tuple.Vector(0, 1, 0);
            Tuple r = v.Reflect(n);

            Assert.Equal(Tuple.Vector(1, 1, 0), r);
        }

        [Fact]
        private static void TestCase05()
        {
            Tuple v = Tuple.Vector(0, -1, 0);
            Tuple n = Tuple.Vector(Sqrt(2) / 2, Sqrt(2) / 2, 0);
            Tuple r = v.Reflect(n);

            Assert.Equal(Tuple.Vector(1, 0, 0), r);
        }

        [Fact]
        private static void TestCase06()
        {
            Tuple intensity = Tuple.Color(1, 1, 1);
            Tuple position = Tuple.Point(0, 0, 0);

            PointLight light = new PointLight(position, intensity);

            Assert.Equal(position, light.Position);
            Assert.Equal(intensity, light.Intensity);
        }

        [Fact]
        private static void TestCase07()
        {
            Material m = new Material();

            Assert.Equal(Tuple.Color(1, 1, 1), m.Color);
            Assert.Equal(0.1f, m.Ambient);
            Assert.Equal(0.9f, m.Diffuse);
            Assert.Equal(0.9f, m.Specular);
            Assert.Equal(200.0f, m.Shininess);
        }

        [Fact]
        private static void TestCase08()
        {
            Sphere s = new Sphere();
            Material m = s.Material;

            Assert.Equal(new Material(), m);

            m = new Material();
            m.Ambient = 1;
            s.Material = m;

            Assert.Equal(m, s.Material);
        }

        [Fact]
        private static void TestCase09()
        {
            Material m = new Material();
            Tuple position = Tuple.Point(0, 0, 0);

            Tuple eyev = Tuple.Vector(0, 0, -1);
            Tuple normalv = Tuple.Vector(0, 0, -1);
            Light light = new PointLight(Tuple.Point(0, 0, -10), Tuple.Color(1, 1, 1));

            LightingModel phong = new PhongReflection();
            Tuple result = phong.Lighting(m, light, position, eyev, normalv);

            Assert.Equal(Tuple.Color(1.9f, 1.9f, 1.9f), result);

            eyev = Tuple.Vector(0, Sqrt(2) / 2, -Sqrt(2) / 2);

            result = phong.Lighting(m, light, position, eyev, normalv);

            Assert.Equal(Tuple.Color(1.0f, 1.0f, 1.0f), result);

            eyev = Tuple.Vector(0, 0, -1);
            light = new PointLight(Tuple.Point(0, 10, -10), Tuple.Color(1, 1, 1));

            result = phong.Lighting(m, light, position, eyev, normalv);

            Assert.Equal(Tuple.Color(0.7364f, 0.7364f, 0.7364f), result);

            eyev = Tuple.Vector(0, -Sqrt(2) / 2, -Sqrt(2) / 2);

            result = phong.Lighting(m, light, position, eyev, normalv);

            Assert.Equal(Tuple.Color(1.6364f, 1.6364f, 1.6364f), result);
        }

        [Fact]
        private static void TestCase10()
        {
            Material m = new Material();
            Tuple position = Tuple.Point(0, 0, 0);

            Tuple eyev = Tuple.Vector(0, 0, -1);
            Tuple normalv = Tuple.Vector(0, 0, -1);
            Light light = new PointLight(Tuple.Point(0, 0, 10), Tuple.Color(1, 1, 1));

            LightingModel phong = new PhongReflection();
            Tuple result = phong.Lighting(m, light, position, eyev, normalv);

            Assert.Equal(Tuple.Color(0.1f, 0.1f, 0.1f), result);
        }
    }
}

using RayTracer;
using Xunit;
using static System.MathF;

namespace UnitTest
{
    public static class ReflectionTest
    {
        [Fact]
        private static void TestCase01()
        {
            // Reflectivity for the default material
            Material m = new Material();
            Assert.Equal(0, m.Reflective);
        }

        [Fact]
        private static void TestCase02()
        {
            // Procomputing the reflection vector
            Shape shape = new Plane();
            Ray r = new Ray(Tuple.Point(0, 1, -1), Tuple.Vector(0, -Sqrt(2) / 2, Sqrt(2) / 2));
            Intersection i = new Intersection(Sqrt(2), shape);
            Computation comps = new Computation(i, r);

            Assert.Equal(Tuple.Vector(0, Sqrt(2) / 2, Sqrt(2) / 2), comps.Reflectv );
        }

        [Fact]
        private static void TestCase03()
        {
            // The reflected color for a nonreflective material
            World w = new DefaultWorld();
            Ray r = new Ray(Tuple.Point(0, 0, 0), Tuple.Vector(0, 0, 1));
            Shape shape = w.Shapes[1];
            shape.Material.Ambient = 1;
            Intersection i = new Intersection(1, shape);
            Computation comps = new Computation(i, r);
            LightingModel l = new PhongReflection();
            Tuple color = l.ReflectedColor(w, comps);

            Assert.Equal(Tuple.Color(0, 0, 0), color);
        }

        [Fact]
        private static void TestCase04()
        {
            // The reflected color for a reflective material
            World w = new DefaultWorld();
            Shape shape = new Plane();
            shape.Material.Reflective = 0.5f;
            shape.Transform = Transformation.Translation(0, -1, 0);
            w.Shapes.Add(shape);
            Ray r = new Ray(Tuple.Point(0, 0, -3), Tuple.Vector(0, -Sqrt(2) / 2, Sqrt(2) / 2));
            Intersection i = new Intersection(Sqrt(2), shape);
            Computation comps = new Computation(i, r);
            LightingModel l = new PhongReflection();
            Tuple color = l.ReflectedColor(w, comps);

            Assert.Equal(0.19032f, color.X, 3);
            Assert.Equal(0.2379f, color.Y, 3);
            Assert.Equal(0.14274f, color.Z, 3);
        }

        [Fact]
        private static void TestCase05()
        {
            // ShadeHit() with a reflective material
            World w = new DefaultWorld();
            Shape shape = new Plane();
            shape.Material.Reflective = 0.5f;
            shape.Transform = Transformation.Translation(0, -1, 0);
            w.Shapes.Add(shape);
            Ray r = new Ray(Tuple.Point(0, 0, -3), Tuple.Vector(0, -Sqrt(2) / 2, Sqrt(2) / 2));
            Intersection i = new Intersection(Sqrt(2), shape);
            Computation comps = new Computation(i, r);
            LightingModel l = new PhongReflection();
            Tuple color = l.ShadeHit(w, comps);

            Assert.Equal(Tuple.Color(0.87677f, 0.92436f, 0.82918f), color);
        }

        [Fact]
        private static void TestCase06()
        {
            // ColorAt() with mutually reflective surfaces
            World w = new World();
            w.Lights.Add(new PointLight(Tuple.Point(0, 0, 0), Tuple.Color(1, 1, 1)));
            Shape lower = new Plane();
            lower.Material.Reflective = 1;
            lower.Transform = Transformation.Translation(0, -1, 0);
            w.Shapes.Add(lower);
            Shape upper = new Plane();
            upper.Material.Reflective = 1;
            upper.Transform = Transformation.Translation(0, 1, 0);
            w.Shapes.Add(upper);
            Ray r = new Ray(Tuple.Point(0, 0, 0), Tuple.Vector(0, 1, 0));
            LightingModel l = new PhongReflection();
            l.ColorAt(w, r);

            Assert.True(true);
        }

        [Fact]
        private static void TestCase07()
        {
            // The reflected color at the maximum recursive depth
            World w = new DefaultWorld();
            Shape shape = new Plane();
            shape.Material.Reflective = 0.5f;
            shape.Transform = Transformation.Translation(0, -1, 0);
            w.Shapes.Add(shape);
            Ray r = new Ray(Tuple.Point(0, 0, -3), Tuple.Vector(0, -Sqrt(2) / 2, Sqrt(2) / 2));
            Intersection i = new Intersection(Sqrt(2), shape);
            Computation comps = new Computation(i, r);
            LightingModel l = new PhongReflection();
            Tuple color = l.ReflectedColor(w, comps, 0);

            Assert.Equal(Tuple.Color(0, 0, 0), color);
        }
    }
}

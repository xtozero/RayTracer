using Xunit;
using RayTracer;

namespace UnitTest
{
    public static class PatternTest
    {
        [Fact]
        private static void TestCase01()
        {
            // Creating a stripe pattern
            StripePattern pattern = new StripePattern(white, black);

            Assert.Equal(white, pattern.FirstColor);
            Assert.Equal(black, pattern.SecondColor);
        }

        [Fact]
        private static void TestCase02()
        {
            // A stripe pattern is constant in y
            StripePattern pattern = new StripePattern(white, black);

            Assert.Equal(white, pattern.ColorAt(Tuple.Point(0, 0, 0)));
            Assert.Equal(white, pattern.ColorAt(Tuple.Point(0, 1, 0)));
            Assert.Equal(white, pattern.ColorAt(Tuple.Point(0, 2, 0)));

            // A stripe pattern is constant in z
            Assert.Equal(white, pattern.ColorAt(Tuple.Point(0, 0, 0)));
            Assert.Equal(white, pattern.ColorAt(Tuple.Point(0, 0, 1)));
            Assert.Equal(white, pattern.ColorAt(Tuple.Point(0, 0, 2)));

            // A stripe pattern alternates in x
            Assert.Equal(white, pattern.ColorAt(Tuple.Point(0, 0, 0)));
            Assert.Equal(white, pattern.ColorAt(Tuple.Point(0.9f, 0, 0)));
            Assert.Equal(black, pattern.ColorAt(Tuple.Point(1, 0, 0)));
            Assert.Equal(black, pattern.ColorAt(Tuple.Point(-0.1f, 0, 0)));
            Assert.Equal(black, pattern.ColorAt(Tuple.Point(-1, 0, 0)));
            Assert.Equal(white, pattern.ColorAt(Tuple.Point(-1.1f, 0, 0)));
        }

        [Fact]
        private static void TestCase03()
        {
            // Lighting with a pattern applied
            Material m = new Material();
            m.Pattern = new StripePattern(white, black);
            m.Ambient = 1;
            m.Diffuse = 0;
            m.Specular = 0;

            Tuple eyev = Tuple.Vector(0, 0, -1);
            Tuple normalv = Tuple.Vector(0, 0, -1);

            Light light = new PointLight(Tuple.Point(0, 0, -10), Tuple.Color(1, 1, 1));

            LightingModel l = new PhongReflection();
            Tuple c1 = l.Lighting(m, new Sphere(), light, Tuple.Point(0.9f, 0, 0), eyev, normalv, false);
            Tuple c2 = l.Lighting(m, new Sphere(), light, Tuple.Point(1.1f, 0, 0), eyev, normalv, false);

            Assert.Equal(white, c1);
            Assert.Equal(black, c2);
        }

        [Fact]
        private static void TestCase04()
        {
            // Stripes with an object transformation
            Shape obj = new Sphere();
            obj.Transform = Transformation.Scaling(2, 2, 2);

            Pattern pattern = new StripePattern(white, black);

            Tuple c = pattern.ColorAtShape(obj, Tuple.Point(1.5f, 0, 0));

            Assert.Equal(white, c);

            // Stripes with a pattern transformation
            obj.Transform = Matrix.Identity();

            pattern.Transform = Transformation.Scaling(2, 2, 2);

            c = pattern.ColorAtShape(obj, Tuple.Point(1.5f, 0, 0));

            Assert.Equal(white, c);

            // Stripes with both an object and a pattern transformation
            obj.Transform = Transformation.Scaling(2, 2, 2);

            pattern.Transform = Transformation.Translation(0.5f, 0, 0);

            c = pattern.ColorAtShape(obj, Tuple.Point(2.5f, 0, 0));

            Assert.Equal(white, c);
        }

        [Fact]
        private static void TestCase05()
        {
            // A gradient linearly interpolates between colors
            Pattern pattern = new GradientPattern(white, black);

            Assert.Equal( white, pattern.ColorAt(Tuple.Point(0, 0, 0)));
            Assert.Equal( Tuple.Color(0.75f, 0.75f, 0.75f), pattern.ColorAt(Tuple.Point(0.25f, 0, 0)));
            Assert.Equal( Tuple.Color(0.5f, 0.5f, 0.5f), pattern.ColorAt(Tuple.Point(0.5f, 0, 0)));
            Assert.Equal( Tuple.Color(0.25f, 0.25f, 0.25f), pattern.ColorAt(Tuple.Point(0.75f, 0, 0)));
        }

        [Fact]
        private static void TestCase06()
        {
            // A ring should extend in both x and z
            Pattern pattern = new RingPattern(white, black);

            Assert.Equal(white, pattern.ColorAt(Tuple.Point(0, 0, 0)));
            Assert.Equal(black, pattern.ColorAt(Tuple.Point(1, 0, 0)));
            Assert.Equal(black, pattern.ColorAt(Tuple.Point(0, 0, 1)));
            Assert.Equal(black, pattern.ColorAt(Tuple.Point(0.708f, 0, 0.708f)));
        }

        [Fact]
        private static void TestCase07()
        {
            // Checker should repeat in x
            Pattern pattern = new CheckersPattern(white, black);

            Assert.Equal(white, pattern.ColorAt(Tuple.Point(0, 0, 0)));
            Assert.Equal(white, pattern.ColorAt(Tuple.Point(0.99f, 0, 0)));
            Assert.Equal(black, pattern.ColorAt(Tuple.Point(1.01f, 0, 0)));

            // Checker should repeat in y
            Assert.Equal(white, pattern.ColorAt(Tuple.Point(0, 0, 0)));
            Assert.Equal(white, pattern.ColorAt(Tuple.Point(0, 0.99f, 0)));
            Assert.Equal(black, pattern.ColorAt(Tuple.Point(0, 1.01f, 0)));

            // Checker should repeat in z
            Assert.Equal(white, pattern.ColorAt(Tuple.Point(0, 0, 0)));
            Assert.Equal(white, pattern.ColorAt(Tuple.Point(0, 0, 0.99f)));
            Assert.Equal(black, pattern.ColorAt(Tuple.Point(0, 0, 1.01f)));
        }

        private static readonly Tuple black = Tuple.Color(0, 0, 0);
        private static readonly Tuple white = Tuple.Color(1, 1, 1);
    }
}

using Xunit;
using RayTracer;
using static System.MathF;

namespace UnitTest
{
    public static class CameraTest
    {
        [Fact]
        private static void TestCase01()
        {
            int hSize = 160;
            int vSize = 120;
            float fieldOfView = PI / 2.0f;

            Camera c = new Camera(hSize, vSize, fieldOfView);

            Assert.Equal(160, c.HSize);
            Assert.Equal(120, c.VSize);
            Assert.Equal(PI / 2.0f, c.FieldOfView);
            Assert.Equal(Matrix.Identity(), c.Transform);
        }

        [Fact]
        private static void TestCase02()
        {
            Camera c = new Camera(200, 125, PI / 2.0f);
            Assert.Equal(0.01f, c.PixelSize);

            c = new Camera(125, 200, PI / 2.0f);
            Assert.Equal(0.01f, c.PixelSize);
        }

        [Fact]
        private static void TestCase03()
        {
            Camera c = new Camera(201, 101, PI / 2.0f);
            Ray r = c.RayForPixel(100, 50);

            Assert.Equal(Tuple.Point(0, 0, 0), r.Origin);
            Assert.Equal(Tuple.Vector(0, 0, -1), r.Direction);

            r = c.RayForPixel(0, 0);

            Assert.Equal(Tuple.Point(0, 0, 0), r.Origin);
            Assert.Equal(Tuple.Vector(0.66519f, 0.33259f, -0.66851f), r.Direction);

            c.Transform = Transformation.RotationY(PI / 4.0f) * Transformation.Translation(0, -2, 5);
            r = c.RayForPixel(100, 50);

            Assert.Equal(Tuple.Point(0, 2, -5), r.Origin);
            Assert.Equal(Tuple.Vector(Sqrt(2) / 2, 0, -Sqrt(2) / 2), r.Direction);
        }

        [Fact]
        private static void TestCase04()
        {
            World w = new DefaultWorld();

            Camera c = new Camera(11, 11, PI / 2.0f);

            Tuple from = Tuple.Point(0, 0, -5);
            Tuple to = Tuple.Point(0, 0, 0);
            Tuple up = Tuple.Vector(0, 1, 0);

            c.Transform = Transformation.LookAt(from, to, up);

            LightingModel l = new PhongReflection();
            Canvas image = Canvas.Render(c, w, l);

            Assert.Equal(image.PixelAt(5, 5), Tuple.Color(0.38066f, 0.47583f, 0.2855f));
        }
    }
}

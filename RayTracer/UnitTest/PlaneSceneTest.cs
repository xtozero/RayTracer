using Xunit;
using RayTracer;
using static System.MathF;
using System.IO;

namespace UnitTest
{
    class PlaneScene : World
    {
        public PlaneScene()
        {
            Shape floor = new Plane();
            floor.Material.Color = Tuple.Color(1, 0.9f, 0.9f);
            floor.Material.Specular = 0;

            Shapes.Add(floor);

            Shape ceiling = new Plane();
            ceiling.Transform = Transformation.Translation(0, 2.5f, 0) * Transformation.RotationZ(PI);
            floor.Material.Color = Tuple.Color(1, 0.9f, 0.9f);
            floor.Material.Specular = 0;

            Shapes.Add(ceiling);

            Shape middle = new Sphere();
            middle.Transform = Transformation.Translation(-0.5f, 1, 0.5f);
            middle.Material.Color = Tuple.Color(0.1f, 1, 0.5f);
            middle.Material.Diffuse = 0.7f;
            middle.Material.Specular = 0.3f;

            Shapes.Add(middle);

            Shape right = new Sphere();
            right.Transform = Transformation.Translation(1.5f, 0.0f, -0.5f) * Transformation.Scaling(0.5f, 0.5f, 0.5f);
            right.Material.Color = Tuple.Color(0, 0, 0.7f);
            right.Material.Diffuse = 0.7f;
            right.Material.Specular = 0.3f;

            Shapes.Add(right);

            Shape left = new Sphere();
            left.Transform = Transformation.Translation(-1.5f, 0.33f, -0.75f) * Transformation.Scaling(0.33f, 0.33f, 0.33f);
            left.Material.Color = Tuple.Color(1, 0.2f, 0.1f);
            left.Material.Diffuse = 0.7f;
            left.Material.Specular = 0.3f;

            Shapes.Add(left);

            Light = new PointLight(Tuple.Point(-1, 1, -1), Tuple.Color(1, 1, 1));
        }
    }

    public static class PlaneSceneTest
    {
        [Fact]
        private static void Simulate()
        {
            Camera c = new Camera(400, 200, PI / 3);
            c.Transform = Transformation.LookAt(Tuple.Point(0, 1.5f, -5),
                                                Tuple.Point(0, 1, 0),
                                                Tuple.Vector(0, 1, 0));
            World w = new PlaneScene();
            LightingModel l = new PhongReflection();

            Canvas canvas = Canvas.Render(c, w, l);

            using (StreamWriter sw = new StreamWriter(@"./planeScene.ppm"))
            {
                sw.Write(canvas.ToPPM());
            }

            Assert.True(true);
        }
    }
}

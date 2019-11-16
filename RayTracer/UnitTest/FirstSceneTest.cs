using Xunit;
using RayTracer;
using static System.MathF;
using System.IO;

namespace UnitTest
{
    class FirstScene : World
    {
        public FirstScene()
        {
            Shape floor = new Sphere();
            floor.Transform = Transformation.Scaling(10, 0.01f, 10);
            floor.Material.Color = Tuple.Color(1, 0.9f, 0.9f);
            floor.Material.Specular = 0;

            Shapes.Add(floor);

            Shape leftWall = new Sphere();
            leftWall.Transform = Transformation.Translation(0, 0, 5) *
                                Transformation.RotationY(-PI / 4) * Transformation.RotationX(PI / 2) *
                                Transformation.Scaling(10, 0.2f, 10);

            leftWall.Material = floor.Material;

            Shapes.Add(leftWall);

            Shape rightWall = new Sphere();
            rightWall.Transform = Transformation.Translation(0, 0, 5) *
                                Transformation.RotationY(PI / 4) * Transformation.RotationX(PI / 2) *
                                Transformation.Scaling(10, 0.2f, 10);

            rightWall.Material = floor.Material;

            Shapes.Add(rightWall);

            Shape middle = new Sphere();
            middle.Transform = Transformation.Translation(-0.5f, 1, 0.5f);
            middle.Material.Color = Tuple.Color(0.1f, 1, 0.5f);
            middle.Material.Diffuse = 0.7f;
            middle.Material.Specular = 0.3f;

            Shapes.Add(middle);

            Shape right = new Sphere();
            right.Transform = Transformation.Translation(1.5f, 0.5f, -0.5f) * Transformation.Scaling(0.5f, 0.5f, 0.5f);
            right.Material.Color = Tuple.Color(0.5f, 1, 0.1f);
            right.Material.Diffuse = 0.7f;
            right.Material.Specular = 0.3f;

            Shapes.Add(right);

            Shape left = new Sphere();
            left.Transform = Transformation.Translation(-1.5f, 0.33f, -0.75f) * Transformation.Scaling(0.33f, 0.33f, 0.33f);
            left.Material.Color = Tuple.Color(1, 0.8f, 0.1f);
            left.Material.Diffuse = 0.7f;
            left.Material.Specular = 0.3f;

            Shapes.Add(left);

            Lights.Add(new PointLight(Tuple.Point(-10, 10, -10), Tuple.Color(1, 1, 1)));
        }
    }

    public static class FirstSceneTest
    {
        [Fact]
        private static void Simulate()
        {
            Camera c = new Camera(400, 200, PI / 3);
            c.Transform = Transformation.LookAt(Tuple.Point(0, 1.5f, -5),
                                                Tuple.Point(0, 1, 0),
                                                Tuple.Vector(0, 1, 0));
            World w = new FirstScene();
            LightingModel l = new PhongReflection();

            Canvas canvas = Canvas.Render(c, w, l);

            using (StreamWriter sw = new StreamWriter(@"./firstScene.ppm"))
            {
                sw.Write(canvas.ToPPM());
            }

            Assert.True(true);
        }
    }
}

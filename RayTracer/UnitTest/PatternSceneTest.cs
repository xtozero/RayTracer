using RayTracer;
using System.IO;
using Xunit;
using static System.MathF;

namespace UnitTest
{
    class PatternScene : World
    {
        public PatternScene()
        {
            Shape floor = new Plane();
            floor.Material.Specular = 0;
            floor.Material.Pattern = new CheckersPattern(Tuple.Color(0, 0, 0),
                                                        Tuple.Color(1, 1, 1));
            floor.Material.Pattern.Transform = Transformation.Translation(0, 0.1f, 0);

            Shapes.Add(floor);

            Shape[] sphere = new Sphere[3];

            sphere[0] = new Sphere();
            sphere[0].Material.Pattern = new StripePattern(Tuple.Color(0.908f, 0.901f, 0.882f),
                                                         Tuple.Color(0.699f, 0.199f, 0.316f));
            sphere[0].Material.Pattern.Transform = Transformation.Scaling(0.5f, 0.5f, 0.5f);
            sphere[0].Transform = Transformation.Translation(-1.2f, 0.5f, 0) * Transformation.Scaling(0.5f, 0.5f, 0.5f);
            Shapes.Add(sphere[0]);

            sphere[1] = new Sphere();
            sphere[1].Material.Pattern = new GradientPattern(Tuple.Color(0.908f, 0.901f, 0.882f),
                                                         Tuple.Color(0.199f, 0.699f, 0.316f));
            sphere[1].Material.Pattern.Transform = Transformation.Scaling(0.5f, 0.5f, 0.5f);
            sphere[1].Transform = Transformation.Translation(0, 0.5f, 0) * Transformation.Scaling(0.5f, 0.5f, 0.5f);
            Shapes.Add(sphere[1]);

            sphere[2] = new Sphere();
            sphere[2].Material.Pattern = new RingPattern(Tuple.Color(0.908f, 0.901f, 0.882f),
                                                         Tuple.Color(0.316f, 0.199f, 0.699f));
            sphere[2].Material.Pattern.Transform = Transformation.Scaling(0.1f, 0.1f, 0.1f) * Transformation.RotationX(PI/2);
            sphere[2].Transform = Transformation.Translation(1.2f, 0.5f, 0) * Transformation.Scaling(0.5f, 0.5f, 0.5f);
            Shapes.Add(sphere[2]);

            Lights.Add(new PointLight(Tuple.Point(-10, 10, -10), Tuple.Color(1, 1, 1)));
        }
    }

    public static class PatternSceneTest
    {
        [Fact]
        private static void Simulate()
        {
            Camera c = new Camera(400, 200, PI / 3);
            c.Transform = Transformation.LookAt(Tuple.Point(0, 1, -3.5f),
                                                Tuple.Point(0, 0.5f, 0),
                                                Tuple.Vector(0, 1, 0));

            PatternScene w = new PatternScene();
            LightingModel l = new PhongReflection();

            Canvas canvas = Canvas.Render(c, w, l);

            using (StreamWriter sw = new StreamWriter(@"./patternScene.ppm"))
            {
                sw.Write(canvas.ToPPM());
            }

            Assert.True(true);
        }
    }
}

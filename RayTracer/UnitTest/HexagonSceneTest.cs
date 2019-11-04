using RayTracer;
using Xunit;
using static System.MathF;
using System.IO;

namespace UnitTest
{
    class HexagonScene : World
    {
        public HexagonScene()
        {
            Shapes.Add(Hexagon());

            Light = new PointLight(Tuple.Point(0, 10, 0), Tuple.Color(1, 1, 1));
        }

        private static Shape HexagonCorner()
        {
            return new Sphere
            {
                Transform = Transformation.Translation(0, 0, -1) * Transformation.Scaling(0.25f, 0.25f, 0.25f)
            };
        }

        private static Shape HexagonEdge()
        {
            return new Cylinder
            {
                Minimum = 0,
                Maximum = 1,
                Transform = Transformation.Translation(0, 0, -1) *
                            Transformation.RotationY(-PI / 6) *
                            Transformation.RotationZ(-PI / 2) *
                            Transformation.Scaling(0.25f, 1, 0.25f)
            };
        }

        private static Shape HexagonSide()
        {
            Group side = new Group();

            side.AddChild(HexagonCorner());
            side.AddChild(HexagonEdge());

            return side;
        }

        private static Shape Hexagon()
        {
            Group hex = new Group();

            for (int i = 0; i < 6; ++i)
            {
                Shape side = HexagonSide();
                side.Transform = Transformation.RotationY(i * PI / 3);
                hex.AddChild(side);
            }

            return hex;
        }
    }

    public static class HexagonSceneTest
    {
        [Fact]
        private static void Simulate()
        {
            Camera c = new Camera( 400, 200, PI / 2.0f );
            c.Transform = Transformation.LookAt(Tuple.Point(0, 1.5f, -2.5f),
                                                Tuple.Point(0, 0.1f, 0),
                                                Tuple.Vector(0, 1, 0));

            HexagonScene w = new HexagonScene();
            LightingModel l = new PhongReflection();

            Canvas canvas = Canvas.Render(c, w, l);

            using (StreamWriter sw = new StreamWriter(@"./hexagonScene.ppm"))
            {
                sw.Write(canvas.ToPPM());
            }

            Assert.True(true);
        }
    }
}

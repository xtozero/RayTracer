using RayTracer;
using Xunit;

namespace UnitTest
{
    public static class JsonSceneTest
    {
        [Fact]
        private static void Test01()
        {
            // Parse reflect-refract scene
            Scene s = new JsonScene(@"../scene/reflect-refract.json");

            // Camera
            Assert.Equal(400, s.Camera.HSize);
            Assert.Equal(200, s.Camera.VSize);
            Assert.Equal(1.152f, s.Camera.FieldOfView, 5);
            Matrix cameraTransform = Transformation.LookAt(Tuple.Point(-2.6f, 1.5f, -3.9f),
                                                           Tuple.Point(-0.6f, 1, -0.8f),
                                                           Tuple.Vector(0, 1, 0));
            Assert.Equal(cameraTransform, s.Camera.Transform);

            // Light
            Assert.Equal(Tuple.Point(-4.9f, 4.9f, -1), s.World.Light.Position);
            Assert.Equal(Tuple.Color(1, 1, 1), s.World.Light.Intensity);

            // Shapes
            Shape floor = s.World.Shapes[0];
            Assert.Equal(Transformation.RotationY(0.31415f), floor.Transform);
            Assert.Equal(0, floor.Material.Specular);
            Assert.Equal(0.4f, floor.Material.Reflective);

            Shape ceiling = s.World.Shapes[1];
            Assert.Equal(Tuple.Color(0.8f, 0.8f, 0.8f), ceiling.Material.Color);

            Shape westWall = s.World.Shapes[2];
            Assert.Equal(Transformation.Translation(-5, 0, 0) * Transformation.RotationZ(1.5708f) * Transformation.RotationY(1.5708f), westWall.Transform);
            Assert.True(westWall.Material.Pattern is StripePattern);

            Shape redSphere = s.World.Shapes[10];
            Assert.True(redSphere is Sphere);

            Material blueGassMaterial = s.World.Shapes[11].Material;
            Assert.Equal(0, blueGassMaterial.Ambient, 5);
            Assert.Equal(0.4f, blueGassMaterial.Diffuse, 5);
            Assert.Equal(300, blueGassMaterial.Shininess, 5);
            Assert.Equal(0.9, blueGassMaterial.Reflective, 5);
            Assert.Equal(0.9, blueGassMaterial.Transparency, 5);
            Assert.Equal(1.5, blueGassMaterial.RefractiveIndex, 5);

            s.Capture(@"./reflectRefract.ppm");
        }
    }
}

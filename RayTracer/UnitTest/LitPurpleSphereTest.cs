using RayTracer;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace UnitTest
{
    public static class LitPurpleSphereTest
    {
        [Fact]
        private static void Simulate()
        {
            int canvasPixel = 256;
            Canvas c = new Canvas(canvasPixel, canvasPixel);
            Sphere s = new Sphere();
            s.Material.Color = Tuple.Color(1, 0.2f, 1);

            float wallSize = 7.0f;
            float half = wallSize * 0.5f;
            float pixelSize = wallSize / canvasPixel;

            Tuple lightPosition = Tuple.Point(-10, 10, -10);
            Tuple lightColor = Tuple.Color(1, 1, 1);
            Light light = new PointLight(lightPosition, lightColor);
            Tuple rayOrigin = Tuple.Point(0, 0, -5);
            LightingModel phong = new PhongReflection();
            for (int h = 0; h < c.Height; ++h)
            {
                float worldY = pixelSize * h - half;
                for (int w = 0; w < c.Width; ++w)
                {
                    float worldX = pixelSize * w - half;
                    Tuple position = Tuple.Point(worldX, worldY, 10);

                    Ray r = new Ray(rayOrigin, (position - rayOrigin).Normalize());
                    List<Intersection> xs = s.Intersect(r);

                    Intersection hit = Intersection.Hit(xs);
                    if (hit != null)
                    {
                        Tuple point = r.Position(hit.T);
                        Tuple normal = hit.Object.NormalAt(point, hit);
                        Tuple eye = -r.Direction;
                        Tuple color = phong.Lighting(hit.Object.Material, hit.Object, light, point, eye, normal, false);

                        c.WritePixel(w, c.Height - h, color);
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter(@"./purpleSphere.ppm"))
            {
                sw.Write(c.ToPPM());
            }

            Assert.True(true);
        }
    }
}

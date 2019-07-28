using RayTracer;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace UnitTest
{
    public class RedSphereTest
    {
        [Fact]
        public void Simulate()
        {
            int canvasPixel = 256;
            Canvas c = new Canvas(canvasPixel, canvasPixel);
            Sphere s = new Sphere();

            float wallSize = 7.0f;
            float half = wallSize * 0.5f;
            float pixelSize = wallSize / canvasPixel;

            Tuple rayOrigin = Tuple.Point(0, 0, -5);
            for ( int h = 0; h < c.Height; ++h )
            {
                float worldY = pixelSize * h - half;
                for (int w = 0; w < c.Width; ++w)
                {
                    float worldX = pixelSize * w - half;
                    Tuple position = Tuple.Point(worldX, worldY, 10);

                    Ray r = new Ray(rayOrigin, (position - rayOrigin).Normalize());
                    List<Intersection> xs = s.Intersect(r);

                    if ( Intersection.Hit(xs) != null )
                    {
                        c.WritePixel(w, h, Tuple.Color(1, 0, 0));
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter(@"./redSphere.ppm"))
            {
                sw.Write(c.ToPPM());
            }

            Assert.True(true);
        }
    }
}

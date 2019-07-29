using RayTracer;
using Xunit;

namespace UnitTest
{
    public static class ClockHourTest
    {
        [Fact]
        private static void Simulate()
        {
            Canvas c = new Canvas(256, 256);

            float halfHeight = c.Height * 0.5f;
            Tuple p = Tuple.Point(0, halfHeight * ( 3.0f / 4.0f ), 0);

            Matrix t = Transformation.RotationZ(System.MathF.PI / 6);

            float halfWidth = c.Width * 0.5f;
            Tuple white = Tuple.Color(1, 1, 1);
            for ( int i = 0; i < 12; ++i )
            {
                c.WritePixel((int)System.MathF.Round(p.X + halfWidth), (int)System.MathF.Round(p.Y + halfHeight), white);
                p = t * p;
            }

            using (System.IO.StreamWriter sw = 
                new System.IO.StreamWriter(@"./clockHour.ppm"))
            {
                sw.Write(c.ToPPM());
            }

            Assert.True(true);
        }
    }
}

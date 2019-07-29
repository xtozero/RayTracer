using RayTracer;
using Xunit;

namespace UnitTest
{
    public class CanvasTest
    {
        [Fact]
        private static void TestCase1()
        {
            Canvas c = new Canvas(10, 20);
            Assert.Equal(10, c.Width);
            Assert.Equal(20, c.Height);

            Tuple white = Tuple.Color(0, 0, 0);
            for ( int y = 0; y < c.Height; ++y )
            {
                for ( int x = 0; x < c.Width; ++x )
                {
                    Assert.Equal(white, c.PixelAt(x, y));
                }
            }
        }

        [Fact]
        private static void TestCase2()
        {
            Canvas c = new Canvas(10, 20);
            Tuple red = Tuple.Color(1, 0, 0);
            c.WritePixel(2, 3, red);
            Assert.Equal(red, c.PixelAt(2, 3));
        }

        [Fact]
        private static void TestCase3()
        {
            Canvas c = new Canvas(5, 3);
            string ppm = c.ToPPM();
            Assert.StartsWith("P3\r\n5 3\r\n255", ppm);
        }

        [Fact]
        private static void TestCase4()
        {
            Canvas c = new Canvas(5, 3);
            Tuple c1 = Tuple.Color(1.5f, 0, 0);
            Tuple c2 = Tuple.Color(0, 0.5f, 0);
            Tuple c3 = Tuple.Color(-0.5f, 0, 1);
            c.WritePixel(0, 0, c1);
            c.WritePixel(2, 1, c2);
            c.WritePixel(4, 2, c3);
            string ppm = c.ToPPM();
            Assert.Contains(
                " 255 0 0 0 0 0 0 0 0 0 0 0 0 0 0 \r\n" +
                " 0 0 0 0 0 0 0 128 0 0 0 0 0 0 0 \r\n" +
                " 0 0 0 0 0 0 0 0 0 0 0 0 0 0 255 \r\n"
                , ppm);
        }

        [Fact]
        private static void TestCase5()
        {
            Canvas c = new Canvas(10, 2);
            Tuple PeachOrange = Tuple.Color(1, 0.8f, 0.6f);
            for (int y = 0; y < c.Height; ++y)
            {
                for (int x = 0; x < c.Width; ++x)
                {
                    c.WritePixel(x, y, PeachOrange);
                }
            }
            string ppm = c.ToPPM();
            Assert.Contains(
                " 255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204 \r\n" +
                " 153 255 204 153 255 204 153 255 204 153 255 204 153 \r\n" +
                " 255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204 \r\n" +
                " 153 255 204 153 255 204 153 255 204 153 255 204 153 \r\n"
                , ppm);
        }

        [Fact]
        private static void TestCase6()
        {
            Canvas c = new Canvas(5, 3);
            string ppm = c.ToPPM();
            Assert.EndsWith("\n", ppm);
        }
    }
}

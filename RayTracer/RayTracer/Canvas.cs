using System.IO;

namespace RayTracer
{
    public class Canvas
    {
        public Tuple PixelAt(int x, int y)
        {
            return _pixels[y, x];
        }

        public void WritePixel(int x, int y, Tuple color)
        {
            _pixels[y, x] = color;
        }

        public string ToPPM()
        {
            StringWriter ppm = new StringWriter();

            ppm.WriteLine("P3");
            ppm.WriteLine("{0} {1}", Width, Height);
            ppm.WriteLine("255");
            int writedChar = 1;
            for (int y = 0; y < Height; ++y)
            {
                ppm.Write(" ");
                for (int x = 0; x < Width; ++x)
                {
                    Tuple c = _pixels[y, x];
                    var colors = new []
                    {
                        (byte)System.Math.Clamp(System.Math.Round(c.R * 255), 0, 255),
                        (byte)System.Math.Clamp(System.Math.Round(c.G * 255), 0, 255),
                        (byte)System.Math.Clamp(System.Math.Round(c.B * 255), 0, 255)
                    };

                    foreach (byte color in colors)
                    {
                        if ( writedChar + ( color.ToString().Length + 1 ) > 70 )
                        {
                            writedChar = 1;
                            ppm.WriteLine();
                            ppm.Write(" ");
                        }

                        ppm.Write("{0} ", color);
                        writedChar += (color.ToString().Length + 1);
                    }
                }
                writedChar = 1;
                ppm.WriteLine();
            }

            return ppm.ToString();
        }

        public static Canvas Render(Camera c, World w, LightingModel l)
        {
            Canvas image = new Canvas(c.HSize, c.VSize);

            for ( int y = 0; y < image.Height; ++y )
            {
                for ( int x = 0; x < image.Width; ++x )
                {
                    Ray r = c.RayForPixel(x, y);
                    Tuple color = l.ColorAt(w, r);
                    image.WritePixel(x, y, color);
                }
            }

            return image;
        }

        public Canvas(int width, int height)
        {
            Width = width;
            Height = height;
            _pixels = new Tuple[Height, Width];

            for ( int y = 0; y < Height; ++y )
            {
                for ( int x = 0; x < Width; ++x )
                {
                    _pixels[y, x] = Tuple.Color(0, 0, 0);
                }
            }
        }

        public int Width { get; set; }
        public int Height { get; set; }

        readonly Tuple[,] _pixels;
    }
}

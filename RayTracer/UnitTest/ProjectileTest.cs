using RayTracer;
using Xunit;

namespace UnitTest
{
    class Projectile
    {
        public Projectile(Tuple position, Tuple velocity)
        {
            Position = position;
            Velocity = velocity;
        }

        public Tuple Position { get; set; }
        public Tuple Velocity { get; set; }
    }

    class Enviroment
    {
        public Enviroment(Tuple gravity, Tuple wind)
        {
            Gravity = gravity;
            Wind = wind;
        }

        public Tuple Gravity { get; set; }
        public Tuple Wind { get; set; }
    }

    public class ProjectileTest
    {
        [Fact]
        public void Simulate()
        {
            Tuple start = Tuple.Point(0, 1, 0);
            Tuple velocity = Tuple.Vector(1, 1.8f, 0).Normalize() * 11.25f;
            Projectile p = new Projectile(start, velocity);

            Tuple gravity = Tuple.Vector(0, -0.1f, 0);
            Tuple wind = Tuple.Vector(-0.01f, 0, 0);
            Enviroment e = new Enviroment(gravity, wind);

            Canvas c = new Canvas(900, 550);

            Tuple red = Tuple.Color(1, 0, 0);
            while (p.Position.Y > 0)
            {
                p = Tick(e, p);

                int x = (int)System.MathF.Round(p.Position.X);
                int y = (int)System.MathF.Round(p.Position.Y);

                if ( x >= 0 && y >= 0 && x < c.Width && y < c.Height )
                {
                    c.WritePixel(x, c.Height - y, red);
                }
            }

            string ppm = c.ToPPM();

            using (System.IO.StreamWriter sw =
                new System.IO.StreamWriter(@"./projectile.ppm"))
            {
                sw.Write(ppm);
            }

            Assert.True(true);
        }

        private Projectile Tick(Enviroment env, Projectile proj)
        {
            Tuple position = proj.Position + proj.Velocity;
            Tuple velocity = proj.Velocity + env.Gravity + env.Wind;
            return new Projectile(position, velocity);
        }
    }
}

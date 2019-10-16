using System.Collections.Generic;
using System.Linq;

namespace RayTracer
{
    public class Computation
    {
        public Computation( Intersection i, Ray r )
        {
            T = i.T;
            Object = i.Object;

            Point = r.Position(T);
            Eyev = -r.Direction;
            Normalv = Object.NormalAt(Point);

            if ( Eyev.Dot(Normalv) < 0 )
            {
                Inside = true;
                Normalv = -Normalv;
            }
            else
            {
                Inside = false;
            }

            OverPoint = Point + Normalv * Constants.floatEps;
            UnderPoint = Point - Normalv * Constants.floatEps;
            Reflectv = r.Direction.Reflect(Normalv);
        }

        public Computation(Intersection i, Ray r, List<Intersection> xs) : this(i, r)
        {
            List<Shape> containers = new List<Shape>();

            foreach (Intersection candidate in xs)
            {
                if (candidate == i)
                {
                    if (containers.Count == 0)
                    {
                        N1 = 1;
                    }
                    else
                    {
                        N1 = containers.Last().Material.RefractiveIndex;
                    }
                }

                int found = containers.IndexOf(candidate.Object);
                if (found != -1)
                {
                    containers.RemoveAt(found);
                }
                else
                {
                    containers.Add(candidate.Object);
                }

                if (candidate == i)
                {
                    if (containers.Count == 0)
                    {
                        N2 = 1;
                    }
                    else
                    {
                        N2 = containers.Last().Material.RefractiveIndex;
                    }
                    break;
                }
            }
        }

        public float T { get; private set; }
        public Shape Object { get; private set; }
        public Tuple Point { get; private set; }
        public Tuple OverPoint { get; private set; }
        public Tuple UnderPoint { get; private set; }
        public Tuple Eyev { get; private set; }
        public Tuple Normalv { get; private set; }
        public bool Inside { get; private set; }
        public Tuple Reflectv { get; private set; }
        public float N1 { get; private set; }
        public float N2 { get; private set; }
    }
}

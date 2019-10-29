using System.Collections.Generic;
using static System.MathF;

namespace RayTracer
{
    public class Cylinder : Shape
    {
        protected override List<Intersection> LocalIntersect(Ray localRay)
        {
            float a = localRay.Direction.X * localRay.Direction.X + localRay.Direction.Z * localRay.Direction.Z;

            List<Intersection> xs = Intersection.Aggregate();

            if (Abs(a) < Constants.floatEps)
            {
                IntersectCaps(localRay, ref xs);
                return xs;
            }

            float b = localRay.Origin.X * localRay.Direction.X + localRay.Origin.Z * localRay.Direction.Z;
            float c = localRay.Origin.X * localRay.Origin.X + localRay.Origin.Z * localRay.Origin.Z - 1;

            float disc = b * b - a * c;
            if (disc < 0)
            {
                if (Abs(disc) >= Constants.floatEps)
                {
                    return Intersection.Aggregate();
                }

                disc = 0;
            }

            float t0 = (-b - Sqrt(disc)) / a;
            float t1 = (-b + Sqrt(disc)) / a;

            float y0 = localRay.Origin.Y + t0 * localRay.Direction.Y;
            if ( Minimum < y0 && y0 < Maximum )
            {
                xs.Add(new Intersection(t0, this));
            }

            float y1 = localRay.Origin.Y + t1 * localRay.Direction.Y;
            if ( Minimum < y1 && y1 < Maximum )
            {
                xs.Add(new Intersection(t1, this));
            }

            IntersectCaps(localRay, ref xs);
            return xs;
        }

        protected override Tuple LocalNormalAt(Tuple localPoint)
        {
            float dist = localPoint.X * localPoint.X + localPoint.Z * localPoint.Z;

            if (dist < 1)
            {
                if (localPoint.Y >= Maximum - Constants.floatEps)
                {
                    return Tuple.Vector(0, 1, 0);
                }
                else if (localPoint.Y <= Minimum + Constants.floatEps)
                {
                    return Tuple.Vector(0, -1, 0);
                }
            }

            return Tuple.Vector(localPoint.X, 0, localPoint.Z);
        }

        protected bool CheckCap(Ray r, float t)
        {
            Tuple pos = r.Position(t);

            return (pos.X * pos.X - pos.Y * pos.Y + pos.Z * pos.Z) < Constants.floatEps;
        }

        protected void IntersectCaps(Ray r, ref List<Intersection> xs)
        {
            if ( ( Closed == false ) || Abs( r.Direction.Y ) < Constants.floatEps )
            {
                return;
            }

            float t = (Minimum - r.Origin.Y) / r.Direction.Y;
            if (CheckCap(r, t))
            {
                xs.Add(new Intersection(t, this));
            }

            t = (Maximum - r.Origin.Y) / r.Direction.Y;
            if (CheckCap(r, t))
            {
                xs.Add(new Intersection(t, this));
            }
        }

        public Cylinder()
        {
            Minimum = float.NegativeInfinity;
            Maximum = float.PositiveInfinity;
        }

        public float Minimum { get; set; }
        public float Maximum { get; set; }
        public bool Closed { get; set; }
    }
}

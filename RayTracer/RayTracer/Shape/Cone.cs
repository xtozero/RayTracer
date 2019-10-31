using System.Collections.Generic;
using static System.MathF;

namespace RayTracer
{
    public class Cone : Cylinder
    {
        protected override List<Intersection> LocalIntersect(Ray localRay)
        {
            float a = localRay.Direction.X * localRay.Direction.X -
                    localRay.Direction.Y * localRay.Direction.Y +
                    localRay.Direction.Z * localRay.Direction.Z;

            float b = localRay.Origin.X * localRay.Direction.X -
                    localRay.Origin.Y * localRay.Direction.Y +
                    localRay.Origin.Z * localRay.Direction.Z;

            float c = localRay.Origin.X * localRay.Origin.X -
                    localRay.Origin.Y * localRay.Origin.Y +
                    localRay.Origin.Z * localRay.Origin.Z;

            List<Intersection> xs = Intersection.Aggregate();

            if (Abs(a) < Constants.floatEps)
            {
                if (Abs(b) < Constants.floatEps)
                {
                    IntersectCaps(localRay, xs);
                    return xs;
                }

                xs.Add(new Intersection(-c / 2 * b, this));
            }

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
            if (Minimum < y0 && y0 < Maximum)
            {
                xs.Add(new Intersection(t0, this));
            }

            float y1 = localRay.Origin.Y + t1 * localRay.Direction.Y;
            if (Minimum < y1 && y1 < Maximum)
            {
                xs.Add(new Intersection(t1, this));
            }

            IntersectCaps(localRay, xs);
            return xs;
        }

        protected override Tuple LocalNormalAt(Tuple localPoint)
        {
            float dist = localPoint.X * localPoint.X + localPoint.Z * localPoint.Z;

            if (dist < (localPoint.Y * localPoint.Y))
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

            float y = ( localPoint.Y > 0 ) ? -Sqrt(dist) : Sqrt(dist);
            return Tuple.Vector(localPoint.X, y, localPoint.Z);
        }

        protected override bool CheckCap(Ray r, float t)
        {
            Tuple pos = r.Position(t);

            return (pos.X * pos.X - pos.Y * pos.Y + pos.Z * pos.Z) < Constants.floatEps;
        }
    }
}

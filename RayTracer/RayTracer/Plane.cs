using System;
using System.Collections.Generic;
using static System.MathF;

namespace RayTracer
{
    public class Plane : Shape
    {
        protected override Tuple LocalNormalAt(Tuple p)
        {
            return Tuple.Vector(0, 1, 0);
        }

        protected override List<Intersection> LocalIntersect(Ray r)
        {
            if (Abs(r.Direction.Y) < Constants.floatEps)
            {
                return Intersection.Aggregate();
            }
            else
            {
                return Intersection.Aggregate(new Intersection(
                    -r.Origin.Y / r.Direction.Y,
                    this
                    ));
            }
        }
    }
}

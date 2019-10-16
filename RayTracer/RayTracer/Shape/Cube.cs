using System.Collections.Generic;
using static System.MathF;

namespace RayTracer
{
    public class Cube : Shape
    {
        private static void CheckAxis(float origin, float direction, out float tmin, out float tmax)
        {
            float tminNumerator = (-origin - 1);
            float tmaxNumerator = (-origin + 1);

            if (Abs(direction) > float.Epsilon)
            {
                tmin = tminNumerator / direction;
                tmax = tmaxNumerator / direction;
            }
            else
            {
                tmin = tminNumerator * float.PositiveInfinity;
                tmax = tmaxNumerator * float.PositiveInfinity;
            }

            if (tmin > tmax)
            {
                float tmp = tmin;
                tmin = tmax;
                tmax = tmp;
            }
        }

        protected override List<Intersection> LocalIntersect(Ray localRay)
        {
            CheckAxis(localRay.Origin.X, localRay.Direction.X, out float xtmin, out float xtmax);
            CheckAxis(localRay.Origin.Y, localRay.Direction.Y, out float ytmin, out float ytmax);
            CheckAxis(localRay.Origin.Z, localRay.Direction.Z, out float ztmin, out float ztmax);

            float tmin = Max(Max(xtmin, ytmin), ztmin);
            float tmax = Min(Min(xtmax, ytmax), ztmax);

            if (tmin > tmax)
            {
                return Intersection.Aggregate();
            }

            return Intersection.Aggregate(new Intersection(tmin, this), new Intersection(tmax, this));
        }

        protected override Tuple LocalNormalAt(Tuple localPoint)
        {
            float absX = Abs(localPoint.X);
            float absY = Abs(localPoint.Y);
            float absZ = Abs(localPoint.Z);

            float maxc = Max(Max(absX, absY), absZ);

            if (Abs(absX - maxc) < Constants.floatEps)
            {
                return Tuple.Vector(localPoint.X, 0, 0);
            }
            else if (Abs(absY - maxc) < Constants.floatEps)
            {
                return Tuple.Vector(0, localPoint.Y, 0);
            }

            return Tuple.Vector(0, 0, localPoint.Z);
        }
    }
}

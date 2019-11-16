using System.Collections.Generic;

namespace RayTracer
{
    public class Sphere : Shape
    {
        protected override Tuple LocalNormalAt(Tuple localPoint)
        {
            return localPoint - Tuple.Point(0, 0, 0);
        }

        protected override List<Intersection> LocalIntersect(Ray localRay)
        {
            Tuple sphereToRay = localRay.Origin - Tuple.Point(0, 0, 0);

            float a = localRay.Direction.Dot(localRay.Direction);
            float b = localRay.Direction.Dot(sphereToRay);
            float c = sphereToRay.Dot(sphereToRay) - 1;
             
            float discriminant = b * b - a * c;

            if (discriminant >= 0)
            {
                float sqrtDisc = System.MathF.Sqrt(discriminant);
                return Intersection.Aggregate(
                    new Intersection((-b - sqrtDisc) / a, this),
                    new Intersection((-b + sqrtDisc) / a, this));
            }

            return Intersection.Aggregate();
        }

        protected override Shape CloneImple()
        {
            return new Sphere
            {
                Material = Material.Clone() as Material,
                Transform = Transform.Clone() as Matrix,
                Parent = Parent
            };
        }
    }
}

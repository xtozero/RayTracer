using System.Collections.Generic;

namespace RayTracer
{
    public class Sphere : Shape
    {
        public override Tuple NormalAt(Tuple p)
        {
            Tuple objectNormal = Transform.Inverse() * p;
            objectNormal = objectNormal - Tuple.Point(0, 0, 0);
            Tuple worldNormal = Transform.Inverse().Transpose() * objectNormal;
            worldNormal.W = 0;
            return worldNormal.Normalize();
        }

        public override List<Intersection> Intersect(Ray r)
        {
            Ray objectSpaceRay = r.Transform(Transform.Inverse());

            Tuple sphereToRay = objectSpaceRay.Origin - Tuple.Point(0, 0, 0);

            float a = objectSpaceRay.Direction.Dot(objectSpaceRay.Direction);
            float b = objectSpaceRay.Direction.Dot(sphereToRay);
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

        public Sphere()
        {
            Transform = Matrix.Identity();
            Material = new Material();
        }
    }
}

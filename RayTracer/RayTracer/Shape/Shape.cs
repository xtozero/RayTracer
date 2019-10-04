using System;
using System.Collections.Generic;

namespace RayTracer
{
    public abstract class Shape : IEquatable<Shape>
    {
        protected abstract Tuple LocalNormalAt(Tuple localPoint);
        protected abstract List<Intersection> LocalIntersect(Ray localRay);

        public Tuple NormalAt(Tuple p)
        {
            Tuple localPoint = Transform.Inverse() * p;
            Tuple localNormal = LocalNormalAt(localPoint);
            Tuple worldNormal = Transform.Inverse().Transpose() * localNormal;
            worldNormal.W = 0;
            return worldNormal.Normalize();
        }

        public List<Intersection> Intersect(Ray r)
        {
            Ray localRay = r.Transform(Transform.Inverse());
            return LocalIntersect(localRay);
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is Shape s)
            {
                return Equals(s);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Transform.GetHashCode(), Material.GetHashCode());
        }

        public bool Equals(Shape other)
        {
            return Transform.Equals(other.Transform) && Material.Equals(other.Material);
        }

        protected Shape()
        {
            Transform = Matrix.Identity();
            Material = new Material();
        }

        public Matrix Transform { get; set; }
        public Material Material { get; set; }
    }
}

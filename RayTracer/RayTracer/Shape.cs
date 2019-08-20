using System;
using System.Collections.Generic;

namespace RayTracer
{
    public abstract class Shape : IEquatable<Shape>
    {
        public abstract Tuple NormalAt(Tuple p);
        public abstract List<Intersection> Intersect(Ray r);

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

        public Matrix Transform { get; set; }
        public Material Material { get; set; }
    }
}

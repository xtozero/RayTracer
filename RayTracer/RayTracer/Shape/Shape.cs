using System;
using System.Collections.Generic;

namespace RayTracer
{
    public abstract class Shape : IEquatable<Shape>
    {
        protected abstract Tuple LocalNormalAt(Tuple localPoint, Intersection hit);
        protected abstract List<Intersection> LocalIntersect(Ray localRay);
        protected abstract Shape CloneImple();

        public Tuple NormalAt(Tuple p, Intersection hit)
        {
            Tuple localPoint = WorldToObject(p);
            Tuple localNormal = LocalNormalAt(localPoint, hit);
            return NormalToWorld(localNormal);
        }

        public List<Intersection> Intersect(Ray r)
        {
            Ray localRay = r.Transform(Transform.Inverse());
            return LocalIntersect(localRay);
        }

        public override bool Equals(object obj)
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

        private Tuple WorldToObject(Tuple point)
        {
            if (Parent != null)
            {
                point = Parent.WorldToObject(point);
            }

            return Transform.Inverse() * point;
        }

        private Tuple NormalToWorld(Tuple normal)
        {
            normal = Transform.Inverse().Transpose() * normal;
            normal.W = 0;
            normal = normal.Normalize();

            if (Parent != null)
            {
                normal = Parent.NormalToWorld(normal);
            }

            return normal;
        }

        // perform deep copy
        public object Clone()
        {
            return CloneImple();
        }

        public Matrix Transform { get; set; }
        public Material Material { get; set; }
        public Shape Parent { get; set; }
    }
}

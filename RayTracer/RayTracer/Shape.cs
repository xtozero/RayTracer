using System.Collections.Generic;

namespace RayTracer
{
    public abstract class Shape
    {
        public abstract Tuple NormalAt(Tuple p);
        public abstract List<Intersection> Intersect(Ray r);

        public Matrix Transform { get; set; }
        public Material Material { get; set; }
    }
}

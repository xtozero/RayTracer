using System.Collections.Generic;

namespace RayTracer
{
    public class World
    {
        public bool IsShadowed(Tuple p)
        {
            Tuple v = Light.Position - p;
            float distance = v.Magnitude();
            Tuple direction = v.Normalize();

            Ray r = new Ray(p, direction);
            List<Intersection> intersections = Intersection(r);

            RayTracer.Intersection h = RayTracer.Intersection.Hit(intersections);
            return (h != null) && (h.T < distance);
        }

        public List<Intersection> Intersection(Ray r)
        {
            List<Intersection> result = new List<Intersection>();

            foreach( Shape s in Shapes )
            {
                result.AddRange(s.Intersect(r));
            }

            result.Sort(new IntersectionCompare());

            return result;
        }

        public World()
        {
            Shapes = new List<Shape>();
        }

        public Light Light { get; set; }
        public List<Shape> Shapes { get; private set; }
    }
}

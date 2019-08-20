using System.Collections.Generic;

namespace RayTracer
{
    public class World
    {
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
        public List<Shape> Shapes { get; set; }
    }
}

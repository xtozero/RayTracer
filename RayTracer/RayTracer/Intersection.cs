using System.Collections.Generic;

namespace RayTracer
{
    public class Intersection
    {
        public static Intersection Hit(List<Intersection> intersections)
        {
            Intersection result = null;
            float minT = float.MaxValue;

            foreach ( Intersection i in intersections )
            {
                if ( i.T >= 0 && minT > i.T )
                {
                    minT = i.T;
                    result = i;
                }
            }

            return result;
        }

        public static List<Intersection> Aggregate(params Intersection[] i)
        {
            return new List<Intersection>(i);
        }

        public Intersection(float t, Shape obj)
        {
            T = t;
            Object = obj;
        }

        public float T { get; set; }
        public Shape Object { get; set; }
    }
}

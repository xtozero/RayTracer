using System;
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

        public float T { get; private set; }
        public Shape Object { get; private set; }

        public virtual float U { get { throw new NotSupportedException(); } protected set { throw new NotSupportedException(); } }
        public virtual float V { get { throw new NotSupportedException(); } protected set { throw new NotSupportedException(); } }
    }

    public class IntersectionWithUV : Intersection
    {
        public IntersectionWithUV(float t, Shape obj, float u, float v) : base(t, obj)
        {
            U = u;
            V = v;
        }

        public override float U { get; protected set; }
        public override float V { get; protected set; }
    }

    public class IntersectionCompare : IComparer<Intersection>
    {
        public int Compare(Intersection x, Intersection y)
        {
            return x.T.CompareTo(y.T);
        }
    }
}

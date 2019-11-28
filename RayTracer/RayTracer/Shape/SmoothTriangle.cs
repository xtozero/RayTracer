using System.Collections.Generic;

namespace RayTracer
{
    public class SmoothTriangle : Triangle
    {
        protected override Tuple LocalNormalAt(Tuple localPoint, Intersection hit)
        {
            return N2 * hit.U + N3 * hit.V + N1 * (1 - hit.U - hit.V);
        }

        protected override List<Intersection> LocalIntersect(Ray localRay)
        {
            (bool success, float t, float u, float v) = TestLocalIntersection(localRay);

            if (success)
            {
                return Intersection.Aggregate(new IntersectionWithUV(t, this, u, v));
            }
            else
            {
                return Intersection.Aggregate();
            }
        }

        protected override Shape CloneImple()
        {
            return new SmoothTriangle(P1, P2, P3, N1, N2, N3)
            {
                Material = Material.Clone() as Material,
                Transform = Transform.Clone() as Matrix,
                Parent = Parent
            };
        }

        public SmoothTriangle(Tuple p1, Tuple p2, Tuple p3, Tuple n1, Tuple n2, Tuple n3) 
            : base(p1, p2, p3)
        {
            N1 = n1;
            N2 = n2;
            N3 = n3;
        }

        public Tuple N1 { get; set; }
        public Tuple N2 { get; set; }
        public Tuple N3 { get; set; }
    }
}

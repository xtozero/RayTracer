﻿using System.Collections.Generic;
using static System.MathF;

namespace RayTracer
{
    public class Triangle : Shape
    {
        protected override Tuple LocalNormalAt(Tuple localPoint, Intersection hit)
        {
            return Normal;
        }

        protected override List<Intersection> LocalIntersect(Ray localRay)
        {
            (bool success, float t, _, _) = TestLocalIntersection(localRay);

            if (success)
            {
                return Intersection.Aggregate(new Intersection(t, this));
            }
            else
            {
                return Intersection.Aggregate();
            }
        }

        protected override Shape CloneImple()
        {
            return new Triangle(P1, P2, P3)
            {
                Material = Material.Clone() as Material,
                Transform = Transform.Clone() as Matrix,
                Parent = Parent
            };
        }

        public Triangle(Tuple p1, Tuple p2, Tuple p3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;

            E1 = p2 - p1;
            E2 = p3 - p1;

            Normal = E2.Cross(E1).Normalize();
        }

        protected (bool, float, float, float) TestLocalIntersection(Ray localRay)
        {
            Tuple dirCrossE2 = localRay.Direction.Cross(E2);
            float det = E1.Dot(dirCrossE2);

            if (Abs(det) < Constants.floatEps)
            {
                return (false, 0, 0, 0);
            }

            float invDet = 1.0f / det;

            Tuple p1ToOrigin = localRay.Origin - P1;
            float u = invDet * p1ToOrigin.Dot(dirCrossE2);

            if (u < 0 || u > 1)
            {
                return (false, 0, 0, 0);
            }

            Tuple originCrossE1 = p1ToOrigin.Cross(E1);
            float v = invDet * localRay.Direction.Dot(originCrossE1);

            if (v < 0 || (u + v) > 1)
            {
                return (false, 0, 0, 0);
            }

            float t = invDet * E2.Dot(originCrossE1);

            return (true, t, u, v);
        }

        public Tuple P1 { get; private set; }
        public Tuple P2 { get; private set; }
        public Tuple P3 { get; private set; }

        public Tuple E1 { get; private set; }
        public Tuple E2 { get; private set; }

        public Tuple Normal { get; private set; }
    }
}

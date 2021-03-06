﻿using System;
using System.Collections.Generic;

namespace RayTracer
{
    public class Group : Shape
    {
        protected override Tuple LocalNormalAt(Tuple localPoint, Intersection hit)
        {
            throw new NotSupportedException();
        }

        protected override List<Intersection> LocalIntersect(Ray localRay)
        {
            List<Intersection> result = Intersection.Aggregate();

            foreach (Shape s in Shapes)
            {
                result.AddRange(s.Intersect(localRay));
            }

            result.Sort(new IntersectionCompare());

            return result;
        }

        protected override Shape CloneImple()
        {
            var group = new Group
            {
                Transform = Transform.Clone() as Matrix,
                Parent = Parent,
            };

            foreach (var shape in Shapes)
            {
                group.AddChild(shape.Clone() as Shape);
            }

            return group;
        }

        public void AddChild(Shape s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("Can't add null child");
            }

            s.Parent = this;
            Shapes.Add(s);
        }

        public Group()
        {
            Shapes = new List<Shape>();
        }

        public List<Shape> Shapes { get; private set; }
    }
}

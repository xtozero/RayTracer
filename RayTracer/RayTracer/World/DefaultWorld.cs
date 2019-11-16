using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer
{
    public class DefaultWorld : World
    {
        public DefaultWorld()
        {
            Lights.Add(new PointLight(Tuple.Point(-10, 10, -10), Tuple.Color(1, 1, 1)));

            Shape s1 = new Sphere();
            s1.Material.Color = Tuple.Color(0.8f, 1, 0.6f);
            s1.Material.Diffuse = 0.7f;
            s1.Material.Specular = 0.2f;

            Shapes.Add(s1);

            Shape s2 = new Sphere();
            s2.Transform = Transformation.Scaling(0.5f, 0.5f, 0.5f);

            Shapes.Add(s2);
        }
    }
}

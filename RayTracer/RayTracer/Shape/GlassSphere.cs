using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer
{
    public class GlassSphere : Sphere
    {
        public GlassSphere()
        {
            Material.Transparency = 1;
            Material.RefractiveIndex = 1.5f;
        }
    }
}

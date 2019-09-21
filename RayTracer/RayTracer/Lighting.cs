using System.Collections.Generic;
using static System.MathF;

namespace RayTracer
{
    public abstract class LightingModel
    {
        public abstract Tuple Lighting(Material m, Light l, Tuple position, Tuple toEye, Tuple normal, bool inShadow);

        public Tuple ShadeHit(World w, Computation comps)
        {
            bool shadowed = w.IsShadowed(comps.OverPoint);

            return Lighting(comps.Object.Material,
                w.Light,
                comps.OverPoint,
                comps.Eyev,
                comps.Normalv,
                shadowed);
        }

        public Tuple ColorAt(World w, Ray r)
        {
            List<Intersection> intersections = w.Intersection(r);
            Intersection intersection = Intersection.Hit(intersections);
            if (intersection == null)
            {
                return Tuple.Color(0, 0, 0);
            }
            Computation comps = new Computation(intersection, r);
            return ShadeHit(w, comps);
        }
    }

    public class PhongReflection : LightingModel
    {
        public override Tuple Lighting(Material m, Light l, Tuple position, Tuple toEye, Tuple normal, bool inShadow)
        {
            Tuple effectiveColor = m.Color * l.Intensity;

            Tuple toLight = (l.Position - position).Normalize();

            Tuple ambient = effectiveColor * m.Ambient;

            float ndotl = normal.Dot(toLight);

            Tuple diffuse = Tuple.Color(0, 0, 0);
            Tuple specular = Tuple.Color(0, 0, 0);

            if ((inShadow == false) && (ndotl >= 0))
            {
                diffuse = effectiveColor * m.Diffuse * ndotl;

                Tuple reflect = (-toLight).Reflect(normal);
                float rdote = reflect.Dot(toEye);

                if (rdote > 0)
                {
                    specular = l.Intensity * m.Specular * Pow(rdote, m.Shininess);
                }
            }

            return ambient + diffuse + specular;
        }
    }
}

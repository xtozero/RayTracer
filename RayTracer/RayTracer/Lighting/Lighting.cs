using System.Collections.Generic;
using static System.MathF;

namespace RayTracer
{
    public static class Fresnel
    {
        public static float Schlick(Computation comps)
        {
            float cosTheta_i = comps.Eyev.Dot(comps.Normalv);

            if (comps.N1 > comps.N2)
            {
                float nRatio = comps.N1 / comps.N2;
                float sin2Theta_t = nRatio * nRatio * (1 - cosTheta_i * cosTheta_i);
                if (sin2Theta_t >= 1)
                {
                    return 1;
                }

                cosTheta_i = Sqrt(1 - sin2Theta_t);
            }

            float r0 = (comps.N1 - comps.N2) / (comps.N1 + comps.N2);
            r0 *= r0;

            return r0 + (1 - r0) * Pow(1 - cosTheta_i, 5);
        }
    }

    public abstract class LightingModel
    {
        public abstract Tuple Lighting(Material m, Shape shape, Light l, Tuple position, Tuple toEye, Tuple normal, bool inShadow);

        public Tuple ShadeHit(World w, Computation comps, int remaining = 5)
        {
            bool shadowed = w.IsShadowed(comps.OverPoint);

            Tuple surface = Lighting(comps.Object.Material,
                comps.Object,
                w.Light,
                comps.Point,
                comps.Eyev,
                comps.Normalv,
                shadowed);

            Tuple reflected = ReflectedColor(w, comps, remaining);
            Tuple refracted = RefractedColor(w, comps, remaining);

            Material material = comps.Object.Material;
            if (material.Reflective > 0 && material.Transparency > 0)
            {
                float reflectance = Fresnel.Schlick(comps);
                return surface + reflected * reflectance + refracted * (1 - reflectance);
            }

            return surface + reflected + refracted;
        }

        public Tuple ColorAt(World w, Ray r, int remaining = 5)
        {
            List<Intersection> intersections = w.Intersection(r);
            Intersection intersection = Intersection.Hit(intersections);
            if (intersection == null)
            {
                return Tuple.Color(0, 0, 0);
            }
            Computation comps = new Computation(intersection, r);
            return ShadeHit(w, comps, remaining);
        }

        public Tuple ReflectedColor(World w, Computation comps, int remaining = 5)
        {
            if ((remaining <= 0) ||
                (Abs(comps.Object.Material.Reflective) < Constants.floatEps))
            {
                return Tuple.Color(0, 0, 0);
            }

            Ray reflectRay = new Ray(comps.OverPoint, comps.Reflectv);
            Tuple color = ColorAt(w, reflectRay, remaining - 1);

            return color * comps.Object.Material.Reflective;
        }

        public Tuple RefractedColor(World w, Computation comps, int remaining = 5)
        {
            if ((remaining <= 0) ||
                (Abs(comps.Object.Material.Transparency) < Constants.floatEps))
            {
                return Tuple.Color(0, 0, 0);
            }

            float nRatio = comps.N1 / comps.N2;
            float cosTheta_i = comps.Eyev.Dot(comps.Normalv);
            float sin2Theta_t = nRatio * nRatio * (1 - cosTheta_i * cosTheta_i);

            if (sin2Theta_t >= 1)
            {
                return Tuple.Color(0, 0, 0);
            }

            float cosTheta_t = Sqrt(1 - sin2Theta_t);

            Tuple direction = comps.Normalv * (nRatio * cosTheta_i - cosTheta_t) - comps.Eyev * nRatio;

            Ray refractRay = new Ray(comps.UnderPoint, direction);
            Tuple color = ColorAt(w, refractRay, remaining - 1);

            return color * comps.Object.Material.Transparency;
        }
    }

    public class PhongReflection : LightingModel
    {
        public override Tuple Lighting(Material m, Shape shape, Light l, Tuple position, Tuple toEye, Tuple normal, bool inShadow)
        {
            Tuple color = (m.Pattern == null)? m.Color : m.Pattern.ColorAtShape(shape, position);

            Tuple effectiveColor = color * l.Intensity;

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

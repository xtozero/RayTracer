using static System.MathF;

namespace RayTracer
{
    public static class PhongReflection
    {
        public static Tuple Lighting(Material m, Light l, Tuple position, Tuple toEye, Tuple normal)
        {
            Tuple effectiveColor = m.Color * l.Intensity;

            Tuple toLight = (l.Position - position).Normalize();

            Tuple ambient = effectiveColor * m.Ambient;

            float ndotl = normal.Dot(toLight);

            Tuple diffuse = Tuple.Color(0, 0, 0);
            Tuple specular = Tuple.Color(0, 0, 0);

            if ( ndotl >= 0 )
            {
                diffuse = effectiveColor * m.Diffuse * ndotl;

                Tuple reflect = (-toLight).Reflect(normal);
                float rdote = reflect.Dot(toEye);

                if (rdote > 0)
                {
                    specular = effectiveColor * m.Specular * Pow(rdote, m.Shininess);
                }
            }

            return ambient + diffuse + specular;
        }
    }
}

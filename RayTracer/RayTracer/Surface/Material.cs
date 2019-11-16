using System;

namespace RayTracer
{
    public class Material : IEquatable<Material>, ICloneable
    {
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is Material m)
            {
                return Equals(m);
            }

            return false;
        }

        public bool Equals(Material other)
        {
            return Color.Equals(other.Color) &&
                Ambient.Equals(other.Ambient) &&
                Diffuse.Equals(other.Diffuse) &&
                Specular.Equals(other.Specular) &&
                Shininess.Equals(other.Shininess);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Color, Ambient, Diffuse, Specular, Shininess);
        }

        public object Clone()
        {
            return new Material
            {
                Color = Color.Clone() as Tuple,
                Ambient = Ambient,
                Diffuse = Diffuse,
                Specular = Specular,
                Shininess = Shininess,
                Pattern = Pattern?.Clone() as Pattern,
                Reflective = Reflective,
                Transparency = Transparency,
                RefractiveIndex = RefractiveIndex,
            };
        }

        public Material()
        {
            Color = Tuple.Color(1, 1, 1);
            Ambient = 0.1f;
            Diffuse = 0.9f;
            Specular = 0.9f;
            Shininess = 200.0f;
            RefractiveIndex = 1;
        }

        public Tuple Color { get; set; }
        public float Ambient { get; set; }
        public float Diffuse { get; set; }
        public float Specular { get; set; }
        public float Shininess { get; set; }
        public Pattern Pattern { get; set; }
        public float Reflective { get; set; }
        public float Transparency { get; set; }
        public float RefractiveIndex { get; set; }
    }
}

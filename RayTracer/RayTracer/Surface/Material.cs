using System;

namespace RayTracer
{
    public class Material : IEquatable<Material>
    {
        public override bool Equals(Object obj)
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

        public Material()
        {
            Color = Tuple.Color(1, 1, 1);
            Ambient = 0.1f;
            Diffuse = 0.9f;
            Specular = 0.9f;
            Shininess = 200.0f;
        }

        public Tuple Color { get; set; }
        public float Ambient { get; set; }
        public float Diffuse { get; set; }
        public float Specular { get; set; }
        public float Shininess { get; set; }
        public Pattern Pattern { get; set; }
    }
}

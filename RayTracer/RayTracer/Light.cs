using System;

namespace RayTracer
{
    public abstract class Light : IEquatable<Light>
    {
        protected Light(Tuple position, Tuple intensity)
        {
            Intensity = intensity;
            Position = position;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is Light t)
            {
                return Equals(t);
            }

            return false;
        }

        public bool Equals(Light other)
        {
            return Intensity.Equals(other.Intensity) &&
                Position.Equals(other.Position);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Intensity.GetHashCode(), Position.GetHashCode());
        }

        public Tuple Intensity { get; set; }
        public Tuple Position { get; set; }
    }

    public class PointLight : Light
    {
        public PointLight(Tuple position, Tuple intensity) : base(position, intensity) {}
    }
}

namespace RayTracer
{
    public abstract class Light
    {
        protected Light(Tuple position, Tuple intensity)
        {
            Position = position;
            Intensity = intensity;
        }

        public Tuple Intensity { get; set; }
        public Tuple Position { get; set; }
    }

    public class PointLight : Light
    {
        public PointLight(Tuple position, Tuple intensity) : base(position, intensity) {}
    }
}

namespace RayTracer
{
    public class Ray
    {
        public Ray Transform(Matrix m)
        {
            return new Ray(m * Origin, m * Direction);
        }

        public Tuple Position(float t)
        {
            return Origin + Direction * t;
        }

        public Ray(Tuple origin, Tuple direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public Tuple Origin { get; set; }
        public Tuple Direction { get; set; }
    }
}

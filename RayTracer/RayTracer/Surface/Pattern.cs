using static System.MathF;

namespace RayTracer
{
    public abstract class Pattern
    {
        public Tuple ColorAtShape(Shape shape, Tuple worldPoint)
        {
            Tuple localPoint = shape.Transform.Inverse() * worldPoint;
            Tuple patternPoint = Transform.Inverse() * localPoint;

            return ColorAt(patternPoint);
        }

        public abstract Tuple ColorAt(Tuple point);

        protected Pattern()
        {
            Transform = Matrix.Identity();
        }

        public Matrix Transform { get; set; }
    }

    public class StripePattern : Pattern
    {
        public override Tuple ColorAt(Tuple point)
        {
            return (Floor(point.X) % 2 == 0) ? FirstColor : SecondColor;
        }

        public StripePattern(Tuple firstColor, Tuple secondColor)
        {
            FirstColor = firstColor;
            SecondColor = secondColor;
        }

        public Tuple FirstColor { get; private set; }
        public Tuple SecondColor { get; private set; }
    }

    public class GradientPattern : Pattern
    {
        public override Tuple ColorAt(Tuple point)
        {
            return FirstColor + (SecondColor - FirstColor) * (point.X - Floor(point.X));
        }

        public GradientPattern(Tuple firstColor, Tuple secondColor)
        {
            FirstColor = firstColor;
            SecondColor = secondColor;
        }

        public Tuple FirstColor { get; private set; }
        public Tuple SecondColor { get; private set; }
    }
    
    public class RingPattern : Pattern
    {
        public override Tuple ColorAt(Tuple point)
        {
            float distance = Sqrt(point.X * point.X + point.Z * point.Z);

            return (Floor(distance) % 2 == 0) ? FirstColor : SecondColor;
        }

        public RingPattern(Tuple firstColor, Tuple secondColor)
        {
            FirstColor = firstColor;
            SecondColor = secondColor;
        }

        public Tuple FirstColor { get; private set; }
        public Tuple SecondColor { get; private set; }
    }

    public class CheckersPattern : Pattern
    {
        public override Tuple ColorAt(Tuple point)
        {
            float FloorX = Floor(point.X);
            float FloorY = Floor(point.Y);
            float FloorZ = Floor(point.Z);

            float determinant = Floor(point.X) + Floor(point.Y) + Floor(point.Z);
            return (determinant % 2 == 0) ? FirstColor : SecondColor;
        }

        public CheckersPattern(Tuple firstColor, Tuple secondColor)
        {
            FirstColor = firstColor;
            SecondColor = secondColor;
        }

        public Tuple FirstColor { get; private set; }
        public Tuple SecondColor { get; private set; }
    }
}

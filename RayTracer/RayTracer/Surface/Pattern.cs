using System;
using static System.MathF;

namespace RayTracer
{
    public abstract class Pattern : ICloneable
    {
        public Tuple ColorAtShape(Shape shape, Tuple worldPoint)
        {
            Tuple localPoint = shape.Transform.Inverse() * worldPoint;
            Tuple patternPoint = Transform.Inverse() * localPoint;

            return ColorAt(patternPoint);
        }

        public abstract Tuple ColorAt(Tuple point);

        public abstract Pattern CloneImple();

        public object Clone()
        {
            return CloneImple();
        }

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

        public override Pattern CloneImple()
        {
            return new StripePattern(FirstColor.Clone() as Tuple,
                                        SecondColor.Clone() as Tuple);
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

        public override Pattern CloneImple()
        {
            return new GradientPattern(FirstColor.Clone() as Tuple,
                                        SecondColor.Clone() as Tuple);
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

            return (Abs(Floor(distance) % 2) < Constants.floatEps) ? FirstColor : SecondColor;
        }

        public override Pattern CloneImple()
        {
            return new RingPattern(FirstColor.Clone() as Tuple,
                                    SecondColor.Clone() as Tuple);
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
            float determinant = Floor(point.X) + Floor(point.Y) + Floor(point.Z);
            return (Abs(determinant % 2) < Constants.floatEps) ? FirstColor : SecondColor;
        }

        public override Pattern CloneImple()
        {
            return new CheckersPattern(FirstColor.Clone() as Tuple,
                                        SecondColor.Clone() as Tuple);
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

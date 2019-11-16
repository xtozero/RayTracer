using System;
using static System.MathF;

namespace RayTracer
{
    public class Tuple : IEquatable<Tuple>, ICloneable
    {
        public Tuple Reflect(Tuple normal)
        {
            return this - normal * 2 * Dot(normal);
        }

        public bool IsPoint()
        {
            return Abs(W - 1.0f) < Constants.floatEps;
        }

        public bool IsVector()
        {
            return Abs(W) < Constants.floatEps;
        }

        public float Magnitude()
        {
            return System.MathF.Sqrt( X * X + Y * Y + Z * Z + W * W );
        }

        public Tuple Normalize()
        {
            float magnitude = Magnitude();

            return new Tuple(X / magnitude, Y / magnitude, Z / magnitude, W / magnitude);
        }

        public float Dot(Tuple t)
        {
            return X * t.X + Y * t.Y + Z * t.Z + W * t.W;
        }

        public Tuple Cross(Tuple t)
        {
            return new Tuple(Y * t.Z - Z * t.Y, Z * t.X - X * t.Z, X * t.Y - Y * t.X, 0);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is Tuple t)
            {
                return Equals(t);
            }

            return false;
        }

        public bool Equals(Tuple other)
        {
            return Abs(X - other.X) < Constants.floatEps &&
                Abs(Y - other.Y) < Constants.floatEps &&
                Abs(Z - other.Z) < Constants.floatEps &&
                Abs(W - other.W) < Constants.floatEps;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z, W);
        }

        public static Tuple Vector(float x, float y, float z)
        {
            return new Tuple(x, y, z, 0);
        }

        public static Tuple Point(float x, float y, float z)
        {
            return new Tuple(x, y, z, 1);
        }

        public static Tuple Color(float r, float g, float b)
        {
            return new Tuple(r, g, b, 0);
        }

        public object Clone()
        {
            return new Tuple(X, Y, Z, W);
        }

        public static Tuple operator+(Tuple lhs, Tuple rhs)
        {
            return new Tuple(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z, lhs.W + rhs.W);
        }

        public static Tuple operator-(Tuple lhs, Tuple rhs)
        {
            return new Tuple(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z, lhs.W - rhs.W);
        }

        public static Tuple operator*(Tuple lhs, float rhs)
        {
            return new Tuple(lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs, lhs.W * rhs);
        }

        public static Tuple operator*(Tuple lhs, Tuple rhs)
        {
            return new Tuple(lhs.X * rhs.X, lhs.Y * rhs.Y, lhs.Z * rhs.Z, lhs.W * rhs.W);
        }

        public static Tuple operator/(Tuple lhs, float rhs)
        {
            return new Tuple(lhs.X / rhs, lhs.Y / rhs, lhs.Z / rhs, lhs.W / rhs);
        }

        public static Tuple operator-(Tuple t)
        {
            return new Tuple(-t.X, -t.Y, -t.Z, -t.W);
        }

        public Tuple( float x, float y, float z, float w )
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public float R
        {
            get
            {
                return X;
            }

            set
            {
                X = value;
            }
        }

        public float G
        {
            get
            {
                return Y;
            }

            set
            {
                Y = value;
            }
        }

        public float B
        {
            get
            {
                return Z;
            }

            set
            {
                Z = value;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer
{
    public class Tuple
    {
        public bool IsPoint()
        {
            return W == 1.0f;
        }

        public bool IsVector()
        {
            return W == 0.0f;
        }

        public override bool Equals(Object obj)
        {
            if ( obj == null)
            {
                return false;
            }

            if (obj is Tuple t)
            {
                return X.Equals(t.X) && Y.Equals(t.Y) && Z.Equals(t.Z) && W.Equals(t.W);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode() ^ W.GetHashCode();
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
    }
}

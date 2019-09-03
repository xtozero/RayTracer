namespace RayTracer
{
    public class Computation
    {
        public Computation( Intersection i, Ray r )
        {
            T = i.T;
            Object = i.Object;

            Point = r.Position(T);
            Eyev = -r.Direction;
            Normalv = Object.NormalAt(Point);

            if ( Eyev.Dot(Normalv) < 0 )
            {
                Inside = true;
                Normalv = -Normalv;
            }
            else
            {
                Inside = false;
            }
        }

        public float T { get; private set; }
        public Shape Object { get; private set; }
        public Tuple Point { get; private set; }
        public Tuple Eyev { get; private set; }
        public Tuple Normalv { get; private set; }
        public bool Inside { get; private set; }
    }
}

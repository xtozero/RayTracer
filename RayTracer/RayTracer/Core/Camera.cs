using static System.MathF;

namespace RayTracer
{
    public class Camera
    {
        public Ray RayForPixel(int px, int py)
        {
            float xOffset = (px + 0.5f) * PixelSize;
            float yOffset = (py + 0.5f) * PixelSize;

            float worldX = HalfWidth - xOffset;
            float worldY = HalfHeight - yOffset;

            Tuple pixel = Transform.Inverse() * Tuple.Point(worldX, worldY, -1);
            Tuple origin = Transform.Inverse() * Tuple.Point(0, 0, 0);

            return new Ray(origin, (pixel - origin).Normalize());
        }

        public Camera( int hSize, int vSize, float fieldOfView )
        {
            HSize = hSize;
            VSize = vSize;
            FieldOfView = fieldOfView;
            Transform = Matrix.Identity();

            float halfView = Tan(FieldOfView * 0.5f);
            float aspect = (float)HSize / VSize;

            if (aspect >= 1)
            {
                HalfWidth = halfView;
                HalfHeight = halfView / aspect;
            }
            else
            {
                HalfWidth = halfView * aspect;
                HalfHeight = halfView;
            }

            PixelSize = (HalfWidth * 2) / HSize;
        }

        public int HSize { get; private set; }
        public int VSize { get; private set; }
        public float FieldOfView { get; private set; }
        public Matrix Transform { get; set; }
        public float PixelSize { get; private set; }
        public float HalfWidth { get; private set; }
        public float HalfHeight { get; private set; }
    }
}

using Xunit;

using RayTracer;

namespace UnitTest
{
    public class PointTest
    {
        [Fact]
        public void Scenario1()
        {
            Point p = new Point(4, -4, 3);
            Assert.True(p.Equals(new Tuple(4, -4, 3, 1)));
        }
    }
}

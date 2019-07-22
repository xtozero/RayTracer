using Xunit;

using RayTracer;

namespace UnitTest
{
    public class VectorTest
    {
        [Fact]
        public void Scenario1()
        {
            Vector v = new Vector(4, -4, 3);
            Assert.True(v.Equals(new Tuple(4, -4, 3, 0)));
        }
    }
}

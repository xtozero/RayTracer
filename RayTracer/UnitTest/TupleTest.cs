using Xunit;

using RayTracer;

namespace UnitTest
{
    public class TupleTest
    {
        [Fact]
        public void Scenario1()
        {
            Tuple a = new Tuple(4.3f, -4.2f, 3.1f, 1.0f);
            Assert.Equal(4.3f, a.X);
            Assert.Equal(-4.2f, a.Y);
            Assert.Equal(3.1f, a.Z);
            Assert.Equal(1.0f, a.W);
            Assert.True(a.IsPoint());
            Assert.False(a.IsVector());
        }

        [Fact]
        public void Scenario2()
        {
            Tuple a = new Tuple(4.3f, -4.2f, 3.1f, 0.0f);
            Assert.Equal(4.3f, a.X);
            Assert.Equal(-4.2f, a.Y);
            Assert.Equal(3.1f, a.Z);
            Assert.Equal(0.0f, a.W);
            Assert.False(a.IsPoint());
            Assert.True(a.IsVector());
        }
    }
}

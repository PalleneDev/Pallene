using System.Diagnostics;

namespace Pallene.Sdk.Cdda.Tests
{
    public class UnitTest1
    {

        [Fact]
        public void Test2()
        {
            CDTimecode t1 = new(2352);
            CDTimecode t2 = new(0,0,1);
            Assert.Equal(t2, t1);
        }

        [Fact]
        public void Test3()
        {
            CDTimecode t1 = new();
            CDTimecode t2 = new(1, 2, 3);
            CDTimecode t3 = t1 + t2;

            Assert.NotEqual(t1, t3);
        }

        [Fact]
        public void Test4()
        {
            CDTimecode t1 = new(1, 1, 1);
            CDTimecode t2 = new(1, 2, 0);
            Assert.True(t2 > t1);
        }

        [Fact]
        public void Test5()
        {
            CDTimecode t1 = new(1, 1, 1);
            CDTimecode t2 = new(1, 1, 1);
            Assert.True(t2 >= t1);
        }
    }
}

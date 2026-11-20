namespace Pallene.Sdk.Cdda.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Tes1()
        {
            Assert.Throws<ArgumentException>(() => new Isrc("1234"));
        }

        [Fact]
        public void Test2()
        {
            Isrc a = new("0123456789AB");
            Assert.Throws<ArgumentException>(() => { Isrc i = a + "C"; });
            Assert.Throws<ArgumentException>(() => a += "C");
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

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
    }
}

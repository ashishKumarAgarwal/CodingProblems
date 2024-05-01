using CodingSolutions.Cache.MultiEvictionPolicyCache;
using FluentAssertions;
using Moq;

namespace TestProject1
{
    public class CacheServiceTest
    {
        private CacheService _cacheService;

        [Fact]
        public void Test_ForUpdatedValueAndNoValueOnExpiry()
        {
            //Arrange
            var mock = new Mock<IExpirationTimeService>();
            _cacheService = new CacheService(mock.Object, 3);

            _cacheService.Put("key1", 1, 2, 5);
            _cacheService.Put("key2", 2, 1, 10);
            _cacheService.Put("key3", 3, 3, 8);

            mock.Setup(x => x.GetExpiryThreshold()).Returns(4);

            _cacheService.Get("key1").Should().Be(1);

            mock.Setup(x => x.GetExpiryThreshold()).Returns(5);

            _cacheService.Get("key1").Should().Be(1);

            mock.Setup(x => x.GetExpiryThreshold()).Returns(6);

            _cacheService.Get("key1").Should().Be(-1);

            mock.Setup(x => x.GetExpiryThreshold()).Returns(2);
            _cacheService.Put("key4", 4, 2, 15);
            _cacheService.Put("key5", 5, 2, 15);
            _cacheService.Put("key6", 6, 2, 15);
            _cacheService.Get("key3").Should().Be(-1);

            mock.Setup(x => x.GetExpiryThreshold()).Returns(5);
            _cacheService.Put("key5", 5, 2, 15);
            _cacheService.Get("key2").Should().Be(-1);
        }

       

        [Fact]
        public void Test_Eviction_On_ExpirationTime_Priority_LRU()
        {
            //Arrange
            var mock = new Mock<IExpirationTimeService>();
            mock.Setup(x => x.GetExpiryThreshold()).Returns(0);
            _cacheService = new CacheService(mock.Object, 5);

            var expected = new List<CacheItem>()
            {
                new CacheItem("A", 5, 1, 1000),
                new CacheItem("B", 15, 5, 500),
                new CacheItem("C", 0, 5, 2000),
                new CacheItem("D", 1, 5, 2000),
                new CacheItem("E", 10, 5, 3000)
            };

            _cacheService.Put("A", 5, 1, 1000);
            _cacheService.Put("B", 15, 5, 500);
            _cacheService.Put("C", 0, 5, 2000);
            _cacheService.Put("D", 1, 5, 2000);
            _cacheService.Put("E", 10, 5, 3000);

            expected.Should().BeEquivalentTo(_cacheService.Cache.Values.ToList());

            // Expiration Time Test

            mock.Setup(x => x.GetExpiryThreshold()).Returns(8300);
            _cacheService.Put("F", 15, 5, 1000);

            expected.Remove(expected.First(i => i.Key == "A"));
            expected.Add(new CacheItem("F", 15, 5, 1000));
            expected.Should().BeEquivalentTo(_cacheService.Cache.Values.ToList());

            //LRU Cache test

            mock.Setup(x => x.GetExpiryThreshold()).Returns(300);
            _cacheService.Put("G", 0, 6, 2000);

            expected.Remove(expected.First(i => i.Key == "B"));
            expected.Add(new CacheItem("G", 0, 6, 2000));
            expected.Should().BeEquivalentTo(_cacheService.Cache.Values.ToList());


            // Priority Queue test
            mock.Setup(x => x.GetExpiryThreshold()).Returns(300);
            _cacheService.Put("H", 0, 7, 1000);
            expected.Remove(expected.First(i => i.Key == "G"));
            expected.Add(new CacheItem("H", 0, 7, 1000));
            expected.Should().BeEquivalentTo(_cacheService.Cache.Values.ToList());
        }
    }
}
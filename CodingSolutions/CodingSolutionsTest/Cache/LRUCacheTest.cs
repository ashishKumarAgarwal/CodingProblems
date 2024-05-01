using FluentAssertions;
using FluentAssertions.Execution;

namespace TestProject1
{
    public class LRUCacheTest
    {
        private LRUCache _lruCache;

        [Fact]
        public void Test1()
        {
            //Arrange
            _lruCache = new LRUCache(2);

            //Act
            _lruCache.Put(1, 1); // cache is {1=1}
            _lruCache.Put(2, 2); // cache is {1=1, 2=2}
            var get1 = _lruCache.Get(1);    // return 1
            _lruCache.Put(3, 3); // LRU key was 2, evicts key 2, cache is {1=1, 3=3}
            var getMin1 = _lruCache.Get(2);    // returns -1 (not found)
            _lruCache.Put(4, 4); // LRU key was 1, evicts key 1, cache is {4=4, 3=3}
            var getMin1Again = _lruCache.Get(1);    // return -1 (not found)
            var get3 = _lruCache.Get(3);    // return 3
            var get4 = _lruCache.Get(4);    // return 4

            //Assert

            using(new AssertionScope())
            {
                get1.Should().Be(1);
                getMin1.Should().Be(-1);
                getMin1Again.Should().Be(-1);
                get3.Should().Be(3);
                get4.Should().Be(4);
            }
        }
    }
}
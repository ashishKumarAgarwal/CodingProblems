using CodingSolutions.Cache.PriorityCache;
using FluentAssertions;

namespace CodingSolutionsTest.Cache
{
    public class PriorityExpiryCacheTest
    {

        [Fact]
        public void Test()
        {
            PriorityExpiryCache priorityExpiryCache = new PriorityExpiryCache(5);

            priorityExpiryCache.SetItem(new Item("A", "val1", 5, 100), 0);
            priorityExpiryCache.SetItem(new Item("B", "val2", 15, 3), 0);
            priorityExpiryCache.SetItem(new Item("C", "val3", 5, 10), 0);
            priorityExpiryCache.SetItem(new Item("D", "val4", 1, 15), 0);
            priorityExpiryCache.SetItem(new Item("E", "val5", 5, 150), 0);

            priorityExpiryCache.GetItem("C");
            priorityExpiryCache.GetKeys().Should().BeEquivalentTo(new HashSet<string> { "A", "B", "C", "D", "E" });

            priorityExpiryCache.EvictItem(5);
            priorityExpiryCache.GetKeys().Should().BeEquivalentTo(new HashSet<string> { "A","C", "D", "E" });

            priorityExpiryCache.EvictItem(5);
            priorityExpiryCache.GetKeys().Should().BeEquivalentTo(new HashSet<string> { "A","C","E" });
            
            
            priorityExpiryCache.EvictItem(5);
            priorityExpiryCache.GetKeys().Should().BeEquivalentTo(new HashSet<string> {  "C",  "E" });
            
            priorityExpiryCache.EvictItem(5);
            priorityExpiryCache.GetKeys().Should().BeEquivalentTo(new HashSet<string> {  "C"});
        }
    }
}

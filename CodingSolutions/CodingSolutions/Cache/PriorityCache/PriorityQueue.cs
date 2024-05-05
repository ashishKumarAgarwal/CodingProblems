namespace CodingSolutions.Cache.PriorityCache
{
    public class PriorityQueue
    {
        private List<Item> items;
        private Comparison<Item> _comparison;

        public PriorityQueue(Comparison<Item> comparison)
        {
            items = new List<Item>();
            _comparison = comparison;
        }

        public void Add(Item item)
        {
            items.Add(item);
            items.Sort(_comparison);
        }

        public Item Peek()
        {
            if (items.Count == 0) throw new InvalidOperationException("Queue is empty.");
            return items[0];
        }

        public Item Poll()
        {
            if (items.Count == 0) throw new InvalidOperationException("Queue is empty.");
            Item item = items[0];
            items.RemoveAt(0);
            return item;
        }

        public void Remove(Item item)
        {
            items.Remove(item);
            items.Sort(_comparison);
        }
    }
}

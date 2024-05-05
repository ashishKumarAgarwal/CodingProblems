namespace CodingSolutions.Cache.PriorityCache
{
    public class PriorityQueue
    {
        private List<Item> data;
        private Comparison<Item> comparison;

        public PriorityQueue(Comparison<Item> comparison)
        {
            data = new List<Item>();
            this.comparison = comparison;
        }

        public void Add(Item item)
        {
            data.Add(item);
            data.Sort(comparison);
        }

        public Item Peek()
        {
            if (data.Count == 0) throw new InvalidOperationException("Queue is empty.");
            return data[0];
        }

        public Item Poll()
        {
            if (data.Count == 0) throw new InvalidOperationException("Queue is empty.");
            Item item = data[0];
            data.RemoveAt(0);
            return item;
        }

        public void Remove(Item item)
        {
            data.Remove(item);
            data.Sort(comparison);
        }
    }
}

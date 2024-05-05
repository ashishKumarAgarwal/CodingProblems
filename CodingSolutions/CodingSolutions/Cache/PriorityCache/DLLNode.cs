namespace CodingSolutions.Cache.PriorityCache
{
    public class DLLNode
    {
        public Item Value { get; set; }
        public DLLNode Next { get; set; }
        public DLLNode Prev { get; set; }

        public DLLNode(Item data)
        {
            Value = data;
        }

        public DLLNode(DLLNode prev, Item data, DLLNode next)
        {
            Value = data;
            Next = next;
            Prev = prev;
        }
    }
}
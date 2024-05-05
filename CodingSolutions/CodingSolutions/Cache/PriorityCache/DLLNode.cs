namespace CodingSolutions.Cache.PriorityCache
{
    public class DLLNode
    {
        public Item Data { get; set; }
        public DLLNode Next { get; set; }
        public DLLNode Prev { get; set; }

        public DLLNode(Item data)
        {
            Data = data;
        }

        public DLLNode(DLLNode prev, Item data, DLLNode next)
        {
            Data = data;
            Next = next;
            Prev = prev;
        }
    }
}
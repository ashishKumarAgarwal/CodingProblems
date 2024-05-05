namespace CodingSolutions.Cache.PriorityCache
{
    public class DoublyLinkedList
    {
        private DLLNode _head;
        private DLLNode _tail;
        private int size;

        public DLLNode AddFront(Item item)
        {
            DLLNode retVal;
            if (size == 0)
            {
                _head = new DLLNode(item);
                _tail = _head;
                retVal = _head;
            }
            else
            {
                DLLNode newNode = new DLLNode(null, item, null);
                newNode.Next = _head;
                _head.Prev = newNode;
                _head = newNode;
                retVal = newNode;
            }
            size++;
            return retVal;
        }

        public DLLNode RemoveLast()
        {
            DLLNode item = _tail;
            _tail = _tail.Prev;
            size--;
            return item;
        }

        public void RemoveNode(DLLNode node)
        {
            if (size == 0)
            {
                return;
            }

            if (size == 1)
            {
                _tail = _head = null;
            }
            else
            {
                DLLNode prev = node.Prev;
                DLLNode next = node.Next;

                if (prev != null)
                    prev.Next = next;

                if (next != null)
                    next.Prev = prev;
            }

            size--;
        }

        public int Size()
        {
            return size;
        }
    }
}
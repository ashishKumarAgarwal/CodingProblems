namespace CodingSolutions.Cache.PriorityCache
{
    public class DoublyLinkedList
    {
        private DLLNode front;
        private DLLNode end;
        private int size;

        public DoublyLinkedList()
        {
            end = front = null;
        }

        public DLLNode AddFront(Item x)
        {
            DLLNode retVal;
            if (size == 0)
            {
                front = new DLLNode(x);
                end = front;
                retVal = front;
            }
            else
            {
                DLLNode newNode = new DLLNode(null, x, null);
                newNode.Next = front;
                front.Prev = newNode;
                front = newNode;
                retVal = newNode;
            }
            size++;
            return retVal;
        }

        public DLLNode RemoveLast()
        {
            DLLNode item = end;
            end = end.Prev;
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
                end = front = null;
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
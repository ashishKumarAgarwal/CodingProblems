namespace CodingSolutions.Cache.PriorityCache
{
    public class DoublyLinkedList<T>
    {
        private ListNode<T> front;
        private ListNode<T> end;
        private int size;

        public DoublyLinkedList()
        {
            end = front = null;
        }

        public ListNode<T> AddFront(T x)
        {
            ListNode<T> retVal;
            if (size == 0)
            {
                front = new ListNode<T>(x);
                end = front;
                retVal = front;
            }
            else
            {
                ListNode<T> newNode = new ListNode<T>(null, x, null);
                newNode.Next = front;
                front.Prev = newNode;
                front = newNode;
                retVal = newNode;
            }
            size++;
            return retVal;
        }

        public ListNode<T> RemoveLast()
        {
            ListNode<T> item = end;
            end = end.Prev;
            size--;
            return item;
        }

        public void RemoveNode(ListNode<T> node)
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
                ListNode<T> prev = node.Prev;
                ListNode<T> next = node.Next;

                if (prev != null)
                    prev.Next = next;

                if (next != null)
                    next.Prev = prev;

                node = null;
            }

            size--;
        }

        public int Size()
        {
            return size;
        }
    }

}

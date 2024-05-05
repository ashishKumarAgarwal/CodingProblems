namespace CodingSolutions.Cache.Educative
{
    public class PriorityExpiryCache
    {
        private int maxSize;
        private int currSize;

        private PriorityQueue<ListNode<Item>> pqByExpiryTime = new PriorityQueue<ListNode<Item>>((a, b) => a.Data.ExpireAfter - b.Data.ExpireAfter);
        private PriorityQueue<ListNode<Item>> pqByPreference = new PriorityQueue<ListNode<Item>>((a, b) => a.Data.Preference - b.Data.Preference);
        private Dictionary<int, DoublyLinkedList<Item>> preferrenceToList = new Dictionary<int, DoublyLinkedList<Item>>();
        private Dictionary<string, ListNode<Item>> keyToItemNode = new Dictionary<string, ListNode<Item>>();

        public PriorityExpiryCache(int maxSize)
        {
            this.maxSize = maxSize;
            this.currSize = 0;
        }

        public HashSet<string> GetKeys()
        {
            return new HashSet<string>(keyToItemNode.Keys);
        }

        public void EvictItem(int currentTime)
        {
            if (currSize == 0) return;

            currSize--;

            if (pqByExpiryTime.Peek().Data.ExpireAfter < currentTime)
            {
                ListNode<Item> node = pqByExpiryTime.Poll();
                Item item = node.Data;

                DoublyLinkedList<Item> dList = preferrenceToList[item.Preference];
                dList.RemoveNode(node);

                if (dList.Size() == 0)
                {
                    preferrenceToList.Remove(item.Preference);
                }

                keyToItemNode.Remove(item.Key);
                pqByPreference.Remove(new ListNode<Item>(item));

                return;
            }

            int preference = pqByPreference.Poll().Data.Preference;

            DoublyLinkedList<Item> dList2 = preferrenceToList[preference];

            ListNode<Item> leastRecentlyUsedWithLeastPreference = dList2.RemoveLast();
            keyToItemNode.Remove(leastRecentlyUsedWithLeastPreference.Data.Key);
            pqByExpiryTime.Remove(leastRecentlyUsedWithLeastPreference);

            if (dList2.Size() == 0)
            {
                preferrenceToList.Remove(preference);
            }
        }

        public Item GetItem(string key)
        {
            if (keyToItemNode.ContainsKey(key))
            {
                ListNode<Item> node = keyToItemNode[key];
                Item itemToReturn = node.Data;

                DoublyLinkedList<Item> dList = preferrenceToList[itemToReturn.Preference];

                dList.RemoveNode(node);
                dList.AddFront(itemToReturn);

                return itemToReturn;
            }

            return null;
        }

        public void SetItem(Item item, int currentTime)
        {
            if (currSize == maxSize)
            {
                EvictItem(currentTime);
            }

            DoublyLinkedList<Item> dlist;
            if (preferrenceToList.ContainsKey(item.Preference))
            {
                dlist = preferrenceToList[item.Preference];
            }
            else
            {
                dlist = new DoublyLinkedList<Item>();
                preferrenceToList.Add(item.Preference, dlist);
            }
            ListNode<Item> node = dlist.AddFront(item);
            keyToItemNode[item.Key] = node;
            pqByExpiryTime.Add(node);
            pqByPreference.Add(node);
            currSize++;
        }
    }

    public class PriorityQueue<T>
    {
        private List<T> data;
        private Comparison<T> comparison;

        public PriorityQueue(Comparison<T> comparison)
        {
            this.data = new List<T>();
            this.comparison = comparison;
        }

        public void Add(T item)
        {
            data.Add(item);
            data.Sort(comparison);
        }

        public T Peek()
        {
            if (data.Count == 0) throw new InvalidOperationException("Queue is empty.");
            return data[0];
        }

        public T Poll()
        {
            if (data.Count == 0) throw new InvalidOperationException("Queue is empty.");
            T item = data[0];
            data.RemoveAt(0);
            return item;
        }

        public void Remove(T item)
        {
            data.Remove(item);
            data.Sort(comparison);
        }
    }
}
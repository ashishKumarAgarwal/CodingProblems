namespace CodingSolutions.Cache.PriorityCache
{
    public class PriorityExpiryCache
    {
        private int _maxSize;
        private int _currSize;

        private PriorityQueue<DLLNode> pqByExpiryTime = new((a, b) => a.Value.Expiry - b.Value.Expiry);
        private PriorityQueue<DLLNode> pqByPreference = new((a, b) => a.Value.Preference - b.Value.Preference);
        private Dictionary<int, DoublyLinkedList> preferenceDLLMap = new();
        private Dictionary<string, DLLNode> keyDLLNodeMap = new();

        public PriorityExpiryCache(int maxSize)
        {
            _maxSize = maxSize;
            _currSize = 0;
        }

        public HashSet<string> GetKeys()
        {
            return new HashSet<string>(keyDLLNodeMap.Keys);
        }

        public void EvictItem(int currentTime)
        {
            if (_currSize == 0) return;

            _currSize--;

            if (pqByExpiryTime.Peek().Value.Expiry < currentTime)
            {
                DLLNode node = pqByExpiryTime.Poll();
                Item item = node.Value;

                DoublyLinkedList dList = preferenceDLLMap[item.Preference];
                dList.RemoveNode(node);

                if (dList.Size() == 0)
                {
                    preferenceDLLMap.Remove(item.Preference);
                }

                keyDLLNodeMap.Remove(item.Key);
                pqByPreference.Remove(new DLLNode(item));

                return;
            }

            int preference = pqByPreference.Poll().Value.Preference;

            DoublyLinkedList dList2 = preferenceDLLMap[preference];

            DLLNode leastRecentlyUsedWithLeastPreference = dList2.RemoveLast();
            keyDLLNodeMap.Remove(leastRecentlyUsedWithLeastPreference.Value.Key);
            pqByExpiryTime.Remove(leastRecentlyUsedWithLeastPreference);

            if (dList2.Size() == 0)
            {
                preferenceDLLMap.Remove(preference);
            }
        }

        public Item GetItem(string key)
        {
            if (keyDLLNodeMap.ContainsKey(key))
            {
                DLLNode node = keyDLLNodeMap[key];
                Item itemToReturn = node.Value;

                var dList = preferenceDLLMap[itemToReturn.Preference];

                dList.RemoveNode(node);
                dList.AddFront(itemToReturn);

                return itemToReturn;
            }

            return null;
        }

        public void SetItem(Item item, int currentTime)
        {
            if (_currSize == _maxSize)
            {
                EvictItem(currentTime);
            }

            DoublyLinkedList dlist;
            if (preferenceDLLMap.ContainsKey(item.Preference))
            {
                dlist = preferenceDLLMap[item.Preference];
            }
            else
            {
                dlist = new DoublyLinkedList();
                preferenceDLLMap.Add(item.Preference, dlist);
            }
            DLLNode node = dlist.AddFront(item);
            keyDLLNodeMap[item.Key] = node;
            pqByExpiryTime.Add(node);
            pqByPreference.Add(node);
            _currSize++;
        }
    }

    public class PriorityQueue<T>
    {
        private List<T> data;
        private Comparison<T> comparison;

        public PriorityQueue(Comparison<T> comparison)
        {
            data = new List<T>();
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
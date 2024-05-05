namespace CodingSolutions.Cache.PriorityCache
{
    public class PriorityExpiryCache
    {
        private int _maxSize;
        private int _currSize;

        private PriorityQueue pqByExpiryTime = new((a, b) => a.Expiry - b.Expiry);
        private PriorityQueue pqByPreference = new((a, b) => a.Preference - b.Preference);
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

            if (pqByExpiryTime.Peek().Expiry < currentTime)
            {
                var item = pqByExpiryTime.Poll();

                DoublyLinkedList dList = preferenceDLLMap[item.Preference];
                dList.RemoveNode(new DLLNode(item));

                if (dList.Size() == 0)
                {
                    preferenceDLLMap.Remove(item.Preference);
                }

                keyDLLNodeMap.Remove(item.Key);
                pqByPreference.Remove(item);

                return;
            }

            int preference = pqByPreference.Poll().Preference;

            DoublyLinkedList dList2 = preferenceDLLMap[preference];

            DLLNode leastRecentlyUsedWithLeastPreference = dList2.RemoveLast();
            keyDLLNodeMap.Remove(leastRecentlyUsedWithLeastPreference.Value.Key);
            pqByExpiryTime.Remove(leastRecentlyUsedWithLeastPreference.Value);

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
            pqByExpiryTime.Add(item);
            pqByPreference.Add(item);
            _currSize++;
        }
    }
}
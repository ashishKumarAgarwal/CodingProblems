namespace CodingSolutions.Cache.MultiEvictionPolicyCache
{
    public class DLLNode
    {
        public string Key;
        public DLLNode Previous;
        public DLLNode Next;

        public DLLNode(string key)
        {
            Key = key;
        }
    }

    public class LRUCache
    {
        private Dictionary<string, DLLNode> _keyNodeMap;
        private int _capacity;
        private DLLNode _lruTrackerNode = new DLLNode(string.Empty);
        private DLLNode _tail;

        public LRUCache(int capacity)
        {
            _keyNodeMap = new Dictionary<string, DLLNode>();
            _capacity = capacity;
            _tail = new DLLNode(string.Empty);
            _lruTrackerNode.Next = _tail;
            _tail.Previous = _lruTrackerNode;
        }

        public string DeleteLastNodeAndUpdateLinkedList()
        {
            var leastRecentlyUsedNode = _tail.Previous;
            _keyNodeMap.Remove(leastRecentlyUsedNode.Key);
            _tail.Previous = leastRecentlyUsedNode.Previous;
            leastRecentlyUsedNode.Previous.Next = _tail;
            return leastRecentlyUsedNode.Key;
        }

        public void Put(string key)
        {
            if (_keyNodeMap.ContainsKey(key))
            {
                var node = _keyNodeMap[key];
                MakeNodeMostRecentlyUsed(node);
            }
            else
            {
                if (_keyNodeMap.Count == _capacity)
                {
                    //Remove Least Recently used
                    DeleteLastNodeAndUpdateLinkedList();
                }

                InsertNewNode(key);
            }
        }

        public void UpdateDDL(string key)
        {
            var node = _keyNodeMap[key];
            MakeNodeMostRecentlyUsed(node);
        }

        public void MakeNodeMostRecentlyUsed(DLLNode nodeToUpdate)
        {
            var prevNode = nodeToUpdate.Previous;
            var nextNode = nodeToUpdate.Next;

            prevNode.Next = nextNode;
            nextNode.Previous = prevNode;

            var trackerNext = _lruTrackerNode.Next;
            _lruTrackerNode.Next = nodeToUpdate;
            nodeToUpdate.Previous = _lruTrackerNode;
            nodeToUpdate.Next = trackerNext;
            trackerNext.Previous = nodeToUpdate;
        }

        public void Delete(string key)
        {
            var nodeToDelete = _keyNodeMap[key];
            var prevNode = nodeToDelete.Previous;
            var nextNode = nodeToDelete.Next;
            prevNode.Next = nextNode;
            nextNode.Previous = prevNode;
            _keyNodeMap.Remove(key);
        }

        private void InsertNewNode(string key)
        {
            var newNode = new DLLNode(key);
            _keyNodeMap.Add(key, newNode);
            var trackerNext = _lruTrackerNode.Next;
            _lruTrackerNode.Next = newNode;
            newNode.Previous = _lruTrackerNode;
            newNode.Next = trackerNext;
            trackerNext.Previous = newNode;
        }
    }
}
public class LRUCache
{

    Dictionary<int, Node> _keyNodeMap;
    int _capacity;
    Node _lruTrackerNode = new Node(0, 0);
    Node _tail;
    public LRUCache(int capacity)
    {
        _keyNodeMap = new Dictionary<int, Node>();
        _capacity = capacity;
        _tail = new Node(0, 0);
        _lruTrackerNode.Next = _tail;
        _tail.Previous = _lruTrackerNode;
    }

    public int Get(int key)
    {

        if (!_keyNodeMap.ContainsKey(key))
        {
            return -1;
        }
        var node = _keyNodeMap[key];
        MakeNodeMostRecentlyUsed(node);
        return node.Value;

    }

    public void Put(int key, int value)
    {
        if (_keyNodeMap.ContainsKey(key))
        {
            var node = _keyNodeMap[key];
            node.Value = value;
            MakeNodeMostRecentlyUsed(node);
        }
        else
        {
            if (_keyNodeMap.Count == _capacity)
            {
                //Remove Least Recently used
                var leastRecentlyUsedNode = _tail.Previous;
                _keyNodeMap.Remove(leastRecentlyUsedNode.Key);
                _tail.Previous = leastRecentlyUsedNode.Previous;
                leastRecentlyUsedNode.Previous.Next = _tail;
            }

            InsertNewNode(key, value);


        }
    }

    private void InsertNewNode(int key, int value)
    {
        var newNode = new Node(key, value);
        _keyNodeMap.Add(key, newNode);
        var trackerNext = _lruTrackerNode.Next;
        _lruTrackerNode.Next = newNode;
        newNode.Previous = _lruTrackerNode;
        newNode.Next = trackerNext;
        trackerNext.Previous = newNode;
    }

    private void MakeNodeMostRecentlyUsed(Node nodeToUpdate)
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
}

// Boiler Plate

public class Node
{

    public int Key;
    public int Value;
    public Node Previous;
    public Node Next;

    public Node(int key, int value)
    {
        Value = value;
        Key = key;
    }
}

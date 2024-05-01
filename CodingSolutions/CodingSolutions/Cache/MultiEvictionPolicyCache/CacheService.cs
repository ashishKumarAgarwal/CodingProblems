namespace CodingSolutions.Cache.MultiEvictionPolicyCache;

// Boiler Plate
public class CacheItem
{
    public string Key;
    public int Value;
    public int Priority;
    public int Expiry;

    public CacheItem(string key, int value, int priority, int expiry)
    {
        Key = key;
        Value = value;
        Priority = priority;
        Expiry = expiry;
    }
}

public class CacheService
{
    private int _cacheCapacity;
    private readonly IExpirationTimeService _expirationTimeService;
    public Dictionary<string, CacheItem> Cache = new();
    private LRUCache _lruCache;
    private PriorityQueue<CacheItem, int> maxHeapPriorityCache = new(Comparer<int>.Create((x, y) => y.CompareTo(x)));
    private readonly BinaryTree _expiryTimeTree = new();

    public CacheService(IExpirationTimeService expirationTime, int cacheCapacity)
    {
        _expirationTimeService = expirationTime;
        _cacheCapacity = cacheCapacity;
        _lruCache = new LRUCache(cacheCapacity);
    }

    public void Put(string key, int value, int priority, int expiryTimeInSec)
    {
        if (Cache.ContainsKey(key))
        {
            _expiryTimeTree.Update(expiryTimeInSec, key);
            var newCache = new CacheItem(key, value, priority, expiryTimeInSec);
            UpdateElementInPriorityQueue(maxHeapPriorityCache, newCache);
            _lruCache.UpdateDDL(key);
            Cache[key] = newCache;
        }
        else
        {
            var newCache = new CacheItem(key, value, priority, expiryTimeInSec);
            if (Cache.Count == _cacheCapacity)
            {
                var nodeToEvict = _expiryTimeTree.SearchForValueLessThen(_expirationTimeService.GetExpiryThreshold());
                if (nodeToEvict != null)
                {
                    var keyToEvict = nodeToEvict.Key;
                    var valueToEvict = nodeToEvict.Value;
                    // Delete from binary tree
                    _expiryTimeTree.DeleteFromBinaryTree(valueToEvict, keyToEvict);
                    // Delete from priorityQueue
                    maxHeapPriorityCache = RemoveElementInPriorityQueue(maxHeapPriorityCache, keyToEvict);
                    // Delete from LRU
                    _lruCache.Delete(keyToEvict);
                    // Cache
                    Cache.Remove(keyToEvict);
                }
                else
                {
                    var p1 = maxHeapPriorityCache.Dequeue();
                    if (maxHeapPriorityCache.Count > 0)
                    {
                        var p2 = maxHeapPriorityCache.Peek();
                        if (p1.Priority != p2.Priority)
                        {
                            // Priority is not same
                            var nodeToDelete = p1;
                            if (p1.Priority < p2.Priority)
                            {
                                nodeToDelete = p2;
                            }
                            // Delete from LRU
                            _lruCache.Delete(nodeToDelete.Key);
                            // Delete from tree
                            _expiryTimeTree.DeleteFromBinaryTree(nodeToDelete.Expiry, nodeToDelete.Key);
                            // Delete from Cache
                            Cache.Remove(nodeToDelete.Key);
                        }
                        else
                        {
                            //Same Priority
                            maxHeapPriorityCache.Enqueue(p1, p1.Priority);
                            // Check for LRU
                            var keyToRemove = _lruCache.DeleteLastNodeAndUpdateLinkedList();
                            _expiryTimeTree.DeleteFromBinaryTree(Cache[keyToRemove].Value, keyToRemove);
                            maxHeapPriorityCache = RemoveElementInPriorityQueue(maxHeapPriorityCache, keyToRemove);
                            Cache.Remove(keyToRemove);
                        }
                    }
                    else
                    {
                        maxHeapPriorityCache.Enqueue(p1, p1.Priority);
                        // Check for LRU
                        var keyToRemove = _lruCache.DeleteLastNodeAndUpdateLinkedList();
                        _expiryTimeTree.DeleteFromBinaryTree(Cache[keyToRemove].Value, keyToRemove);
                        maxHeapPriorityCache = RemoveElementInPriorityQueue(maxHeapPriorityCache, keyToRemove);
                        Cache.Remove(keyToRemove);
                    }
                }
            }
            Cache.Add(key, newCache);
            maxHeapPriorityCache.Enqueue(newCache, priority);
            _expiryTimeTree.Insert(expiryTimeInSec, key);
            _lruCache.Put(key);
        }
    }

    public int Get(string key)
    {
        if (Cache.ContainsKey(key))
        {
            //check validity

            RemoveIfExpired(key);

            if (Cache.ContainsKey(key))
            {
                var cache = Cache[key];
                _lruCache.UpdateDDL(key);
                return cache.Value;
            }
        }
        return -1;
    }

    private void RemoveIfExpired(string key)
    {
        var threshold = _expirationTimeService.GetExpiryThreshold();
        var cache = Cache[key];
        if (cache.Expiry < threshold)
        {
            _expiryTimeTree.DeleteFromBinaryTree(cache.Expiry, cache.Key);
            maxHeapPriorityCache = RemoveElementInPriorityQueue(maxHeapPriorityCache, cache.Key);
            _lruCache.Delete(cache.Key);
            Cache.Remove(cache.Key);
        }
    }

    private static PriorityQueue<CacheItem, int> RemoveElementInPriorityQueue(PriorityQueue<CacheItem, int> queue, string key)
    {
        var tempQueue = new PriorityQueue<CacheItem, int>(queue.Comparer);
        while (queue.Count > 0)
        {
            var item = queue.Dequeue();
            if (!item.Key.Equals(key))
            {
                tempQueue.Enqueue(item, item.Priority);
            }
        }
        return tempQueue;
    }

    private static void UpdateElementInPriorityQueue(PriorityQueue<CacheItem, int> queue, CacheItem cacheItem)
    {
        var tempQueue = new PriorityQueue<CacheItem, int>(queue.Comparer);
        while (queue.Count > 0)
        {
            var item = queue.Dequeue();
            if (!item.Key.Equals(cacheItem.Key))
            {
                tempQueue.Enqueue(item, item.Priority);
            }
        }
        tempQueue.Enqueue(cacheItem, cacheItem.Priority);
    }
}
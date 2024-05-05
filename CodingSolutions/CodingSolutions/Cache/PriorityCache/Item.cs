namespace CodingSolutions.Cache.PriorityCache
{
    public class Item
    {
        public Item(string key, string value, int priority, int expireAfter)
        {
            Key = key;
            Value = value;
            Preference = priority;
            ExpireAfter = expireAfter;
        }

        public int Preference { get; set; }
        public int ExpireAfter { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

}

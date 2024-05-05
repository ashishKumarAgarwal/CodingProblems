namespace CodingSolutions.Cache.PriorityCache
{
    public class Item
    {
        public Item(string key, string value, int priority, int expiry)
        {
            Key = key;
            Value = value;
            Preference = priority;
            Expiry = expiry;
        }

        public int Preference { get; set; }
        public int Expiry { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

}

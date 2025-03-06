namespace Configuration;

public interface IKeyValueDictionary<TKey, TValue> : IKeyValueCollection<TKey, TValue>
    where TKey : notnull
    where TValue : notnull
{
    Option<TValue> this[TKey key] { get; set; }

    ICollection<TKey> Keys { get; }

    ICollection<TValue> Values { get; }

    IEqualityComparer<TKey> Comparer { get; }

    bool ContainsKey(TKey key);

    Option<TValue> Add(TKey key, TValue value);

    Option<TValue> Remove(TKey key);

    Option<TValue> Get(TKey key);
}
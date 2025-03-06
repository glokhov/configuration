namespace Configuration;

public interface IKeyValueCollection<TKey, TValue> : IEnumerable<KeyValue<TKey, TValue>>
    where TKey : notnull
    where TValue : notnull
{
    int Count { get; }

    void Clear();

    bool Contains(KeyValue<TKey, TValue> keyValue);

    Option<TValue> Add(KeyValue<TKey, TValue> keyValue);
}
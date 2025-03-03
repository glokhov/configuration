using System.Collections;

namespace Configuration;

public abstract class KeyValueDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary) : IKeyValueDictionary<TKey, TValue>
    where TKey : notnull
    where TValue : notnull
{
    protected KeyValueDictionary() : this(new Dictionary<TKey, TValue>())
    {
    }

    protected KeyValueDictionary(IEqualityComparer<TKey> comparer) : this(new Dictionary<TKey, TValue>(comparer))
    {
    }

    protected KeyValueDictionary(KeyValueDictionary<TKey, TValue> dictionary) : this(new Dictionary<TKey, TValue>(dictionary.Dictionary, dictionary.Comparer))
    {
    }

    protected Dictionary<TKey, TValue> Dictionary { get; } = dictionary;

    public Option<TValue> this[TKey key]
    {
        get => Get(key);
        set => value.Match(val => Add(key, val), () => Remove(key));
    }

    public ICollection<TKey> Keys => Dictionary.Keys;

    public ICollection<TValue> Values => Dictionary.Values;

    public IEqualityComparer<TKey> Comparer => Dictionary.Comparer;

    public int Count => Dictionary.Count;

    public void Clear()
    {
        Dictionary.Clear();
    }

    public bool Contains(TKey key)
    {
        return Dictionary.ContainsKey(key);
    }

    public Option<TValue> Add(TKey key, TValue value)
    {
        return Some(Dictionary[key] = value);
    }

    public Option<TValue> Remove(TKey key)
    {
#if NET
        return Dictionary.Remove(key, out var value) ? Some(value) : None;
#else
        return Dictionary.TryGetValue(key, out var value) && Dictionary.Remove(key) ? Some(value) : None;
#endif
    }

    public Option<TValue> Get(TKey key)
    {
        return Dictionary.TryGetValue(key, out var value) ? Some(value) : None;
    }

    public IEnumerator<KeyValue<TKey, TValue>> GetEnumerator()
    {
        return new KeyValueEnumerator(Dictionary.GetEnumerator());
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private sealed class KeyValueEnumerator(IEnumerator<KeyValuePair<TKey, TValue>> enumerator) : IEnumerator<KeyValue<TKey, TValue>>
    {
        public void Dispose()
        {
            enumerator.Dispose();
        }

        public bool MoveNext()
        {
            return enumerator.MoveNext();
        }

        public void Reset()
        {
            enumerator.Reset();
        }

        public KeyValue<TKey, TValue> Current => new(enumerator.Current.Key, enumerator.Current.Value);

        object IEnumerator.Current => Current;
    }
}
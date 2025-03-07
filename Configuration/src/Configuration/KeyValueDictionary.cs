using System.Collections;

namespace Configuration;

/// <summary>
/// Represents a collection of keys and values.
/// </summary>
/// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
/// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
public abstract class KeyValueDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary) : IKeyValueDictionary<TKey, TValue>
    where TKey : notnull
    where TValue : notnull
{
    /// <summary>
    /// Initializes a new instance of the <c>KeyValueDictionary&lt;TKey, TValue&gt;</c> class.
    /// </summary>
    protected KeyValueDictionary() : this(new Dictionary<TKey, TValue>())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <c>KeyValueDictionary&lt;TKey, TValue&gt;</c> class.
    /// </summary>
    /// <param name="comparer">The IEqualityComparer&lt;T&gt; implementation to use when comparing keys,
    /// or null to use the default EqualityComparer&lt;T&gt; for the type of the key.
    /// </param>
    protected KeyValueDictionary(IEqualityComparer<TKey> comparer) : this(new Dictionary<TKey, TValue>(comparer))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <c>KeyValueDictionary&lt;TKey, TValue&gt;</c> class.
    /// </summary>
    /// <param name="dictionary">The <c>KeyValueDictionary&lt;TKey, TValue&gt;</c>
    /// whose elements are copied to the new
    /// <c>KeyValueDictionary&lt;TKey, TValue&gt;</c>
    /// </param>
    protected KeyValueDictionary(KeyValueDictionary<TKey, TValue> dictionary) : this(new Dictionary<TKey, TValue>(dictionary.Dictionary))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <c>KeyValueDictionary&lt;TKey, TValue&gt;</c> class.
    /// </summary>
    /// <param name="dictionary">The <c>KeyValueDictionary&lt;TKey, TValue&gt;</c>
    /// whose elements are copied to the new
    /// <c>KeyValueDictionary&lt;TKey, TValue&gt;</c>
    /// </param>
    /// <param name="comparer">The IEqualityComparer&lt;T&gt; implementation to use when comparing keys,
    /// or null to use the default EqualityComparer&lt;T&gt; for the type of the key.
    /// </param>
    protected KeyValueDictionary(KeyValueDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : this(new Dictionary<TKey, TValue>(dictionary.Dictionary, comparer))
    {
    }

    /// <summary>
    /// Internal <c>Dictionary&lt;TKey, TValue&gt;</c>
    /// </summary>
    protected Dictionary<TKey, TValue> Dictionary { get; } = dictionary;

    /// <summary>
    /// Gets or sets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get or set.</param>
    /// <value>The value associated with the specified key.
    /// If the specified key is not found,
    /// a get operation returns <c>None</c>, and
    /// a set operation creates a new element with the specified key.
    /// If the value is <c>None</c>,
    /// a set operation removes the value with the specified key from the dictionary.
    /// </value>
    public Option<TValue> this[TKey key]
    {
        get => Get(key);
        set => value.Match(val => Add(key, val), () => Remove(key));
    }

    /// <summary>
    /// Gets a collection containing the keys in the dictionary.
    /// </summary>
    public ICollection<TKey> Keys => Dictionary.Keys;

    /// <summary>
    /// Gets a collection containing the values in the dictionary.
    /// </summary>
    public ICollection<TValue> Values => Dictionary.Values;

    /// <summary>
    /// Gets the IEqualityComparer&lt;T&gt; that is used to determine equality of keys for the dictionary.
    /// </summary>
    public IEqualityComparer<TKey> Comparer => Dictionary.Comparer;

    /// <summary>
    /// Gets the number of key/value pairs contained in the dictionary.
    /// </summary>
    public int Count => Dictionary.Count;

    /// <summary>
    /// Removes all keys and values from the dictionary.
    /// </summary>
    public void Clear()
    {
        Dictionary.Clear();
    }

    /// <summary>
    /// Determines whether the dictionary contains a specific item.
    /// </summary>
    /// <param name="item">The object to locate in the dictionary.</param>
    /// <returns>true if item is found in the dictionary; otherwise, false.</returns>
    public bool Contains(KeyValue<TKey, TValue> item)
    {
        return Dictionary.TryGetValue(item.Key, out var value) && value.Equals(item.Value);
    }

    /// <summary>
    /// Adds an item to the dictionary.
    /// </summary>
    /// <param name="item">The item to add to the dictionary.</param>
    /// <returns><c>Some(item)</c> of the added element.</returns>
    public Option<TValue> Add(KeyValue<TKey, TValue> item)
    {
        return Add(item.Key, item.Value);
    }

    /// <summary>
    /// Determines whether the dictionary contains the specified key.
    /// </summary>
    /// <param name="key">The key to locate in the dictionary.</param>
    /// <returns>true if the dictionary contains an element with the specified key; otherwise, false.</returns>
    public bool ContainsKey(TKey key)
    {
        return Dictionary.ContainsKey(key);
    }

    /// <summary>
    /// Adds the specified key and value to the dictionary.
    /// </summary>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="value">The value of the element to add.</param>
    /// <returns><c>Some(value)</c> of the added element.</returns>
    public Option<TValue> Add(TKey key, TValue value)
    {
        return Some(Dictionary[key] = value);
    }

    /// <summary>
    /// Removes the value with the specified key from the dictionary, and returns the removed element.
    /// </summary>
    /// <param name="key">The key of the element to remove.</param>
    /// <returns><c>Some(value)</c> if the element is successfully found and removed; otherwise, <c>None</c>.</returns>
    public Option<TValue> Remove(TKey key)
    {
#if NET
        return Dictionary.Remove(key, out var value) ? Some(value) : None;
#else
        return Dictionary.TryGetValue(key, out var value) && Dictionary.Remove(key) ? Some(value) : None;
#endif
    }

    /// <summary>
    /// Gets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get.</param>
    /// <returns><c>Some(value)</c> if the dictionary contains an element with the specified key; otherwise, <c>None</c>.</returns>
    public Option<TValue> Get(TKey key)
    {
        return Dictionary.TryGetValue(key, out var value) ? Some(value) : None;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the dictionary.
    /// </summary>
    /// <returns>A <c>Enumerator</c> structure for the dictionary.</returns>
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
namespace Configuration;

/// <summary>
/// Represents a collection of keys and values.
/// </summary>
/// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
/// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
public interface IKeyValueDictionary<TKey, TValue> : IKeyValueCollection<TKey, TValue>
    where TKey : notnull
    where TValue : notnull
{
    /// <summary>
    /// Gets or sets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get or set.</param>
    /// <value> The value associated with the specified key. </value>
    /// <remarks>
    /// If the specified key is not found, a get operation returns <c>None</c>, and a set operation creates a new
    /// element with the specified key. If the value is <c>None</c>, a set operation removes the value with the
    /// specified key from the dictionary.
    /// </remarks>
    Option<TValue> this[TKey key] { get; set; }

    /// <summary>
    /// Gets a collection containing the keys in the dictionary.
    /// </summary>
    ICollection<TKey> Keys { get; }

    /// <summary>
    /// Gets a collection containing the values in the dictionary.
    /// </summary>
    ICollection<TValue> Values { get; }

    /// <summary>
    /// Gets the IEqualityComparer&lt;T&gt; that is used to determine equality of keys for the dictionary.
    /// </summary>
    IEqualityComparer<TKey> Comparer { get; }

    /// <summary>
    /// Determines whether the dictionary contains the specified key.
    /// </summary>
    /// <param name="key">The key to locate in the dictionary.</param>
    /// <returns>
    /// true if the dictionary contains an element with the specified key; otherwise, false.
    /// </returns>
    bool ContainsKey(TKey key);

    /// <summary>
    /// Adds the specified key and value to the dictionary.
    /// </summary>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="value">The value of the element to add.</param>
    /// <returns><c>Some(value)</c> of the added element.</returns>
    Option<TValue> Add(TKey key, TValue value);

    /// <summary>
    /// Removes the value with the specified key from the dictionary, and returns the removed element.
    /// </summary>
    /// <param name="key">The key of the element to remove.</param>
    /// <returns>
    /// <c>Some(value)</c> if the element is successfully found and removed; otherwise, <c>None</c>.
    /// </returns>
    Option<TValue> Remove(TKey key);

    /// <summary>
    /// Gets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get.</param>
    /// <returns>
    /// <c>Some(value)</c> if the dictionary contains an element with the specified key; otherwise, <c>None</c>.
    /// </returns>
    Option<TValue> Get(TKey key);
}
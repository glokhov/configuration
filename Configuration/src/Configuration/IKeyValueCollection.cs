namespace Configuration;

/// <summary>
/// Defines Count, Clear, Contains and Add methods for a collection of keys and values.
/// </summary>
/// <typeparam name="TKey">The type of the keys in the collection.</typeparam>
/// <typeparam name="TValue">The type of the values in the collection.</typeparam>
public interface IKeyValueCollection<TKey, TValue> : IEnumerable<KeyValue<TKey, TValue>>
    where TKey : notnull
    where TValue : notnull
{
    /// <summary>
    /// Gets the number of items contained in the collection.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Removes all items from the collection.
    /// </summary>
    void Clear();

    /// <summary>
    /// Determines whether the collection contains a specific item.
    /// </summary>
    /// <param name="item">The object to locate in the collection.</param>
    /// <returns>true if item is found in the collection; otherwise, false.</returns>
    bool Contains(KeyValue<TKey, TValue> item);

    /// <summary>
    /// Adds an item to the collection.
    /// </summary>
    /// <param name="item">The item to add to the collection.</param>
    /// <returns><c>Some(item)</c> of the added element.</returns>
    Option<TValue> Add(KeyValue<TKey, TValue> item);
}
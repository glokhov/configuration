namespace Configuration;

/// <summary>
/// Defines a key/value pair that can be set or retrieved.
/// </summary>
/// <param name="Key">A <c>TKey</c> that is the key of the <c>KeyValue&lt;TKey,TValue&gt;</c>.</param>
/// <param name="Value">A <c>TValue</c> that is the value of the <c>KeyValue&lt;TKey,TValue&gt;</c>.</param>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
public readonly record struct KeyValue<TKey, TValue>(TKey Key, TValue Value)
    where TKey : notnull
    where TValue : notnull
{
    /// <summary>
    /// Deconstructs <c>KeyValue</c> by <c>Key</c> and <c>Value</c>.
    /// </summary>
    /// <param name="key">The <c>Key</c> of this <c>KeyValue</c> instance.</param>
    /// <param name="value">The <c>Value</c> of this <c>KeyValue</c> instance.</param>
    public void Deconstruct(out TKey key, out TValue value)
    {
        key = Key;
        value = Value;
    }

    /// <summary>
    /// Returns the string representation of this instance.
    /// </summary>
    /// <returns>The string representation of this instance.</returns>
    public override string ToString()
    {
        return $"[{Key}, {Value}]";
    }
}
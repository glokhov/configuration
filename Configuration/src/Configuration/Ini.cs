using System.Collections;
using System.Text;

namespace Configuration;

/// <summary>
/// Represents an INI configuration.
/// </summary>
[System.Diagnostics.DebuggerDisplay("Count = {Count}")]
public sealed class Ini : IEnumerable<(string Section, string Key, string Value)>
{
    internal const string Global = "";

    /// <summary>
    /// The empty configuration.
    /// </summary>
    public static Ini Empty => [];

    /// <summary>
    /// Initializes a new instance of the <c>Ini</c> class.
    /// </summary>
    public Ini() : this([], StringComparer.OrdinalIgnoreCase)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <c>Ini</c> class.
    /// </summary>
    /// <param name="comparer">
    /// The IEqualityComparer&lt;string&gt; that is used to determine equality of section names and keys.
    /// </param>
    public Ini(IEqualityComparer<string> comparer) : this([], comparer)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <c>Ini</c> class.
    /// </summary>
    /// <param name="elements">An <c>IEnumerable</c> to create an <c>Ini</c> from.</param>
    public Ini(IEnumerable<(string Section, string Key, string Value)> elements) : this(elements, StringComparer.OrdinalIgnoreCase)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <c>Ini</c> class.
    /// </summary>
    /// <param name="elements">An <c>IEnumerable</c> to create an <c>Ini</c> from.</param>
    /// <param name="comparer">
    /// The IEqualityComparer&lt;string&gt; that is used to determine equality of section names and keys.
    /// </param>
    public Ini(IEnumerable<(string Section, string Key, string Value)> elements, IEqualityComparer<string> comparer)
    {
        Comparer = comparer;
        Dict = elements.ToDictionary(item => (item.Section, item.Key), item => item.Value, new KeyComparer(comparer));
    }

    internal IEqualityComparer<string> Comparer { get; }

    internal Dictionary<(string Section, string Key), string> Dict { get; }

    /// <summary>
    /// Gets whether the configuration is empty.
    /// </summary>
    public bool IsEmpty => Count == 0;

    /// <summary>
    /// The number of values in the configuration.
    /// </summary>
    public int Count => Dict.Count;

    /// <summary>
    /// Gets or sets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of a value in the global section.</param>
    /// <remarks>
    /// If the specified value is not found, a get operation returns <c>None</c>,
    /// and a set operation creates a new value with the specified key.
    /// If the value is <c>None</c>, a set operation removes the value
    /// with the specified key from the configuration.
    /// </remarks>
    public Option<string> this[string key]
    {
        get => this[Global, key];
        set => this[Global, key] = value;
    }

    /// <summary>
    /// Gets or sets the value associated with the specified key.
    /// </summary>
    /// <param name="section">The name of a section in the configuration.</param>
    /// <param name="key">The key of a value in the specified section.</param>
    /// <remarks>
    /// If the specified value is not found, a get operation returns <c>None</c>,
    /// and a set operation creates a new value with the specified section name and key.
    /// If the value is <c>None</c>, a set operation removes the value
    /// with the specified section name and key from the configuration.
    /// </remarks>
    public Option<string> this[string section, string key]
    {
        get => Get(section, key);
        set => Set(section, key, value);
    }

    /// <summary>
    /// Removes all values from the configuration.
    /// </summary>
    public void Clear() => Dict.Clear();

    /// <summary>
    /// Determines whether the configuration contains a specific value.
    /// </summary>
    /// <param name="key">The key of a value in the global section.</param>
    /// <returns>true if a value is found in the configuration; otherwise, false.</returns>
    public bool Contains(string key) => Contains(Global, key);

    /// <summary>
    /// Determines whether the configuration contains a specific value.
    /// </summary>
    /// <param name="section">The name of a section in the configuration.</param>
    /// <param name="key">The key of a value in the specified section.</param>
    /// <returns>true if a value is found in the configuration; otherwise, false.</returns>
    public bool Contains(string section, string key) => Dict.ContainsKey((section, key));

    /// <summary>
    /// Adds a value to the configuration.
    /// </summary>
    /// <param name="key">The key of a value in the global section.</param>
    /// <param name="value">The value to add.</param>
    public void Add(string key, string value) => Add(Global, key, value);

    /// <summary>
    /// Adds a value to the configuration.
    /// </summary>
    /// <param name="section">The name of a section in the configuration.</param>
    /// <param name="key">The key of a value in the specified section.</param>
    /// <param name="value">The value to add.</param>
    public void Add(string section, string key, string value) => Dict[(section, key)] = value;

    /// <summary>
    /// Removes a value from the configuration.
    /// </summary>
    /// <param name="key">The key of a value in the global section.</param>
    public void Remove(string key) => Remove(Global, key);

    /// <summary>
    /// Removes a value from the configuration.
    /// </summary>
    /// <param name="section">The name of a section in the configuration.</param>
    /// <param name="key">The key of a value in the specified section.</param>
    public void Remove(string section, string key) => Dict.Remove((section, key));

    private Option<string> Get(string section, string key)
    {
        return Dict.TryGetValue((section, key), out var value) ? Some(value) : None;
    }

    private void Set(string section, string key, Option<string> value)
    {
        value.Match(val => Add(section, key, val), () => Remove(section, key));
    }

    /// <inheritdoc />
    public IEnumerator<(string Section, string Key, string Value)> GetEnumerator()
    {
        return Dict.Select(entry => (entry.Key.Section, entry.Key.Key, entry.Value)).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        using var enumerator = GetEnumerator();

        var builder = new StringBuilder("ini [");

        if (enumerator.MoveNext()) // 1
        {
            var (s, k, v) = enumerator.Current;

            builder.Append(s);
            builder.Append('.');
            builder.Append(k);
            builder.Append(" = ");
            builder.Append(v);
        }
        else
        {
            builder.Append(']');
            return builder.ToString();
        }

        if (enumerator.MoveNext()) // 2
        {
            var (s, k, v) = enumerator.Current;

            builder.Append("; ");
            builder.Append(s);
            builder.Append('.');
            builder.Append(k);
            builder.Append(" = ");
            builder.Append(v);
        }
        else
        {
            builder.Append(']');
            return builder.ToString();
        }

        if (enumerator.MoveNext()) // 3
        {
            var (s, k, v) = enumerator.Current;

            builder.Append("; ");
            builder.Append(s);
            builder.Append('.');
            builder.Append(k);
            builder.Append(" = ");
            builder.Append(v);
        }
        else
        {
            builder.Append(']');
            return builder.ToString();
        }

        if (enumerator.MoveNext()) // 4
        {
            builder.Append("; ...]");
            return builder.ToString();
        }

        builder.Append(']');
        return builder.ToString();
    }

    private class KeyComparer(IEqualityComparer<string> comparer) : IEqualityComparer<(string Section, string Key)>
    {
        public bool Equals((string Section, string Key) x, (string Section, string Key) y)
        {
            return comparer.Equals(x.Section, y.Section) && comparer.Equals(x.Key, y.Key);
        }

        public int GetHashCode((string Section, string Key) obj)
        {
            unchecked
            {
                return (comparer.GetHashCode(obj.Section) * 397) ^ comparer.GetHashCode(obj.Key);
            }
        }
    }
}
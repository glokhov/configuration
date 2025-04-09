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

    #region Static

    /// <summary>
    /// The empty configuration.
    /// </summary>
    public static Ini Empty => [];

    /// <summary>
    /// Initializes new <c>Ini</c> from a file.
    /// </summary>
    /// <param name="path">The full name of a configuration file.</param>
    /// <returns><c>Ok(Ini)</c> if successful; otherwise, <c>Err(string)</c>.</returns>
    public static Result<Ini, string> FromFile(string path)
    {
        return Empty.AppendFromFile(path);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a file.
    /// </summary>
    /// <param name="path">The full name of a configuration file.</param>
    /// <param name="comparer">
    /// The IEqualityComparer&lt;string&gt; that is used to determine equality of section names and keys.
    /// </param>
    /// <returns><c>Ok(Ini)</c> if successful; otherwise, <c>Err(string)</c>.</returns>
    public static Result<Ini, string> FromFile(string path, IEqualityComparer<string> comparer)
    {
        return new Ini(comparer).AppendFromFile(path);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a file.
    /// </summary>
    /// <param name="file">The <c>FileInfo</c> of a configuration file.</param>
    /// <returns><c>Ok(Ini)</c> if successful; otherwise, <c>Err(string)</c>.</returns>
    public static Result<Ini, string> FromFile(FileInfo file)
    {
        return Empty.AppendFromFile(file);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a file.
    /// </summary>
    /// <param name="file">The <c>FileInfo</c> of a configuration file.</param>
    /// <param name="comparer">
    /// The IEqualityComparer&lt;string&gt; that is used to determine equality of section names and keys.
    /// </param>
    /// <returns><c>Ok(Ini)</c> if successful; otherwise, <c>Err(string)</c>.</returns>
    public static Result<Ini, string> FromFile(FileInfo file, IEqualityComparer<string> comparer)
    {
        return new Ini(comparer).AppendFromFile(file);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a string.
    /// </summary>
    /// <param name="text">The string representation of a configuration file.</param>
    /// <returns><c>Ok(Ini)</c> if successful; otherwise, <c>Err(string)</c>.</returns>
    public static Result<Ini, string> FromString(string text)
    {
        return Empty.AppendFromString(text);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a string.
    /// </summary>
    /// <param name="text">The string representation of a configuration file.</param>
    /// <param name="comparer">
    /// The IEqualityComparer&lt;string&gt; that is used to determine equality of section names and keys.
    /// </param>
    /// <returns><c>Ok(Ini)</c> if successful; otherwise, <c>Err(string)</c>.</returns>
    public static Result<Ini, string> FromString(string text, IEqualityComparer<string> comparer)
    {
        return new Ini(comparer).AppendFromString(text);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a <c>TextReader</c>.
    /// </summary>
    /// <param name="reader">The <c>TextReader</c> of a configuration file.</param>
    /// <returns><c>Ok(Ini)</c> if successful; otherwise, <c>Err(string)</c>.</returns>
    public static Result<Ini, string> FromReader(TextReader reader)
    {
        return Empty.AppendFromReader(reader);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a <c>TextReader</c>.
    /// </summary>
    /// <param name="reader">The <c>TextReader</c> of a configuration file.</param>
    /// <param name="comparer">
    /// The IEqualityComparer&lt;string&gt; that is used to determine equality of section names and keys.
    /// </param>
    /// <returns><c>Ok(Ini)</c> if successful; otherwise, <c>Err(string)</c>.</returns>
    public static Result<Ini, string> FromReader(TextReader reader, IEqualityComparer<string> comparer)
    {
        return new Ini(comparer).AppendFromReader(reader);
    }

    #endregion

    #region Ctor

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
        Dict = elements.ToDictionary(item => (item.Section, item.Key), item => item.Value, new KeyComparer(Comparer = comparer));
    }

    internal Dictionary<(string Section, string Key), string> Dict { get; }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the IEqualityComparer&lt;string&gt; that is used to determine equality of section names and keys.
    /// </summary>
    public IEqualityComparer<string> Comparer { get; }

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
        get => this.Get(key);
        set => this.Set(key, value);
    }

    /// <summary>
    /// Gets or sets the value associated with the specified section name and key.
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
        get => this.Get(section, key);
        set => this.Set(section, key, value);
    }

    #endregion

    #region IEnumerable

    /// <inheritdoc />
    public IEnumerator<(string Section, string Key, string Value)> GetEnumerator()
    {
        return Dict.Select(entry => (entry.Key.Section, entry.Key.Key, entry.Value)).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion

    #region ToString

    /// <inheritdoc />
    public override string ToString()
    {
        using var writer = new StringWriter();

        this.ToWriter(writer);

        return writer.ToString();
    }

    #endregion

    #region IEqualityComparer

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

    #endregion
}
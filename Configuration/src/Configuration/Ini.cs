using System.Collections;

namespace Configuration;

public sealed partial class Ini(Config config) : IKeyValueCollection<string, Section>
{
    public Ini() : this([])
    {
    }

    public Ini(IEqualityComparer<string> comparer) : this(new Config(comparer))
    {
    }

    public Config Config { get; } = config;

    /// <summary>
    /// Gets the IEqualityComparer&lt;string&gt; that is used to determine equality of keys for the dictionary.
    /// </summary>
    public IEqualityComparer<string> Comparer { get; } = config.Comparer;

    /// <summary>
    /// Gets or sets the section associated with the specified section name.
    /// </summary>
    /// <param name="section">The section name of the section to get or set.</param>
    /// <value>The section associated with the specified section name.
    /// If the specified section is not found,
    /// a get operation returns <c>None</c>, and
    /// a set operation creates a new section with the specified section name.
    /// If the section is <c>None</c>,
    /// a set operation removes the section with the specified section name from the configuration.
    /// </value>
    public Option<Section> this[string section]
    {
        get => Config[section];
        set => Config[section] = value;
    }

    /// <summary>
    /// Gets or sets the value associated with the specified section name and key.
    /// </summary>
    /// <param name="section">The section name of the section to get or set.</param>
    /// <param name="key">The key of the value to get or set.</param>
    /// <value>The value associated with the specified section name and key.
    /// If the specified value is not found,
    /// a get operation returns <c>None</c>, and
    /// a set operation creates a new value with the specified section name and key.
    /// If the value is <c>None</c>,
    /// a set operation removes the value with the specified section name and key from the configuration.
    /// </value>
    public Option<string> this[string section, string key]
    {
        get => Config[section].Bind(sec => sec[key]);
        set => Config[section].Match(sec => sec[key] = value, () => Config[section] = Some(new Section(Comparer) { [key] = value }));
    }

    /// <summary>
    /// Gets the number of key/value pairs contained in the configuration.
    /// </summary>
    public int Count => Config.Count;

    /// <summary>
    /// Removes all keys and values from the configuration.
    /// </summary>
    public void Clear()
    {
        Config.Clear();
    }

    /// <summary>
    /// Determines whether the configuration contains a specific item.
    /// </summary>
    /// <param name="item">The object to locate in the configuration.</param>
    /// <returns>true if item is found in the configuration; otherwise, false.</returns>
    public bool Contains(KeyValue<string, Section> item)
    {
        return Config.Contains(item);
    }

    /// <summary>
    /// Adds an item to the configuration.
    /// </summary>
    /// <param name="item">The item to add to the configuration.</param>
    /// <returns><c>Some(item)</c> of the added element.</returns>
    public Option<Section> Add(KeyValue<string, Section> item)
    {
        return Config.Add(item);
    }

    /// <summary>
    /// Returns an enumerator that iterates through the configuration.
    /// </summary>
    /// <returns>A <c>Enumerator</c> structure for the configuration.</returns>
    public IEnumerator<KeyValue<string, Section>> GetEnumerator()
    {
        return Config.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public override string ToString()
    {
        return Config.ToString();
    }
}
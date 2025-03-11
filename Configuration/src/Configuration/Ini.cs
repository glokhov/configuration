using System.Collections;

namespace Configuration;

/// <summary>
/// Represents an ini file configuration.
/// </summary>
public sealed partial class Ini(ConfigDictionary configDictionary) : IKeyValueCollection<string, SectionDictionary>
{
    /// <summary>
    /// Initializes a new instance of the <c>Ini</c> class.
    /// </summary>
    public Ini() : this([])
    {
    }

    /// <summary>
    /// Initializes a new instance of the <c>Ini</c> class.
    /// </summary>
    /// <param name="comparer">
    /// The IEqualityComparer&lt;T&gt; implementation to use when comparing section names and keys.
    /// </param>
    public Ini(IEqualityComparer<string> comparer) : this(new ConfigDictionary(comparer))
    {
    }

    /// <summary>
    /// The internal <c>ConfigDictionary</c> dictionary.
    /// </summary>
    public ConfigDictionary Config { get; } = configDictionary;

    /// <summary>
    /// Gets the IEqualityComparer&lt;string&gt; that is used to determine equality of section names and keys.
    /// </summary>
    public IEqualityComparer<string> Comparer { get; } = configDictionary.Comparer;

    /// <summary>
    /// Gets or sets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get or set.</param>
    /// <remarks>
    /// If the specified value is not found, a get operation returns <c>None</c>, and a set operation creates a new
    /// value with the specified key. If the value is <c>None</c>, a set operation removes the value
    /// with the specified key from the configuration.
    /// </remarks>
    public Option<string> this[string key]
    {
        get => this[GlobalSection, key];
        set => this[GlobalSection, key] = value;
    }

    /// <summary>
    /// Gets or sets the value associated with the specified section name and key.
    /// </summary>
    /// <param name="section">The section name of the section to get or set.</param>
    /// <param name="key">The key of the value to get or set.</param>
    /// <remarks>
    /// If the specified value is not found, a get operation returns <c>None</c>, and a set operation creates a new
    /// value with the specified section name and key. If the value is <c>None</c>, a set operation removes the value
    /// with the specified section name and key from the configuration.
    /// </remarks>
    public Option<string> this[string section, string key]
    {
        get => GetValue(section, key);
        set => SetValue(section, key, value);
    }

    /// <summary>
    /// Gets the global section.
    /// </summary>
    /// <returns><c>Some</c> value if global section exists in the configuration; otherwise, <c>None</c>.</returns>
    public Option<SectionDictionary> GetGlobalSection()
    {
        return GetSection(GlobalSection);
    }

    /// <summary>
    /// Sets the global section. Removes the section, if the section is <c>None</c>.
    /// </summary>
    /// <param name="value">The section to set.</param>
    /// <returns><c>None</c> if section is removed from the configuration; otherwise, <c>Some</c> value.</returns>
    public Option<SectionDictionary> SetGlobalSection(Option<SectionDictionary> value)
    {
        return SetSection(GlobalSection, value);
    }

    /// <summary>
    /// Gets the section associated with the specified section name.
    /// </summary>
    /// <param name="section">The section name of the section to get.</param>
    /// <returns><c>Some</c> value if section is found in the configuration; otherwise, <c>None</c>.</returns>
    public Option<SectionDictionary> GetSection(string section)
    {
        return Config[section];
    }

    /// <summary>
    /// Sets the section associated with the specified section name. Removes the section, if the section is <c>None</c>.
    /// </summary>
    /// <param name="section">The section name of the section to set.</param>
    /// <param name="value">The section to set.</param>
    /// <returns><c>None</c> if section is removed from the configuration; otherwise, <c>Some</c> value.</returns>
    public Option<SectionDictionary> SetSection(string section, Option<SectionDictionary> value)
    {
        return Config[section] = value;
    }

    private Option<string> GetValue(string section, string key)
    {
        return Config[section].Bind(sec => sec[key]);
    }

    private Option<string> SetValue(string section, string key, Option<string> value)
    {
        return Config[section].Match(SetSectionValue, AddSectionValue);

        Option<string> SetSectionValue(SectionDictionary sectionDictionary)
        {
            return sectionDictionary[key] = value;
        }

        Option<string> AddSectionValue()
        {
            return (Config[section] = Some(new SectionDictionary(Comparer) { [key] = value })).Bind(sec => sec[key]);
        }
    }

    /// <summary>
    /// Gets the number of sections contained in the configuration.
    /// </summary>
    public int Count => Config.Count;

    /// <summary>
    /// Removes all sections from the configuration.
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
    public bool Contains(KeyValue<string, SectionDictionary> item)
    {
        return Config.Contains(item);
    }

    /// <summary>
    /// Adds an item to the configuration.
    /// </summary>
    /// <param name="item">The item to add to the configuration.</param>
    /// <returns><c>Some(item)</c> of the added element.</returns>
    public Option<SectionDictionary> Add(KeyValue<string, SectionDictionary> item)
    {
        return Config.Add(item);
    }

    /// <summary>
    /// Returns an enumerator that iterates through the configuration.
    /// </summary>
    /// <returns>A <c>Enumerator</c> structure for the configuration.</returns>
    public IEnumerator<KeyValue<string, SectionDictionary>> GetEnumerator()
    {
        return Config.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Returns the string representation of this instance.
    /// </summary>
    /// <returns>The string representation of this instance.</returns>
    public override string ToString()
    {
        return Config.ToString();
    }
}
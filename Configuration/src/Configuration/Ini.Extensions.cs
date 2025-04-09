namespace Configuration;

/// <summary>
/// Extension methods.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Removes all values from the configuration.
    /// </summary>
    /// <param name="ini">The <c>Ini</c> configuration.</param>
    public static void Clear(this Ini ini) => ini.Dict.Clear();

    /// <summary>
    /// Determines whether the configuration contains a specific value.
    /// </summary>
    /// <param name="ini">The <c>Ini</c> configuration.</param>
    /// <param name="key">The key of a value in the global section.</param>
    /// <returns>true if a value is found in the configuration; otherwise, false.</returns>
    public static bool Contains(this Ini ini, string key) => ini.Contains(Ini.Global, key);

    /// <summary>
    /// Determines whether the configuration contains a specific value.
    /// </summary>
    /// <param name="ini">The <c>Ini</c> configuration.</param>
    /// <param name="section">The name of a section in the configuration.</param>
    /// <param name="key">The key of a value in the specified section.</param>
    /// <returns>true if a value is found in the configuration; otherwise, false.</returns>
    public static bool Contains(this Ini ini, string section, string key) => ini.Dict.ContainsKey((section, key));

    /// <summary>
    /// Gets the value associated with the specified key.
    /// </summary>
    /// <param name="ini">The <c>Ini</c> configuration.</param>
    /// <param name="key">The key of a value in the specified section.</param>
    /// <returns><c>Some(value)</c> if a value is found in the configuration; otherwise, <c>None</c>.</returns>
    public static Option<string> Get(this Ini ini, string key) => ini.GetExact(Ini.Global, key);

    private static Option<string> GetExact(this Ini ini, string section, string key)
    {
        return ini.Dict.TryGetValue((section, key), out var value) ? Some(value) : None;
    }

    /// <summary>
    /// Gets the value associated with the specified section name and key.
    /// </summary>
    /// <remarks>
    /// Gets the value by looking in the specified section and its parent sections in the configuration.
    /// </remarks>
    /// <param name="ini">The <c>Ini</c> configuration.</param>
    /// <param name="section">The name of a section in the configuration.</param>
    /// <param name="key">The key of a value in the specified section.</param>
    /// <returns><c>Some(value)</c> if a value is found in the configuration; otherwise, <c>None</c>.</returns>
#pragma warning disable
    public static Option<string> Get(this Ini ini, string section, string key) => ini.GetNested(section, key);
#pragma warning enable

    /// <summary>
    /// Gets the value associated with the specified section name and key.
    /// </summary>
    [Obsolete("Use Configuration.Ini.Get(section, key) instead. This method will be removed in future versions.")]
    public static Option<string> GetNested(this Ini ini, string section, string key)
    {
        return NestedSections(section).Select(sec => ini.GetExact(sec, key)).FirstOrDefault(val => val.IsSome);
    }

    private static IEnumerable<string> NestedSections(string section)
    {
        int index;

        yield return section;

        while ((index = section.LastIndexOf('.')) >= 0) yield return section = section[..index];
    }

    /// <summary>
    /// Sets the value associated with the specified key.
    /// </summary>
    /// <param name="ini">The <c>Ini</c> configuration.</param>
    /// <param name="key">The key of a value in the specified section.</param>
    /// <param name="value">The value to set.</param>
    public static void Set(this Ini ini, string key, Option<string> value) => ini.Set(Ini.Global, key, value);

    /// <summary>
    /// Sets the value associated with the specified section name and key.
    /// </summary>
    /// <param name="ini">The <c>Ini</c> configuration.</param>
    /// <param name="section">The name of a section in the configuration.</param>
    /// <param name="key">The key of a value in the specified section.</param>
    /// <param name="value">The value to set.</param>
    public static void Set(this Ini ini, string section, string key, Option<string> value)
    {
        value.Match(val => ini.Add(section, key, val), () => ini.Remove(section, key));
    }

    /// <summary>
    /// Adds a value to the configuration.
    /// </summary>
    /// <param name="ini">The <c>Ini</c> configuration.</param>
    /// <param name="key">The key of a value in the global section.</param>
    /// <param name="value">The value to add.</param>
    public static void Add(this Ini ini, string key, string value) => ini.Add(Ini.Global, key, value);

    /// <summary>
    /// Adds a value to the configuration.
    /// </summary>
    /// <param name="ini">The <c>Ini</c> configuration.</param>
    /// <param name="section">The name of a section in the configuration.</param>
    /// <param name="key">The key of a value in the specified section.</param>
    /// <param name="value">The value to add.</param>
    public static void Add(this Ini ini, string section, string key, string value) => ini.Dict[(section, key)] = value;

    /// <summary>
    /// Removes a value from the configuration.
    /// </summary>
    /// <param name="ini">The <c>Ini</c> configuration.</param>
    /// <param name="key">The key of a value in the global section.</param>
    public static void Remove(this Ini ini, string key) => ini.Remove(Ini.Global, key);

    /// <summary>
    /// Removes a value from the configuration.
    /// </summary>
    /// <param name="ini">The <c>Ini</c> configuration.</param>
    /// <param name="section">The name of a section in the configuration.</param>
    /// <param name="key">The key of a value in the specified section.</param>
    public static void Remove(this Ini ini, string section, string key) => ini.Dict.Remove((section, key));

    /// <summary>
    /// Gets a collection containing the section names in the configuration.
    /// </summary>
    /// <param name="ini">The existing <c>Ini</c> configuration.</param>
    /// <returns>The collection of the section names.</returns>
    public static IEnumerable<string> GetSections(this Ini ini)
    {
        return ini.Select(element => element.Section);
    }

    /// <summary>
    /// Gets a collection containing the keys in the global section.
    /// </summary>
    /// <param name="ini">The <c>Ini</c> configuration.</param>
    /// <returns>The collection of the keys.</returns>
    public static IEnumerable<string> GetGlobalKeys(this Ini ini)
    {
        return ini.GetKeys(Ini.Global);
    }

    /// <summary>
    /// Gets a collection containing the values in the global section.
    /// </summary>
    /// <param name="ini">The <c>Ini</c> configuration.</param>
    /// <returns>The collection of the values.</returns>
    public static IEnumerable<string> GetGlobalValues(this Ini ini)
    {
        return ini.GetValues(Ini.Global);
    }

    /// <summary>
    /// Gets a <c>Dictionary&lt;string, string&gt;</c> of the global section.
    /// </summary>
    /// <param name="ini">The <c>Ini</c> configuration.</param>
    /// <returns>The <c>Dictionary&lt;string, string&gt;</c>.</returns>
    public static Dictionary<string, string> GetGlobalSection(this Ini ini)
    {
        return ini.GetSection(Ini.Global);
    }

    /// <summary>
    /// Gets a collection containing the keys in the specified section.
    /// </summary>
    /// <param name="ini">The existing <c>Ini</c> configuration.</param>
    /// <param name="section">The section name.</param>
    /// <returns>The collection of the keys.</returns>
    public static IEnumerable<string> GetKeys(this Ini ini, string section)
    {
        return ini.Where(element => element.Section == section).Select(element => element.Key);
    }

    /// <summary>
    /// Gets a collection containing the values in the specified section.
    /// </summary>
    /// <param name="ini">The <c>Ini</c> configuration.</param>
    /// <param name="section">The section name.</param>
    /// <returns>The collection of the values.</returns>
    public static IEnumerable<string> GetValues(this Ini ini, string section)
    {
        return ini.Where(element => element.Section == section).Select(element => element.Value);
    }

    /// <summary>
    /// Gets a <c>Dictionary&lt;string, string&gt;</c> of the specified section.
    /// </summary>
    /// <param name="ini">The <c>Ini</c> configuration.</param>
    /// <param name="section">The section name.</param>
    /// <returns>The <c>Dictionary&lt;string, string&gt;</c>.</returns>
    public static Dictionary<string, string> GetSection(this Ini ini, string section)
    {
        return ToDictionary(ini.GetKeys(section), ini.GetValues(section));
    }

    private static Dictionary<string, string> ToDictionary(IEnumerable<string> keys, IEnumerable<string> values)
    {
        return keys.Zip(values, (key, value) => (key, value)).ToDictionary(kvp => kvp.key, kvp => kvp.value);
    }
}
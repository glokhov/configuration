namespace Configuration;

/// <summary>
/// Extension methods.
/// </summary>
public static class Extensions
{
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
    /// <param name="ini">The existing <c>Ini</c> configuration.</param>
    /// <returns>The collection of the keys.</returns>
    public static IEnumerable<string> GetGlobalKeys(this Ini ini)
    {
        return ini.GetKeys(Ini.Global);
    }

    /// <summary>
    /// Gets a collection containing the values in the global section.
    /// </summary>
    /// <param name="ini">The existing <c>Ini</c> configuration.</param>
    /// <returns>The collection of the values.</returns>
    public static IEnumerable<string> GetGlobalValues(this Ini ini)
    {
        return ini.GetValues(Ini.Global);
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
    /// <param name="ini">The existing <c>Ini</c> configuration.</param>
    /// <param name="section">The section name.</param>
    /// <returns>The collection of the values.</returns>
    public static IEnumerable<string> GetValues(this Ini ini, string section)
    {
        return ini.Where(element => element.Section == section).Select(element => element.Value);
    }

    /// <summary>
    /// Gets a <c>Dictionary&lt;string, string&gt;</c> of the global section.
    /// </summary>
    /// <param name="ini">The existing <c>Ini</c> configuration.</param>
    /// <returns>The <c>Dictionary&lt;string, string&gt;</c>.</returns>
    public static Dictionary<string, string> GetGlobalSection(this Ini ini)
    {
        return ini.GetSection(Ini.Global);
    }

    /// <summary>
    /// Gets a <c>Dictionary&lt;string, string&gt;</c> of the specified section.
    /// </summary>
    /// <param name="ini">The existing <c>Ini</c> configuration.</param>
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
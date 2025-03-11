namespace Configuration;

/// <summary>
/// Extensions for the type <c>Ini</c>.
/// </summary>
public static class IniExtensions
{
    /// <summary>
    /// Writes the text representing of an ini configuration to the file.
    /// </summary>
    /// <param name="ini">The <c>Ini</c> representation of an ini configuration.</param>
    /// <param name="fileInfo">>The <c>FileInfo</c> instance.</param>
    /// <returns>The <c>Result</c> of a write operation.</returns>
    public static Result<Unit, string> WriteTo(this Ini ini, FileInfo fileInfo)
    {
        try
        {
            using var textWriter = fileInfo.CreateText();
            return ini.WriteTo(textWriter);
        }
        catch (Exception exception)
        {
            return Err(exception.Message);
        }
    }

    /// <summary>
    /// Writes the text representing of an ini configuration to the text stream.
    /// </summary>
    /// <param name="ini">The <c>Ini</c> representation of an ini configuration.</param>
    /// <param name="textWriter">>The <c>TextWriter</c> instance.</param>
    /// <returns>The <c>Result</c> of a write operation.</returns>
    public static Result<Unit, string> WriteTo(this Ini ini, TextWriter textWriter)
    {
        try
        {
            textWriter.Write(ini.ToString());
            textWriter.Flush();
        }
        catch (Exception exception)
        {
            return Err(exception.Message);
        }

        return Ok(Unit.Default);
    }

    /// <summary>
    /// Gets the section associated with the specified section name.
    /// </summary>
    /// <remarks>
    /// Creates a new instance of Section class by merging nested sections of an ini configuration.
    /// </remarks>
    /// <param name="ini">The <c>Ini</c> representation of an ini configuration.</param>
    /// <param name="section">The section name of the section to get.</param>
    /// <returns><c>Some(Section)</c> if the specified section is found; otherwise, <c>None</c>.</returns>
    public static Option<SectionDictionary> GetNested(this Ini ini, string section)
    {
        return NestedForward(section).Select(ini.GetSection).Aggregate(SomeSection(ini), MergeSections);
    }

    /// <summary>
    /// Gets the value associated with the specified section name and key.
    /// </summary>
    /// <remarks>
    /// Gets the value by looking in all nested sections of an ini configuration.
    /// </remarks>
    /// <param name="ini">The <c>Ini</c> representation of an ini configuration.</param>
    /// <param name="section">The section name of the section to get.</param>
    /// <param name="key">The key of the value to get.</param>
    /// <returns><c>Some(string)</c> if the specified value is found; otherwise, <c>None</c>.</returns>
    public static Option<string> GetNested(this Ini ini, string section, string key)
    {
        return NestedBackward(section).Select(nested => ini.GetSection(nested).Bind(sec => sec[key])).FirstOrDefault(opt => opt.IsSome);
    }

    private static Option<SectionDictionary> SomeSection(Ini ini)
    {
        return Some(new SectionDictionary(ini.Comparer));
    }

    private static Option<SectionDictionary> MergeSections(Option<SectionDictionary> current, Option<SectionDictionary> next)
    {
        return current.Bind(cur => next.IsSome ? next.Map(cur.Merge) : current);
    }

    private static IEnumerable<string> NestedForward(string section)
    {
        var index = -1;

        while ((index = section.IndexOf('.', index + 1)) > 0)
        {
            yield return section[..index];
        }

        yield return section;
    }

    private static IEnumerable<string> NestedBackward(string section)
    {
        int index;

        yield return section;

        while ((index = section.LastIndexOf('.')) > 0)
        {
            yield return section = section[..index];
        }
    }
}
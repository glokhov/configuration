using Configuration.Misc;

namespace Configuration;

/// <summary>
/// Extension methods.
/// </summary>
public static class Reader
{
    /// <summary>
    /// Appends elements to the existing <c>Ini</c> configuration from a file.
    /// </summary>
    /// <param name="ini">The existing <c>Ini</c> configuration.</param>
    /// <param name="path">The full name of a configuration file.</param>
    /// <returns><c>Ok(Ini)</c> if successful; otherwise, <c>Err(string)</c>.</returns>
    public static Result<Ini, string> AppendFromFile(this Ini ini, string path)
    {
        try
        {
            return ini.AppendFromFile(new FileInfo(path));
        }
        catch (Exception e)
        {
            return Err(e.Message);
        }
    }

    /// <summary>
    /// Appends elements to the existing <c>Ini</c> configuration from a file.
    /// </summary>
    /// <param name="ini">The existing <c>Ini</c> configuration.</param>
    /// <param name="file">The <c>FileInfo</c> of a configuration file.</param>
    /// <returns><c>Ok(Ini)</c> if successful; otherwise, <c>Err(string)</c>.</returns>
    public static Result<Ini, string> AppendFromFile(this Ini ini, FileInfo file)
    {
        try
        {
            using var reader = file.OpenText();
            return ini.AppendFromReader(reader);
        }
        catch (Exception e)
        {
            return Err(e.Message);
        }
    }

    /// <summary>
    /// Appends elements to the existing <c>Ini</c> configuration from a <c>TextReader</c>.
    /// </summary>
    /// <param name="ini">The existing <c>Ini</c> configuration.</param>
    /// <param name="reader">The <c>TextReader</c> of a configuration file.</param>
    /// <returns><c>Ok(Ini)</c> if successful; otherwise, <c>Err(string)</c>.</returns>
    public static Result<Ini, string> AppendFromReader(this Ini ini, TextReader reader)
    {
        try
        {
            return reader.ReadLines().Select(Parser.ParseLine).Collect().Bind(lines => AppendLines(ini, lines));
        }
        catch (Exception e)
        {
            return Err(e.Message);
        }
    }

    private static Result<Ini, string> AppendLines(this Ini ini, List<Line> lines)
    {
        var currentSection = Ini.Global;

        foreach (var line in lines)
        {
            switch (line)
            {
                case Section section:
                    currentSection = section.Name;
                    break;
                case Parameter parameter:
                    ini[currentSection, parameter.Key] = Some(parameter.Value);
                    break;
            }
        }

        return Ok(ini);
    }
}
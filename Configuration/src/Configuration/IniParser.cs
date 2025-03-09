namespace Configuration;

public partial class Ini
{
    /// <summary>
    /// Parses the text representing of an ini configuration into an instance of the type <c>Ini</c>.
    /// </summary>
    /// <param name="input">The text representation of an ini configuration.</param>
    /// <returns>The <c>Ini</c> representation of an ini configuration.</returns>
    public static Result<Ini, string> Parse(string input)
    {
        return Parse(input, []);
    }

    /// <summary>
    /// Parses the text representing of an ini configuration into an instance of the type <c>Ini</c>.
    /// </summary>
    /// <param name="input">The text representation of an ini configuration.</param>
    /// <param name="comparer">The IEqualityComparer&lt;T&gt; implementation to use when comparing keys.</param>
    /// <returns>The <c>Ini</c> representation of an ini configuration.</returns>
    public static Result<Ini, string> Parse(string input, IEqualityComparer<string> comparer)
    {
        return Parse(input, new Ini(comparer));
    }

    private static Result<Ini, string> Parse(string input, Ini ini)
    {
        var section = "default";

        foreach (var (number, line) in input.Split('\n').Select(RemoveComment).Select(NumberAndLine).Where(IsNotEmpty))
        {
            if (ParseSectionName(line).Bind(CreateSection).IsSome)
            {
                continue;
            }

            if (ParseParameter(line).Bind(AddOrUpdateParameter).IsSome)
            {
                continue;
            }

            return Err($"Cannot parse line {number}: {line}.");
        }

        return Ok(ini);

        Option<Section> CreateSection(string name)
        {
            return ini[section = name] = Some(new Section(ini.Comparer));
        }

        Option<string> AddOrUpdateParameter((string key, string value) parameter)
        {
            return ini[section, parameter.key] = Some(parameter.value);
        }
    }

    private static Option<string> ParseSectionName(string line)
    {
        return TrimSectionStart(line).Bind(TrimSectionEnd);
    }

    private static Option<string> TrimSectionStart(string line)
    {
        line = line.TrimStart();
#if NET
        return line.StartsWith('[') ? Some(line[1..].TrimStart()) : None;
#else
        return line.StartsWith("[") ? Some(line[1..].TrimStart()) : None;
#endif
    }

    private static Option<string> TrimSectionEnd(string line)
    {
        line = line.TrimEnd();
#if NET
        return line.EndsWith(']') ? Some(line[..^1].TrimEnd()) : None;
#else
        return line.EndsWith("]") ? Some(line[..^1].TrimEnd()) : None;
#endif
    }

    private static Option<(string, string)> ParseParameter(string line)
    {
        return GetDelimeterIndex(line).Map(index => (line[..index].Trim(), line[(index + 1)..].Trim()));
    }

    private static string RemoveComment(string line)
    {
        return GetCommentIndex(line).Match(index => line[..index], line);
    }

    private static Option<int> GetDelimeterIndex(string line)
    {
        return GetSomeIndex(line.IndexOf('='));
    }

    private static Option<int> GetCommentIndex(string line)
    {
        return GetSomeIndex(line.IndexOf('#'));
    }

    private static Option<int> GetSomeIndex(int index)
    {
        return index >= 0 ? Some(index) : None;
    }

    private static (int, string) NumberAndLine(string line, int index)
    {
        return (index + 1, line);
    }

    private static bool IsNotEmpty((int number, string line) line)
    {
        return !string.IsNullOrWhiteSpace(line.line);
    }
}
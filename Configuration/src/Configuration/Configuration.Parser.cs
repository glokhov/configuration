namespace Configuration;

public partial class Configuration
{
    public static Result<Configuration, string> Parse(string input)
    {
        return Parse(input, new Configuration());
    }

    public static Result<Configuration, string> Parse(string input, IEqualityComparer<string> comparer)
    {
        return Parse(input, new Configuration(comparer));
    }

    private static Result<Configuration, string> Parse(string input, Configuration configuration)
    {
        var section = "default";

        foreach (var (line, number) in input.Split('\n').Select(RemoveComment).Select(Trim).Select(WithNumber).Where(IsNotEmpty))
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

        return Ok(configuration);

        Option<Section> CreateSection(SectionName sectionName)
        {
            return configuration[section = sectionName.Name] = Some(new Section(configuration.Comparer));
        }

        Option<string> AddOrUpdateParameter(Parameter parameter)
        {
            return configuration[section, parameter.Key] = Some(parameter.Value);
        }
    }

    private static Option<SectionName> ParseSectionName(string line)
    {
        return IsSection(line) ? Some(new SectionName(line[1..^1].Trim())) : None;
    }

    private static bool IsSection(string line)
    {
#if NET
        return line.StartsWith('[') && line.EndsWith(']');
#else
        return line.StartsWith("[") && line.EndsWith("]");
#endif
    }

    private static Option<Parameter> ParseParameter(string line)
    {
        return SplitParameter(line, line.IndexOf('='));
    }

    private static Option<Parameter> SplitParameter(string line, int index)
    {
        return index < 0 ? None : Some(new Parameter(line[..index].Trim(), line[(index + 1)..].Trim()));
    }

    private static string RemoveComment(string line)
    {
        return LineWithoutComment(line, line.IndexOf('#'));
    }

    private static string LineWithoutComment(string line, int indexOfComment)
    {
        return indexOfComment < 0 ? line : line[..indexOfComment];
    }

    private static string Trim(string line)
    {
        return line.Trim();
    }

    private static LineWithNumber WithNumber(string line, int indexOfLine)
    {
        return new LineWithNumber(line, indexOfLine + 1);
    }

    private static bool IsNotEmpty(LineWithNumber line)
    {
        return line.Text.Length > 0;
    }

    private readonly record struct SectionName(string Name);

    private readonly record struct Parameter(string Key, string Value);

    private readonly record struct LineWithNumber(string Text, int Number);
}
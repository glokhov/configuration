using System.Text.RegularExpressions;

namespace Configuration;

internal record Line;

internal record Section(string Name) : Line;

internal record Parameter(string Key, string Value) : Line;

internal record Comment(string Text) : Line;

internal static class Parser
{
    private static readonly Regex SectionRegex = new(@"^(\s*\[\s*)([^\]\s]+)(\s*\]\s*)$", RegexOptions.Compiled);
    private static readonly Regex ParameterRegex = new(@"^(\s*)(\S+?)(\s*=\s*)(.*?)(\s*)$", RegexOptions.Compiled);
    private static readonly Regex CommentRegex = new(@"^(\s*#\s*)(.*)", RegexOptions.Compiled);
    private static readonly Regex EmptySpaceRegex = new(@"^\s*$", RegexOptions.Compiled);

    private static Option<string[]> ParseRegex(Regex regex, string input)
    {
        var match = regex.Match(input);

        if (!match.Success)
        {
            return None;
        }

        return Some(match.Groups.Cast<Group>().Select(group => group.Value).ToArray());
    }

    private static Option<string> ParseSection(string text)
    {
        return ParseRegex(SectionRegex, text).Map(values => values[2]);
    }

    private static Option<(string, string)> ParseParameter(string text)
    {
        return ParseRegex(ParameterRegex, text).Map(values => (values[2], values[4]));
    }

    private static Option<string> ParseComment(string text)
    {
        return ParseRegex(CommentRegex, text).Map(values => values[2]);
    }

    private static Option<string> ParseEmptySpace(string text)
    {
        return ParseRegex(EmptySpaceRegex, text).Map(_ => "");
    }

    public static Result<Line, string> ParseLine(string text)
    {
        return ParseSection(text)
            .Match(name => Option<Line>.Some(new Section(name)), () => ParseParameter(text)
                .Match(param => Option<Line>.Some(new Parameter(param.Item1, param.Item2)), () => ParseComment(text)
                    .Match(comm => Option<Line>.Some(new Comment(comm)), () => ParseEmptySpace(text)
                        .Match(_ => Option<Line>.Some(new Line()), Option<Line>.None))))
            .ToResult(() => "Cannot parse line '" + text + "'");
    }
}
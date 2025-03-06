namespace Configuration;

public static class IniExtensions
{
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

    public static Option<Section> GetNested(this Ini ini, string section)
    {
        return EnumerateForward(section).Select(segment => ini[segment]).Aggregate(SomeSection(ini), (current, next) => current.Bind(cur => next.IsSome ? next.Map(cur.Merge) : current));
    }

    public static Option<string> GetNested(this Ini ini, string section, string key)
    {
        return EnumerateBackward(section).Select(segment => ini[segment].Bind(sec => sec[key])).FirstOrDefault(opt => opt.IsSome);
    }

    private static Option<Section> SomeSection(Ini ini)
    {
        return Some(new Section(ini.Comparer));
    }

    private static IEnumerable<string> EnumerateForward(string section)
    {
        var index = -1;

        while ((index = section.IndexOf('.', index + 1)) > 0)
        {
            yield return section[..index];
        }

        yield return section;
    }

    private static IEnumerable<string> EnumerateBackward(string section)
    {
        int index;

        yield return section;

        while ((index = section.LastIndexOf('.')) > 0)
        {
            yield return section = section[..index];
        }
    }
}
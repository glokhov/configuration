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
}
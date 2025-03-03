namespace Configuration;

public static class ConfigurationExtensions
{
    public static Result<Unit, string> WriteTo(this Configuration configuration, FileInfo fileInfo)
    {
        try
        {
            using var textWriter = fileInfo.CreateText();
            return configuration.WriteTo(textWriter);
        }
        catch (Exception exception)
        {
            return Err(exception.Message);
        }
    }

    public static Result<Unit, string> WriteTo(this Configuration configuration, TextWriter textWriter)
    {
        try
        {
            textWriter.Write(configuration.ToString());
            textWriter.Flush();
        }
        catch (Exception exception)
        {
            return Err(exception.Message);
        }

        return Ok(Unit.Default);
    }
}
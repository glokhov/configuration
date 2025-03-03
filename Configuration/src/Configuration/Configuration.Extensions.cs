namespace Configuration;

public static class ConfigurationExtensions
{
    public static Result<Unit, string> SaveToFile(this Configuration configuration, string path)
    {
        try
        {
            File.WriteAllText(path, configuration.ToString());
        }
        catch (Exception exception)
        {
            return Err(exception.Message);
        }

        return Ok(Unit.Default);
    }
}
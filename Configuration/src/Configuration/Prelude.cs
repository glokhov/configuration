namespace Configuration;

public static class Prelude
{
    public static Result<Configuration, string> Ini(string path)
    {
        try
        {
            return File.Exists(path) ? Configuration.Parse(File.ReadAllText(path)) : Err($"File does not exist: {path}.");
        }
        catch (Exception exception)
        {
            return Err(exception.Message);
        }
    }

    public static Result<Configuration, string> Ini(string path, StringComparer comparer)
    {
        try
        {
            return File.Exists(path) ? Configuration.Parse(File.ReadAllText(path), comparer) : Err($"File does not exist: {path}.");
        }
        catch (Exception exception)
        {
            return Err(exception.Message);
        }
    }
}
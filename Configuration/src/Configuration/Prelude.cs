namespace Configuration;

public static class Prelude
{
    public static Result<Configuration, string> Ini(FileInfo fileInfo)
    {
        try
        {
            return fileInfo.Exists ? Ini(fileInfo.OpenText()) : Err($"File does not exist: {fileInfo}.");
        }
        catch (Exception exception)
        {
            return Err(exception.Message);
        }
    }

    public static Result<Configuration, string> Ini(FileInfo fileInfo, IEqualityComparer<string> comparer)
    {
        try
        {
            return fileInfo.Exists ? Ini(fileInfo.OpenText(), comparer) : Err($"File does not exist: {fileInfo}.");
        }
        catch (Exception exception)
        {
            return Err(exception.Message);
        }
    }

    public static Result<Configuration, string> Ini(TextReader textReader)
    {
        try
        {
            return Ini(textReader.ReadToEnd());
        }
        catch (Exception exception)
        {
            return Err(exception.Message);
        }
        finally
        {
            textReader.Close();
        }
    }

    public static Result<Configuration, string> Ini(TextReader textReader, IEqualityComparer<string> comparer)
    {
        try
        {
            return Ini(textReader.ReadToEnd(), comparer);
        }
        catch (Exception exception)
        {
            return Err(exception.Message);
        }
        finally
        {
            textReader.Close();
        }
    }

    public static Result<Configuration, string> Ini(string input)
    {
        return Configuration.Parse(input);
    }

    public static Result<Configuration, string> Ini(string input, IEqualityComparer<string> comparer)
    {
        return Configuration.Parse(input, comparer);
    }
}
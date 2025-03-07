namespace Configuration;

/// <summary>
/// Static functions.
/// </summary>
public static class Prelude
{
    /// <summary>
    /// Parses the text representing of an ini configuration into an instance of the type <c>Ini</c>.
    /// </summary>
    /// <param name="fileInfo">The <c>FileInfo</c> instance.</param>
    /// <returns>The <c>Ini</c> representation of an ini configuration.</returns>
    public static Result<Ini, string> Ini(FileInfo fileInfo)
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

    /// <summary>
    /// Parses the text representing of an ini configuration into an instance of the type <c>Ini</c>.
    /// </summary>
    /// <param name="fileInfo">The <c>FileInfo</c> instance.</param>
    /// <param name="comparer">The IEqualityComparer&lt;T&gt; implementation to use when comparing keys.</param>
    /// <returns>The <c>Ini</c> representation of an ini configuration.</returns>
    public static Result<Ini, string> Ini(FileInfo fileInfo, IEqualityComparer<string> comparer)
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

    /// <summary>
    /// Parses the text representing of an ini configuration into an instance of the type <c>Ini</c>.
    /// </summary>
    /// <param name="textReader">The <c>TextReader</c> instance.</param>
    /// <returns>The <c>Ini</c> representation of an ini configuration.</returns>
    public static Result<Ini, string> Ini(TextReader textReader)
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

    /// <summary>
    /// Parses the text representing of an ini configuration into an instance of the type <c>Ini</c>.
    /// </summary>
    /// <param name="textReader">The <c>TextReader</c> instance.</param>
    /// <param name="comparer">The IEqualityComparer&lt;T&gt; implementation to use when comparing keys.</param>
    /// <returns>The <c>Ini</c> representation of an ini configuration.</returns>
    public static Result<Ini, string> Ini(TextReader textReader, IEqualityComparer<string> comparer)
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

    /// <summary>
    /// Parses the text representing of an ini configuration into an instance of the type <c>Ini</c>.
    /// </summary>
    /// <param name="input">The text representation of an ini configuration.</param>
    /// <returns>The <c>Ini</c> representation of an ini configuration.</returns>
    public static Result<Ini, string> Ini(string input)
    {
        return Configuration.Ini.Parse(input);
    }

    /// <summary>
    /// Parses the text representing of an ini configuration into an instance of the type <c>Ini</c>.
    /// </summary>
    /// <param name="input">The text representation of an ini configuration.</param>
    /// <param name="comparer">The IEqualityComparer&lt;T&gt; implementation to use when comparing keys.</param>
    /// <returns>The <c>Ini</c> representation of an ini configuration.</returns>
    public static Result<Ini, string> Ini(string input, IEqualityComparer<string> comparer)
    {
        return Configuration.Ini.Parse(input, comparer);
    }
}
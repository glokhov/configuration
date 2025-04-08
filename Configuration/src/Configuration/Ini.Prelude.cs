namespace Configuration;

/// <summary>
/// Static methods.
/// </summary>
public static class Prelude
{
    /// <summary>
    /// Initializes new <c>Ini</c> from a file.
    /// </summary>
    /// <param name="path">The full name of a configuration file.</param>
    /// <returns><c>Ok(Ini)</c> if successful; otherwise, <c>Err(string)</c>.</returns>
    public static Result<Ini, string> FromFile(string path)
    {
        return Ini.Empty.AppendFromFile(path);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a file.
    /// </summary>
    /// <param name="file">The <c>FileInfo</c> of a configuration file.</param>
    /// <returns><c>Ok(Ini)</c> if successful; otherwise, <c>Err(string)</c>.</returns>
    public static Result<Ini, string> FromFile(FileInfo file)
    {
        return Ini.Empty.AppendFromFile(file);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a <c>TextReader</c>.
    /// </summary>
    /// <param name="reader">The <c>TextReader</c> of a configuration file.</param>
    /// <returns><c>Ok(Ini)</c> if successful; otherwise, <c>Err(string)</c>.</returns>
    public static Result<Ini, string> FromReader(TextReader reader)
    {
        return Ini.Empty.AppendFromReader(reader);
    }
}
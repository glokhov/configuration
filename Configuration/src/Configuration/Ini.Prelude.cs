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
        return Configuration.Ini.Empty.AppendFromFile(path);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a file.
    /// </summary>
    /// <param name="path">The full name of a configuration file.</param>
    /// <param name="comparer">
    /// The IEqualityComparer&lt;string&gt; that is used to determine equality of section names and keys.
    /// </param>
    /// <returns><c>Ok(Ini)</c> if successful; otherwise, <c>Err(string)</c>.</returns>
    public static Result<Ini, string> FromFile(string path, IEqualityComparer<string> comparer)
    {
        return new Ini(comparer).AppendFromFile(path);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a file.
    /// </summary>
    /// <param name="file">The <c>FileInfo</c> of a configuration file.</param>
    /// <returns><c>Ok(Ini)</c> if successful; otherwise, <c>Err(string)</c>.</returns>
    public static Result<Ini, string> FromFile(FileInfo file)
    {
        return Configuration.Ini.Empty.AppendFromFile(file);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a file.
    /// </summary>
    /// <param name="file">The <c>FileInfo</c> of a configuration file.</param>
    /// <param name="comparer">
    /// The IEqualityComparer&lt;string&gt; that is used to determine equality of section names and keys.
    /// </param>
    /// <returns><c>Ok(Ini)</c> if successful; otherwise, <c>Err(string)</c>.</returns>
    public static Result<Ini, string> FromFile(FileInfo file, IEqualityComparer<string> comparer)
    {
        return new Ini(comparer).AppendFromFile(file);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a string.
    /// </summary>
    /// <param name="text">The string representation of a configuration file.</param>
    /// <returns><c>Ok(Ini)</c> if successful; otherwise, <c>Err(string)</c>.</returns>
    public static Result<Ini, string> FromString(string text)
    {
        return Configuration.Ini.Empty.AppendFromString(text);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a string.
    /// </summary>
    /// <param name="text">The string representation of a configuration file.</param>
    /// <param name="comparer">
    /// The IEqualityComparer&lt;string&gt; that is used to determine equality of section names and keys.
    /// </param>
    /// <returns><c>Ok(Ini)</c> if successful; otherwise, <c>Err(string)</c>.</returns>
    public static Result<Ini, string> FromString(string text, IEqualityComparer<string> comparer)
    {
        return new Ini(comparer).AppendFromString(text);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a <c>TextReader</c>.
    /// </summary>
    /// <param name="reader">The <c>TextReader</c> of a configuration file.</param>
    /// <returns><c>Ok(Ini)</c> if successful; otherwise, <c>Err(string)</c>.</returns>
    public static Result<Ini, string> FromReader(TextReader reader)
    {
        return Configuration.Ini.Empty.AppendFromReader(reader);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a <c>TextReader</c>.
    /// </summary>
    /// <param name="reader">The <c>TextReader</c> of a configuration file.</param>
    /// <param name="comparer">
    /// The IEqualityComparer&lt;string&gt; that is used to determine equality of section names and keys.
    /// </param>
    /// <returns><c>Ok(Ini)</c> if successful; otherwise, <c>Err(string)</c>.</returns>
    public static Result<Ini, string> FromReader(TextReader reader, IEqualityComparer<string> comparer)
    {
        return new Ini(comparer).AppendFromReader(reader);
    }

    #region Obsolete

    /// <summary>
    /// Initializes new <c>Ini</c> from a file.
    /// </summary>
    [Obsolete("Use FromFile instead. This method will be removed in future versions.")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static Result<Ini, string> Ini(FileInfo fileInfo)
    {
        return FromFile(fileInfo);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a file.
    /// </summary>
    [Obsolete("Use FromFile instead. This method will be removed in future versions.")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static Result<Ini, string> Ini(FileInfo fileInfo, IEqualityComparer<string> comparer)
    {
        return FromFile(fileInfo, comparer);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a <c>TextReader</c>.
    /// </summary>
    [Obsolete("Use FromReader instead. This method will be removed in future versions.")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static Result<Ini, string> Ini(TextReader textReader)
    {
        return FromReader(textReader);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a <c>TextReader</c>.
    /// </summary>
    [Obsolete("Use FromReader instead. This method will be removed in future versions.")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static Result<Ini, string> Ini(TextReader textReader, IEqualityComparer<string> comparer)
    {
        return FromReader(textReader, comparer);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a string.
    /// </summary>
    [Obsolete("Use FromString instead. This method will be removed in future versions.")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static Result<Ini, string> Ini(string input)
    {
        return FromString(input);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a string.
    /// </summary>
    [Obsolete("Use FromString instead. This method will be removed in future versions.")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static Result<Ini, string> Ini(string input, IEqualityComparer<string> comparer)
    {
        return FromString(input, comparer);
    }

    #endregion
}
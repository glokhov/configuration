namespace Configuration;

/// <summary>
/// Static methods.
/// </summary>
public static class Prelude
{
    /// <summary>
    /// Initializes new <c>Ini</c> from a file.
    /// </summary>
    [Obsolete("Use Configuration.Ini.FromFile(FileInfo) instead. This method will be removed in future versions.")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static Result<Ini, string> Ini(FileInfo fileInfo)
    {
        return Configuration.Ini.FromFile(fileInfo);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a file.
    /// </summary>
    [Obsolete("Use Configuration.Ini.FromFile(FileInfo, IEqualityComparer<string>) instead. This method will be removed in future versions.")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static Result<Ini, string> Ini(FileInfo fileInfo, IEqualityComparer<string> comparer)
    {
        return Configuration.Ini.FromFile(fileInfo, comparer);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a <c>TextReader</c>.
    /// </summary>
    [Obsolete("Use Configuration.Ini.FromReader(TextReader) instead. This method will be removed in future versions.")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static Result<Ini, string> Ini(TextReader textReader)
    {
        return Configuration.Ini.FromReader(textReader);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a <c>TextReader</c>.
    /// </summary>
    [Obsolete("Use Configuration.Ini.FromReader(TextReader, IEqualityComparer<string>) instead. This method will be removed in future versions.")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static Result<Ini, string> Ini(TextReader textReader, IEqualityComparer<string> comparer)
    {
        return Configuration.Ini.FromReader(textReader, comparer);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a string.
    /// </summary>
    [Obsolete("Use Configuration.Ini.FromString(string) instead. This method will be removed in future versions.")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static Result<Ini, string> Ini(string input)
    {
        return Configuration.Ini.FromString(input);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a string.
    /// </summary>
    [Obsolete("Use Configuration.Ini.FromString(string, IEqualityComparer<string>) instead. This method will be removed in future versions.")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static Result<Ini, string> Ini(string input, IEqualityComparer<string> comparer)
    {
        return Configuration.Ini.FromString(input, comparer);
    }
}
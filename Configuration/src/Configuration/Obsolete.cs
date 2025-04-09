namespace Configuration;

/// <summary>
/// Static methods.
/// </summary>
public static class IniExtensions
{
    /// <summary>
    /// Writes the text representing of an ini configuration to the file.
    /// </summary>
    [Obsolete("Use Configuration.Ini.ToFile(fileInfo) instead. This method will be removed in future versions.")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static Result<Unit, string> WriteTo(this Ini ini, FileInfo fileInfo)
    {
        return ini.ToFile(fileInfo);
    }

    /// <summary>
    /// Writes the text representing of an ini configuration to the text stream.
    /// </summary>
    [Obsolete("Use Configuration.Ini.ToWriter(textWriter) instead. This method will be removed in future versions.")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static Result<Unit, string> WriteTo(this Ini ini, TextWriter textWriter)
    {
        return ini.ToWriter(textWriter);
    }
}

/// <summary>
/// Static methods.
/// </summary>
public static class Prelude
{
    /// <summary>
    /// Initializes new <c>Ini</c> from a file.
    /// </summary>
    [Obsolete("Use Configuration.Ini.FromFile(fileInfo) instead. This method will be removed in future versions.")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static Result<Ini, string> Ini(FileInfo fileInfo)
    {
        return Configuration.Ini.FromFile(fileInfo);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a file.
    /// </summary>
    [Obsolete("Use Configuration.Ini.FromFile(fileInfo, comparer) instead. This method will be removed in future versions.")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static Result<Ini, string> Ini(FileInfo fileInfo, IEqualityComparer<string> comparer)
    {
        return Configuration.Ini.FromFile(fileInfo, comparer);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a <c>TextReader</c>.
    /// </summary>
    [Obsolete("Use Configuration.Ini.FromReader(textReader) instead. This method will be removed in future versions.")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static Result<Ini, string> Ini(TextReader textReader)
    {
        return Configuration.Ini.FromReader(textReader);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a <c>TextReader</c>.
    /// </summary>
    [Obsolete("Use Configuration.Ini.FromReader(textReader, comparer) instead. This method will be removed in future versions.")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static Result<Ini, string> Ini(TextReader textReader, IEqualityComparer<string> comparer)
    {
        return Configuration.Ini.FromReader(textReader, comparer);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a string.
    /// </summary>
    [Obsolete("Use Configuration.Ini.FromString(input) instead. This method will be removed in future versions.")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static Result<Ini, string> Ini(string input)
    {
        return Configuration.Ini.FromString(input);
    }

    /// <summary>
    /// Initializes new <c>Ini</c> from a string.
    /// </summary>
    [Obsolete("Use Configuration.Ini.FromString(input, comparer) instead. This method will be removed in future versions.")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static Result<Ini, string> Ini(string input, IEqualityComparer<string> comparer)
    {
        return Configuration.Ini.FromString(input, comparer);
    }
}
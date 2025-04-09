namespace Configuration;

/// <summary>
/// Extension methods.
/// </summary>
public static class Writer
{
    /// <summary>
    /// Writes elements of the <c>Ini</c> configuration to a file.
    /// </summary>
    /// <param name="ini">The <c>Ini</c> configuration.</param>
    /// <param name="path">The full name of a configuration file.</param>
    /// <returns><c>Ok(Unit)</c> if successful; otherwise, <c>Err(string)</c>.</returns>
    public static Result<Unit, string> ToFile(this Ini ini, string path)
    {
        try
        {
            return ini.ToFile(new FileInfo(path));
        }
        catch (Exception e)
        {
            return Err(e.Message);
        }
    }

    /// <summary>
    /// Writes elements of the <c>Ini</c> configuration to a file.
    /// </summary>
    /// <param name="ini">The <c>Ini</c> configuration.</param>
    /// <param name="file">The <c>FileInfo</c> of a configuration file.</param>
    /// <returns><c>Ok(Unit)</c> if successful; otherwise, <c>Err(string)</c>.</returns>
    public static Result<Unit, string> ToFile(this Ini ini, FileInfo file)
    {
        try
        {
            using var writer = file.CreateText();
            return ini.ToWriter(writer);
        }
        catch (Exception e)
        {
            return Err(e.Message);
        }
    }

    /// <summary>
    /// Writes elements of the <c>Ini</c> configuration to a <c>TextWriter</c>.
    /// </summary>
    /// <param name="ini">The <c>Ini</c> configuration.</param>
    /// <param name="writer">The <c>TextWriter</c> of a configuration file.</param>
    /// <returns><c>Ok(Unit)</c> if successful; otherwise, <c>Err(string)</c>.</returns>
    public static Result<Unit, string> ToWriter(this Ini ini, TextWriter writer)
    {
        try
        {
            var current = Ini.Global;
            var fist = true;

            foreach (var (section, key, value) in ini)
            {
                if (current != section)
                {
                    if (!fist) writer.WriteLine();
                    writer.Write('[');
                    writer.Write(section);
                    writer.Write(']');
                    writer.WriteLine();
                    writer.WriteLine();
                }

                writer.Write(key.PadRight(MaxKeyLength(section)));
                writer.Write(" = ");
                writer.Write(value);
                writer.WriteLine();

                current = section;
                fist = false;
            }

            writer.Flush();

            return Ok(Unit.Default);

            int MaxKeyLength(string section)
            {
                return ini.GetSection(section).Keys.Max(key => key.Length);
            }
        }
        catch (Exception e)
        {
            return Err(e.Message);
        }
    }
}
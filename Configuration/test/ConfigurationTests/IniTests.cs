namespace ConfigurationTests;

public sealed class IniTests
{
    private static readonly string A = "[default]" + Environment.NewLine;

    private static readonly string B = "A = aaa" + Environment.NewLine +
                                       "B = bbb" + Environment.NewLine;

    private static readonly string C = "[foo] # foo" + Environment.NewLine +
                                       "C = ccc" + Environment.NewLine +
                                       "D = ddd" + Environment.NewLine;

    private static readonly string D = "[bar]" + Environment.NewLine +
                                       "E = eee" + Environment.NewLine +
                                       "F = fff # fff";

    private static readonly string X = "xxx" + Environment.NewLine;

    // ReSharper disable once InconsistentNaming
    private static readonly string ABD = "[default]" + Environment.NewLine +
                                         "" + Environment.NewLine +
                                         "A = aaa" + Environment.NewLine +
                                         "B = bbb" + Environment.NewLine +
                                         "" + Environment.NewLine +
                                         "[bar]" + Environment.NewLine +
                                         "" + Environment.NewLine +
                                         "E = eee" + Environment.NewLine +
                                         "F = fff" + Environment.NewLine;

    // ReSharper disable once InconsistentNaming
    private static readonly string BC = "[default]" + Environment.NewLine +
                                        "" + Environment.NewLine +
                                        "A = aaa" + Environment.NewLine +
                                        "B = bbb" + Environment.NewLine +
                                        "" + Environment.NewLine +
                                        "[foo]" + Environment.NewLine +
                                        "" + Environment.NewLine +
                                        "C = ccc" + Environment.NewLine +
                                        "D = ddd" + Environment.NewLine;

    [Fact]
    public void Default_Ctor()
    {
        var ini = new Ini();

        Assert.Empty(ini.Config);
        Assert.Equal("StringEqualityComparer", ini.Comparer.GetType().Name);
    }

    [Fact]
    public void Comparer_Ctor()
    {
        var ini = new Ini(StringComparer.Ordinal);

        Assert.Empty(ini.Config);
        Assert.Equal("OrdinalCaseSensitiveComparer", ini.Comparer.GetType().Name);
    }

    [Fact]
    public void Parse_Empty_String_Returns_Empty_Config()
    {
        Assert.Equal("", Configuration.Ini.Parse("").Pure().Value.ToString());
    }

    [Fact]
    public void Parse_Calls_Default_Configuration_Ctor()
    {
        var configuration = Configuration.Ini.Parse(C).Pure().Value;

        Assert.True(configuration["FOO"].IsNone);
        Assert.True(configuration["foo", "c"].IsNone);
    }

    [Fact]
    public void Parse_Calls_Comparer_Configuration_Ctor()
    {
        var ini = Configuration.Ini.Parse(C, StringComparer.OrdinalIgnoreCase).Pure().Value;

        Assert.True(ini["FOO"].IsSome);
        Assert.True(ini["foo", "c"].IsSome);
    }

    [Fact]
    public void Parse_Returns_Error_If_Cannot_Parse_Line()
    {
        Assert.Equal("Cannot parse line 2: xxx.", Configuration.Ini.Parse(A + X).Fail().Error);
    }

    [Fact]
    public void Parse_No_Eol_At_End()
    {
        Assert.Equal($"{ABD}", Configuration.Ini.Parse(A + B + D).Pure().Value.ToString());
    }

    [Fact]
    public void Parse_No_Section_At_Begin()
    {
        Assert.Equal($"{BC}", Configuration.Ini.Parse(B + C).Pure().Value.ToString());
    }

    [Fact]
    public void WriteTo_Writes_To_File_And_Returns_Ok()
    {
        var temp = Path.GetTempFileName();

        var result = Configuration.Ini.Parse(ABD).Pure().Value.WriteTo(new FileInfo(temp));

        try
        {
            Assert.True(result.IsOk);
            Assert.Equal(ABD, File.ReadAllText(temp));
        }
        finally
        {
            File.Delete(temp);
        }
    }

    [Fact]
    public void WriteTo_Returns_Error_If_Cannot_Write_To_File()
    {
        var temp = Path.GetTempPath();

        var error = Configuration.Ini.Parse(ABD).Pure().Value.WriteTo(new FileInfo(temp)).Fail().Error;

        Assert.Contains(temp, error);
    }

    [Fact]
    public void WriteTo_Writes_To_TextWriter_And_Returns_Ok()
    {
        var stringWriter = new StringWriter();

        var result = Configuration.Ini.Parse(ABD).Pure().Value.WriteTo(stringWriter);

        Assert.True(result.IsOk);
        Assert.Equal(ABD, stringWriter.ToString());
    }

    [Fact]
    public void WriteTo_Returns_Error_If_Cannot_Write_To_TextWriter()
    {
        var stringWriter = new StringWriter();
        stringWriter.Close();

        var error = Configuration.Ini.Parse(ABD).Pure().Value.WriteTo(stringWriter).Fail().Error;

        Assert.Contains("Cannot write to a closed TextWriter.", error);
    }
}
namespace ConfigurationTests;

public sealed class ConfigurationTests
{
    private static readonly string A = "[default]" + Environment.NewLine;

    private static readonly string B = "A = aaa" + Environment.NewLine +
                                       "B = bbb" + Environment.NewLine;

    private static readonly string C = "[foo]" + Environment.NewLine +
                                       "C = ccc" + Environment.NewLine +
                                       "D = ddd" + Environment.NewLine;

    private static readonly string D = "[bar]" + Environment.NewLine +
                                       "E = eee" + Environment.NewLine +
                                       "F = fff";

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
    public void Parse_Empty_String_Returns_Empty_Config()
    {
        Assert.Equal("Ok()", Configuration.Configuration.Parse("").ToString());
    }

    [Fact]
    public void Parse_Calls_Default_Configuration_Ctor()
    {
        var configuration = Configuration.Configuration.Parse(C).Match(ok => ok, _ => throw new Exception());

        Assert.True(configuration["FOO"].IsNone);
        Assert.True(configuration["foo", "c"].IsNone);
    }

    [Fact]
    public void Parse_Calls_Comparer_Configuration_Ctor()
    {
        var configuration = Configuration.Configuration.Parse(C, StringComparer.OrdinalIgnoreCase).Match(ok => ok, _ => throw new Exception());

        Assert.True(configuration["FOO"].IsSome);
        Assert.True(configuration["foo", "c"].IsSome);
    }

    [Fact]
    public void Parse_Returns_Error_If_Cannot_Parse_Line()
    {
        Assert.Equal("Err(Cannot parse line 2: xxx.)", Configuration.Configuration.Parse(A + X).ToString());
    }

    [Fact]
    public void Parse_No_Eol_At_End()
    {
        Assert.Equal($"Ok({ABD})", Configuration.Configuration.Parse(A + B + D).ToString());
    }

    [Fact]
    public void Parse_No_Section_At_Begin()
    {
        Assert.Equal($"Ok({BC})", Configuration.Configuration.Parse(B + C).ToString());
    }
}
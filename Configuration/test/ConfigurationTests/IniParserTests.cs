namespace ConfigurationTests;

public partial class IniTests
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
    public void Parse_Empty_String_Returns_Empty_Config()
    {
        Assert.Equal("", Configuration.Ini.Parse("").Unwrap().ToString());
    }

    [Fact]
    public void Parse_Calls_Default_Configuration_Ctor()
    {
        var configuration = Configuration.Ini.Parse(C).Unwrap();

        Assert.True(configuration["FOO"].IsNone);
        Assert.True(configuration["foo", "c"].IsNone);
    }

    [Fact]
    public void Parse_Calls_Comparer_Configuration_Ctor()
    {
        var ini = Configuration.Ini.Parse(C, StringComparer.OrdinalIgnoreCase).Unwrap();

        Assert.True(ini["FOO"].IsSome);
        Assert.True(ini["foo", "c"].IsSome);
    }

    [Fact]
    public void Parse_Returns_Error_If_Cannot_Parse_Line()
    {
        Assert.Equal("Cannot parse line 2: xxx.", Configuration.Ini.Parse(A + X).ExpectError());
    }

    [Fact]
    public void Parse_No_Eol_At_End()
    {
        Assert.Equal($"{ABD}", Configuration.Ini.Parse(A + B + D).Unwrap().ToString());
    }

    [Fact]
    public void Parse_No_Section_At_Begin()
    {
        Assert.Equal($"{BC}", Configuration.Ini.Parse(B + C).Unwrap().ToString());
    }
}
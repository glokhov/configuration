namespace ConfigurationTests;

public sealed class PreludeTests
{
    private static readonly string Input = "[default]" + Environment.NewLine +
                                           "" + Environment.NewLine +
                                           "A = foo" + Environment.NewLine +
                                           "B = bar" + Environment.NewLine;

    [Fact]
    public static void Ini_Returns_Error_If_File_Does_Not_Exist()
    {
        Assert.Equal("File does not exist: path.", Ini(new FileInfo("path")).Fail().Error);
    }

    [Fact]
    public static void Ini_Comparer_Returns_Error_If_File_Does_Not_Exist()
    {
        Assert.Equal("File does not exist: path.", Ini(new FileInfo("path"), StringComparer.Ordinal).Fail().Error);
    }

    [Fact]
    public static void Ini_FileInfo_Calls_Parse_Path()
    {
        var temp = Path.GetTempFileName();

        File.WriteAllText(temp, Input);

        var ini = Ini(new FileInfo(temp)).Pure().Value;
        var foo = ini["default", "A"].Pure().Value;
        var bar = ini["default", "B"].Pure().Value;

        try
        {
            Assert.Equal("foo", foo);
            Assert.Equal("bar", bar);
        }
        finally
        {
            File.Delete(temp);
        }
    }

    [Fact]
    public static void Ini_FileInfo_Comparer_Calls_Parse_Path_Comparer()
    {
        var temp = Path.GetTempFileName();

        File.WriteAllText(temp, Input);

        var ini = Ini(new FileInfo(temp), StringComparer.OrdinalIgnoreCase).Pure().Value;
        var foo = ini["DEFAULT", "a"].Pure().Value;
        var bar = ini["DEFAULT", "b"].Pure().Value;

        try
        {
            Assert.Equal("foo", foo);
            Assert.Equal("bar", bar);
        }
        finally
        {
            File.Delete(temp);
        }
    }

    [Fact]
    public static void Ini_FileInfo_Returns_Error_If_Exception_Is_Thrown()
    {
        var temp = Path.GetTempFileName();
        var file = File.Open(temp, FileMode.Open);

        var error = Ini(new FileInfo(temp)).Fail().Error;

        try
        {
            Assert.Contains(temp, error);
        }
        finally
        {
            file.Close();
            File.Delete(temp);
        }
    }

    [Fact]
    public static void Ini_FileInfo_Comparer_Returns_Error_If_Exception_Is_Thrown()
    {
        var temp = Path.GetTempFileName();
        var file = File.Open(temp, FileMode.Open);

        var error = Ini(new FileInfo(temp), StringComparer.Ordinal).Fail().Error;

        try
        {
            Assert.Contains(temp, error);
        }
        finally
        {
            file.Close();
            File.Delete(temp);
        }
    }

    [Fact]
    public static void Ini_TextReader_Calls_Parse_Path()
    {
        var ini = Ini(new StringReader(Input)).Pure().Value;
        var foo = ini["default", "A"].Pure().Value;
        var bar = ini["default", "B"].Pure().Value;

        Assert.Equal("foo", foo);
        Assert.Equal("bar", bar);
    }

    [Fact]
    public static void Ini_TextReader_Comparer_Calls_Parse_Path_Comparer()
    {
        var ini = Ini(new StringReader(Input), StringComparer.OrdinalIgnoreCase).Pure().Value;
        var foo = ini["DEFAULT", "a"].Pure().Value;
        var bar = ini["DEFAULT", "b"].Pure().Value;

        Assert.Equal("foo", foo);
        Assert.Equal("bar", bar);
    }

    [Fact]
    public static void Ini_TextReader_Returns_Error_If_Exception_Is_Thrown()
    {
        var stringReader = new StringReader(Input);
        stringReader.Close();

        var error = Ini(stringReader).Fail().Error;

        Assert.Contains("Cannot read from a closed TextReader.", error);
    }

    [Fact]
    public static void Ini_TextReader_Comparer_Returns_Error_If_Exception_Is_Thrown()
    {
        var stringReader = new StringReader(Input);
        stringReader.Close();

        var error = Ini(stringReader, StringComparer.Ordinal).Fail().Error;

        Assert.Contains("Cannot read from a closed TextReader.", error);
    }

    [Fact]
    public static void Ini_Input_Calls_Parse_Path()
    {
        var ini = Ini(Input).Pure().Value;
        var foo = ini["default", "A"].Pure().Value;
        var bar = ini["default", "B"].Pure().Value;

        Assert.Equal("foo", foo);
        Assert.Equal("bar", bar);
    }

    [Fact]
    public static void Ini_Input_Comparer_Calls_Parse_Path_Comparer()
    {
        var ini = Ini(Input, StringComparer.OrdinalIgnoreCase).Pure().Value;
        var foo = ini["DEFAULT", "a"].Pure().Value;
        var bar = ini["DEFAULT", "b"].Pure().Value;

        Assert.Equal("foo", foo);
        Assert.Equal("bar", bar);
    }
}
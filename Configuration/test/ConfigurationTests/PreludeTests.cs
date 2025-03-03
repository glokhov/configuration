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
        Assert.Equal("File does not exist: path.", Ini(new FileInfo("path")).Match(_ => throw new Exception(), err => err));
    }

    [Fact]
    public static void Ini_Comparer_Returns_Error_If_File_Does_Not_Exist()
    {
        Assert.Equal("File does not exist: path.", Ini(new FileInfo("path"), StringComparer.Ordinal).Match(_ => throw new Exception(), err => err));
    }

    [Fact]
    public static void Ini_FileInfo_Calls_Parse_Path()
    {
        var temp = Path.GetTempFileName();

        File.WriteAllText(temp, Input);

        var ini = Ini(new FileInfo(temp)).Match(ok => ok, _ => throw new Exception());
        var foo = ini["default", "A"].Match(some => some, () => throw new Exception());
        var bar = ini["default", "B"].Match(some => some, () => throw new Exception());

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

        var ini = Ini(new FileInfo(temp), StringComparer.OrdinalIgnoreCase).Match(ok => ok, _ => throw new Exception());
        var foo = ini["DEFAULT", "a"].Match(some => some, () => throw new Exception());
        var bar = ini["DEFAULT", "b"].Match(some => some, () => throw new Exception());

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

        var error = Ini(new FileInfo(temp)).Match(_ => throw new Exception(), err => err);

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

        var error = Ini(new FileInfo(temp), StringComparer.Ordinal).Match(_ => throw new Exception(), err => err);

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
        var ini = Ini(new StringReader(Input)).Match(ok => ok, _ => throw new Exception());
        var foo = ini["default", "A"].Match(some => some, () => throw new Exception());
        var bar = ini["default", "B"].Match(some => some, () => throw new Exception());

        Assert.Equal("foo", foo);
        Assert.Equal("bar", bar);
    }

    [Fact]
    public static void Ini_TextReader_Comparer_Calls_Parse_Path_Comparer()
    {
        var ini = Ini(new StringReader(Input), StringComparer.OrdinalIgnoreCase).Match(ok => ok, _ => throw new Exception());
        var foo = ini["DEFAULT", "a"].Match(some => some, () => throw new Exception());
        var bar = ini["DEFAULT", "b"].Match(some => some, () => throw new Exception());

        Assert.Equal("foo", foo);
        Assert.Equal("bar", bar);
    }

    [Fact]
    public static void Ini_TextReader_Returns_Error_If_Exception_Is_Thrown()
    {
        var stringReader = new StringReader(Input);
        stringReader.Close();

        var error = Ini(stringReader).Match(_ => throw new Exception(), err => err);

        Assert.Contains("Cannot read from a closed TextReader.", error);
    }

    [Fact]
    public static void Ini_TextReader_Comparer_Returns_Error_If_Exception_Is_Thrown()
    {
        var stringReader = new StringReader(Input);
        stringReader.Close();

        var error = Ini(stringReader, StringComparer.Ordinal).Match(_ => throw new Exception(), err => err);

        Assert.Contains("Cannot read from a closed TextReader.", error);
    }

    [Fact]
    public static void Ini_Input_Calls_Parse_Path()
    {
        var ini = Ini(Input).Match(ok => ok, _ => throw new Exception());
        var foo = ini["default", "A"].Match(some => some, () => throw new Exception());
        var bar = ini["default", "B"].Match(some => some, () => throw new Exception());

        Assert.Equal("foo", foo);
        Assert.Equal("bar", bar);
    }

    [Fact]
    public static void Ini_Input_Comparer_Calls_Parse_Path_Comparer()
    {
        var ini = Ini(Input, StringComparer.OrdinalIgnoreCase).Match(ok => ok, _ => throw new Exception());
        var foo = ini["DEFAULT", "a"].Match(some => some, () => throw new Exception());
        var bar = ini["DEFAULT", "b"].Match(some => some, () => throw new Exception());

        Assert.Equal("foo", foo);
        Assert.Equal("bar", bar);
    }
}
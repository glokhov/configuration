namespace ConfigurationTests;

public sealed class PreludeTests
{
    private static readonly string Content = "[default]" + Environment.NewLine +
                                             "" + Environment.NewLine +
                                             "A = foo" + Environment.NewLine +
                                             "B = bar" + Environment.NewLine;

    [Fact]
    public static void Ini_Calls_Parse_Path()
    {
        var temp = Path.GetTempFileName();

        File.WriteAllText(temp, Content);

        var ini = Ini(temp).Match(ok => ok, _ => throw new Exception());
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
    public static void Ini_Comparer_Calls_Parse_Path_Comparer()
    {
        var temp = Path.GetTempFileName();

        File.WriteAllText(temp, Content);

        var ini = Ini(temp, StringComparer.OrdinalIgnoreCase).Match(ok => ok, _ => throw new Exception());
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
    public static void Ini_Returns_Error_If_File_Does_Not_Exist()
    {
        Assert.Equal("File does not exist: path.", Ini("path").Match(_ => throw new Exception(), err => err));
    }

    [Fact]
    public static void Ini_Comparer_Returns_Error_If_File_Does_Not_Exist()
    {
        Assert.Equal("File does not exist: path.", Ini("path", StringComparer.Ordinal).Match(_ => throw new Exception(), err => err));
    }

    [Fact]
    public static void Ini_Returns_Error_If_Exception_Is_Thrown()
    {
        var temp = Path.GetTempFileName();
        var file = File.Open(temp, FileMode.Open);

        var error = Ini(temp).Match(_ => throw new Exception(), err => err);

        try
        {
            Assert.Contains(temp, error);
        }
        finally
        {
            file.Dispose();
            File.Delete(temp);
        }
    }

    [Fact]
    public static void Ini_Comparer_Returns_Error_If_Exception_Is_Thrown()
    {
        var temp = Path.GetTempFileName();
        var file = File.Open(temp, FileMode.Open);

        var error = Ini(temp, StringComparer.Ordinal).Match(_ => throw new Exception(), err => err);

        try
        {
            Assert.Contains(temp, error);
        }
        finally
        {
            file.Dispose();
            File.Delete(temp);
        }
    }
}
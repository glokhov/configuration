using System.Text;

namespace ConfigurationTests;

public sealed class PreludeTests : IDisposable
{
    private static readonly string Input = "[default]" + Environment.NewLine +
                                           "" + Environment.NewLine +
                                           "A = foo" + Environment.NewLine +
                                           "B = bar" + Environment.NewLine;

    private FileInfo? _tempFile;

    private FileInfo CreateTempFile(string? input = null)
    {
        _tempFile = new FileInfo(Path.GetTempFileName());

        if (input == null)
        {
            return _tempFile;
        }

        using var fileStream = _tempFile.OpenWrite();
        fileStream.Write(Encoding.UTF8.GetBytes(input));

        return _tempFile;
    }

    [Fact]
    public void Ini_Returns_Error_If_File_Does_Not_Exist()
    {
        Assert.Equal("File does not exist: path.", Ini(new FileInfo("path")).Fail().Error);
    }

    [Fact]
    public void Ini_Comparer_Returns_Error_If_File_Does_Not_Exist()
    {
        Assert.Equal("File does not exist: path.", Ini(new FileInfo("path"), StringComparer.Ordinal).Fail().Error);
    }

    [Fact]
    public void Ini_FileInfo_Calls_Parse_Path()
    {
        var temp = CreateTempFile(Input);

        var ini = Ini(temp).Pure().Value;
        var foo = ini["default", "A"].Pure().Value;
        var bar = ini["default", "B"].Pure().Value;

        Assert.Equal("foo", foo);
        Assert.Equal("bar", bar);
    }

    [Fact]
    public void Ini_FileInfo_Comparer_Calls_Parse_Path_Comparer()
    {
        var temp = CreateTempFile(Input);

        var ini = Ini(temp, StringComparer.OrdinalIgnoreCase).Pure().Value;
        var foo = ini["DEFAULT", "a"].Pure().Value;
        var bar = ini["DEFAULT", "b"].Pure().Value;

        Assert.Equal("foo", foo);
        Assert.Equal("bar", bar);
    }

    [Fact]
    public void Ini_FileInfo_Returns_Error_If_Exception_Is_Thrown()
    {
        var temp = CreateTempFile();
        var file = temp.Open(FileMode.Open);

        var error = Ini(temp).Fail().Error;

        file.Close();

        Assert.Contains(temp.FullName, error);
    }

    [Fact]
    public void Ini_FileInfo_Comparer_Returns_Error_If_Exception_Is_Thrown()
    {
        var temp = CreateTempFile();
        var file = temp.Open(FileMode.Open);

        var error = Ini(temp, StringComparer.Ordinal).Fail().Error;

        file.Close();

        Assert.Contains(temp.FullName, error);
    }

    [Fact]
    public void Ini_TextReader_Calls_Parse_Path()
    {
        var ini = Ini(new StringReader(Input)).Pure().Value;

        var foo = ini["default", "A"].Pure().Value;
        var bar = ini["default", "B"].Pure().Value;

        Assert.Equal("foo", foo);
        Assert.Equal("bar", bar);
    }

    [Fact]
    public void Ini_TextReader_Comparer_Calls_Parse_Path_Comparer()
    {
        var ini = Ini(new StringReader(Input), StringComparer.OrdinalIgnoreCase).Pure().Value;

        var foo = ini["DEFAULT", "a"].Pure().Value;
        var bar = ini["DEFAULT", "b"].Pure().Value;

        Assert.Equal("foo", foo);
        Assert.Equal("bar", bar);
    }

    [Fact]
    public void Ini_TextReader_Returns_Error_If_Exception_Is_Thrown()
    {
        var stringReader = new StringReader(Input);
        stringReader.Close();

        var error = Ini(stringReader).Fail().Error;

        Assert.Contains("Cannot read from a closed TextReader.", error);
    }

    [Fact]
    public void Ini_TextReader_Comparer_Returns_Error_If_Exception_Is_Thrown()
    {
        var stringReader = new StringReader(Input);
        stringReader.Close();

        var error = Ini(stringReader, StringComparer.Ordinal).Fail().Error;

        Assert.Contains("Cannot read from a closed TextReader.", error);
    }

    [Fact]
    public void Ini_Input_Calls_Parse_Path()
    {
        var ini = Ini(Input).Pure().Value;

        var foo = ini["default", "A"].Pure().Value;
        var bar = ini["default", "B"].Pure().Value;

        Assert.Equal("foo", foo);
        Assert.Equal("bar", bar);
    }

    [Fact]
    public void Ini_Input_Comparer_Calls_Parse_Path_Comparer()
    {
        var ini = Ini(Input, StringComparer.OrdinalIgnoreCase).Pure().Value;

        var foo = ini["DEFAULT", "a"].Pure().Value;
        var bar = ini["DEFAULT", "b"].Pure().Value;

        Assert.Equal("foo", foo);
        Assert.Equal("bar", bar);
    }

    public void Dispose()
    {
        _tempFile?.Delete();
    }
}
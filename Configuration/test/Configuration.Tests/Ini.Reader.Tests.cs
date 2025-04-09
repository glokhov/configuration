using Functional;

namespace Configuration.Tests;

public sealed class ReaderTests : IDisposable
{
    private const string TempContents = "a=b\n[foo]\nc=d\n[bar]\ne=f\n";

    private readonly string _tempFile = Path.GetTempFileName();

    public ReaderTests()
    {
        File.WriteAllText(_tempFile, TempContents);
    }

    public void Dispose()
    {
        File.Delete(_tempFile);
    }

    // extensions

    [Fact]
    public void FromFile_Path()
    {
        var ini = Ini.FromFile(_tempFile).Unwrap();

        Assert.Equal("b", ini["A"].Unwrap());
        Assert.Equal("d", ini["foo", "c"].Unwrap());
        Assert.Equal("f", ini["bar", "e"].Unwrap());
    }

    [Fact]
    public void FromFile_FileInfo()
    {
        var ini = Ini.FromFile(new FileInfo(_tempFile)).Unwrap();

        Assert.Equal("b", ini["A"].Unwrap());
        Assert.Equal("d", ini["foo", "c"].Unwrap());
        Assert.Equal("f", ini["bar", "e"].Unwrap());
    }

    [Fact]
    public void FromString_TextReader()
    {
        var text = File.ReadAllText(_tempFile);

        var ini = Ini.FromString(text).Unwrap();

        Assert.Equal("b", ini["A"].Unwrap());
        Assert.Equal("d", ini["foo", "c"].Unwrap());
        Assert.Equal("f", ini["bar", "e"].Unwrap());
    }

    [Fact]
    public void FromReader_TextReader()
    {
        using var reader = new FileInfo(_tempFile).OpenText();

        var ini = Ini.FromReader(reader).Unwrap();

        Assert.Equal("b", ini["A"].Unwrap());
        Assert.Equal("d", ini["foo", "c"].Unwrap());
        Assert.Equal("f", ini["bar", "e"].Unwrap());
    }

    // prelude comparer

    [Fact]
    public void FromFile_Path_Comparer()
    {
        var ini = Ini.FromFile(_tempFile, EqualityComparer<string>.Default).Unwrap();

        Assert.Equal(Unit.Default, ini["A"].ExpectUnit());
        Assert.Equal("d", ini["foo", "c"].Unwrap());
        Assert.Equal("f", ini["bar", "e"].Unwrap());
    }

    [Fact]
    public void FromFile_FileInfo_Comparer()
    {
        var ini = Ini.FromFile(new FileInfo(_tempFile), EqualityComparer<string>.Default).Unwrap();

        Assert.Equal(Unit.Default, ini["A"].ExpectUnit());
        Assert.Equal("d", ini["foo", "c"].Unwrap());
        Assert.Equal("f", ini["bar", "e"].Unwrap());
    }

    [Fact]
    public void FromString_TextReader_Comparer()
    {
        var text = File.ReadAllText(_tempFile);

        var ini = Ini.FromString(text, EqualityComparer<string>.Default).Unwrap();

        Assert.Equal(Unit.Default, ini["A"].ExpectUnit());
        Assert.Equal("d", ini["foo", "c"].Unwrap());
        Assert.Equal("f", ini["bar", "e"].Unwrap());
    }

    [Fact]
    public void FromReader_TextReader_Comparer()
    {
        using var reader = new FileInfo(_tempFile).OpenText();

        var ini = Ini.FromReader(reader, EqualityComparer<string>.Default).Unwrap();

        Assert.Equal(Unit.Default, ini["A"].ExpectUnit());
        Assert.Equal("d", ini["foo", "c"].Unwrap());
        Assert.Equal("f", ini["bar", "e"].Unwrap());
    }

    // reader

    [Fact]
    public void AppendFromFile_Path()
    {
        var ini = Ini.Empty;

        ini["x"] = Option<string>.Some("y");
        ini["x", "y"] = Option<string>.Some("z");

        ini.AppendFromFile(_tempFile).Unwrap();

        Assert.Equal("b", ini["a"].Unwrap());
        Assert.Equal("y", ini["x"].Unwrap());
        Assert.Equal("d", ini["foo", "c"].Unwrap());
        Assert.Equal("f", ini["bar", "e"].Unwrap());
        Assert.Equal("z", ini["x", "y"].Unwrap());
    }

    [Fact]
    public void AppendFromFile_FileInfo()
    {
        var ini = Ini.Empty;

        ini["x"] = Option<string>.Some("y");
        ini["x", "y"] = Option<string>.Some("z");

        ini.AppendFromFile(new FileInfo(_tempFile)).Unwrap();

        Assert.Equal("b", ini["a"].Unwrap());
        Assert.Equal("y", ini["x"].Unwrap());
        Assert.Equal("d", ini["foo", "c"].Unwrap());
        Assert.Equal("f", ini["bar", "e"].Unwrap());
        Assert.Equal("z", ini["x", "y"].Unwrap());
    }

    [Fact]
    public void AppendFromString_TextReader()
    {
        var text = File.ReadAllText(_tempFile);

        var ini = Ini.Empty;

        ini["x"] = Option<string>.Some("y");
        ini["x", "y"] = Option<string>.Some("z");

        ini.AppendFromString(text).Unwrap();

        Assert.Equal("b", ini["a"].Unwrap());
        Assert.Equal("y", ini["x"].Unwrap());
        Assert.Equal("d", ini["foo", "c"].Unwrap());
        Assert.Equal("f", ini["bar", "e"].Unwrap());
        Assert.Equal("z", ini["x", "y"].Unwrap());
    }

    [Fact]
    public void AppendFromReader_TextReader()
    {
        using var reader = new FileInfo(_tempFile).OpenText();

        var ini = Ini.Empty;

        ini["x"] = Option<string>.Some("y");
        ini["x", "y"] = Option<string>.Some("z");

        ini.AppendFromReader(reader).Unwrap();

        Assert.Equal("b", ini["a"].Unwrap());
        Assert.Equal("y", ini["x"].Unwrap());
        Assert.Equal("d", ini["foo", "c"].Unwrap());
        Assert.Equal("f", ini["bar", "e"].Unwrap());
        Assert.Equal("z", ini["x", "y"].Unwrap());
    }

    // reader exception

    [Fact]
    public void AppendFromFile_Path_Exception()
    {
        var error = Ini.Empty.AppendFromFile((string)null!).ExpectError();

        Assert.StartsWith("Value cannot be null", error);
    }

    [Fact]
    public void AppendFromFile_FileInfo_Exception()
    {
        var error = Ini.Empty.AppendFromFile((FileInfo)null!).ExpectError();

        Assert.StartsWith("Object reference not set", error);
    }

    [Fact]
    public void AppendFromString_TextReader_Exception()
    {
        var error = Ini.Empty.AppendFromString(null!).ExpectError();

        Assert.StartsWith("Value cannot be null", error);
    }

    [Fact]
    public void AppendFromReader_TextReader_Exception()
    {
        using var reader = new FileInfo(_tempFile).OpenText();
        reader.Close();

        var error = Ini.Empty.AppendFromReader(reader).ExpectError();

        Assert.StartsWith("Cannot read from a closed TextReader", error);
    }
}
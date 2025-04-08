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

    // prelude

    [Fact]
    public void FromFile_Path()
    {
        var ini = Prelude.FromFile(_tempFile).Unwrap();

        Assert.Equal("b", ini["a"].Unwrap());
        Assert.Equal("d", ini["foo", "c"].Unwrap());
        Assert.Equal("f", ini["bar", "e"].Unwrap());
    }

    [Fact]
    public void FromFile_FileInfo()
    {
        var ini = Prelude.FromFile(new FileInfo(_tempFile)).Unwrap();

        Assert.Equal("b", ini["a"].Unwrap());
        Assert.Equal("d", ini["foo", "c"].Unwrap());
        Assert.Equal("f", ini["bar", "e"].Unwrap());
    }

    [Fact]
    public void FromReader_TextReader()
    {
        using var reader = new FileInfo(_tempFile).OpenText();

        var ini = Prelude.FromReader(reader).Unwrap();

        Assert.Equal("b", ini["a"].Unwrap());
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
}
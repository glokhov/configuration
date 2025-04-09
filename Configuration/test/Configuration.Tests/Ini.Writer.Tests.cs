using Functional;

namespace Configuration.Tests;

public sealed class WriterTests : IDisposable
{
    private readonly string _tempFile = Path.GetTempFileName();

    public void Dispose()
    {
        File.Delete(_tempFile);
    }

    private static readonly Ini Ini = new([
        ("", "b", "c"),
        ("", "e", "f"),
        ("g", "h", "i"),
        ("g", "k", "l"),
        ("m", "n", "o"),
        ("m", "q", "r")
    ]);

    private static readonly string Text = "b = c" + Environment.NewLine +
                                          "e = f" + Environment.NewLine +
                                          "" + Environment.NewLine +
                                          "[g]" + Environment.NewLine +
                                          "" + Environment.NewLine +
                                          "h = i" + Environment.NewLine +
                                          "k = l" + Environment.NewLine +
                                          "" + Environment.NewLine +
                                          "[m]" + Environment.NewLine +
                                          "" + Environment.NewLine +
                                          "n = o" + Environment.NewLine +
                                          "q = r" + Environment.NewLine;

    // writer

    [Fact]
    public void ToFile_Path()
    {
        Ini.ToFile(_tempFile);

        var text = File.ReadAllText(_tempFile);

        Assert.Equal(Text, text);
    }

    [Fact]
    public void ToFile_FileInfo()
    {
        Ini.ToFile(new FileInfo(_tempFile));

        var text = File.ReadAllText(_tempFile);

        Assert.Equal(Text, text);
    }

    [Fact]
    public void ToWriter_TextWriter()
    {
        using var writer = new FileInfo(_tempFile).CreateText();

        Ini.ToWriter(writer);
        
        writer.Close();

        var text = File.ReadAllText(_tempFile);

        Assert.Equal(Text, text);
    }

    [Fact]
    public void ToString_Test()
    {
        var text = Ini.ToString();

        Assert.Equal(Text, text);
    }

    // writer exception

    [Fact]
    public void ToFile_Path_Exception()
    {
        var error = Ini.ToFile((string)null!).ExpectError();

        Assert.StartsWith("Value cannot be null", error);
    }

    [Fact]
    public void ToFile_FileInfo_Exception()
    {
        var error = Ini.ToFile((FileInfo)null!).ExpectError();

        Assert.StartsWith("Object reference not set", error);
    }

    [Fact]
    public void ToWriter_TextWriter_Exception()
    {
        using var writer = new FileInfo(_tempFile).CreateText();
        writer.Close();

        var error = Ini.ToWriter(writer).ExpectError();

        Assert.StartsWith("Cannot write to a closed TextWriter", error);
    }
}
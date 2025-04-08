namespace Configuration.Tests;

public sealed class ExtensionsTests
{
    [Fact]
    public void GetSections()
    {
        var ini = new Ini([
            ("", "b", "c"),
            ("d", "e", "f")
        ]);

        var sections = ini.GetSections();

        Assert.Equal(["", "d"], sections);
    }

    [Fact]
    public void GetGlobalKeys()
    {
        var ini = new Ini([
            ("", "b", "c"),
            ("", "e", "f"),
            ("g", "h", "i"),
            ("g", "k", "l")
        ]);

        var keys = ini.GetGlobalKeys();

        Assert.Equal(["b", "e"], keys);
    }

    [Fact]
    public void GetGlobalValues()
    {
        var ini = new Ini([
            ("", "b", "c"),
            ("", "e", "f"),
            ("g", "h", "i"),
            ("g", "k", "l")
        ]);

        var keys = ini.GetGlobalValues();

        Assert.Equal(["c", "f"], keys);
    }

    [Fact]
    public void GetKeys()
    {
        var ini = new Ini([
            ("", "b", "c"),
            ("", "e", "f"),
            ("g", "h", "i"),
            ("g", "k", "l")
        ]);

        var keys = ini.GetKeys("g");

        Assert.Equal(["h", "k"], keys);
    }

    [Fact]
    public void GetValues()
    {
        var ini = new Ini([
            ("", "b", "c"),
            ("", "e", "f"),
            ("g", "h", "i"),
            ("g", "k", "l")
        ]);

        var keys = ini.GetValues("g");

        Assert.Equal(["i", "l"], keys);
    }

    [Fact]
    public void GetGlobalSection()
    {
        var ini = new Ini([
            ("", "b", "c"),
            ("d", "e", "f")
        ]);

        var section = ini.GetGlobalSection();

        Assert.Equal("c", section["b"]);
    }

    [Fact]
    public void GetSection()
    {
        var ini = new Ini([
            ("", "b", "c"),
            ("d", "e", "f")
        ]);

        var section = ini.GetSection("d");

        Assert.Equal("f", section["e"]);
    }
}
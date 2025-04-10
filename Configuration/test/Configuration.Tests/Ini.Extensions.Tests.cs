using Functional;

namespace Configuration.Tests;

public sealed class ExtensionsTests
{
    [Fact]
    public void Clear_Test()
    {
        var ini = new Ini([("a", "b", "c")]);

        ini.Clear();

        Assert.True(ini.IsEmpty);
    }

    [Fact]
    public void Contains_Global_Test()
    {
        var ini = new Ini([("", "b", "c")]);

        Assert.True(ini.Contains("b"));
        Assert.False(ini.Contains("c"));
    }

    [Fact]
    public void Contains_Test()
    {
        var ini = new Ini([("a", "b", "c")]);

        Assert.True(ini.Contains("a", "b"));
        Assert.False(ini.Contains("b", "c"));
    }

    [Fact]
    public void GetNested_Test()
    {
        var ini = new Ini([
            ("", "a", "a"),
            (".b", "b", "b"),
            (".b.c", "c", "c")
        ]);

        var aa = ini.Get(Ini.Global, "a").Unwrap();
        var ba = ini.Get(".b", "a").Unwrap();
        var bb = ini.Get(".b", "b").Unwrap();
        var ca = ini.Get(".b.c", "a").Unwrap();
        var cb = ini.Get(".b.c", "b").Unwrap();
        var cc = ini.Get(".b.c", "c").Unwrap();
        var bc = ini.Get(".b", "c").ExpectUnit();

        Assert.Equal("a", aa);
        Assert.Equal("a", ba);
        Assert.Equal("b", bb);
        Assert.Equal("a", ca);
        Assert.Equal("b", cb);
        Assert.Equal("c", cc);
        Assert.Equal(Unit.Default, bc);
    }

    [Fact]
    public void Add_Global_Test()
    {
        var ini = Ini.Empty;

        ini.Add("b", "c");

        Assert.True(ini.Contains("b"));
    }

    [Fact]
    public void Add_Test()
    {
        var ini = Ini.Empty;

        ini.Add("a", "b", "c");

        Assert.True(ini.Contains("a", "b"));
    }

    [Fact]
    public void Remove_Global_Test()
    {
        var ini = new Ini([
            ("", "b", "c"),
            ("", "e", "f")
        ]);

        ini.Remove("b");

        Assert.False(ini.Contains("b"));
        Assert.True(ini.Contains("e"));
    }

    [Fact]
    public void Remove_Test()
    {
        var ini = new Ini([
            ("a", "b", "c"),
            ("d", "e", "f")
        ]);

        ini.Remove("a", "b");

        Assert.False(ini.Contains("a", "b"));
        Assert.True(ini.Contains("d", "e"));
    }

    [Fact]
    public void GetSections()
    {
        var ini = new Ini([
            ("", "b", "c"),
            ("d", "e", "f"),
            ("", "bb", "cc"),
            ("d", "ee", "ff")
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
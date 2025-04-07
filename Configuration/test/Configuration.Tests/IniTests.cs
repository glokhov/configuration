using Functional;

namespace Configuration.Tests;

using Configuration;

public sealed class IniTests
{
    [Fact]
    public void Empty_Test()
    {
        var ini = Ini.Empty;

        Assert.True(ini.IsEmpty);
        Assert.Equal(0, ini.Count);
        Assert.Equal("OrdinalIgnoreCaseComparer", ini.Comparer.GetType().Name);
    }

    [Fact]
    public void Ctor_Default_Test()
    {
        var ini = new Ini();

        Assert.True(ini.IsEmpty);
        Assert.Equal(0, ini.Count);
        Assert.Equal("OrdinalIgnoreCaseComparer", ini.Comparer.GetType().Name);
    }

    [Fact]
    public void Ctor_Comparer_Test()
    {
        var ini = new Ini(EqualityComparer<string>.Default);

        Assert.True(ini.IsEmpty);
        Assert.Equal(0, ini.Count);
        Assert.Equal("StringEqualityComparer", ini.Comparer.GetType().Name);
    }

    [Fact]
    public void Ctor_Elements_Test()
    {
        var ini = new Ini([("a", "b", "c")]);

        Assert.False(ini.IsEmpty);
        Assert.Equal(1, ini.Count);
        Assert.Equal("OrdinalIgnoreCaseComparer", ini.Comparer.GetType().Name);

        Assert.True(ini["a", "b"].IsSome);
        Assert.True(ini["A", "B"].IsSome);
    }

    [Fact]
    public void Ctor_Elements_Comparer_Test()
    {
        var ini = new Ini([("a", "b", "c"), ("d", "e", "f")], EqualityComparer<string>.Default);

        Assert.False(ini.IsEmpty);
        Assert.Equal(2, ini.Count);
        Assert.Equal("StringEqualityComparer", ini.Comparer.GetType().Name);

        Assert.True(ini["a", "b"].IsSome);
        Assert.True(ini["A", "B"].IsNone);
    }

    [Fact]
    public void Indexer_Global_Get_Test()
    {
        var ini = new Ini([("", "b", "c")]);

        Assert.Equal("c", ini["b"].Unwrap());
        Assert.True(ini["c"].IsNone);
    }

    [Fact]
    public void Indexer_Get_Test()
    {
        var ini = new Ini([("a", "b", "c")]);

        Assert.Equal("c", ini["a", "b"].Unwrap());
        Assert.True(ini["b", "c"].IsNone);
    }

    [Fact]
    public void Indexer_Global_Add_Test()
    {
        var ini = Ini.Empty;

        ini["b"] = Option<string>.Some("c");

        Assert.Equal("c", ini["b"].Unwrap());
    }

    [Fact]
    public void Indexer_Add_Test()
    {
        var ini = Ini.Empty;

        ini["a", "b"] = Option<string>.Some("c");

        Assert.Equal("c", ini["a", "b"].Unwrap());
    }

    [Fact]
    public void Indexer_Global_Remove_Test()
    {
        var ini = new Ini([("", "b", "c"), ("", "e", "f")]);

        ini["b"] = Option<string>.None;

        Assert.Equal(Unit.Default, ini["b"].ExpectUnit());
        Assert.Equal("f", ini["e"].Unwrap());
    }

    [Fact]
    public void Indexer_Remove_Test()
    {
        var ini = new Ini([("a", "b", "c"), ("d", "e", "f")]);

        ini["a", "b"] = Option<string>.None;

        Assert.Equal(Unit.Default, ini["a", "b"].ExpectUnit());
        Assert.Equal("f", ini["d", "e"].Unwrap());
    }

    [Fact]
    public void IsClear_Test()
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
        var ini = new Ini([("", "b", "c"), ("", "e", "f")]);

        ini.Remove("b");

        Assert.False(ini.Contains("b"));
        Assert.True(ini.Contains("e"));
    }

    [Fact]
    public void Remove_Test()
    {
        var ini = new Ini([("a", "b", "c"), ("d", "e", "f")]);

        ini.Remove("a", "b");

        Assert.False(ini.Contains("a", "b"));
        Assert.True(ini.Contains("d", "e"));
    }

    [Fact]
    public void GetEnumerator_Test()
    {
        var ini = new Ini([("a", "b", "c"), ("d", "e", "f")]);

        using var enumerator = ini.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(("a", "b", "c"), enumerator.Current);
        Assert.True(enumerator.MoveNext());
        Assert.Equal(("d", "e", "f"), enumerator.Current);
        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    public void ToString_Test()
    {
        var empty = Ini.Empty.ToString();
        var one = new Ini([("a", "b", "c")]).ToString();
        var two = new Ini([("a", "b", "c"), ("d", "e", "f")]).ToString();
        var three = new Ini([("a", "b", "c"), ("d", "e", "f"), ("g", "h", "i")]).ToString();
        var four = new Ini([("a", "b", "c"), ("d", "e", "f"), ("g", "h", "i"), ("j", "k", "l")]).ToString();
        
        Assert.Equal("ini []", empty);
        Assert.Equal("ini [a.b = c]", one);
        Assert.Equal("ini [a.b = c; d.e = f]", two);
        Assert.Equal("ini [a.b = c; d.e = f; g.h = i]", three);
        Assert.Equal("ini [a.b = c; d.e = f; g.h = i; ...]", four);
    }
}
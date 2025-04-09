using System.Collections;
using Functional;

namespace Configuration.Tests;

using Configuration;

public sealed class IniTests
{
    [Fact]
    public void Property_Empty()
    {
        var ini = Ini.Empty;

        Assert.True(ini.IsEmpty);
        Assert.Equal(0, ini.Count);
        Assert.Equal("OrdinalIgnoreCaseComparer", ini.Comparer.GetType().Name);
    }

    [Fact]
    public void Ctor_Default()
    {
        var ini = new Ini();

        Assert.True(ini.IsEmpty);
        Assert.Equal(0, ini.Count);
        Assert.Equal("OrdinalIgnoreCaseComparer", ini.Comparer.GetType().Name);
    }

    [Fact]
    public void Ctor_Comparer()
    {
        var ini = new Ini(EqualityComparer<string>.Default);

        Assert.True(ini.IsEmpty);
        Assert.Equal(0, ini.Count);
        Assert.Equal("StringEqualityComparer", ini.Comparer.GetType().Name);
    }

    [Fact]
    public void Ctor_Elements()
    {
        var ini = new Ini([("a", "b", "c")]);

        Assert.False(ini.IsEmpty);
        Assert.Equal(1, ini.Count);
        Assert.Equal("OrdinalIgnoreCaseComparer", ini.Comparer.GetType().Name);

        Assert.True(ini["a", "b"].IsSome);
        Assert.True(ini["A", "B"].IsSome);
    }

    [Fact]
    public void Ctor_Elements_Comparer()
    {
        var ini = new Ini([("a", "b", "c"), ("d", "e", "f")], EqualityComparer<string>.Default);

        Assert.False(ini.IsEmpty);
        Assert.Equal(2, ini.Count);
        Assert.Equal("StringEqualityComparer", ini.Comparer.GetType().Name);

        Assert.True(ini["a", "b"].IsSome);
        Assert.True(ini["A", "B"].IsNone);
    }

    [Fact]
    public void Indexer_Global_Get()
    {
        var ini = new Ini([("", "b", "c")]);

        Assert.Equal("c", ini["b"].Unwrap());
        Assert.True(ini["c"].IsNone);
    }

    [Fact]
    public void Indexer_Get()
    {
        var ini = new Ini([("a", "b", "c")]);

        Assert.Equal("c", ini["a", "b"].Unwrap());
        Assert.True(ini["b", "c"].IsNone);
    }

    [Fact]
    public void Indexer_Global_Add()
    {
        var ini = Ini.Empty;

        ini["b"] = Option<string>.Some("c");

        Assert.Equal("c", ini["b"].Unwrap());
    }

    [Fact]
    public void Indexer_Add()
    {
        var ini = Ini.Empty;

        ini["a", "b"] = Option<string>.Some("c");

        Assert.Equal("c", ini["a", "b"].Unwrap());
    }

    [Fact]
    public void Indexer_Global_Remove()
    {
        var ini = new Ini([("", "b", "c"), ("", "e", "f")]);

        ini["b"] = Option<string>.None;

        Assert.Equal(Unit.Default, ini["b"].ExpectUnit());
        Assert.Equal("f", ini["e"].Unwrap());
    }

    [Fact]
    public void Indexer_Remove()
    {
        var ini = new Ini([("a", "b", "c"), ("d", "e", "f")]);

        ini["a", "b"] = Option<string>.None;

        Assert.Equal(Unit.Default, ini["a", "b"].ExpectUnit());
        Assert.Equal("f", ini["d", "e"].Unwrap());
    }

    [Fact]
    public void GetEnumerator()
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
    public void GetEnumerator_IEnumerable()
    {
        IEnumerable ini = new Ini([("a", "b", "c"), ("d", "e", "f")]);
        
        var enumerator = ini.GetEnumerator();
        using var disposable = enumerator as IDisposable;

        Assert.True(enumerator.MoveNext());
        Assert.Equal(("a", "b", "c"), enumerator.Current);
        Assert.True(enumerator.MoveNext());
        Assert.Equal(("d", "e", "f"), enumerator.Current);
        Assert.False(enumerator.MoveNext());
    }
}
using System.Collections;

namespace ConfigurationTests;

public sealed partial class IniTests
{
    [Fact]
    public void Default_Ctor()
    {
        var ini = new Ini();

        Assert.Empty(ini.Config);
        Assert.Equal("StringEqualityComparer", ini.Comparer.GetType().Name);
    }

    [Fact]
    public void Comparer_Ctor()
    {
        var ini = new Ini(StringComparer.Ordinal);

        Assert.Empty(ini.Config);
        Assert.Equal("OrdinalCaseSensitiveComparer", ini.Comparer.GetType().Name);
    }

    [Fact]
    public void Collection_Expression_Empty()
    {
        Ini ini = [];
        using var iniEnumerator = ini.GetEnumerator();

        IEnumerable enumerable = ini;
        var genEnumerator = enumerable.GetEnumerator();

        using var _ = genEnumerator as IDisposable;

        Assert.NotSame(iniEnumerator, genEnumerator);
        Assert.False(iniEnumerator.MoveNext());
        Assert.False(genEnumerator.MoveNext());
    }

    [Fact]
    public void Collection_Expression_Add_Clear()
    {
        var keyValue = new KeyValue<string, string>("key", "value");
        var sectionKeyValue = new KeyValue<string, Section>("section", [keyValue]);

        Ini ini = [sectionKeyValue];

        Assert.True(ini.Contains(sectionKeyValue));
        Assert.Equal(1, ini.Count);

        using var iniEnumerator = ini.GetEnumerator();
        iniEnumerator.MoveNext();

        var (name, section) = iniEnumerator.Current;

        Assert.Equal("section", name);

        using var sectionEnumerator = section.GetEnumerator();
        sectionEnumerator.MoveNext();

        var (key, value) = sectionEnumerator.Current;

        Assert.Equal("key", key);
        Assert.Equal("value", value);

        ini.Clear();

        Assert.False(ini.Contains(sectionKeyValue));
        Assert.Equal(0, ini.Count);
    }
}
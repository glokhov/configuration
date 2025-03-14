using System.Collections;

namespace ConfigurationTests;

public sealed partial class IniTests
{
    [Fact]
    public void Default_Ctor()
    {
        var ini = new Ini();

        Assert.Empty(ini);
        Assert.Empty(ini.Config);
        Assert.Equal("StringEqualityComparer", ini.Comparer.GetType().Name);
    }

    [Fact]
    public void Comparer_Ctor()
    {
        var ini = new Ini(StringComparer.Ordinal);

        Assert.Empty(ini);
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
        var sectionKeyValue = new KeyValue<string, SectionDictionary>("section", [keyValue]);

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

    [Fact]
    public void Indexer_Global_Section_Sets_Removes()
    {
        Ini ini = [];

        ini["key"] = Some("Value");

        Assert.Equal("Value", ini["key"].Unwrap());

        ini["key"] = None;

        Assert.True(ini["key"].IsNone);
    }

    [Fact]
    public void GetSection_SetSection_Get_Sets_Removes()
    {
        var ini = new Ini();

        Assert.True(ini.GetSection("section").IsNone);

        ini.SetSection("section", Some<SectionDictionary>([]));

        Assert.True(ini.GetSection("section").IsSome);

        ini.SetSection("section", None);

        Assert.True(ini.GetSection("section").IsNone);
    }

    [Fact]
    public void GetGlobalSection_SetGlobalSection_Get_Sets_Removes()
    {
        var ini = new Ini();

        Assert.True(ini.GetGlobalSection().IsNone);

        ini.SetGlobalSection(Some<SectionDictionary>([]));

        Assert.True(ini.GetGlobalSection().IsSome);

        ini.SetGlobalSection(None);

        Assert.True(ini.GetGlobalSection().IsNone);
    }

    [Fact]
    public void ToString_Places_Global_Section_At_Start()
    {
        var ini = new Ini
        {
            ["key"] = Some("value"),
            ["foo", "foo"] = Some("value"),
            ["bar", "bar"] = Some("value")
        };

        var expected = "key = value" + Environment.NewLine +
                       "" + Environment.NewLine +
                       "[foo]" + Environment.NewLine +
                       "" + Environment.NewLine +
                       "foo = value" + Environment.NewLine +
                       "" + Environment.NewLine +
                       "[bar]" + Environment.NewLine +
                       "" + Environment.NewLine +
                       "bar = value" +
                       "" + Environment.NewLine;
        
        Assert.Equal(expected, ini.ToString());
    }

    [Fact]
    public void ToString_Moves_Global_Section_To_Start()
    {
        var ini = new Ini
        {
            ["foo", "foo"] = Some("value"),
            ["bar", "bar"] = Some("value"),
            ["key"] = Some("value")
        };

        var expected = "key = value" + Environment.NewLine +
                       "" + Environment.NewLine +
                       "[foo]" + Environment.NewLine +
                       "" + Environment.NewLine +
                       "foo = value" + Environment.NewLine +
                       "" + Environment.NewLine +
                       "[bar]" + Environment.NewLine +
                       "" + Environment.NewLine +
                       "bar = value" +
                       "" + Environment.NewLine;
        
        Assert.Equal(expected, ini.ToString());
    }
}
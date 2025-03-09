namespace ConfigurationTests;

public sealed class ConfigDictionaryTests
{
    [Fact]
    public void Default_Ctor_Config_Ctor()
    {
        var config = new ConfigDictionary { ["key"] = Some(new SectionDictionary()) };
        var copy = new ConfigDictionary(config);

        Assert.True(copy["key"].IsSome);
        Assert.True(copy["KEY"].IsNone);
    }

    [Fact]
    public void Comparer_Ctor_Config_Ctor()
    {
        var config = new ConfigDictionary(StringComparer.OrdinalIgnoreCase) { ["key"] = Some(new SectionDictionary()) };
        var copy = new ConfigDictionary(config, config.Comparer);

        Assert.True(copy["key"].IsSome);
        Assert.True(copy["KEY"].IsSome);
    }

    [Fact]
    public void Merge_Replaces_OldValues_Adds_NewValues()
    {
        var first = new ConfigDictionary
        {
            ["aaa"] = Some(new SectionDictionary
            {
                ["aaa"] = Some("aaa")
            }),
            ["bbb"] = Some(new SectionDictionary
            {
                ["ccc"] = Some("ccc"),
                ["ddd"] = Some("xxx")
            })
        };

        var second = new ConfigDictionary
        {
            ["bbb"] = Some(new SectionDictionary
            {
                ["ddd"] = Some("ddd"),
                ["eee"] = Some("eee")
            }),
            ["fff"] = Some(new SectionDictionary
            {
                ["fff"] = Some("fff")
            })
        };

        var merged = new ConfigDictionary
        {
            ["aaa"] = Some(new SectionDictionary
            {
                ["aaa"] = Some("aaa")
            }),
            ["bbb"] = Some(new SectionDictionary
            {
                ["ccc"] = Some("ccc"),
                ["ddd"] = Some("ddd"),
                ["eee"] = Some("eee")
            }),
            ["fff"] = Some(new SectionDictionary
            {
                ["fff"] = Some("fff")
            })
        };

        Assert.Equivalent(merged, first.Merge(second));
    }

    [Fact]
    public void ToString_Creates_Ini_File_Content()
    {
        var config = new ConfigDictionary
        {
            ["foo"] = Some(new SectionDictionary
            {
                ["A"] = Some("aaa")
            }),
            ["bar"] = Some(new SectionDictionary
            {
                ["AB"] = Some("abb"),
                ["ABC"] = Some("abc")
            })
        };

        var result = "[foo]" + Environment.NewLine +
                     "" + Environment.NewLine +
                     "A = aaa" + Environment.NewLine +
                     "" + Environment.NewLine +
                     "[bar]" + Environment.NewLine +
                     "" + Environment.NewLine +
                     "AB  = abb" + Environment.NewLine +
                     "ABC = abc" + Environment.NewLine;

        Assert.Equal(result, config.ToString());
    }

    [Fact]
    public void If_Section_Empty_ToString_Returns_Empty_String()
    {
        Assert.Empty(new ConfigDictionary().ToString());
    }
}
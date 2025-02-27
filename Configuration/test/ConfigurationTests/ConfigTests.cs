namespace ConfigurationTests;

public sealed class ConfigTests
{
    [Fact]
    public void Default_Ctor_Config_Ctor()
    {
        var config = new Config { ["key"] = Some(new Section()) };
        var copy = new Config(config);

        Assert.True(copy["key"].IsSome);
        Assert.True(copy["KEY"].IsNone);
    }

    [Fact]
    public void Comparer_Ctor_Config_Ctor()
    {
        var config = new Config(StringComparer.OrdinalIgnoreCase) { ["key"] = Some(new Section()) };
        var copy = new Config(config);

        Assert.True(copy["key"].IsSome);
        Assert.True(copy["KEY"].IsSome);
    }

    [Fact]
    public void Merge_Replaces_OldValues_Adds_NewValues()
    {
        var first = new Config
        {
            ["aaa"] = Some(new Section
            {
                ["aaa"] = Some("aaa")
            }),
            ["bbb"] = Some(new Section
            {
                ["ccc"] = Some("ccc"),
                ["ddd"] = Some("xxx")
            })
        };

        var second = new Config
        {
            ["bbb"] = Some(new Section
            {
                ["ddd"] = Some("ddd"),
                ["eee"] = Some("eee")
            }),
            ["fff"] = Some(new Section
            {
                ["fff"] = Some("fff")
            })
        };

        var merged = new Config
        {
            ["aaa"] = Some(new Section
            {
                ["aaa"] = Some("aaa")
            }),
            ["bbb"] = Some(new Section
            {
                ["ccc"] = Some("ccc"),
                ["ddd"] = Some("ddd"),
                ["eee"] = Some("eee")
            }),
            ["fff"] = Some(new Section
            {
                ["fff"] = Some("fff")
            })
        };

        Assert.Equivalent(merged, first.Merge(second));
    }

    [Fact]
    public void ToString_Creates_Ini_File_Content()
    {
        var config = new Config
        {
            ["foo"] = Some(new Section
            {
                ["A"] = Some("aaa")
            }),
            ["bar"] = Some(new Section
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
        Assert.Empty(new Config().ToString());
    }
}
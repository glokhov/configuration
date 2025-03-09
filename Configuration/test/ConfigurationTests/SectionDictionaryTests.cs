namespace ConfigurationTests;

public sealed class SectionDictionaryTests
{
    [Fact]
    public void Default_Ctor_Section_Ctor()
    {
        var section = new SectionDictionary { ["key"] = Some("value") };
        var copy = new SectionDictionary(section);

        Assert.True(copy["key"].IsSome);
        Assert.True(copy["KEY"].IsNone);
    }

    [Fact]
    public void Comparer_Ctor_Section_Ctor()
    {
        var section = new SectionDictionary(StringComparer.OrdinalIgnoreCase) { ["key"] = Some("value") };
        var copy = new SectionDictionary(section, section.Comparer);

        Assert.True(copy["key"].IsSome);
        Assert.True(copy["KEY"].IsSome);
    }

    [Fact]
    public void Merge_Replaces_OldValues_Adds_NewValues()
    {
        var first = new SectionDictionary
        {
            ["aaa"] = Some("aaa"),
            ["bbb"] = Some("bbb"),
            ["ccc"] = Some("xxx")
        };

        var second = new SectionDictionary
        {
            ["bbb"] = Some("bbb"),
            ["ccc"] = Some("ccc"),
            ["zzz"] = Some("zzz")
        };

        var merged = new SectionDictionary
        {
            ["aaa"] = Some("aaa"),
            ["bbb"] = Some("bbb"),
            ["ccc"] = Some("ccc"),
            ["zzz"] = Some("zzz")
        };

        Assert.Equivalent(merged, first.Merge(second));
    }

    [Fact]
    public void ToString_Creates_Ini_File_Section_With_Padding()
    {
        var section = new SectionDictionary
        {
            ["A"] = Some("aaa"),
            ["AB"] = Some("abb"),
            ["ABC"] = Some("abc")
        };

        var result = "A   = aaa" + Environment.NewLine +
                     "AB  = abb" + Environment.NewLine +
                     "ABC = abc" + Environment.NewLine;

        Assert.Equal(result, section.ToString());
    }

    [Fact]
    public void If_Section_Empty_ToString_Returns_Empty_String()
    {
        Assert.Empty(new SectionDictionary().ToString());
    }
}
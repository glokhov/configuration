namespace ConfigurationTests;

public sealed class SectionTests
{
    [Fact]
    public void Default_Ctor_Section_Ctor()
    {
        var section = new Section { ["key"] = Some("value") };
        var copy = new Section(section);

        Assert.True(copy["key"].IsSome);
        Assert.True(copy["KEY"].IsNone);
    }

    [Fact]
    public void Comparer_Ctor_Section_Ctor()
    {
        var section = new Section(StringComparer.OrdinalIgnoreCase) { ["key"] = Some("value") };
        var copy = new Section(section, section.Comparer);

        Assert.True(copy["key"].IsSome);
        Assert.True(copy["KEY"].IsSome);
    }

    [Fact]
    public void Merge_Replaces_OldValues_Adds_NewValues()
    {
        var first = new Section
        {
            ["aaa"] = Some("aaa"),
            ["bbb"] = Some("bbb"),
            ["ccc"] = Some("xxx")
        };

        var second = new Section
        {
            ["bbb"] = Some("bbb"),
            ["ccc"] = Some("ccc"),
            ["zzz"] = Some("zzz")
        };

        var merged = new Section
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
        var section = new Section
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
        Assert.Empty(new Section().ToString());
    }
}
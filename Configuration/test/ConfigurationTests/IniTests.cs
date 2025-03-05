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
}
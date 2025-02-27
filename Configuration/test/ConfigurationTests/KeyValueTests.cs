namespace ConfigurationTests;

public sealed class KeyValueTests
{
    [Fact]
    public void Ctor()
    {
        var keyValue = new KeyValue<string, string>("key", "value");

        Assert.Equal("key", keyValue.Key);
        Assert.Equal("value", keyValue.Value);
    }

    [Fact]
    public void Object_Initializer()
    {
        var keyValue = new KeyValue<string, string> { Key = "key", Value = "value" };

        Assert.Equal("key", keyValue.Key);
        Assert.Equal("value", keyValue.Value);
    }

    [Fact]
    public void Deconstruct()
    {
        var (key, value) = new KeyValue<string, string>("key", "value");

        Assert.Equal("key", key);
        Assert.Equal("value", value);
    }

    [Fact]
    public void ToString_Returns_Key_Value()
    {
        Assert.Equal("[key, value]", new KeyValue<string, string>("key", "value").ToString());
    }
}
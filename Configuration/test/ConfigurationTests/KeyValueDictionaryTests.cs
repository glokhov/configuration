using System.Collections;

namespace ConfigurationTests;

public sealed class KeyValueDictionaryTests
{
    [Fact]
    public void Collection_Expression_Empty()
    {
        KeyValueTestDictionary dictionary = [];

        Assert.Empty(dictionary);
        Assert.Empty(dictionary.Keys);
        Assert.Empty(dictionary.Values);
        Assert.Equal(0, dictionary.Count);
    }

    [Fact]
    public void Default_Ctor_Creates_Empty_Dictionary()
    {
        var dictionary = new KeyValueTestDictionary();

        Assert.Empty(dictionary);
        Assert.Empty(dictionary.Keys);
        Assert.Empty(dictionary.Values);
        Assert.Equal(0, dictionary.Count);
    }

    [Fact]
    public void Collection_Expression_Add()
    {
        KeyValueTestDictionary dictionary = [new KeyValue<string, string>("key", "value")];

        Assert.True(dictionary.ContainsKey("key"));
        Assert.True(dictionary.Keys.Contains("key"));
        Assert.True(dictionary.Values.Contains("value"));
        Assert.Equal(1, dictionary.Count);
    }

    [Fact]
    public void KeyValueCollection_Add_Contains_Clear_Count()
    {
        var keyValue = new KeyValue<string, string>("key", "value");

        KeyValueTestDictionary dictionary = [keyValue];

        Assert.True(dictionary.Contains(keyValue));

        dictionary.Clear();

        Assert.Equal(0, dictionary.Count);
    }

    [Fact]
    public void Object_Initializer_Calls_Add()
    {
        var dictionary = new KeyValueTestDictionary { { "key", "value" } };

        Assert.True(dictionary.ContainsKey("key"));
        Assert.True(dictionary.Keys.Contains("key"));
        Assert.True(dictionary.Values.Contains("value"));
        Assert.Equal(1, dictionary.Count);
    }

    [Fact]
    public void Object_Initializer_Calls_Indexer()
    {
        var dictionary = new KeyValueTestDictionary { ["key"] = Some("value") };

        Assert.True(dictionary.ContainsKey("key"));
        Assert.True(dictionary.Keys.Contains("key"));
        Assert.True(dictionary.Values.Contains("value"));
        Assert.Equal(1, dictionary.Count);
    }

    [Fact]
    public void Indexer_Adds_Updates_Removes()
    {
        var dictionary = new KeyValueTestDictionary();

        var before = dictionary["key"];
        var add = dictionary["key"] = Some("add");
        var update = dictionary["key"] = Some("update");
        var none = dictionary["key"] = None;

        Assert.True(before.IsNone);
        Assert.Equal("add", add.AsPure().Value);
        Assert.Equal("update", update.AsPure().Value);
        Assert.True(none.IsNone);
    }

    [Fact]
    public void Clear_Removes_All_Items()
    {
        var dictionary = new KeyValueTestDictionary
        {
            ["abc"] = Some("value"),
            ["xyz"] = Some("value")
        };

        dictionary.Clear();

        Assert.Empty(dictionary);
    }

    [Fact]
    public void Add_Adds_Updates_Item_And_Returns_Added_Updated_Item()
    {
        var dictionary = new KeyValueTestDictionary();

        var add = dictionary.Add("key", "add");
        var update = dictionary.Add("key", "update");

        Assert.Equal("add", add.AsPure().Value);
        Assert.Equal("update", update.AsPure().Value);
    }

    [Fact]
    public void Remove_Removes_Item_And_Returns_Removed_Item_Or_None()
    {
        var dictionary = new KeyValueTestDictionary { ["key"] = Some("value") };

        var value = dictionary.Remove("key");
        var none = dictionary.Remove("key");

        Assert.Equal("value", value.AsPure().Value);
        Assert.True(none.IsNone);
    }

    [Fact]
    public void Get_Gets_Item_Or_None()
    {
        var dictionary = new KeyValueTestDictionary { ["key"] = Some("value") };

        var value = dictionary.Get("key");
        var none = dictionary.Get("abc");

        Assert.Equal("value", value.AsPure().Value);
        Assert.True(none.IsNone);
    }

    [Fact]
    public void GetEnumerator_Returns_KeyValueEnumerator()
    {
        var dictionary = new KeyValueTestDictionary { ["key"] = Some("value") };
        using var enumerator = dictionary.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new KeyValue<string, string>("key", "value"), enumerator.Current);
        Assert.False(enumerator.MoveNext());

        enumerator.Reset();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new KeyValue<string, string>("key", "value"), enumerator.Current);
        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    public void IEnumerable_GetEnumerator_Returns_IEnumerator()
    {
        IEnumerable enumerable = new KeyValueTestDictionary { ["key"] = Some("value") };
        var enumerator = enumerable.GetEnumerator();
        using var disposable = enumerator as IDisposable;

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new KeyValue<string, string>("key", "value"), enumerator.Current);
        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    public void Default_Ctor_Sets_Default_Comparer()
    {
        var dictionary = new KeyValueTestDictionary { ["key"] = Some("value") };

        Assert.True(dictionary["KEY"].IsNone);
        Assert.Equal("StringEqualityComparer", dictionary.Comparer.GetType().Name);
    }

    [Fact]
    public void Comparer_Ctor_Sets_Custom_Comparer()
    {
        var dictionary = new KeyValueTestDictionary(StringComparer.OrdinalIgnoreCase) { ["key"] = Some("value") };

        Assert.True(dictionary["KEY"].IsSome);
        Assert.Equal("OrdinalIgnoreCaseComparer", dictionary.Comparer.GetType().Name);
    }
}
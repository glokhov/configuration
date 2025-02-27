namespace ConfigurationTests;

public sealed class KeyValueTestDictionary : KeyValueDictionary<string, string>
{
    public KeyValueTestDictionary()
    {
    }

    public KeyValueTestDictionary(StringComparer comparer) : base(comparer)
    {
    }

    public KeyValueTestDictionary(KeyValueTestDictionary dictionary) : base(dictionary)
    {
    }

    public Dictionary<string, string> InternalDictionary => Dictionary;
}
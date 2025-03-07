namespace ConfigurationTests;

public sealed class KeyValueTestDictionary : KeyValueDictionary<string, string>
{
    public KeyValueTestDictionary()
    {
    }

    public KeyValueTestDictionary(IEqualityComparer<string> comparer) : base(comparer)
    {
    }

    public KeyValueTestDictionary(KeyValueTestDictionary dictionary) : base(dictionary)
    {
    }

    public KeyValueTestDictionary(KeyValueTestDictionary dictionary, IEqualityComparer<string> comparer) : base(dictionary, comparer)
    {
    }

    public Dictionary<string, string> InternalDictionary => Dictionary;
}
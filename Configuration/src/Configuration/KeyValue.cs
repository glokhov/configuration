namespace Configuration;

public readonly record struct KeyValue<TKey, TValue>(TKey Key, TValue Value)
{
    public void Deconstruct(out TKey key, out TValue value)
    {
        key = Key;
        value = Value;
    }

    public override string ToString()
    {
        return $"[{Key}, {Value}]";
    }
}
using System.Collections;

namespace Configuration;

internal class KeyValueEnumerator<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>> enumerator) : IEnumerator<KeyValue<TKey, TValue>>
{
    public void Dispose()
    {
        enumerator.Dispose();
    }

    public bool MoveNext()
    {
        return enumerator.MoveNext();
    }

    public void Reset()
    {
        enumerator.Reset();
    }

    public KeyValue<TKey, TValue> Current => new(enumerator.Current.Key, enumerator.Current.Value);

    object IEnumerator.Current => Current;
}
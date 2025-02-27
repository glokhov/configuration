using System.Collections;
using System.Text;

namespace Configuration;

public sealed class Section : IEnumerable<KeyValue<string, string>>
{
    private Dictionary<string, string> Parameters { get; }

    public Section() : this(new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase))
    {
    }

    private Section(Dictionary<string, string> parameters)
    {
        Parameters = parameters;
    }

    public Option<string> this[string name]
    {
        get => Parameters.TryGetValue(name, out var value) ? Some(value) : None;
        set => value.Match(val => { Parameters[name] = val; }, () => { Parameters.Remove(name); });
    }

    public int Count => Parameters.Count;

    public ICollection<string> Keys => Parameters.Keys;

    public ICollection<string> Values => Parameters.Values;

    public Section Merge(Section section)
    {
        var merge = new Section(Parameters);

        foreach (var (key, value) in section)
        {
            merge[key] = Some(value);
        }

        return merge;
    }

    public IEnumerator<KeyValue<string, string>> GetEnumerator()
    {
        return new KeyValueEnumerator<string, string>(Parameters.GetEnumerator());
    }

    public override string ToString()
    {
        if (Count == 0) return "";

        var width = Keys.Max(key => key.Length);

        var builder = new StringBuilder();

        foreach (var (key, value) in this)
        {
            builder.Append(key.PadRight(width));
            builder.Append(" = ");
            builder.Append(value);
            builder.Append(Environment.NewLine);
        }

        return builder.ToString();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
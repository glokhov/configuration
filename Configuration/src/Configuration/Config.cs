using System.Collections;
using System.Text;

namespace Configuration;

public sealed class Config : IEnumerable<KeyValue<string, Section>>
{
    private Dictionary<string, Section> Sections { get; }

    public Config() : this(new Dictionary<string, Section>(StringComparer.InvariantCultureIgnoreCase))
    {
    }

    private Config(Dictionary<string, Section> sections)
    {
        Sections = sections;
    }

    public Option<Section> this[string name]
    {
        get => Sections.TryGetValue(name, out var value) ? Some(value) : None;
        set => value.Match(sec => { Sections[name] = sec; }, () => { Sections.Remove(name); });
    }

    public int Count => Sections.Count;

    public ICollection<string> Keys => Sections.Keys;

    public ICollection<Section> Values => Sections.Values;

    public Config Merge(Config config)
    {
        var merge = new Config(Sections);

        foreach (var (key, section) in config)
        {
            merge[key] = Some(merge[key].Map(sec => sec.Merge(section)).Match(sec => sec, section));
        }

        return merge;
    }

    public IEnumerator<KeyValue<string, Section>> GetEnumerator()
    {
        return new KeyValueEnumerator<string, Section>(Sections.GetEnumerator());
    }

    public override string ToString()
    {
        if (Count == 0 || this.All(pair => pair.Value.Count == 0)) return "";

        var builder = new StringBuilder();

        foreach (var (key, value) in this.Where(pair => pair.Value.Count > 0))
        {
            builder.Append('[');
            builder.Append(key);
            builder.Append(']');
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);
            builder.Append(value);
            builder.Append(Environment.NewLine);
        }

        return builder.Remove(builder.Length - Environment.NewLine.Length, Environment.NewLine.Length).ToString();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
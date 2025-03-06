using System.Collections;

namespace Configuration;

public sealed partial class Ini(Config config) : IKeyValueCollection<string, Section>
{
    public Ini() : this([])
    {
    }

    public Ini(IEqualityComparer<string> comparer) : this(new Config(comparer))
    {
    }

    public Config Config { get; } = config;

    public IEqualityComparer<string> Comparer { get; } = config.Comparer;

    public Option<Section> this[string section]
    {
        get => Config[section];
        set => Config[section] = value;
    }

    public Option<string> this[string section, string key]
    {
        get => Config[section].Bind(sec => sec[key]);
        set => Config[section].Match(sec => sec[key] = value, () => Config[section] = Some(new Section(Comparer) { [key] = value }));
    }

    public int Count => Config.Count;

    public void Clear()
    {
        Config.Clear();
    }

    public bool Contains(KeyValue<string, Section> keyValue)
    {
        return Config.Contains(keyValue);
    }

    public Option<Section> Add(KeyValue<string, Section> keyValue)
    {
        return Config.Add(keyValue);
    }

    public IEnumerator<KeyValue<string, Section>> GetEnumerator()
    {
        return Config.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public override string ToString()
    {
        return Config.ToString();
    }
}
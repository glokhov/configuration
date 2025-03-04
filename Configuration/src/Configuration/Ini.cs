namespace Configuration;

public sealed partial class Ini(Config config)
{
    public Ini() : this(new Config())
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
        get => Config[section].Match(sec => sec[key], None);
        set => Config[section].Match(sec => sec[key] = value, () => Config[section] = Some(new Section(Comparer) { [key] = value }));
    }

    public override string ToString()
    {
        return Config.ToString();
    }
}
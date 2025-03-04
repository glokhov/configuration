namespace Configuration;

public sealed partial class Configuration
{
    public Configuration() : this(new Config())
    {
    }

    public Configuration(IEqualityComparer<string> comparer) : this(new Config(comparer))
    {
    }

    private Configuration(Config config)
    {
        Config = config;
        Comparer = config.Comparer;
    }

    private Config Config { get; }

    private IEqualityComparer<string> Comparer { get; }

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
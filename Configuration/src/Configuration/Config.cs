using System.Text;

namespace Configuration;

public sealed class Config : KeyValueDictionary<string, Section>
{
    public Config()
    {
    }

    public Config(StringComparer comparer) : base(comparer)
    {
    }

    public Config(Config config) : base(config)
    {
    }

    public Config Merge(Config config)
    {
        var merge = new Config(this);

        foreach (var (key, section) in config)
        {
            merge[key] = Some(merge[key].Map(sec => sec.Merge(section)).Match(sec => sec, section));
        }

        return merge;
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
}
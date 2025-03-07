using System.Text;

namespace Configuration;

/// <summary>
/// Represents a collection of sections of keys and values.
/// </summary>
public sealed class Config : KeyValueDictionary<string, Section>
{
    /// <summary>
    /// Initializes a new instance of the <c>Config</c> class.
    /// </summary>
    public Config()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <c>Config</c> class.
    /// </summary>
    /// <param name="comparer">
    /// The <c>IEqualityComparer&lt;string&gt;</c> implementation to use when comparing section names.
    /// </param>
    public Config(IEqualityComparer<string> comparer) : base(comparer)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <c>Config</c> class.
    /// </summary>
    /// <param name="config">The <c>Config</c> whose elements are copied to the new <c>Config</c>.</param>
    public Config(Config config) : base(config)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <c>Config</c> class.
    /// </summary>
    /// <param name="config">The <c>Config</c> whose elements are copied to the new <c>Config</c>.</param>
    /// <param name="comparer">
    /// The <c>IEqualityComparer&lt;string&gt;</c> implementation to use when comparing section names.
    /// </param>
    public Config(Config config, IEqualityComparer<string> comparer) : base(config, comparer)
    {
    }

    /// <summary>
    /// Creates a new instance of <c>Config</c> class by merging this <c>Config</c> with the supplied one.
    /// </summary>
    /// <param name="config">The <c>Config</c> whose elements are merged with this <c>Config</c>.</param>
    /// <returns>The new instance of <c>Config</c>.</returns>
    public Config Merge(Config config)
    {
        var merge = new Config(this, Comparer);

        foreach (var (key, section) in config)
        {
            merge[key] = Some(merge[key].Map(sec => sec.Merge(section)).Match(sec => sec, section));
        }

        return merge;
    }

    /// <summary>
    /// Returns the string representation of this instance.
    /// </summary>
    /// <returns>The string representation of this instance.</returns>
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
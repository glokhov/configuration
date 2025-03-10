using System.Text;

namespace Configuration;

/// <summary>
/// Represents a collection of sections of keys and values.
/// </summary>
public sealed class ConfigDictionary : KeyValueDictionary<string, SectionDictionary>
{
    /// <summary>
    /// Initializes a new instance of the <c>ConfigDictionary</c> class.
    /// </summary>
    public ConfigDictionary()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <c>ConfigDictionary</c> class.
    /// </summary>
    /// <param name="comparer">
    /// The <c>IEqualityComparer&lt;string&gt;</c> implementation to use when comparing section names.
    /// </param>
    public ConfigDictionary(IEqualityComparer<string> comparer) : base(comparer)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <c>ConfigDictionary</c> class.
    /// </summary>
    /// <param name="dictionary">
    /// The <c>ConfigDictionary</c> whose elements are copied to the new <c>ConfigDictionary</c>.
    /// </param>
    public ConfigDictionary(ConfigDictionary dictionary) : base(dictionary)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <c>ConfigDictionary</c> class.
    /// </summary>
    /// <param name="dictionary">
    /// The <c>ConfigDictionary</c> whose elements are copied to the new <c>ConfigDictionary</c>.
    /// </param>
    /// <param name="comparer">
    /// The <c>IEqualityComparer&lt;string&gt;</c> implementation to use when comparing section names.
    /// </param>
    public ConfigDictionary(ConfigDictionary dictionary, IEqualityComparer<string> comparer) : base(dictionary, comparer)
    {
    }

    /// <summary>
    /// Creates a new instance of <c>ConfigDictionary</c> class by merging this <c>ConfigDictionary</c>
    /// with the supplied one.
    /// </summary>
    /// <param name="dictionary">
    /// The <c>ConfigDictionary</c> whose elements are merged with this <c>ConfigDictionary</c>.
    /// </param>
    /// <returns>The new instance of <c>ConfigDictionary</c>.</returns>
    public ConfigDictionary Merge(ConfigDictionary dictionary)
    {
        var merge = new ConfigDictionary(this, Comparer);

        foreach (var (key, section) in dictionary)
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
            if (key.Length > 0)
            {
                builder.Append('[');
                builder.Append(key);
                builder.Append(']');
                builder.Append(Environment.NewLine);
                builder.Append(Environment.NewLine);
            }
            builder.Append(value);
            builder.Append(Environment.NewLine);
        }

        return builder.Remove(builder.Length - Environment.NewLine.Length, Environment.NewLine.Length).ToString();
    }
}
using System.Text;

namespace Configuration;

/// <summary>
/// Represents a collection of keys and values.
/// </summary>
public sealed class SectionDictionary : KeyValueDictionary<string, string>
{
    /// <summary>
    /// Initializes a new instance of the <c>SectionDictionary</c> class.
    /// </summary>
    public SectionDictionary()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <c>SectionDictionary</c> class.
    /// </summary>
    /// <param name="comparer">
    /// The <c>IEqualityComparer&lt;string&gt;</c> implementation to use when comparing keys.
    /// </param>
    public SectionDictionary(IEqualityComparer<string> comparer) : base(comparer)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <c>SectionDictionary</c> class.
    /// </summary>
    /// <param name="dictionary">
    /// The <c>SectionDictionary</c> whose elements are copied to the new <c>SectionDictionary</c>.
    /// </param>
    public SectionDictionary(SectionDictionary dictionary) : base(dictionary)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <c>SectionDictionary</c> class.
    /// </summary>
    /// <param name="dictionary">
    /// The <c>SectionDictionary</c> whose elements are copied to the new <c>SectionDictionary</c>.
    /// </param>
    /// <param name="comparer">
    /// The <c>IEqualityComparer&lt;string&gt;</c> implementation to use when comparing keys.
    /// </param>
    public SectionDictionary(SectionDictionary dictionary, IEqualityComparer<string> comparer)
        : base(dictionary, comparer)
    {
    }

    /// <summary>
    /// Creates a new instance of <c>SectionDictionary</c> class by merging this <c>SectionDictionary</c>
    /// with the supplied one.
    /// </summary>
    /// <param name="dictionary">
    /// The <c>SectionDictionary</c> whose elements are merged with this <c>SectionDictionary</c>.
    /// </param>
    /// <returns>The new instance of <c>SectionDictionary</c>.</returns>
    public SectionDictionary Merge(SectionDictionary dictionary)
    {
        var merge = new SectionDictionary(this, Comparer);

        foreach (var (key, value) in dictionary)
        {
            merge[key] = Some(value);
        }

        return merge;
    }

    /// <summary>
    /// Returns the string representation of this instance.
    /// </summary>
    /// <returns>The string representation of this instance.</returns>
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
}
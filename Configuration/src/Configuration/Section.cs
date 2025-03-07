using System.Text;

namespace Configuration;

/// <summary>
/// Represents a collection of keys and values.
/// </summary>
public sealed class Section : KeyValueDictionary<string, string>
{
    /// <summary>
    /// Initializes a new instance of the <c>Section</c> class.
    /// </summary>
    public Section()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <c>Section</c> class.
    /// </summary>
    /// <param name="comparer">
    /// The <c>IEqualityComparer&lt;string&gt;</c> implementation to use when comparing keys.
    /// </param>
    public Section(IEqualityComparer<string> comparer) : base(comparer)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <c>Section</c> class.
    /// </summary>
    /// <param name="section">The <c>Section</c> whose elements are copied to the new <c>Section</c>.</param>
    public Section(Section section) : base(section)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <c>Section</c> class.
    /// </summary>
    /// <param name="section">The <c>Section</c> whose elements are copied to the new <c>Section</c>.</param>
    /// <param name="comparer">
    /// The <c>IEqualityComparer&lt;string&gt;</c> implementation to use when comparing keys.
    /// </param>
    public Section(Section section, IEqualityComparer<string> comparer) : base(section, comparer)
    {
    }

    /// <summary>
    /// Creates a new instance of <c>Section</c> class by merging this <c>Section</c> with the supplied one.
    /// </summary>
    /// <param name="section">The <c>Section</c> whose elements are merged with this <c>Section</c>.</param>
    /// <returns>The new instance of <c>Section</c>.</returns>
    public Section Merge(Section section)
    {
        var merge = new Section(this, Comparer);

        foreach (var (key, value) in section)
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
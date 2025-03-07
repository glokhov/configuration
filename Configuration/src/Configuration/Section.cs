using System.Text;

namespace Configuration;

public sealed class Section : KeyValueDictionary<string, string>
{
    public Section()
    {
    }

    public Section(IEqualityComparer<string> comparer) : base(comparer)
    {
    }

    public Section(Section section) : base(section)
    {
    }

    public Section(Section section, IEqualityComparer<string> comparer) : base(section, comparer)
    {
    }

    public Section Merge(Section section)
    {
        var merge = new Section(this, Comparer);

        foreach (var (key, value) in section)
        {
            merge[key] = Some(value);
        }

        return merge;
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
}
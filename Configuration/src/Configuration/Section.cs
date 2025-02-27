using System.Text;

namespace Configuration;

public sealed class Section : KeyValueDictionary<string, string>
{
    public Section()
    {
    }

    public Section(StringComparer comparer) : base(comparer)
    {
    }

    public Section(Section section) : base(section)
    {
    }

    public Section Merge(Section section)
    {
        var merge = new Section(this);

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
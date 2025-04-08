namespace Configuration.Misc;

internal static class TextReaderExtensions
{
    public static IEnumerable<string> ReadLines(this TextReader reader)
    {
        while (reader.ReadLine() is { } text)
        {
            yield return text;
        }
    }
}
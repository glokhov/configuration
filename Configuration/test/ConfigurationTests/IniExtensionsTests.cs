namespace ConfigurationTests;

public partial class IniTests : IDisposable
{
    private FileInfo? _tempFile;

    private FileInfo CreateTempFile()
    {
        return _tempFile = new FileInfo(Path.GetTempFileName());
    }

    [Fact]
    public void WriteTo_Writes_To_File_And_Returns_Ok()
    {
        var temp = CreateTempFile();

        var result = Configuration.Ini.Parse(ABD).Pure().Value.WriteTo(temp);

        using var streamReader = new StreamReader(temp.OpenRead());
        var abd = streamReader.ReadToEnd();

        Assert.True(result.IsOk);
        Assert.Equal(ABD, abd);
    }

    [Fact]
    public void WriteTo_Returns_Error_If_Cannot_Write_To_File()
    {
        var temp = Path.GetTempPath();

        var error = new Ini().WriteTo(new FileInfo(temp)).Fail().Error;

        Assert.Contains(temp, error);
    }

    [Fact]
    public void WriteTo_Writes_To_TextWriter_And_Returns_Ok()
    {
        using var stringWriter = new StringWriter();

        var result = Configuration.Ini.Parse(ABD).Pure().Value.WriteTo(stringWriter);

        Assert.True(result.IsOk);
        Assert.Equal(ABD, stringWriter.ToString());
    }

    [Fact]
    public void WriteTo_Returns_Error_If_Cannot_Write_To_TextWriter()
    {
        var stringWriter = new StringWriter();
        stringWriter.Close();

        var error = new Ini().WriteTo(stringWriter).Fail().Error;

        Assert.Contains("Cannot write to a closed TextWriter.", error);
    }

    [Fact]
    public void Get_Section_Key_Returns_Parent_Values()
    {
        var ini = new Ini
        {
            ["foo", "foo"] = Some("foo"),
            ["foo", "xxx"] = Some("xxx"),
            ["foo.bar", "bar"] = Some("bar"),
            ["foo.bar", "xxx"] = Some("yyy"),
            ["foo.bar.baz", "xxx"] = Some("zzz")
        };

        Assert.Equal("foo", ini.GetNested("foo", "foo").Pure().Value);
        Assert.Equal("xxx", ini.GetNested("foo", "xxx").Pure().Value);

        Assert.Equal("foo", ini.GetNested("foo.bar", "foo").Pure().Value);
        Assert.Equal("bar", ini.GetNested("foo.bar", "bar").Pure().Value);
        Assert.Equal("yyy", ini.GetNested("foo.bar", "xxx").Pure().Value);

        Assert.Equal("foo", ini.GetNested("foo.bar.baz", "foo").Pure().Value);
        Assert.Equal("bar", ini.GetNested("foo.bar.baz", "bar").Pure().Value);
        Assert.Equal("zzz", ini.GetNested("foo.bar.baz", "xxx").Pure().Value);
    }

    [Fact]
    public void Get_Section_Returns_Parent_Values()
    {
        var ini = new Ini
        {
            ["foo", "foo"] = Some("foo"),
            ["foo", "xxx"] = Some("xxx"),
            ["foo.bar", "bar"] = Some("bar"),
            ["foo.bar", "xxx"] = Some("yyy"),
            ["foo.bar.baz", "xxx"] = Some("zzz")
        };

        var foo = ini.GetNested("foo").Pure().Value;
        var bar = ini.GetNested("foo.bar").Pure().Value;
        var baz = ini.GetNested("foo.bar.baz").Pure().Value;

        Assert.Equal("foo", foo["foo"].Pure().Value);
        Assert.Equal("xxx", foo["xxx"].Pure().Value);

        Assert.Equal("foo", bar["foo"].Pure().Value);
        Assert.Equal("bar", bar["bar"].Pure().Value);
        Assert.Equal("yyy", bar["xxx"].Pure().Value);

        Assert.Equal("foo", baz["foo"].Pure().Value);
        Assert.Equal("bar", baz["bar"].Pure().Value);
        Assert.Equal("zzz", baz["xxx"].Pure().Value);
    }

    public void Dispose()
    {
        _tempFile?.Delete();
    }
}
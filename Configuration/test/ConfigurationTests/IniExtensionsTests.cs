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

        var result = Configuration.Ini.Parse(ABD).Unwrap().WriteTo(temp);

        using var streamReader = new StreamReader(temp.OpenRead());
        var abd = streamReader.ReadToEnd();

        Assert.True(result.IsOk);
        Assert.Equal(ABD, abd);
    }

    [Fact]
    public void WriteTo_Returns_Error_If_Cannot_Write_To_File()
    {
        var temp = Path.GetTempPath();

        var error = new Ini().WriteTo(new FileInfo(temp)).ExpectError();

        Assert.Contains(temp, error);
    }

    [Fact]
    public void WriteTo_Writes_To_TextWriter_And_Returns_Ok()
    {
        using var stringWriter = new StringWriter();

        var result = Configuration.Ini.Parse(ABD).Unwrap().WriteTo(stringWriter);

        Assert.True(result.IsOk);
        Assert.Equal(ABD, stringWriter.ToString());
    }

    [Fact]
    public void WriteTo_Returns_Error_If_Cannot_Write_To_TextWriter()
    {
        var stringWriter = new StringWriter();
        stringWriter.Close();

        var error = new Ini().WriteTo(stringWriter).ExpectError();

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

        Assert.Equal("foo", ini.GetNested("foo", "foo").Unwrap());
        Assert.Equal("xxx", ini.GetNested("foo", "xxx").Unwrap());

        Assert.Equal("foo", ini.GetNested("foo.bar", "foo").Unwrap());
        Assert.Equal("bar", ini.GetNested("foo.bar", "bar").Unwrap());
        Assert.Equal("yyy", ini.GetNested("foo.bar", "xxx").Unwrap());

        Assert.Equal("foo", ini.GetNested("foo.bar.baz", "foo").Unwrap());
        Assert.Equal("bar", ini.GetNested("foo.bar.baz", "bar").Unwrap());
        Assert.Equal("zzz", ini.GetNested("foo.bar.baz", "xxx").Unwrap());

        Assert.True(ini.GetNested("foo.bar.baz", "aaa").IsNone);
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

        var foo = ini.GetNested("foo").Unwrap();
        var bar = ini.GetNested("foo.bar").Unwrap();
        var baz = ini.GetNested("foo.bar.baz").Unwrap();

        Assert.Equal("foo", foo["foo"].Unwrap());
        Assert.Equal("xxx", foo["xxx"].Unwrap());

        Assert.Equal("foo", bar["foo"].Unwrap());
        Assert.Equal("bar", bar["bar"].Unwrap());
        Assert.Equal("yyy", bar["xxx"].Unwrap());

        Assert.Equal("foo", baz["foo"].Unwrap());
        Assert.Equal("bar", baz["bar"].Unwrap());
        Assert.Equal("zzz", baz["xxx"].Unwrap());
    }

    [Fact]
    public void Test()
    {
        var ini = new Ini
        {
            ["foo.bar.baz", "fuz"] = Some("fuz")
        };

        var foo = ini.GetNested("foo").Unwrap();
        var bar = ini.GetNested("foo.bar").Unwrap();
        var baz = ini.GetNested("foo.bar.baz").Unwrap();
        var fuz = baz["fuz"].Unwrap();

        Assert.Empty(foo);
        Assert.Empty(bar);
        Assert.Equal("fuz", fuz);
    }

    public void Dispose()
    {
        _tempFile?.Delete();
    }
}
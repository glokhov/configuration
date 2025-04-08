using Functional;

namespace Configuration.Tests;

public sealed class ParserTests
{
    [Fact]
    public void FromResult_Section_Successful()
    {
        var section = Parser.ParseLine("[foo]").Unwrap() as Section;

        Assert.Equal("foo", section?.Name);
    }

    [Fact]
    public void FromResult_Section_Spaces_Successful()
    {
        var section = Parser.ParseLine(" [ foo ] ").Unwrap() as Section;

        Assert.Equal("foo", section?.Name);
    }
    
    [Fact]
    public void FromResult_Parameter_Successful()
    {
        var parameter = Parser.ParseLine("foo=bar").Unwrap() as Parameter;

        Assert.Equal("foo", parameter?.Key);
        Assert.Equal("bar", parameter?.Value);
    }

    [Fact]
    public void FromResult_Parameter_Spaces_Successful()
    {
        var parameter = Parser.ParseLine(" foo = bar ").Unwrap() as Parameter;

        Assert.Equal("foo", parameter?.Key);
        Assert.Equal("bar", parameter?.Value);
    }
    
    [Fact]
    public void FromResult_Comment_Successful()
    {
        var comment = Parser.ParseLine("#foo").Unwrap() as Comment;

        Assert.Equal("foo", comment?.Text);
    }

    [Fact]
    public void FromResult_Comment_Spaces_Successful()
    {
        var comment = Parser.ParseLine(" # foo ").Unwrap() as Comment;

        Assert.Equal("foo ", comment?.Text);
    }
    
    [Fact]
    public void FromResult_Line_Empty_Successful()
    {
        var line = Parser.ParseLine("").Unwrap();

        Assert.NotNull(line);
    }

    [Fact]
    public void FromResult_Line_Spaces_Successful()
    {
        var line = Parser.ParseLine(" ").Unwrap();

        Assert.NotNull(line);
    }

    [Fact]
    public void FromResult_Line_Failed()
    {
        var error = Parser.ParseLine("foo").ExpectError();

        Assert.Equal("Cannot parse line 'foo'", error);
    }
}
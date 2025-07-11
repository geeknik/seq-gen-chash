using SequenceGenerator.Core.Interfaces;
using SequenceGenerator.Core.Parsing;

namespace SequenceGenerator.Tests.Parsing;

public class SequenceTemplateParserTests
{
    private readonly SequenceTemplateParser _parser = new();

    [Fact]
    public void Parse_EmptyTemplate_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _parser.Parse(""));
        Assert.Throws<ArgumentException>(() => _parser.Parse(null!));
    }

    [Fact]
    public void Parse_LiteralOnly_ReturnsOneLiteralPart()
    {
        var result = _parser.Parse("Hello World");
        
        Assert.Single(result.Parts);
        var part = Assert.IsType<LiteralPart>(result.Parts[0]);
        Assert.Equal("Hello World", part.Value);
    }

    [Fact]
    public void Parse_SimpleToken_ReturnsTokenPart()
    {
        var result = _parser.Parse("{seq}");
        
        Assert.Single(result.Parts);
        var part = Assert.IsType<TokenPart>(result.Parts[0]);
        Assert.Equal("seq", part.TokenName);
        Assert.Null(part.Format);
    }

    [Fact]
    public void Parse_TokenWithFormat_ReturnsTokenPartWithFormat()
    {
        var result = _parser.Parse("{seq:000}");
        
        Assert.Single(result.Parts);
        var part = Assert.IsType<TokenPart>(result.Parts[0]);
        Assert.Equal("seq", part.TokenName);
        Assert.Equal("000", part.Format);
    }

    [Fact]
    public void Parse_MixedContent_ReturnsCorrectParts()
    {
        var result = _parser.Parse("INV-{yyyyMM}-{seq:000}");
        
        Assert.Equal(4, result.Parts.Count);
        
        var literal1 = Assert.IsType<LiteralPart>(result.Parts[0]);
        Assert.Equal("INV-", literal1.Value);
        
        var token1 = Assert.IsType<TokenPart>(result.Parts[1]);
        Assert.Equal("yyyyMM", token1.TokenName);
        Assert.Null(token1.Format);
        
        var literal2 = Assert.IsType<LiteralPart>(result.Parts[2]);
        Assert.Equal("-", literal2.Value);
        
        var token2 = Assert.IsType<TokenPart>(result.Parts[3]);
        Assert.Equal("seq", token2.TokenName);
        Assert.Equal("000", token2.Format);
    }

    [Fact]
    public void Parse_ConsecutiveTokens_ReturnsCorrectParts()
    {
        var result = _parser.Parse("{year}{month}{day}");
        
        Assert.Equal(3, result.Parts.Count);
        Assert.All(result.Parts, part => Assert.IsType<TokenPart>(part));
        
        Assert.Equal("year", ((TokenPart)result.Parts[0]).TokenName);
        Assert.Equal("month", ((TokenPart)result.Parts[1]).TokenName);
        Assert.Equal("day", ((TokenPart)result.Parts[2]).TokenName);
    }

    [Fact]
    public void Parse_TokenWithParameters_ParsesParametersCorrectly()
    {
        var result = _parser.Parse("{seq:D3,start=100,step=5}");
        
        Assert.Single(result.Parts);
        var part = Assert.IsType<TokenPart>(result.Parts[0]);
        Assert.Equal("seq", part.TokenName);
        Assert.Equal("D3", part.Format);
        Assert.Equal(2, part.Parameters.Count);
        Assert.Equal("100", part.Parameters["start"]);
        Assert.Equal("5", part.Parameters["step"]);
    }
}
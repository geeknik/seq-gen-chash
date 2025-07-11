using SequenceGenerator.Core;
using SequenceGenerator.Core.Models;
using SequenceGenerator.Core.Parsing;
using SequenceGenerator.Core.Storage;
using SequenceGenerator.Core.Tokens;

namespace SequenceGenerator.Tests;

public class SequenceGeneratorTests
{
    private readonly Core.SequenceGenerator _generator;
    private readonly InMemoryStorage _storage;

    public SequenceGeneratorTests()
    {
        _storage = new InMemoryStorage();
        var parser = new SequenceTemplateParser();
        var tokenFactory = new TokenFactory();
        _generator = new Core.SequenceGenerator(parser, _storage, tokenFactory);
    }

    [Fact]
    public async Task GenerateAsync_SimpleSequence_GeneratesCorrectly()
    {
        var definition = new SequenceDefinition
        {
            Id = "test",
            Name = "Test Sequence",
            Template = "SEQ-{seq:000}"
        };

        var result1 = await _generator.GenerateAsync(definition);
        var result2 = await _generator.GenerateAsync(definition);
        var result3 = await _generator.GenerateAsync(definition);

        Assert.Equal("SEQ-001", result1);
        Assert.Equal("SEQ-002", result2);
        Assert.Equal("SEQ-003", result3);
    }

    [Fact]
    public async Task GenerateAsync_WithDateTime_IncludesFormattedDate()
    {
        var definition = new SequenceDefinition
        {
            Id = "invoice",
            Name = "Invoice Number",
            Template = "INV-{date:yyyyMMdd}-{seq:0000}"
        };

        var result = await _generator.GenerateAsync(definition);

        Assert.StartsWith("INV-", result);
        Assert.Matches(@"^INV-\d{8}-\d{4}$", result);
    }

    [Fact]
    public async Task GenerateAsync_ComplexTemplate_GeneratesCorrectly()
    {
        var definition = new SequenceDefinition
        {
            Id = "complex",
            Name = "Complex Sequence",
            Template = "{year}/{month}/ORDER-{seq:00000}"
        };

        var result = await _generator.GenerateAsync(definition);
        var year = DateTime.UtcNow.Year.ToString();
        var month = DateTime.UtcNow.ToString("MM");

        Assert.StartsWith($"{year}/{month}/ORDER-", result);
        Assert.Matches(@"^\d{4}/\d{2}/ORDER-\d{5}$", result);
    }

    [Fact]
    public async Task GenerateBatchAsync_GeneratesMultipleSequences()
    {
        var definition = new SequenceDefinition
        {
            Id = "batch",
            Name = "Batch Sequence",
            Template = "BATCH-{seq:000}"
        };

        var results = await _generator.GenerateBatchAsync(definition, 5);

        var resultList = results.ToList();
        Assert.Equal(5, resultList.Count);
        Assert.Equal("BATCH-001", resultList[0]);
        Assert.Equal("BATCH-002", resultList[1]);
        Assert.Equal("BATCH-003", resultList[2]);
        Assert.Equal("BATCH-004", resultList[3]);
        Assert.Equal("BATCH-005", resultList[4]);
    }

    [Fact]
    public async Task GenerateAsync_UpdatesLastUsedAt()
    {
        var definition = new SequenceDefinition
        {
            Id = "test",
            Name = "Test",
            Template = "{seq}",
            LastUsedAt = null
        };

        await _generator.GenerateAsync(definition);

        Assert.NotNull(definition.LastUsedAt);
        Assert.True(definition.LastUsedAt > DateTime.UtcNow.AddSeconds(-5));
    }

    [Fact]
    public async Task GenerateAsync_WithUnknownToken_ThrowsException()
    {
        var definition = new SequenceDefinition
        {
            Id = "test",
            Name = "Test",
            Template = "{unknowntoken}"
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => _generator.GenerateAsync(definition));
    }

    [Fact]
    public async Task GenerateAsync_NullDefinition_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _generator.GenerateAsync(null!));
    }

    [Fact]
    public async Task GenerateBatchAsync_InvalidCount_ThrowsArgumentException()
    {
        var definition = new SequenceDefinition
        {
            Id = "test",
            Name = "Test",
            Template = "{seq}"
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _generator.GenerateBatchAsync(definition, 0));
        await Assert.ThrowsAsync<ArgumentException>(() => _generator.GenerateBatchAsync(definition, -1));
    }
}
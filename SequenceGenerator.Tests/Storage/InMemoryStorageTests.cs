using SequenceGenerator.Core.Storage;

namespace SequenceGenerator.Tests.Storage;

public class InMemoryStorageTests
{
    private readonly InMemoryStorage _storage = new();

    [Fact]
    public async Task GetNextSequenceValueAsync_FirstCall_ReturnsOne()
    {
        var result = await _storage.GetNextSequenceValueAsync("test-seq");
        
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task GetNextSequenceValueAsync_MultipleCalls_IncrementsCorrectly()
    {
        var seq1 = await _storage.GetNextSequenceValueAsync("test-seq");
        var seq2 = await _storage.GetNextSequenceValueAsync("test-seq");
        var seq3 = await _storage.GetNextSequenceValueAsync("test-seq");
        
        Assert.Equal(1, seq1);
        Assert.Equal(2, seq2);
        Assert.Equal(3, seq3);
    }

    [Fact]
    public async Task GetNextSequenceValueAsync_DifferentSequences_MaintainsSeparateCounters()
    {
        var seq1a = await _storage.GetNextSequenceValueAsync("seq1");
        var seq2a = await _storage.GetNextSequenceValueAsync("seq2");
        var seq1b = await _storage.GetNextSequenceValueAsync("seq1");
        var seq2b = await _storage.GetNextSequenceValueAsync("seq2");
        
        Assert.Equal(1, seq1a);
        Assert.Equal(1, seq2a);
        Assert.Equal(2, seq1b);
        Assert.Equal(2, seq2b);
    }

    [Fact]
    public async Task GetCurrentSequenceValueAsync_BeforeAnyIncrement_ReturnsZero()
    {
        var result = await _storage.GetCurrentSequenceValueAsync("new-seq");
        
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task GetCurrentSequenceValueAsync_AfterIncrement_ReturnsCurrentValue()
    {
        await _storage.GetNextSequenceValueAsync("test-seq");
        await _storage.GetNextSequenceValueAsync("test-seq");
        
        var current = await _storage.GetCurrentSequenceValueAsync("test-seq");
        
        Assert.Equal(2, current);
    }

    [Fact]
    public async Task ResetSequenceAsync_ResetsToSpecifiedValue()
    {
        await _storage.GetNextSequenceValueAsync("test-seq");
        await _storage.GetNextSequenceValueAsync("test-seq");
        
        await _storage.ResetSequenceAsync("test-seq", 100);
        
        var next = await _storage.GetNextSequenceValueAsync("test-seq");
        Assert.Equal(101, next);
    }

    [Fact]
    public async Task GetAllSequenceIdsAsync_ReturnsAllIds()
    {
        await _storage.GetNextSequenceValueAsync("seq1");
        await _storage.GetNextSequenceValueAsync("seq2");
        await _storage.GetNextSequenceValueAsync("seq3");
        
        var ids = await _storage.GetAllSequenceIdsAsync();
        
        Assert.Equal(3, ids.Count());
        Assert.Contains("seq1", ids);
        Assert.Contains("seq2", ids);
        Assert.Contains("seq3", ids);
    }

    [Fact]
    public async Task SequenceExistsAsync_ExistingSequence_ReturnsTrue()
    {
        await _storage.GetNextSequenceValueAsync("existing");
        
        var exists = await _storage.SequenceExistsAsync("existing");
        
        Assert.True(exists);
    }

    [Fact]
    public async Task SequenceExistsAsync_NonExistingSequence_ReturnsFalse()
    {
        var exists = await _storage.SequenceExistsAsync("non-existing");
        
        Assert.False(exists);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task Methods_WithInvalidSequenceId_ThrowsArgumentException(string sequenceId)
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _storage.GetNextSequenceValueAsync(sequenceId));
        await Assert.ThrowsAsync<ArgumentException>(() => _storage.GetCurrentSequenceValueAsync(sequenceId));
        await Assert.ThrowsAsync<ArgumentException>(() => _storage.ResetSequenceAsync(sequenceId));
        await Assert.ThrowsAsync<ArgumentException>(() => _storage.SequenceExistsAsync(sequenceId));
    }
}
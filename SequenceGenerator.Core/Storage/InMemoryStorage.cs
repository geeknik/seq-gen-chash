using System.Collections.Concurrent;
using SequenceGenerator.Core.Interfaces;

namespace SequenceGenerator.Core.Storage;

public class InMemoryStorage : ISequenceStorage
{
    private readonly ConcurrentDictionary<string, long> _sequences = new();

    public Task<long> GetNextSequenceValueAsync(string sequenceId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sequenceId))
            throw new ArgumentException("Sequence ID cannot be null or empty", nameof(sequenceId));

        var nextValue = _sequences.AddOrUpdate(sequenceId, 1, (_, current) => current + 1);
        return Task.FromResult(nextValue);
    }

    public Task<long> GetCurrentSequenceValueAsync(string sequenceId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sequenceId))
            throw new ArgumentException("Sequence ID cannot be null or empty", nameof(sequenceId));

        var currentValue = _sequences.GetOrAdd(sequenceId, 0);
        return Task.FromResult(currentValue);
    }

    public Task ResetSequenceAsync(string sequenceId, long value = 0, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sequenceId))
            throw new ArgumentException("Sequence ID cannot be null or empty", nameof(sequenceId));

        _sequences.AddOrUpdate(sequenceId, value, (_, _) => value);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<string>> GetAllSequenceIdsAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<string>>(_sequences.Keys.ToList());
    }

    public Task<bool> SequenceExistsAsync(string sequenceId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sequenceId))
            throw new ArgumentException("Sequence ID cannot be null or empty", nameof(sequenceId));

        return Task.FromResult(_sequences.ContainsKey(sequenceId));
    }
}
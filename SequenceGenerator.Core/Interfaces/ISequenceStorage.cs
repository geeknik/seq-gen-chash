namespace SequenceGenerator.Core.Interfaces;

public interface ISequenceStorage
{
    Task<long> GetNextSequenceValueAsync(string sequenceId, CancellationToken cancellationToken = default);
    Task<long> GetCurrentSequenceValueAsync(string sequenceId, CancellationToken cancellationToken = default);
    Task ResetSequenceAsync(string sequenceId, long value = 0, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetAllSequenceIdsAsync(CancellationToken cancellationToken = default);
    Task<bool> SequenceExistsAsync(string sequenceId, CancellationToken cancellationToken = default);
}
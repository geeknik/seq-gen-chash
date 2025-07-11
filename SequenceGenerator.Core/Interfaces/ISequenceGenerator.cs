namespace SequenceGenerator.Core.Interfaces;

public interface ISequenceGenerator
{
    Task<string> GenerateAsync(ISequenceDefinition definition, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GenerateBatchAsync(ISequenceDefinition definition, int count, CancellationToken cancellationToken = default);
}
namespace SequenceGenerator.Core.Interfaces;

public interface IToken
{
    string Name { get; }
    Task<string> GenerateAsync(ITokenContext context, CancellationToken cancellationToken = default);
    bool RequiresStorage { get; }
}
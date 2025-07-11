using SequenceGenerator.Core.Interfaces;

namespace SequenceGenerator.Core.Models;

public class TokenContext : ITokenContext
{
    public ISequenceDefinition Definition { get; init; } = null!;
    public ISequenceStorage Storage { get; init; } = null!;
    public Dictionary<string, object> Parameters { get; init; } = new();
    public DateTime GenerationTime { get; init; } = DateTime.UtcNow;
}
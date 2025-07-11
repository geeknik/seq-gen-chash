using SequenceGenerator.Core.Interfaces;

namespace SequenceGenerator.Core.Models;

public class SequenceDefinition : ISequenceDefinition
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string Name { get; init; } = string.Empty;
    public string Template { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? LastUsedAt { get; set; }
}